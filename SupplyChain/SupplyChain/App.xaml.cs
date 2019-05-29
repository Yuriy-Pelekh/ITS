using System.Windows;
using System.Configuration;
using Database.Versioning;

namespace SupplyChain
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void OnStartup(object sender, StartupEventArgs e)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["Main"].ConnectionString;
            var database = new DatabaseManager(connectionString);
            if (!database.Exists())
            {
                database.Create();
            }
            database.Update();
        }
    }
}
