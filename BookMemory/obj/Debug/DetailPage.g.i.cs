﻿#pragma checksum "D:\Document\プログラミング\windows phone app\Projects\BookMemory_まとめフォルダ\BookMemory\BookMemory\DetailPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "CDFF94A61B29BEEB9AB4B9FF6364E08C"
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
    
    
    public partial class DetailPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal Microsoft.Phone.Controls.Panorama Panorama;
        
        internal System.Windows.Controls.Image Image_BookImage;
        
        internal System.Windows.Media.CompositeTransform transform;
        
        internal Microsoft.Phone.Controls.PhoneTextBox TextBox_Title;
        
        internal Microsoft.Phone.Controls.PhoneTextBox TextBox_EAN;
        
        internal Microsoft.Phone.Controls.PhoneTextBox TextBox_Authors;
        
        internal Microsoft.Phone.Controls.PhoneTextBox TextBox_Publisher;
        
        internal Microsoft.Phone.Controls.DatePicker DatePicker_Publish;
        
        internal Microsoft.Phone.Controls.PhoneTextBox TextBox_Price;
        
        internal Microsoft.Phone.Controls.PhoneTextBox TextBox_Currency;
        
        internal Microsoft.Phone.Controls.PhoneTextBox TextBox_Language;
        
        internal Microsoft.Phone.Controls.PhoneTextBox TextBox_Type;
        
        internal System.Windows.Controls.Grid Grid_Notice;
        
        internal System.Windows.Controls.TextBlock TextBlock_Notice;
        
        internal Microsoft.Phone.Controls.ListPicker ListPicker_Status;
        
        internal Microsoft.Phone.Controls.Rating Rating_Rate;
        
        internal Microsoft.Phone.Controls.DatePicker DatePicker_Purchase;
        
        internal Microsoft.Phone.Controls.DatePicker DatePicker_StartRead;
        
        internal Microsoft.Phone.Controls.DatePicker DatePicker_FinishRead;
        
        internal System.Windows.Controls.TextBlock TextBlock_CreateDate;
        
        internal System.Windows.Controls.TextBlock TextBlock_CreateDayOfWeek;
        
        internal System.Windows.Controls.ScrollViewer ScrollViewer_Note;
        
        internal Microsoft.Phone.Controls.PhoneTextBox TextBox_Note;
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton SaveButton;
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton DownloadButton;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/BookMemory;component/DetailPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.Panorama = ((Microsoft.Phone.Controls.Panorama)(this.FindName("Panorama")));
            this.Image_BookImage = ((System.Windows.Controls.Image)(this.FindName("Image_BookImage")));
            this.transform = ((System.Windows.Media.CompositeTransform)(this.FindName("transform")));
            this.TextBox_Title = ((Microsoft.Phone.Controls.PhoneTextBox)(this.FindName("TextBox_Title")));
            this.TextBox_EAN = ((Microsoft.Phone.Controls.PhoneTextBox)(this.FindName("TextBox_EAN")));
            this.TextBox_Authors = ((Microsoft.Phone.Controls.PhoneTextBox)(this.FindName("TextBox_Authors")));
            this.TextBox_Publisher = ((Microsoft.Phone.Controls.PhoneTextBox)(this.FindName("TextBox_Publisher")));
            this.DatePicker_Publish = ((Microsoft.Phone.Controls.DatePicker)(this.FindName("DatePicker_Publish")));
            this.TextBox_Price = ((Microsoft.Phone.Controls.PhoneTextBox)(this.FindName("TextBox_Price")));
            this.TextBox_Currency = ((Microsoft.Phone.Controls.PhoneTextBox)(this.FindName("TextBox_Currency")));
            this.TextBox_Language = ((Microsoft.Phone.Controls.PhoneTextBox)(this.FindName("TextBox_Language")));
            this.TextBox_Type = ((Microsoft.Phone.Controls.PhoneTextBox)(this.FindName("TextBox_Type")));
            this.Grid_Notice = ((System.Windows.Controls.Grid)(this.FindName("Grid_Notice")));
            this.TextBlock_Notice = ((System.Windows.Controls.TextBlock)(this.FindName("TextBlock_Notice")));
            this.ListPicker_Status = ((Microsoft.Phone.Controls.ListPicker)(this.FindName("ListPicker_Status")));
            this.Rating_Rate = ((Microsoft.Phone.Controls.Rating)(this.FindName("Rating_Rate")));
            this.DatePicker_Purchase = ((Microsoft.Phone.Controls.DatePicker)(this.FindName("DatePicker_Purchase")));
            this.DatePicker_StartRead = ((Microsoft.Phone.Controls.DatePicker)(this.FindName("DatePicker_StartRead")));
            this.DatePicker_FinishRead = ((Microsoft.Phone.Controls.DatePicker)(this.FindName("DatePicker_FinishRead")));
            this.TextBlock_CreateDate = ((System.Windows.Controls.TextBlock)(this.FindName("TextBlock_CreateDate")));
            this.TextBlock_CreateDayOfWeek = ((System.Windows.Controls.TextBlock)(this.FindName("TextBlock_CreateDayOfWeek")));
            this.ScrollViewer_Note = ((System.Windows.Controls.ScrollViewer)(this.FindName("ScrollViewer_Note")));
            this.TextBox_Note = ((Microsoft.Phone.Controls.PhoneTextBox)(this.FindName("TextBox_Note")));
            this.SaveButton = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("SaveButton")));
            this.DownloadButton = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("DownloadButton")));
        }
    }
}

