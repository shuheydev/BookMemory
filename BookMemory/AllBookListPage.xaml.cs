using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Windows.Navigation;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Globalization;


namespace BookMemory
{
    public partial class AllBookListPage : PhoneApplicationPage
    {
        App MyApp;
        ViewModel BookView;

        string SortBy="Title";
        string SearchKeyword = "";

        string ImageDIR = "Images";

        public AllBookListPage()
        {
            InitializeComponent();

            MyApp = Application.Current as App;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SortBy = (string)IsolatedStorageSettings.ApplicationSettings["Sort"];
            SearchKeyword = (string)IsolatedStorageSettings.ApplicationSettings["SearchKeyword"];

            //ノートデータの読み込み
            //NoteView = DB.LoadInfoFromXML();
            BookView = MyApp.BookView;

            //リストの読み込みと表示
            MyCollection.Source = BookView.Books;//
            MyCollection.View.Filter = null;
            /*
            MyCollection.View.SortDescriptions.Clear();
            MyCollection.View.SortDescriptions.Add(new SortDescription("OpenDetailDate", ListSortDirection.Descending));
            */
            initList();

            TextBox_SearchBox.Text = SearchKeyword;

            base.OnNavigatedTo(e);
        }

        private void initList()
        {
            initSortOption();
            filteringByKeyword();
            SortCollection();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            IsolatedStorageSettings.ApplicationSettings["Sort"] = SortBy;
            IsolatedStorageSettings.ApplicationSettings["SearchKeyword"] = SearchKeyword;
            base.OnNavigatedFrom(e);
        }



        string selectedItemID="";
        private void ListBox_Books_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine(sender.ToString());
            var selectedItem = ListBox_Books.SelectedItem as BookModel;
            //NoteView.SetSelected(selectedItem);
            if (selectedItem != null)
            {
                //選択中のファイルを識別するために、ノートの作成時刻を使う。
                IsolatedStorageSettings.ApplicationSettings["SelectedItemID"] = selectedItem.EAN;
                selectedItemID = selectedItem.EAN;
            }
        }

        private void StackPanel_DoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine((sender as StackPanel).ToString());
            var selectedItem = (sender as StackPanel).DataContext as BookModel;

            (Application.Current as App).NewestBookID = selectedItem.EAN;

            //選択中のファイルを識別するために、ノートの作成時刻を使う。
            IsolatedStorageSettings.ApplicationSettings["SelectedItemID"] = selectedItem.EAN;
            selectedItemID = selectedItem.EAN;

