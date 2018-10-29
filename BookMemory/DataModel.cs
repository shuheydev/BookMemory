using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Microsoft.Phone.Controls;
using System.Windows.Media;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.IO.IsolatedStorage;

namespace BookMemory
{
    public class ViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private ObservableCollection<BookModel> _books;
        public ObservableCollection<BookModel> Books
        {
            get { return _books; }
            set
            {
                if (_books != value)
                {
                    _books = value;
                    OnPropertyChanged("Books");
                }
            }
        }

        public ViewModel()
        {
            this.Books = new ObservableCollection<BookModel>();
        }

        public BookModel Exist(string targetEAN)
        {
            var books = from book in this.Books
                        where book.EAN == targetEAN
                        select book;

            if (books.Count() != 0)
                return books.First();
            else
                return null;

        }
    }


    public class BookModel : INotifyPropertyChanged
    {
        DateTime NullDate = new DateTime(0);

        public BookModel()
        {
            EAN = "";
            Title = "";
            Authors = "";
            Publisher = "";
            PublishDate = NullDate;
            Price = "";
            Currency = "";
            CreateDate = NullDate;
            StartReadDate = NullDate;
            FinishReadDate = NullDate;
            PurchaseDate = NullDate;
            Status = "Not yet";
            Rate = 0;
            ImageLocalUri = "";
            Note = "";
            this.Type = "";
            Language = "";
            OpenDetailDate = NullDate;
            ImageWebUri = "";
        }

        private string _ean;
        private string _title;
        private string _authors;
        private string _publisher;
        private DateTime _publishDate;
        private string _price;
        private string _currency;
        private DateTime _createDate;
        private DateTime _startReadDate;
        private DateTime _finishReadDate;
        private DateTime _purchaseDate;
        private string _status;//NotYet,Reading,Finished,WannaBuy
        private double _rate;
        private string _imageLocalUri;
        private string _note;
        private string _type;
        private string _language;
        private DateTime _openDetailDate;
        private string _imageWebUri;

        public string EAN
        {
            get
            {
                return _ean;
            }
            set
            {
                if (_ean != value)
                {
                    _ean = value;
                    OnPropertyChanged("EAN");
                }
            }
        }
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged("Title");
                }
            }
        }
        public string Authors
        {
            get
            {
                return _authors;
            }
            set
            {
                if (_authors != value)
                {
                    _authors = value;
                    OnPropertyChanged("Authors");
                }
            }
        }
        public string Publisher
        {
            get
            {
                return _publisher;
            }
            set
            {
                if (_publisher!= value)
                {
                    _publisher = value;
                    OnPropertyChanged("Publisher");
                }
            }
        }
        public DateTime PublishDate
        {
            get
            {
                return _publishDate;
            }
            set
            {
                if (_publishDate != value)
                {
                    _publishDate = value;
                    OnPropertyChanged("PublishedDate");
                }
            }
        }
        public string Price
        {
            get
            {
                return _price;
            }
            set
            {
                if (_price != value)
                {
                    _price = value;
                    OnPropertyChanged("Price");
                }
            }
        }
        public string Currency
        {
            get
            {
                return _currency;
            }
            set
            {
                if (_currency != value)
                {
                    _currency = value;
                    OnPropertyChanged("Currency");
                }
            }
        }
        public DateTime CreateDate
        {
            get
            {
                return _createDate;
            }
            set
            {
                if (_createDate != value)
                {
                    _createDate = value;
                    OnPropertyChanged("CreateDate");
                }
            }
        }
        public DateTime StartReadDate
        {
            get
            {
                return _startReadDate;
            }
            set
            {
                if (_startReadDate != value)
                {
                    _startReadDate = value;
                    OnPropertyChanged("StartReadDate");
                }
            }
        }
        public DateTime FinishReadDate
        {
            get
            {
                return _finishReadDate;
            }
            set
            {
                if (_finishReadDate != value)
                {
                    _finishReadDate = value;
                    OnPropertyChanged("FinishReadDate");
                }
            }
        }
        public DateTime PurchaseDate
        {
            get
            {
                return _purchaseDate;
            }
            set
            {
                if (_purchaseDate != value)
                {
                    _purchaseDate = value;
                    OnPropertyChanged("PurchaseDate");
                }
            }
        }
        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged("Status");
                }
            }
        }
        public double Rate
        {
            get
            {
                return _rate;
            }
            set
            {
                if (_rate != value)
                {
                    _rate = value;
                    OnPropertyChanged("Star");
                }
            }
        }
        public string ImageLocalUri
        {
            get
            {
                return _imageLocalUri;
            }
            set
            {
                if (_imageLocalUri != value)
                {
                    _imageLocalUri = value;
                    OnPropertyChanged("ImageLocalUri");
                }
            }
        }
        public string Note
        {
            get
            {
                return _note;
            }
            set
            {
                if (_note != value)
                {
                    _note = value;
                    OnPropertyChanged("Note");
                }
            }
        }
        public string Type
        {
            get
            {
                return _type;
            }
            set
            {
                if (_type != value)
                {
                    _type= value;
                    OnPropertyChanged("Type");
                }
            }
        }
        public string Language
        {
            get
            {
                return _language;
            }
            set
            {
                if (_language != value)
                {
                    _language = value;
                    OnPropertyChanged("Language");
                }
            }
        }
        public DateTime OpenDetailDate
        {
            get
            {
                return _openDetailDate;
            }
            set
            {
                if (_openDetailDate != value)
                {
                    _openDetailDate = value;
                    OnPropertyChanged("OpenDetailDate");
                }
            }
        }
        public string ImageWebUri
        {
            get
            {
                return _imageWebUri;
            }
            set
            {
                if (_imageWebUri != value)
                {
                    _imageWebUri = value;
                    OnPropertyChanged("ImageWebUri");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }


}
