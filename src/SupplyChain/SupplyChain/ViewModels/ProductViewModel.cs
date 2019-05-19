namespace SupplyChain.ViewModels
{
    public class ProductViewModel : BaseViewModel
    {
        private string _name;
        private string _price;
        private int _id;
        private byte[] _image;
        private string _imgPath;

        public int Id
        {
            get { return _id; }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Price
        {
            get { return _price; }
            set
            {
                if (_price != value)
                {
                    _price = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string imgPath
        {
            get { return _imgPath; }
            set
            {
                if (_imgPath != value)
                {
                    _imgPath = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public byte[] Image
        {
            get { return _image; }
            set
            {
                if (_image != value)
                {
                    _image = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public ProductViewModel()
        {

        }

        public ProductViewModel(Entities.Product product)
        {
            _id = product.Id;
            _name = product.Name;
            _price = product.Price;
            _image = product.Image;
            _imgPath = product.imgPath;
        }
    }
}
