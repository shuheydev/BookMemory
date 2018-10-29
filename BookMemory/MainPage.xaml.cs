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

namespace BookMemory
{
    public partial class MainPage : PhoneApplicationPage
    {
        App MyApp;

        // コンストラクター
        public MainPage()
        {
            InitializeComponent();

            MyApp = Application.Current as App;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {

            base.OnNavigatedFrom(e);
        }


        private void Grid_Tap_BookShelf(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/AllBookListPage.xaml", UriKind.Relative));  
        }

        private void Grid_Tap_ScanBook(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/BarcordScanPage.xaml", UriKind.Relative));  
        }

        private void Grid_Tap_AddNewBook(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/DetailPage.xaml?EditType=New", UriKind.Relative)); 
        }
    }
}