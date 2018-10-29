using System;
using System.IO.IsolatedStorage;
using System.Xml.Linq;
using System.IO;
using System.Linq;
using System.Windows.Media;
using System.Windows;

namespace BookMemory
{
    public class DataBase
    {

        private const string DBFileName = "BookInfo.xml";
        private const string ImageDIR = "Images";

        private DateTime NullDate;

        public DataBase()
        {
            InitDB();

            NullDate = new DateTime(0);(NullDate).ToLocalTime();// (DateTime)IsolatedStorageSettings.ApplicationSettings["NullDate"];
        }

        //分離ストレージのDBファイルの初期化
        private void InitDB()
        {
            //分離ストレージの初期化。ファイルが存在しなければ、新しく作る。
            IsolatedStorageFile isoFile = IsolatedStorageFile.GetUserStoreForApplication();
            if (!isoFile.FileExists(DBFileName))
            {
                //xmlドキュメントの初期化
                XDocument XmlDoc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), new XElement("Books"));


                //xmlドキュメントをファイルへ書き込む
                IsolatedStorageFileStream strm = new IsolatedStorageFileStream(DBFileName, FileMode.Create, FileAccess.ReadWrite, isoFile);
                XmlDoc.Save(strm);
                strm.Dispose();//ストリームを閉じる

            }


            //書籍画像保存用ディレクトリの作成
            if (!isoFile.DirectoryExists(ImageDIR))//ディレクトリがない場合
            {
                isoFile.CreateDirectory(ImageDIR);//ディレクトリ作成

                /*
                //画像が取得できなかったとき用の画像をimages/0として保存
                StreamResourceInfo source = Application.GetResourceStream(new Uri("Images/appbar.questionmark.rest.png", UriKind.Relative));
                BitmapImage bi = new BitmapImage();
                bi.SetSource(source.Stream);

                IsolatedStorageFileStream strm = new IsolatedStorageFileStream(ImageDIR + "/0", FileMode.Create, FileAccess.ReadWrite, isoFile);
                WriteableBitmap wb = new WriteableBitmap(bi);
                wb.SaveJpeg(strm, wb.PixelWidth, wb.PixelHeight, 0, 100);

                strm.Dispose();
                 */

            }

