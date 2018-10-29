using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Xml.Linq;
using System.Windows.Media.Imaging;
using System.IO.IsolatedStorage;
using System.IO;
using System.Windows.Input;

namespace BookMemory
{
    public partial class DetailPage : PhoneApplicationPage
    {
        App MyApp;
        ViewModel BookView;
        BookModel SelectedBook;

        string ImageDIR = "Images";

        DateTime NullDate;// = NullDate;//datetimeに値が入っていないことを意味するものとして使う。

        public DetailPage()
        {
            InitializeComponent();

            MyApp = Application.Current as App;
            NullDate =new DateTime(0);//.ToLocalTime();// ((DateTime)IsolatedStorageSettings.ApplicationSettings["NullDate"]).ToLocalTime();

            DatePicker_Publish.Visibility = Visibility.Collapsed;

            SystemTray.SetProgressIndicator(this, new ProgressIndicator() { IsIndeterminate = true });
        }

        //bool addSelectedBook=false;
        string EditType;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //各種UIの非表示
            Grid_Notice.Visibility = Visibility.Collapsed;


            //ノートデータの読み込み
            //NoteView = DB.LoadInfoFromXML();
            BookView = MyApp.BookView;

            EditType = NavigationContext.QueryString["EditType"];
            //= NavigationContext.QueryString["SelectedItemID"];//(string)IsolatedStorageSettings.ApplicationSettings["EditingType"];

            if (EditType == "New")
            {
                SelectedBook = new BookModel();
                SelectedBook.Title = "New Book";
                SelectedBook.CreateDate = DateTime.Now;

                SelectedBook.OpenDetailDate = DateTime.Now;
                initText("All");
            }


            string ean;
            if (EditType == "Modify")
            {
                ean = NavigationContext.QueryString["SelectedItemID"];

                SelectedBook = BookView.Exist(ean);

                if (SelectedBook == null)
                {
                    return;
                }

                if (dateTimeModifying == false)
                {
                    initText("All");
                    initImage();
                }
                else
                {
                    dateTimeModifying = false;
                }
            }

            SelectedBook.OpenDetailDate = DateTime.Now;

            using (IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (iso.FileExists(SelectedBook.ImageLocalUri) == false)
                {
                    Panorama.DefaultItem = Panorama.Items[1];
                }
                    

            }
            base.OnNavigatedTo(e);
        }



        private void initImage()
        {
            Image_BookImage.Source = GetImage("Images/" + SelectedBook.EAN);
        }
        // 分離ストレージの画像を読み込むメソッド
        public BitmapImage GetImage(string filename)
        {
            using (IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (iso.FileExists(filename) == false)
                    return null;
                using (IsolatedStorageFileStream stream = iso.OpenFile(filename, FileMode.Open))
                {
                    BitmapImage image = new BitmapImage();
                    image.SetSource(stream);
                    return image;
                }
            }
        }


