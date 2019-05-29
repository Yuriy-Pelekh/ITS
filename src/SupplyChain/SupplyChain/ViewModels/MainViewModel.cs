using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Input;
using Dapper;
using System.Windows.Controls;
using System.Windows;
using System.Linq;
using System.ComponentModel;

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

        public ICommand SaveCommand => new CommandHandler(() =>
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"UPDATE [User] SET FirstName=@FirstName, LastName=@LastName, UpdatedDate=GETUTCDATE() WHERE Id=@Id";

                foreach (var user in Users)
                {
                    connection.Execute(sql, user);
                }
            }
        });

        public ICommand AddCommand => new CommandHandler(() =>
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"INSERT INTO [User] ([FirstName], [LastName], [UpdatedDate]) VALUES (@FirstName, @LastName,GETUTCDATE())";

                string text1 = mainWin.textBox1.Text;
                string text2 = mainWin.textBox2.Text;
                UserViewModel user = new UserViewModel();
                user.FirstName = text1;
                user.LastName = text2;

                connection.Execute(sql, user);
            }
        });

        public ICommand RemoveCommand => new CommandHandler(() =>
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"DELETE FROM [User] WHERE Id =" + SelectedUser.Id;

                string text1 = mainWin.textBox1.Text;
                string text2 = mainWin.textBox2.Text;
                UserViewModel user = new UserViewModel();
                user.FirstName = text1;
                user.LastName = text2;

                connection.Execute(sql, user);
            }
        });
    }
}