            NavigationService.Navigate(new Uri("/DetailPage.xaml?EditType=Modify&SelectedItemID="+selectedItemID, UriKind.Relative));
        }

        private void MenuItem_Delete_Click(object sender, RoutedEventArgs e)
        {
            var holdedItem = (sender as MenuItem).DataContext as BookModel;

            var result = MessageBox.Show("Are you sure to delete ''" + holdedItem.Title + "'' ?", "Delete", MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
            {
                BookView.Books.Remove(holdedItem);

                var imageFilePath=ImageDIR+"/"+ holdedItem.EAN;
                var imageFilePath_small = imageFilePath + "_small";

                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    store.DeleteFile(imageFilePath);
                    store.DeleteFile(imageFilePath_small);
                }
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/DetailPage.xaml?EditType=New", UriKind.Relative));
        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            initSortOption();

            var pivot = sender as Pivot;

            switch (pivot.SelectedIndex)
            {
                case 0:
                    ApplicationBar.IsVisible = true;
                    break;
                case 1:
                    ApplicationBar.IsVisible = false;
                    break;
            }
        }
        private void initSortOption()
        {
            //ラジオボタンにチェックを入れるだけ
            switch (SortBy)
            {
                case "Title":
                    {
                        RadioButton_SortOption_Title.IsChecked = true;
                        break;
                    }
                case "Author":
                    {
                        RadioButton_SortOption_Author.IsChecked = true;
                        break;
                    }
                case "Publisher":
                    {
                        RadioButton_SortOption_Publisher.IsChecked = true;
                        break;
                    }
                case "Created Date":
                    {
                        RadioButton_SortOption_CreateDate.IsChecked = true;
                        break;
                    }
                case "Modified Date":
                    {
                        RadioButton_SortOption_ModifyDate.IsChecked = true;
                        break;
                    }
                case "Recent Opened":
                    {
                        RadioButton_SortOption_OpenDetailDate.IsChecked = true;
                        break;
                    }
                case "Publication Date":
                    {
                        RadioButton_SortOption_PublishDate.IsChecked = true;
                        break;
                    }
                case "Rating":
                    {
                        RadioButton_SortOption_Rating.IsChecked = true;
                        break;
                    }
            }
        }

        private void TextBox_Search_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
            {
                e.Handled = true;
                this.Focus();

                SearchKeyword = TextBox_SearchBox.Text;

                filteringByKeyword();

                Pivot_ListPage.SelectedItem = PivotItem1;
            }
        }

        private void filteringByKeyword()
        {
            CompareInfo compareInfo = System.Globalization.CultureInfo.CurrentCulture.CompareInfo;


            if (SearchKeyword != "")
            {
                MyCollection.View.Filter = delegate(object o)
                {
                    BookModel book = (o as BookModel);

                    string compareString = book.Title + book.Authors + book.Publisher + book.Note + book.EAN;

                    if (compareInfo.IndexOf(compareString, SearchKeyword, 0, CompareOptions.IgnoreCase) != -1)
                        return true;
                    else if (compareInfo.IndexOf(compareString, SearchKeyword, 0, CompareOptions.IgnoreKanaType) != -1)
                        return true;
                    return false;
                };

                //SortedFlag = true;
                //Button_ResetSearch.Visibility = Visibility.Visible;
            }
            else
            {
                MyCollection.View.Filter = null;
                //SortedFlag = false;
                //Button_ResetSearch.Visibility = Visibility.Collapsed;
            }
        }


        private void TextBox_DoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            (sender as TextBox).SelectAll();
        }

        string tempCheckedSortOption = "";
        private void RadioButton_SortOption_Checked(object sender, RoutedEventArgs e)
        {
            var checkedItem = sender as RadioButton;

            tempCheckedSortOption = checkedItem.Content.ToString();
        }

        private void Button_SortOption_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            SortBy = tempCheckedSortOption;
            SortCollection();
            Pivot_ListPage.SelectedItem = PivotItem1;
        }
        private void SortCollection()
        {
            MyCollection.View.SortDescriptions.Clear();

            switch (SortBy)
            {
                case "Title":
                    {
                        MyCollection.View.SortDescriptions.Add(new SortDescription("Title", ListSortDirection.Ascending));

                        break;
                    }
                case "Author":
                    {
                        MyCollection.View.SortDescriptions.Add(new SortDescription("Authors", ListSortDirection.Ascending));

                        break;
                    }
                case "Publisher":
                    {
                        MyCollection.View.SortDescriptions.Add(new SortDescription("Publisher", ListSortDirection.Ascending));

                        break;
                    }
                case "Created Date":
                    {
                        MyCollection.View.SortDescriptions.Add(new SortDescription("CreateDate", ListSortDirection.Descending));
                        break;
                    }
                case "Modified Date":
                    {
                        MyCollection.View.SortDescriptions.Add(new SortDescription("ModifyDate", ListSortDirection.Descending));
                        break;
                    }
                case "Recent Opened":
                    {
                        MyCollection.View.SortDescriptions.Add(new SortDescription("OpenDetailDate", ListSortDirection.Descending));
                        break;
                    }
                case "Publication Date":
                    {
                        MyCollection.View.SortDescriptions.Add(new SortDescription("PublishDate", ListSortDirection.Descending));
                        break;
                    }
                case "Rating":
                    {
                        MyCollection.View.SortDescriptions.Add(new SortDescription("Rate", ListSortDirection.Descending));

                        break;
                    }
            }
        }
    }
}