        private void initText(string mode)
        {


            TextBox_Title.Text = SelectedBook.Title;
            TextBox_EAN.Text = SelectedBook.EAN;
            TextBox_Publisher.Text = SelectedBook.Publisher;

            if(SelectedBook.PublishDate.ToUniversalTime()!=NullDate)
            {
                DatePicker_Publish.Value = SelectedBook.PublishDate;
                DatePicker_Publish.Visibility = Visibility.Visible;
                (DatePicker_Publish.Parent as Grid).Children.ElementAt(2).Visibility = Visibility.Collapsed;
            }
            else
            {
                DatePicker_Publish.Visibility = Visibility.Collapsed;
                (DatePicker_Publish.Parent as Grid).Children.ElementAt(2).Visibility = Visibility.Visible;
            }

            TextBox_Price.Text = SelectedBook.Price.ToString();
            TextBox_Currency.Text = SelectedBook.Currency;
            TextBox_Authors.Text = SelectedBook.Authors;
            TextBox_Type.Text = SelectedBook.Type;
            TextBox_Language.Text = SelectedBook.Language;


            if (mode == "All")
            {

                if (SelectedBook.CreateDate.ToUniversalTime() != NullDate)
                {
                    TextBlock_CreateDate.Text = SelectedBook.CreateDate.ToString();
                    TextBlock_CreateDayOfWeek.Text = SelectedBook.CreateDate.DayOfWeek.ToString();
                }
                else
                {
                    TextBlock_CreateDate.Text = "No data";
                    TextBlock_CreateDayOfWeek.Text = "";
                }


                TextBox_Note.Text = SelectedBook.Note;

                switch (SelectedBook.Status)
                {
                    case "Not yet":
                        ListPicker_Status.SelectedIndex = 0;
                        break;
                    case "Reading":
                        ListPicker_Status.SelectedIndex = 1;
                        break;
                    case "Finished":
                        ListPicker_Status.SelectedIndex = 2;
                        break;
                    default:
                        ListPicker_Status.SelectedIndex = 0;
                        break;
                }

                Rating_Rate.Value = Convert.ToDouble(SelectedBook.Rate);

                //xmlからの読み込み時にparseで電話に設定されているタイムゾーンの時刻に変換されている。
                //これに気づき、NullDateLocalは世界標準時0を現在のタイムゾーンでの時刻に変換してある。

                if (SelectedBook.StartReadDate.ToUniversalTime() != NullDate)
                {
                    DatePicker_StartRead.Value = SelectedBook.StartReadDate;
                    DatePicker_StartRead.Visibility = Visibility.Visible;
                    (DatePicker_StartRead.Parent as Grid).Children.ElementAt(2).Visibility = Visibility.Collapsed;
                }
                else
                {
                    DatePicker_StartRead.Visibility = Visibility.Collapsed;
                    (DatePicker_StartRead.Parent as Grid).Children.ElementAt(2).Visibility = Visibility.Visible;
                }

                if (SelectedBook.FinishReadDate.ToUniversalTime() != NullDate)
                {
                    DatePicker_FinishRead.Value = SelectedBook.FinishReadDate;
                    DatePicker_FinishRead.Visibility = Visibility.Visible;
                    (DatePicker_FinishRead.Parent as Grid).Children.ElementAt(2).Visibility = Visibility.Collapsed;
                }
                else
                {
                    DatePicker_FinishRead.Visibility = Visibility.Collapsed;
                    (DatePicker_FinishRead.Parent as Grid).Children.ElementAt(2).Visibility = Visibility.Visible;
                }

                if (SelectedBook.PurchaseDate.ToUniversalTime() != NullDate)
                {
                    DatePicker_Purchase.Value = SelectedBook.PurchaseDate;
                    DatePicker_Purchase.Visibility = Visibility.Visible;
                    (DatePicker_Purchase.Parent as Grid).Children.ElementAt(2).Visibility = Visibility.Collapsed;
                }
                else
                {
                    DatePicker_Purchase.Visibility = Visibility.Collapsed;
                    (DatePicker_Purchase.Parent as Grid).Children.ElementAt(2).Visibility = Visibility.Visible;
                }
            }

        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {

            base.OnNavigatedFrom(e);
        }


        /*
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            (sender as TextBox).SelectAll();
        }
        */

        private void TextBox_Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            Panorama.Title = TextBox_Title.Text; 
            
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            SelectedBook.Title = TextBox_Title.Text;
            SelectedBook.EAN = TextBox_EAN.Text;
            SelectedBook.Publisher = TextBox_Publisher.Text;
            if (DatePicker_Publish.Visibility == Visibility.Visible)
                SelectedBook.PublishDate = (DateTime)DatePicker_Publish.Value;
            else
                SelectedBook.PublishDate = NullDate;
            SelectedBook.Price = TextBox_Price.Text;
            SelectedBook.Currency = TextBox_Currency.Text;
            SelectedBook.Authors = TextBox_Authors.Text;
            SelectedBook.Type = TextBox_Type.Text;
            //TextBox_Language.Text = book.Language;
            SelectedBook.Note = TextBox_Note.Text;
            SelectedBook.Language = TextBox_Language.Text;
            SelectedBook.Rate = Rating_Rate.Value;
            /*
            if (DatePicker_StartRead.Visibility == Visibility.Visible)
                SelectedBook.StartReadDate = (DateTime)DatePicker_StartRead.Value;
            else
                SelectedBook.StartReadDate = NullDate;
            if (DatePicker_FinishRead.Visibility == Visibility.Visible)
                SelectedBook.FinishReadDate = (DateTime)DatePicker_FinishRead.Value;
            else
                SelectedBook.FinishReadDate = NullDate;
            if (DatePicker_Purchase.Visibility == Visibility.Visible)
                SelectedBook.PurchaseDate = (DateTime)DatePicker_Purchase.Value;
            else
                SelectedBook.PurchaseDate = NullDate;
            */
            SelectedBook.Status = (ListPicker_Status.SelectedItem as ListPickerItem).Content.ToString();

            if (EditType == "New")
            {
                BookView.Books.Add(SelectedBook);
                //addSelectedBook = false;
            }

            NavigationService.GoBack();
        }





