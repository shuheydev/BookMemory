using System;
using System.Collections.Generic;
using System.Net;
using AmazonProductAdvtApi;
using System.Xml.Linq;
using System.Diagnostics;
using System.Windows.Media.Imaging;
using System.IO.IsolatedStorage;
using System.IO;

namespace BookMemory
{
    //自作イベント用のイベント引数として使うクラス
    //アマゾンからデータを取得してDBへの格納が完了した時に発行する。
    public class GetItemInfoFromAmazonCompletedEventArgs : EventArgs
    {
        public string Result;//0:Success,1:No Network,2:No Item on Amazon
        public XElement XmlItemInfo;

        public GetItemInfoFromAmazonCompletedEventArgs()
        {
            this.Result = "";
        }
    }


    public class AmazonWebService
    {
        //メンバ変数
        private string TargetItemID { get; set; }

        string ImageDIR = "Images";

        //アマゾンへのリクエスト作成に使用する情報。
        private readonly string MyAWSAccessKeyID = "";
        private readonly string MyAWSSecretKey = "";
        private readonly string Destination = "";
        private readonly string MyAssociateTag = "";
        //private readonly string ItemID = "";
        private readonly string NameSpace = "http://webservices.amazon.com/AWSECommerceService/2011-08-01";


        //自作イベントの宣言、実装
        public delegate void GetItemInfoFromAmazonCompletedEventHandler(object sender, GetItemInfoFromAmazonCompletedEventArgs e);
        public event GetItemInfoFromAmazonCompletedEventHandler GetItemInfoFromAmazonCompleted;
        protected virtual void Completed(GetItemInfoFromAmazonCompletedEventArgs e)
        {
            if (GetItemInfoFromAmazonCompleted != null)
            {
                GetItemInfoFromAmazonCompleted(this, e);
            }
        }

        private void SendCompletedEvent(string result, XElement itemInfo)
        {
            GetItemInfoFromAmazonCompletedEventArgs completedEvent = new GetItemInfoFromAmazonCompletedEventArgs();
            completedEvent.Result = result;
            completedEvent.XmlItemInfo = itemInfo;
            //completedEvent.Item = itemData;
            Completed(completedEvent);
        }


        public AmazonWebService(string targetItemID)
        {
            this.TargetItemID = targetItemID;

            //MessageBox.Show(this.TargetItemID);
        }



        //非同期メソッド。呼び出すときは呼び出し側でイベントハンドラーGetItemInfoFromAmazonCompletedEventHandlerを実装すること。
        //書籍のXMLデータ取得を試み、その結果をメンバ変数のXMLItemInfo入れたあと、イベントを発行する。
        public void GetItemInfo()
        {
            //ItemDataBase itemDB = new ItemDataBase();
            XElement xmlItemInfo;

            if (Microsoft.Phone.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {

                //ネットワークが有効な場合、リクエストを送る。
                var helper = new SignedRequestHelper(MyAWSAccessKeyID, MyAWSSecretKey, Destination);
                // パラメータの設定（取得する製品や取得したい情報によって変わる）
                var r1 = new Dictionary<string, String>();
                r1["Service"] = "AWSECommerceService";
                r1["Version"] = "2011-08-01";
                r1["Operation"] = "ItemLookup";
                r1["ItemId"] = this.TargetItemID;
                r1["ResponseGroup"] = "Large,Reviews";
                r1["IdType"] = "ISBN";
                r1["SearchIndex"] = "Books";
                //r1["Keywords"] = "村上 春樹";
                r1["AssociateTag"] = MyAssociateTag;  // Required from API Version 2011-08-01

                var requestUrl = helper.Sign(r1);//URLエンコード
                Debug.WriteLine(requestUrl);
                // リクエスト送信
                var client = new WebClient();
                client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_DownloadStringCompleted);//イベントハンドラをセット
                client.DownloadStringAsync(new Uri(requestUrl));//リクエスト送信

            }
            else
            {
                //ネットワークが無効な場合
                xmlItemInfo = new XElement("Error", "No Network Connection");
                xmlItemInfo.SetAttributeValue("Code", 1);
                xmlItemInfo.Add(new XElement("EAN", this.TargetItemID));

                SendCompletedEvent("NoNetwork", xmlItemInfo);
            }

        }




