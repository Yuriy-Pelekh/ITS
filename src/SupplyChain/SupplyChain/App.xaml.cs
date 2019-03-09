using System.Windows;
using System.Configuration;

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
            var database = new Database.Versioning.Database(connectionString);
            if (!database.Exists())
            {
                database.Create();
            }
            database.Update();
        }
    }
}