        //自作イベントのイベントハンドラ。
        //getItemInfoFromAmazonToXmlDBが完了したときにイベントが発行される。
        //成功のときも失敗のときも。
        public void aws_GetItemInfoFromAmazonCompleted(object sender, GetItemInfoFromAmazonCompletedEventArgs e)
        {
            //string Namespace = "http://webservices.amazon.com/AWSECommerceService/2011-08-01";
            //XNamespace ns = aws.NameSpace;
            XElement xmlItemInfo = e.XmlItemInfo;
            XNamespace ns = xmlItemInfo.Name.Namespace;

            Grid_Notice.Visibility = Visibility.Visible;
            SystemTray.GetProgressIndicator(this).IsVisible = false;
            switch (e.Result)
            {
                case "Completed":
                    modifyBookModelByDownloadedData(e.XmlItemInfo);
                    System.Diagnostics.Debug.WriteLine("Completed: ");
                    TextBlock_Notice.Text = "Download Completed.";
                    //textBlock2.Text = e.Item.Title;
                    // 分離ストレージから画像ファイルを読み込み。確認用。画面右下に表示するため。
                    //image1.Source = e.Item.GetItemImage();
                    break;
                case "NoNetwork":
                    System.Diagnostics.Debug.WriteLine("No Network: ");
                    //image1.Source = e.Item.GetItemImage();
                    TextBlock_Notice.Text = "No network.";
                    break;
                case "NoItemFound":
                    System.Diagnostics.Debug.WriteLine("No Item : ");
                    TextBlock_Notice.Text = "Item not found.";
                    break;
                case "GotImage":
                    System.Diagnostics.Debug.WriteLine("Got Image Successfully: ");
                    initImage();
                    break;
                case "HTMLLoginPage":
                    TextBlock_Notice.Text = "No network.";
                    break;
                default:
                    TextBlock_Notice.Text = "Failure.";
                    break;

            }

            //イベントハンドラの削除
            //aws.GetItemInfoFromAmazonCompleted -= aws_GetItemInfoFromAmazonCompleted;

        }

