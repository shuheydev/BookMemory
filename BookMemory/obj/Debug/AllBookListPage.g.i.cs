﻿#pragma checksum "D:\Document\プログラミング\windows phone app\Projects\BookMemory_まとめフォルダ\BookMemory\BookMemory\AllBookListPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "FF867C3C95D4C37438239C5B7ABCC4BA"
//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.34011
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace BookMemory {
    
    
    public partial class AllBookListPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Data.CollectionViewSource MyCollection;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal Microsoft.Phone.Controls.Pivot Pivot_ListPage;
        
        internal Microsoft.Phone.Controls.PivotItem PivotItem1;
        
        internal System.Windows.Controls.ListBox ListBox_Books;
        
        internal System.Windows.Controls.Grid Grid_SearchBox;
        
        internal Microsoft.Phone.Controls.PhoneTextBox TextBox_SearchBox;
        
        internal System.Windows.Controls.Grid Grid_SortOption;
        
        internal System.Windows.Controls.RadioButton RadioButton_SortOption_Title;
        
        internal System.Windows.Controls.RadioButton RadioButton_SortOption_Author;
        
        internal System.Windows.Controls.RadioButton RadioButton_SortOption_Publisher;
        
        internal System.Windows.Controls.RadioButton RadioButton_SortOption_PublishDate;
        
        internal System.Windows.Controls.RadioButton RadioButton_SortOption_CreateDate;
        
        internal System.Windows.Controls.RadioButton RadioButton_SortOption_ModifyDate;
        
        internal System.Windows.Controls.RadioButton RadioButton_SortOption_OpenDetailDate;
        
        internal System.Windows.Controls.RadioButton RadioButton_SortOption_Rating;
        
        internal System.Windows.Controls.Button Button_SortOption;
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton AddButton;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/BookMemory;component/AllBookListPage.xaml", System.UriKind.Relative));
            this.MyCollection = ((System.Windows.Data.CollectionViewSource)(this.FindName("MyCollection")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.Pivot_ListPage = ((Microsoft.Phone.Controls.Pivot)(this.FindName("Pivot_ListPage")));
            this.PivotItem1 = ((Microsoft.Phone.Controls.PivotItem)(this.FindName("PivotItem1")));
            this.ListBox_Books = ((System.Windows.Controls.ListBox)(this.FindName("ListBox_Books")));
            this.Grid_SearchBox = ((System.Windows.Controls.Grid)(this.FindName("Grid_SearchBox")));
            this.TextBox_SearchBox = ((Microsoft.Phone.Controls.PhoneTextBox)(this.FindName("TextBox_SearchBox")));
            this.Grid_SortOption = ((System.Windows.Controls.Grid)(this.FindName("Grid_SortOption")));
            this.RadioButton_SortOption_Title = ((System.Windows.Controls.RadioButton)(this.FindName("RadioButton_SortOption_Title")));
            this.RadioButton_SortOption_Author = ((System.Windows.Controls.RadioButton)(this.FindName("RadioButton_SortOption_Author")));
            this.RadioButton_SortOption_Publisher = ((System.Windows.Controls.RadioButton)(this.FindName("RadioButton_SortOption_Publisher")));
            this.RadioButton_SortOption_PublishDate = ((System.Windows.Controls.RadioButton)(this.FindName("RadioButton_SortOption_PublishDate")));
            this.RadioButton_SortOption_CreateDate = ((System.Windows.Controls.RadioButton)(this.FindName("RadioButton_SortOption_CreateDate")));
            this.RadioButton_SortOption_ModifyDate = ((System.Windows.Controls.RadioButton)(this.FindName("RadioButton_SortOption_ModifyDate")));
            this.RadioButton_SortOption_OpenDetailDate = ((System.Windows.Controls.RadioButton)(this.FindName("RadioButton_SortOption_OpenDetailDate")));
            this.RadioButton_SortOption_Rating = ((System.Windows.Controls.RadioButton)(this.FindName("RadioButton_SortOption_Rating")));
            this.Button_SortOption = ((System.Windows.Controls.Button)(this.FindName("Button_SortOption")));
            this.AddButton = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("AddButton")));
        }
    }
}
