using System;
using System.Windows.Media;
using Microsoft.Phone.Controls;
using Microsoft.Devices;
using com.google.zxing;
using com.google.zxing.oned;
using com.google.zxing.common;
using System.Linq;
using System.Xml.Linq;
using System.Windows;
using System.Windows.Resources;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.IO.IsolatedStorage;

namespace BookMemory
{
    public partial class BarcordScanPage : PhoneApplicationPage
    {
        private PhotoCamera camera;
       
        //private ItemDataBase itemDB=new ItemDataBase();//書籍情報が格納されているXMLを操作するための変数。
        private AmazonWebService aws;// = new AmazonWebService();

        public ViewModel BookView;
        App MyApp;


        StreamResourceInfo streamInfo;
        SoundEffect se;
        SoundEffectInstance soundInstance;

        DateTime NullDate;// = NullDate;//datetimeに値が入っていないことを意味するものとして使う。

        private const string ImageDIR = "Images";

        string TargetEAN;//念のためにリクエスト対象のEANを記憶しておく。ただし、リクエストは非同期のためずれるかもしれない。あくまで、レスポンスにEANが入らないときのため。

        //コンストラクタ
        public BarcordScanPage()
        {
            InitializeComponent();

            NullDate = new DateTime(0);//(NullDate).ToLocalTime();// (DateTime)IsolatedStorageSettings.ApplicationSettings["NullDate"];

            camera = null;

            MyApp = Application.Current as App;

            streamInfo = Application.GetResourceStream(new Uri("/BookMemory;component/Sounds/Default.wav", UriKind.Relative));
            se = SoundEffect.FromStream(streamInfo.Stream);
            soundInstance = se.CreateInstance();
        }


        // ページがアクティブになったら呼び出される
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            //カメラ関連の初期化
            camera = new PhotoCamera();
            camera.Initialized += camera_Initialized;//イベントの追加
            PreviewBrush.SetSource(camera);

            //ノートデータの読み込み
            //NoteView = DB.LoadInfoFromXML();
            BookView = MyApp.BookView;

        }