        private void modifyBookModelByDownloadedData(XElement xmlItemInfo)
        {
            XNamespace ns = xmlItemInfo.Name.Namespace;

            SelectedBook.Authors = "";
            try
            {
                var authors = from author in xmlItemInfo.Element(ns + "ItemAttributes").Elements(ns + "Author") select author;

                foreach (var author in authors)
                {
                    if (SelectedBook.Authors == "")
                        SelectedBook.Authors = author.Value;
                    else
                        SelectedBook.Authors = SelectedBook.Authors + ", " + author.Value;
                }
            }
            catch
            {
                //SelectedBook.Authors = "";
            }

            try
            {
                SelectedBook.Title = xmlItemInfo.Element(ns + "ItemAttributes").Element(ns + "Title").Value;
            }
            catch
            {
                //SelectedBook.Title = "";
            }


            try
            {
                //SelectedBook.EAN = xmlItemInfo.Element(ns + "ItemAttributes").Element(ns + "EAN").Value;
                SelectedBook.EAN = TextBox_EAN.Text;
            }
            catch
            {
                //SelectedBook.EAN = "";
            }
            

            try
            {
                SelectedBook.Publisher = xmlItemInfo.Element(ns + "ItemAttributes").Element(ns + "Publisher").Value;
            }
            catch
            {
                //SelectedBook.Publisher = "";
            }

            
            try
            {
                SelectedBook.PublishDate = DateTime.Parse( xmlItemInfo.Element(ns + "ItemAttributes").Element(ns + "PublicationDate").Value);
            }
            catch
            {
                //SelectedBook.PublishDate = NullDate;
            }
            if (SelectedBook.PublishDate.ToUniversalTime() == NullDate)
            {
                try
                {
                    SelectedBook.PublishDate = DateTime.Parse(xmlItemInfo.Element(ns + "ItemAttributes").Element(ns + "ReleaseDate").Value);

                }
                catch
                {
                    //SelectedBook.PublishDate = NullDate;
                }
            }


            try
            {
                SelectedBook.Price = xmlItemInfo.Element(ns + "ItemAttributes").Element(ns + "ListPrice").Element(ns + "Amount").Value;
            }
            catch
            {
                //SelectedBook.Price = "";
            }

            try
            {
                SelectedBook.Currency = xmlItemInfo.Element(ns + "ItemAttributes").Element(ns + "ListPrice").Element(ns + "CurrencyCode").Value;
            }
            catch
            {
                //SelectedBook.Currency = "";
            }

            try
            {
                SelectedBook.Type = xmlItemInfo.Element(ns + "ItemAttributes").Element(ns + "Binding").Value;
            }
            catch
            {
                //SelectedBook.Type = "";
            }

            /*
            if (xmlItemInfo.Element(ns + "SmallImage").Element(ns + "URL") != null)
                SelectedBook.ImageWebUri = xmlItemInfo.Element(ns + "SmallImage").Element(ns + "URL").Value;
            else
                SelectedBook.ImageWebUri = "";
            */

            try
            {
                SelectedBook.ImageWebUri = xmlItemInfo.Element(ns + "LargeImage").Element(ns + "URL").Value;
            }
            catch
            {
                //SelectedBook.ImageWebUri = "";
            }

            try
            {
                SelectedBook.Language = xmlItemInfo.Element(ns + "ItemAttributes").Element(ns + "Languages").Element(ns+"Language").Element(ns+"Name").Value;
            }
            catch
            {
                //SelectedBook.Language = "";//xmlItemInfo.Element(ns + "ItemAttributes").Element(ns + "Languages").Element("Language").Element("Name").Value;
            }

            SelectedBook.ImageLocalUri = ImageDIR + "/" + SelectedBook.EAN;

            initText("Download");
        }



    

        bool dateTimeModifying = false;
        private void DatePicker_ManipulationStarted(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            dateTimeModifying = true;
        }


        private void TextBlock_Set_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var tb = sender as TextBlock;
            var dp = (tb.Parent as Grid).Children.ElementAt(1) as DatePicker;

            tb.Visibility = Visibility.Collapsed;