        //Amazonからデータ取得が完了した際に呼び出されるメソッド
        private void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                XElement responsedXML = XElement.Parse(e.Result);//レスポンスされた商品情報XMLをパース
                //XName name = responseXML.Name;
                XNamespace ns = responsedXML.Name.Namespace;
                XElement xmlItemInfo = responsedXML.Element(ns + "Items").Element(ns + "Item");//商品情報が含まれたItemタグを取り出す。

                if (xmlItemInfo != null)
                {
                    //商品情報を取得できた場合
                    SendCompletedEvent("Completed", xmlItemInfo);

                    string imageWebUri;

                    if (xmlItemInfo.Element(ns + "LargeImage").Element(ns + "URL") != null)
                        imageWebUri = xmlItemInfo.Element(ns + "LargeImage").Element(ns + "URL").Value;
                    else
                        imageWebUri = "";

                    //大きい画像の取得
                    if (imageWebUri != "")
                    {
                        WebClient wc = new WebClient();
                        wc.OpenReadCompleted += new OpenReadCompletedEventHandler(wc_OpenReadCompleted);
                        wc.OpenReadAsync(new Uri(imageWebUri), UriKind.Absolute);
                    }
                }
                else
                {
                    //商品情報を取得できなかった場合
                    var error = responsedXML.Element(ns + "Items").Element(ns + "Request").Element(ns + "Errors").Element(ns + "Error");
                    if (error != null)//念のための確認
                    {
                        xmlItemInfo = new XElement("Error", "No Such Item on Amazon");
                        xmlItemInfo.SetAttributeValue("Code", 2);
                        xmlItemInfo.Add(new XElement("EAN", this.TargetItemID));

                        SendCompletedEvent("NoItemFound", xmlItemInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                XElement xmlItemInfo;
                xmlItemInfo = new XElement("Error", "HTML Login Page");
                xmlItemInfo.SetAttributeValue("Code", 7);
                xmlItemInfo.Add(new XElement("EAN", this.TargetItemID));

                SendCompletedEvent("HTMLLoginPage", xmlItemInfo);
                //return;
            }

        }


        //イメージのダウンロードが完了したら呼ばれるイベントハンドラ。
        private void wc_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                //イベント発行。商品画像保存失敗。
                var xmlItemInfo = new XElement("Error", "image download failed");
                xmlItemInfo.SetAttributeValue("Code", 6);
                xmlItemInfo.Add(new XElement("EAN", this.TargetItemID));

                SendCompletedEvent("ImageDownloadFailed", xmlItemInfo);

                return;
            }
            else
            {
                BitmapImage bi = new BitmapImage();
                bi.SetSource(e.Result);

                //var imageName = e.Result as MemoryStream;


                IsolatedStorageFile isoFile = IsolatedStorageFile.GetUserStoreForApplication();
                IsolatedStorageFileStream strm = new IsolatedStorageFileStream(ImageDIR + "/"+TargetItemID, FileMode.OpenOrCreate, FileAccess.Write, isoFile);
                IsolatedStorageFileStream strm_small = new IsolatedStorageFileStream(ImageDIR + "/" + TargetItemID + "_small", FileMode.OpenOrCreate, FileAccess.Write, isoFile);

                WriteableBitmap wb = new WriteableBitmap(bi);
                wb.SaveJpeg(strm, wb.PixelWidth, wb.PixelHeight, 0, 100);
                wb.SaveJpeg(strm_small, (int)(wb.PixelWidth / 4), (int)(wb.PixelHeight / 4), 0, 100);//サムネイル用に画像を小さくして保存。

                strm.Dispose();
                strm_small.Dispose();
                isoFile.Dispose();

                //イベント発行。商品画像保存完了。
                XElement xmlItemInfo = new XElement("Success", "Got Image Successfully");
                xmlItemInfo.SetAttributeValue("Code", 5);
                xmlItemInfo.Add(new XElement("EAN", this.TargetItemID));

                SendCompletedEvent("GotImage", xmlItemInfo);//5
            }
        }


    }
}
