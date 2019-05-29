using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Input;
using Dapper;
using System.Windows.Controls;
using System.Windows;
using System.Linq;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace SupplyChain.ViewModels
{
    public class MainViewModel : BaseViewModel
    {

        private string _connectionString;
        private MainWindow mainWin = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow) as MainWindow;
        public ObservableCollection<UserViewModel> Users { get; set; }
        public UserViewModel SelectedUser { get; set; }
        

        public MainViewModel()
        {

            _connectionString = ConfigurationManager.ConnectionStrings["Main"].ConnectionString;
            Users = new ObservableCollection<UserViewModel>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var users = connection.Query<Entities.User>("SELECT * FROM [User]");
                foreach (var user in users)
                {
                    Users.Add(new UserViewModel(user));
                }
            }
            
        }
        private bool Validate_email(string str) {
            string pattern = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
           bool isEmail = Regex.IsMatch(str, pattern, RegexOptions.IgnoreCase);

            return isEmail;
        }
        private bool Validate_phoneNumber(string str)
        {
            string pattern = @"(([+]\d{1,3}) ([0-9 ]{0,14}))";

               bool isPhone = Regex.IsMatch(str, pattern);
            return isPhone;
        }
        private bool Validate_name(string str)
        {
            string pattern = @"^[a-zA-Z]+$";

            bool isPhone = Regex.IsMatch(str, pattern);
            return isPhone;
        }

        public bool Validate(string email, string phone, string lastname,string firstname) {

            bool isEmail = Validate_email(email);
            bool isPhone = Validate_phoneNumber(phone);
            bool islName = Validate_name(lastname);
            bool isfName = Validate_name(firstname);



            return isEmail&&isPhone&& isfName && islName;

        }
        

        public ICommand SaveCommand => new CommandHandler(() =>
        {
           

            using (var connection = new SqlConnection(_connectionString))
            {


                var sql = @"UPDATE [User] SET FirstName=@FirstName, LastName=@LastName,Email=@Email,Phone=@PhoneNumber, UpdatedDate=GETUTCDATE() WHERE Id=" + SelectedUser.Id;

                string text_firstName = mainWin.textBox_firstName.Text;
                string text_lastName = mainWin.textBox_lastName.Text;
                string text_phoneNumber = mainWin.textBox_phoneNumber.Text;
                string text_email = mainWin.textBox_email.Text;
            
                UserViewModel user = new UserViewModel();
                user.FirstName = text_firstName;
                user.LastName = text_lastName;
                user.Email = text_email;
                user.PhoneNumber = text_phoneNumber;
               

                connection.Execute(sql, user);

            }
        });

        public ICommand AddCommand => new CommandHandler(() =>
        {

            using (var connection = new SqlConnection(_connectionString))
            {


                var sql = @"INSERT INTO [User] ([FirstName], [LastName], [UpdatedDate],[CreatedDate],[Email],[PhoneNumber])
                                VALUES (@FirstName, @LastName,GETUTCDATE(),GETUTCDATE(),@Email,@PhoneNumber)";

                string text_firstName = mainWin.textBox_firstName.Text;
                string text_lastName = mainWin.textBox_lastName.Text;
                string text_phoneNumber = mainWin.textBox_phoneNumber.Text;
                string text_email = mainWin.textBox_email.Text;

                UserViewModel user = new UserViewModel();              
                user.FirstName = text_firstName;
                user.LastName = text_lastName;
                user.Email = text_email;
                user.PhoneNumber = text_phoneNumber;


                connection.Execute(sql, user);




            }
        });




    }
}