            dp.Value = DateTime.Now;
            dp.Visibility = Visibility.Visible;

        }

        private void MenuItem_Clear_Click(object sender, RoutedEventArgs e)
        {
            var grid = ((sender as MenuItem).Parent as ContextMenu).Owner as Grid;// as DatePicker;
            var dp = grid.Children.ElementAt(1) as DatePicker;
            var tb = grid.Children.ElementAt(2) as TextBlock;
            //System.Diagnostics.Debug.WriteLine(dp.Owner);

            dp.Value = NullDate;
            dp.Visibility = Visibility.Collapsed;
            tb.Visibility = Visibility.Visible;
        }


        private AmazonWebService aws;
        private void DownloadButton_Click(object sender, EventArgs e)
        {
            SystemTray.GetProgressIndicator(this).IsVisible = true;

            var result = MessageBox.Show("Download this book's info again?", "Download", MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
            {
                /*
                var ean = TextBox_EAN.Text;
                
                string newEAN = "";
                foreach (char c in ean)
                {
                    if (0 <= c.CompareTo('0') && c.CompareTo('9') <= 0)
                    {
                        newEAN += c;
                    }
                }

                TextBox_EAN.Text = newEAN;
                */
                

                aws = new AmazonWebService(TextBox_EAN.Text);
                aws.GetItemInfoFromAmazonCompleted += new AmazonWebService.GetItemInfoFromAmazonCompletedEventHandler(this.aws_GetItemInfoFromAmazonCompleted);

                aws.GetItemInfo();
            }
        }


        private void Grid_Nortice_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Grid_Notice.Visibility = Visibility.Collapsed;
        }


        //テキストボックスの長文入力時に、上から下まですべての内容をスクロールで見れるようにするため。
        private void TextBox_Note_TextInputStart(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            this.ScrollViewer_Note.UpdateLayout();
            this.ScrollViewer_Note.ScrollToVerticalOffset(this.TextBox_Note.ActualHeight);
        }


        //テキストボックスの最下部で改行したときにカーソルが見切れないようにするため。
        private void TextBox_Note_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
            {
                //e.Handled = true;
                this.ScrollViewer_Note.UpdateLayout();
                this.ScrollViewer_Note.ScrollToVerticalOffset(this.TextBox_Note.ActualHeight);
            }
        }


        private void TextBox_DoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            (sender as PhoneTextBox).SelectAll();

        }


        // double initialAngle;
        double initialScale;

        /*ダブルタップすると、画像を初期位置初期サイズに戻します。*/
        private void OnDoubleTap(object sender, Microsoft.Phone.Controls.GestureEventArgs e)
        {
            transform.ScaleX = transform.ScaleY = 1;
            transform.TranslateX = transform.TranslateY = 0;
        }

        private void OnHold(object sender, Microsoft.Phone.Controls.GestureEventArgs e)
        {
            transform.TranslateX = transform.TranslateY = 0;
            transform.ScaleX = transform.ScaleY = 1;
            transform.Rotation = 0;
        }

        /*ドラッグで画像を移動させています。*/
        private void OnDragDelta(object sender, DragDeltaGestureEventArgs e)
        {
            transform.TranslateX += e.HorizontalChange;
            transform.TranslateY += e.VerticalChange;
        }

        /*ピンチアウトする前にサイズを保存しておきます、コメントアウトを外すと回転もできます。*/
        private void OnPinchStarted(object sender, PinchStartedGestureEventArgs e)
        {
            //initialAngle = transform.Rotation;
            initialScale = transform.ScaleX;
        }

        /*ピンチアウト中ですね、倍率を見て拡大しています。コメントアウトを外すと回転もできます。*/
        private void OnPinchDelta(object sender, PinchGestureEventArgs e)
        {
            //transform.Rotation = initialAngle + e.TotalAngleDelta;
            transform.ScaleX = transform.ScaleY = initialScale * e.DistanceRatio;
        }

        /*
        private void TextBox_EAN_LostFocus(object sender, RoutedEventArgs e)
        {
           var ean = (sender as TextBox).Text;

            string newEAN="";
            foreach (char c in ean)
            {
                if (0 <= c.CompareTo('0') && c.CompareTo('9') <= 0)
                {
                    newEAN += c;
                }
            }

            TextBox_EAN.Text = newEAN;

        }
         */

        //評価をリセットする。
        private void MenuItem_Rating_Clear_Click(object sender, RoutedEventArgs e)
        {
            //            var grid = ((sender as MenuItem).Parent as ContextMenu).Owner as Grid;// as DatePicker;
            var rating = ((sender as MenuItem).Parent as ContextMenu).Owner as Rating;
            System.Diagnostics.Debug.WriteLine(rating);
            rating.Value = 0;
        }

        private void TextBox_EAN_LostFocus(object sender, RoutedEventArgs e)
        {

        }

    }
}