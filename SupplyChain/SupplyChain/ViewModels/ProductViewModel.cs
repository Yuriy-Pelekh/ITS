using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupplyChain.ViewModels
{
    public class ProductViewModel : BaseViewModel
    {
       
        private string _name;
        private string _price;
        private int _id;

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

        public ProductViewModel()
        {


        }

        public ProductViewModel(Entities.Product product)
        {
            _id = product.Id;
            _name = product.Name;
            _price = product.Price;         
        }

    }
}
