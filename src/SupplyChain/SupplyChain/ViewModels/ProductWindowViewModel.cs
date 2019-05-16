using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Input;
using Dapper;

namespace SupplyChain.ViewModels
{
    class ProductWindowViewModel : BaseViewModel
    {
        private string _connectionString;

        public ObservableCollection<ProductViewModel> Products { get; set; }

        public ProductWindowViewModel()
        {

            _connectionString = ConfigurationManager.ConnectionStrings["Main"].ConnectionString;

            Products = new ObservableCollection<ProductViewModel>();

            using (var connection = new SqlConnection(_connectionString))
            {

                var products = connection.Query<Entities.Product>("SELECT * FROM [Product]");
                foreach (var product in products)
                {
                    Products.Add(new ProductViewModel(product));
                }
            }
        }

        public ICommand SaveCommand => new CommandHandler(() =>
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"UPDATE [Product] SET Name=@Name, Price=@Price,Image=@Image WHERE Id=@Id";

                foreach (var product in Products)
                {
                    connection.Execute(sql, product);
                }
            }
        });
    }
}