            isoFile.Dispose();



        }


        public ViewModel LoadInfoFromXML()
        {
            //目的地データの読み込み。
            IsolatedStorageFile isoFile = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream strm = new IsolatedStorageFileStream(DBFileName, FileMode.Open, FileAccess.Read, isoFile);
            XDocument xmlDoc = XDocument.Load(strm);
            strm.Dispose();
            isoFile.Dispose();

            var books = from book in xmlDoc.Descendants("Book")
                        orderby book.Element("CreateDate").Value descending
                        select book;

            ViewModel BookView = new ViewModel();
            BookModel newBook;//=new PushPinModel();
            foreach (var book in books)
            {
                newBook = new BookModel();

                newBook.EAN = book.Element("EAN").Value;
                newBook.Title = book.Element("Title").Value;
                newBook.Authors = book.Element("Authors").Value;
                newBook.Publisher = book.Element("Publisher").Value;
                newBook.Price = book.Element("Price").Value;
                newBook.Currency = book.Element("Currency").Value;
                try
                {
                    newBook.CreateDate = DateTime.Parse(book.Element("CreateDate").Value);
                }
                catch
                {
                    try
                    {
                        newBook.CreateDate = DateTime.FromFileTime((long)Convert.ToDouble(book.Element("CreateDate").Value));
                    }
                    catch
                    {
                        newBook.CreateDate = NullDate;
                    }
                }

                try
                {
                    newBook.StartReadDate = DateTime.Parse(book.Element("StartReadDate").Value);
                }
                catch
                {
                    try
                    {
                        newBook.StartReadDate = DateTime.FromFileTime((long)Convert.ToDouble(book.Element("StartReadDate").Value));
                    }
                    catch
                    {
                        newBook.StartReadDate = NullDate;
                    }
                }

                try
                {
                    newBook.FinishReadDate = DateTime.Parse(book.Element("FinishReadDate").Value);
                }
                catch
                {

                    try
                    {
                        newBook.FinishReadDate = DateTime.FromFileTime((long)Convert.ToDouble(book.Element("FinishDate").Value));
                    }
                    catch
                    {
                        newBook.FinishReadDate = NullDate;
                    }
                }

                try
                {
                    newBook.PurchaseDate = DateTime.Parse(book.Element("PurchaseDate").Value);
                }
                catch
                {
                    try
                    {
                        newBook.PurchaseDate = DateTime.FromFileTime((long)Convert.ToDouble(book.Element("PurchaseDate").Value));
                    }
                    catch
                    {
                        newBook.PurchaseDate = NullDate;
                    }
                }

                try
                {
                    newBook.PublishDate = DateTime.Parse(book.Element("PublishDate").Value);

                }
                catch
                {
                    try
                    {
                        newBook.PublishDate = DateTime.FromFileTime((long)Convert.ToDouble(book.Element("PublishDate").Value));

                    }
                    catch
                    {
                        newBook.PublishDate = NullDate;
                    }
                }

                try
                {
                    newBook.OpenDetailDate = DateTime.Parse(book.Element("OpenDetailDate").Value);
                }
                catch
                {
                    try
                    {
                        newBook.OpenDetailDate = DateTime.FromFileTime((long)Convert.ToDouble(book.Element("OpenDetailDate").Value));
                    }
                    catch
                    {
                        newBook.OpenDetailDate = NullDate;
                    }
                }

                newBook.Note = book.Element("Note").Value;
                newBook.Rate = Convert.ToDouble( book.Element("Star").Value);
                newBook.Status = book.Element("Status").Value;
                newBook.ImageLocalUri = book.Element("ImageLocalUri").Value;
                newBook.Type = book.Element("Type").Value;
                newBook.Language = book.Element("Language").Value;
                newBook.ImageWebUri = book.Element("ImageWebUri").Value;


                BookView.Books.Add(newBook);
            }


            return BookView;
        }


        public void SaveInfoToIsoStrage(ViewModel BookView)
        {
            //xmlドキュメントの初期化
            XDocument xmlDoc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), new XElement("Books"));

            //コレクションのアイテムをひとつづつXMLに入れていく
            foreach (var book in BookView.Books)
            {
                XElement bookElement = new XElement("Book");

                bookElement.Add(new XElement("EAN", book.EAN));
                bookElement.Add(new XElement("Title", book.Title));
                bookElement.Add(new XElement("Authors", book.Authors));
                bookElement.Add(new XElement("Publisher", book.Publisher));
                bookElement.Add(new XElement("Currency", book.Currency));
                bookElement.Add(new XElement("Price", book.Price.ToString()));
                bookElement.Add(new XElement("CreateDate", book.CreateDate.ToUniversalTime()));
                bookElement.Add(new XElement("PublishDate", book.PublishDate.ToUniversalTime()));
                bookElement.Add(new XElement("FinishReadDate", book.FinishReadDate.ToUniversalTime()));
                bookElement.Add(new XElement("StartReadDate", book.StartReadDate.ToUniversalTime()));
                bookElement.Add(new XElement("PurchaseDate", book.PurchaseDate.ToUniversalTime()));
                bookElement.Add(new XElement("Status", book.Status));
                bookElement.Add(new XElement("Star", book.Rate.ToString()));
                bookElement.Add(new XElement("ImageLocalUri", book.ImageLocalUri));
                bookElement.Add(new XElement("Note", book.Note));
                bookElement.Add(new XElement("Type", book.Type));
                bookElement.Add(new XElement("Language", book.Language));
                bookElement.Add(new XElement("OpenDetailDate", book.OpenDetailDate.ToUniversalTime()));
                bookElement.Add(new XElement("ImageWebUri", book.ImageWebUri));

                xmlDoc.Root.Add(bookElement);
            }


            //XMLファイルを分離ストレージに保存。

            //ロードとセーブでそれぞれストリームを開いて閉じること。でなければ正しく保存できない。
            IsolatedStorageFile isoFile = IsolatedStorageFile.GetUserStoreForApplication();

            //xmlファイルの更新。FileMode.Createとすると、ファイルが存在する場合、同名の（空の）ファイルを新規に作り直してくれる。
            //ここでもしFileMode.Openにすると、下の書き込みのときに、
            //元の内容を一文字ずつ上書きしていく形でファイルの先頭から文字列（XML）
            //が書き込まれていくようで、
            //要素が削除されて短くなっているので、
            //その後ろに以前の文がはみだしてくっついてしまう。
            //そのため、次に読み込むときにエラーが出てしまう。
            //ほんと、要注意。
            //なんでこんな仕様なの？XMLのときだけ？
            //やってることはファイルを消してまた書き込んでいるだけなので、
            //ファイルが大きくなった場合が心配。
            //データ操作のオーバーヘッドはどうなるんだろう？
            //
            IsolatedStorageFileStream strm = new IsolatedStorageFileStream(DBFileName, FileMode.Create, FileAccess.Write, isoFile);
            //xmlDoc.Save(strm);
            xmlDoc.Save(strm);
            strm.Dispose();//セーブしたら閉じる
            isoFile.Dispose();
        }


    }
}
