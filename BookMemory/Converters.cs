using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Windows.Resources;
using System.Windows.Media.Imaging;
using System.Diagnostics;
using System.Windows.Data;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;

namespace BookMemory
{
    public class DateTimeToShortStringConverter : IValueConverter
    {
        DateTime NullDate = new DateTime(0);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime dateTime;

// (DateTime)IsolatedStorageSettings.ApplicationSettings["NullDate"];

            try
            {
                dateTime = (DateTime)value;
            }
            catch
            {
                return "";
            }

            if (dateTime.ToUniversalTime() == NullDate)
                return "";
            else
                return dateTime.ToShortDateString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    //分離ストレージ上の画像URIからBitmap画像を読み込むコンバーター。リストボックスへのバインディングに使う。
    //MainPage.xamlから呼び出される。
    public class StringToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            IsolatedStorageFile isoFile = IsolatedStorageFile.GetUserStoreForApplication();
            var bi = new BitmapImage();
            var filePath = value as string;
            var filePath_small= filePath + "_small";

            /*
                if (isoFile.FileExists(filePath))
                {
                    // return null;
                    IsolatedStorageFileStream strm = new IsolatedStorageFileStream(filePath, FileMode.Open, FileAccess.Read, isoFile);
                    bi.SetSource(strm);
                    strm.Dispose();

                    var strm_small = new IsolatedStorageFileStream(filePath_small, FileMode.OpenOrCreate, FileAccess.Write, isoFile);
                    WriteableBitmap wb = new WriteableBitmap(bi);
                    wb.SaveJpeg(strm_small, (int)(wb.PixelWidth / 4), (int)(wb.PixelHeight / 4), 0, 100);

                    strm_small.Dispose();

                    isoFile.Dispose();
                }
                else
                {
                    return null;
                }
            */
            
            if (isoFile.FileExists(filePath_small))
            {
                IsolatedStorageFileStream strm = new IsolatedStorageFileStream(filePath_small, FileMode.Open, FileAccess.Read, isoFile);
                bi.SetSource(strm);

                strm.Dispose();


                isoFile.Dispose();
            }
            else
            {
                return null;
            }
           
                return bi;
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
