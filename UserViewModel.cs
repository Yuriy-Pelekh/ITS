using System;

namespace SupplyChain.ViewModels
{
    public class UserViewModel : BaseViewModel
    {

        private int _id;
        private string _firstName;
        private string _lastName;
        private DateTime _updatedDate;
        private string _phoneNumber;
        private string _email;
        private DateTime _createdDate;



        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set
            {
                if (_phoneNumber != value)
                {
                    _phoneNumber = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string Email
        {
            get { return _email; }
            set
            {
                if (_email != value)
                {
                    _email = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public DateTime CreatedDate
        {
            get { return _createdDate; }
            set
            {
                if (_createdDate != value)
                {
                    _createdDate = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public int Id
        {
            get { return _id; }
        }
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                if (_firstName != value)
                {
                    _firstName = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string LastName
        {
            get { return _lastName; }
            set
            {
                if (_lastName != value)
                {
                    _lastName = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public DateTime UpdatedDate
        {
            get { return _updatedDate; }
            set
            {
                if (_updatedDate != value)
                {
                    _updatedDate = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public UserViewModel()
        {

        }
        public UserViewModel(Entities.User user)
        {
            _id = user.Id;
            _firstName = user.FirstName;
            _lastName = user.LastName;
            _updatedDate = user.UpdatedDate;
        }

    }

}
