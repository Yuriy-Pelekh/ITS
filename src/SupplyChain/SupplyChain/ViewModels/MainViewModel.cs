using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Input;
using Dapper;

namespace SupplyChain.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string _connectionString;

        public ObservableCollection<UserViewModel> Users { get; set; }

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
    }
}