        // ページがアクティブでなくなったら呼び出される
        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            //カメラを解放
            if (camera != null)
            {
                camera.Dispose();
                camera = null;
            }


        }


        // カメラの初期化処理が完了したら呼び出される。カメラの各種設定
        void camera_Initialized(object sender, CameraOperationCompletedEventArgs e)
        {
            // 初期化処理に失敗した場合は何もしない。即return。
            if (!e.Succeeded)
                return;

            camera.FlashMode = FlashMode.Auto;//オートフォーカスを有効。
            

            // カメラの回転角度に合わせてプレビュー表示も回転させる。
            //コントロールへのアクセスを行うのでUIスレッドで非同期で実行。
            Dispatcher.BeginInvoke(() =>
            {
                PreviewBrush.RelativeTransform = new CompositeTransform()
                {
                    CenterX = 0.5,
                    CenterY = 0.5,
                    Rotation = camera.Orientation
                };
            });
        }


        #region From Read Barcode to Send Request

        //カメラのプレビュー画面がタップされたら呼び出される
        private void PreviewRectangle_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (camera == null)
            {
                return;
            }

            camera.AutoFocusCompleted += new EventHandler<CameraOperationCompletedEventArgs>(camera_AutoFocusCompleted);//オートフォーカスが完了したら呼び出されるイベントハンドラを登録。
            camera.Focus();//オートフォーカス開始。
        }



        //オートフォーカスが完了したら呼び出される
        void camera_AutoFocusCompleted(object sender, CameraOperationCompletedEventArgs e)
        {
            //アマゾンからデータを取得するインスタンスにイベントハンドラを追加
            //AmazonWebService aws = new AmazonWebService();
            //aws.Comp += new AmazonWebService.GetItemInfoFromAmazonCompletedEventHandler(this.aws_GetItemInfoFromAmazonCompleted);

            //ItemData itemData = new ItemData();

            //cameraインスタンスがnullの場合は即return。そうしないと下の「プレビューフレームの取得」でnullreferenceexceptionが出てしまう。readTimerとのタイミングの問題か？
            if (camera == null)
            {
                return;
            }

            // コントロールへのアクセスを行うのでUIスレッドにて非同期で実行。UIへのアクセスは別のスレッドからはできないため。
            Dispatcher.BeginInvoke(() =>
            {
                // プレビューフレームの取得
                var luminanceSource = new PreviewFrameLuminanceSource((int)camera.PreviewResolution.Width, (int)camera.PreviewResolution.Height);
                
                camera.GetPreviewBufferY(luminanceSource.Buffer);
                
                // リーダーインスタンス生成
                var reader = new EAN13Reader();

                // バーコード解析用のBitmapを作成
                var binarizer = new HybridBinarizer(luminanceSource);
                var binBitmap = new BinaryBitmap(binarizer);


                Result result = null;
                try
                {
                    // バーコードの解析(デコード)を行う。
                    //resultにEANが入る。
                    
                    result = reader.decode(binBitmap);

                }
                catch (ReaderException)
                {
                    // プレビューフレーム内にバーコードが見つからなかった場合
                    // 読み取り例外のReaderExceptionが発行されてしまう
                    TextBlock_Result.Text = "Can not read barcode.";
                    return;
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                
                //読み取ったEANがデータベースに登録済みかチェック
                if (BookView.Exist(result.Text)!=null)
                {
                    //登録済みの場合
                    TextBlock_Result.Text = result.Text + " is exists";
                }
                else
                {
                    //未登録の場合。Amazonへリクエスト
                    TargetEAN = result.Text;

                    aws = new AmazonWebService(result.Text);
                    //アマゾンからデータを取得するインスタンスにイベントハンドラを追加
                    aws.GetItemInfoFromAmazonCompleted += new AmazonWebService.GetItemInfoFromAmazonCompletedEventHandler(this.aws_GetItemInfoFromAmazonCompleted);
                    aws.GetItemInfo();
                }

            });

            //イベントを削除
            camera.AutoFocusCompleted -= camera_AutoFocusCompleted;
        }

        #endregion


        public void aws_GetItemInfoFromAmazonCompleted(object sender, GetItemInfoFromAmazonCompletedEventArgs e)
        {
            XElement xmlItemInfo = e.XmlItemInfo;
            XNamespace ns = xmlItemInfo.Name.Namespace;


            switch (e.Result)
            { 
                case "Completed":
                    AddBookInfoToBookView(e.XmlItemInfo);
                    System.Diagnostics.Debug.WriteLine("Completed: ");
                    // 分離ストレージから画像ファイルを読み込み。確認用。画面右下に表示するため。
                    //image1.Source = e.Item.GetItemImage();           
                    break;
                case "NoNetwork":
                    System.Diagnostics.Debug.WriteLine("No Network: "+xmlItemInfo.Element("EAN"));
                    AddBookInfoOnlyEAN(xmlItemInfo);
                    
                    //image1.Source = e.Item.GetItemImage();
                    break;
                case "NoItemFound":
                    System.Diagnostics.Debug.WriteLine("No Item : ");
                    AddBookInfoOnlyEAN(xmlItemInfo);
                    break;
                case "GotImage":
                    System.Diagnostics.Debug.WriteLine("GotImage: ");
                    break;
                case "HTMLLoginPage":
                    AddBookInfoOnlyEAN(xmlItemInfo);
                    break;
                default:
                    AddBookInfoOnlyEAN(xmlItemInfo);
                    break;
            }

           //itemDB.DBManipulationCompleted += new ItemDataBase.DBManipulationCompletedEventHandler(this.ItemDB_DBManipulationCompleted);
            //itemDB.AddByAuto(xmlItemInfo);
            //アマゾンからデータを取得するインスタンスにイベントハンドラを削除
            //aws.GetItemInfoFromAmazonCompleted -= aws_GetItemInfoFromAmazonCompleted;
            
        }

        //ネット接続がない状態や、amazonからのデータ取得に失敗したときの書籍登録メソッド
        //タイトル、EAN、作成日時のみ
        private void AddBookInfoOnlyEAN(XElement xmlItemInfo)
        {
            XNamespace ns = xmlItemInfo.Name.Namespace;

            BookModel newBook = new BookModel();


            newBook.EAN = xmlItemInfo.Element("EAN").Value;
            newBook.Title = newBook.EAN;
            newBook.CreateDate = DateTime.Now;

            newBook.OpenDetailDate = DateTime.Now;

            BookView.Books.Add(newBook);
            TextBlock_Result.Text = newBook.EAN;

            soundInstance.Play();
            var vc = VibrateController.Default;
            vc.Start(TimeSpan.FromMilliseconds(40));
        }



        private void AddBookInfoToBookView(XElement xmlItemInfo)
        {
            XNamespace ns = xmlItemInfo.Name.Namespace;
            BookModel NewBook = new BookModel();


            try
            {
                var authors = from author in xmlItemInfo.Element(ns + "ItemAttributes").Elements(ns + "Author") select author;

                foreach (var author in authors)
                {
                    if (NewBook.Authors == "")
                        NewBook.Authors = author.Value;
                    else
                        NewBook.Authors = NewBook.Authors + ", " + author.Value;
                }
            }
            catch
            {
                //NewBook.Authors = "";
            }

            try
            {
                NewBook.Title = xmlItemInfo.Element(ns + "ItemAttributes").Element(ns + "Title").Value;
            }
            catch
            {
                //NewBook.Title = "";
            }
            try
            {
                NewBook.EAN = xmlItemInfo.Element(ns + "ItemAttributes").Element(ns + "EAN").Value;
            }
            catch
            {
                //NewBook.EAN = "";
            }
            if (NewBook.EAN == "")
            {
                NewBook.EAN = TargetEAN;
            }
            (Application.Current as App).NewestBookID = TargetEAN;

            try
            {
                NewBook.Publisher = xmlItemInfo.Element(ns + "ItemAttributes").Element(ns + "Publisher").Value;
            }
            catch
            {
                //NewBook.Publisher = "";
            }


            try
            {
                NewBook.PublishDate = DateTime.FromFileTime((long)Convert.ToDouble(xmlItemInfo.Element(ns + "ItemAttributes").Element(ns + "PublicationDate").Value));
            }
            catch
            {
                //NewBook.PublishDate = NullDate;
            }
            if (NewBook.PublishDate.ToUniversalTime() == NullDate)
            {
                try
                {
                    NewBook.PublishDate = DateTime.FromFileTime((long)Convert.ToDouble(xmlItemInfo.Element(ns + "ItemAttributes").Element(ns + "ReleaseDate").Value));

                }
                catch
                {
                    //NewBook.PublishDate = NullDate;
                }
            }


            try
            {
                NewBook.Price = xmlItemInfo.Element(ns + "ItemAttributes").Element(ns + "ListPrice").Element(ns + "Amount").Value;
            }
            catch
            {
                //NewBook.Price = "";
            }

            try
            {
                NewBook.Currency = xmlItemInfo.Element(ns + "ItemAttributes").Element(ns + "ListPrice").Element(ns + "CurrencyCode").Value;
            }
            catch
            {
                //NewBook.Currency = "";
            }

            try
            {
                NewBook.Type = xmlItemInfo.Element(ns + "ItemAttributes").Element(ns + "Binding").Value;
            }
            catch
            {
                //NewBook.Type = "";
            }

            /*
            if (xmlItemInfo.Element(ns + "SmallImage").Element(ns + "URL") != null)
                SelectedBook.ImageWebUri = xmlItemInfo.Element(ns + "SmallImage").Element(ns + "URL").Value;
            else
                SelectedBook.ImageWebUri = "";
            */

            try
            {
                NewBook.ImageWebUri = xmlItemInfo.Element(ns + "LargeImage").Element(ns + "URL").Value;
            }
            catch
            {
                //NewBook.ImageWebUri = "";
            }

            try
            {
                NewBook.Language = xmlItemInfo.Element(ns + "ItemAttributes").Element(ns + "Languages").Element(ns + "Language").Element(ns + "Name").Value;
            }
            catch
            {
                //NewBook.Language = "";//xmlItemInfo.Element(ns + "ItemAttributes").Element(ns + "Languages").Element("Language").Element("Name").Value;
            }


            NewBook.Rate = 0;
            NewBook.Status = "Not yet";
            NewBook.CreateDate = DateTime.Now;
            NewBook.FinishReadDate = NullDate;
            NewBook.StartReadDate = NullDate;
            NewBook.PurchaseDate = NullDate;
            NewBook.Note = "";
            NewBook.ImageLocalUri = ImageDIR + "/" + NewBook.EAN;


            NewBook.OpenDetailDate = DateTime.Now;

            BookView.Books.Add(NewBook);
            TextBlock_Result.Text = NewBook.EAN;

            soundInstance.Play();
            var vc = VibrateController.Default;
            vc.Start(TimeSpan.FromMilliseconds(40));
        }

        /*
        //DBの操作が完了したときに発行されるイベントをつかんで実行されるイベントハンドラ
        private void ItemDB_DBManipulationCompleted(object sender, DBManipulationCompletedEventArgs e)
        {
            mediaElement1.Play();
        }
        */





    }
}