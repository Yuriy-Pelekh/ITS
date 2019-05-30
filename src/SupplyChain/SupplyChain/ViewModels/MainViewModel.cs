
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
        private BackgroundWorker bw;
        private void ProgressStatus(object sender, SqlInfoMessageEventArgs e)

        {

            if (e.Errors.Count > 0)
            {

                string message = e.Errors[0].Message;

                int state = e.Errors[0].State;

                // Set status of the progress bar
                bw.ReportProgress(state);
            }

        }
        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (bw.CancellationPending)
            {
                e.Cancel = true;
                return;
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                // Вернуть результат


                var sql = @"UPDATE [User] SET FirstName=@FirstName, LastName=@LastName, UpdatedDate=GETUTCDATE() WHERE Id=" + SelectedUser.Id;

                string text1 = mainWin.textBox1.Text;
                string text2 = mainWin.textBox2.Text;
                UserViewModel user = new UserViewModel();
                user.FirstName = text1;
                user.LastName = text2;

                connection.InfoMessage += new SqlInfoMessageEventHandler(ProgressStatus);

                connection.Execute(sql, user);

            }
        }
        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            mainWin.progressBar.Value = e.ProgressPercentage;
        }
        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Manga Renamer", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Completed", "My Application", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            mainWin.savebtn.IsEnabled = true;
            mainWin.progressBar.Value = 0;


        }

        public MainViewModel()
        {

            bw = ((BackgroundWorker)mainWin.FindResource("backgroundWorker"));
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
            //this.bw.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
            //this.bw.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_ProgressChanged);
            //this.bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);

            bw.WorkerReportsProgress = true;
            bw.DoWork += (s, a) =>
            {
                if (bw.CancellationPending)
                {
                    a.Cancel = true;
                    return;
                }

                using (var connection = new SqlConnection(_connectionString))
                {


                    var sql = @"UPDATE
[User] SET FirstName=@FirstName, LastName=@LastName, UpdatedDate=GETUTCDATE() WHERE Id=" + SelectedUser.Id;

                    string text1 = mainWin.textBox1.Text;
                    string text2 = mainWin.textBox2.Text;
                    UserViewModel user = new UserViewModel();
                    user.FirstName = text1;
                    user.LastName = text2;

                    connection.InfoMessage += new SqlInfoMessageEventHandler(ProgressStatus);

                    connection.Execute(sql, user);

                }
            };
            bw.ProgressChanged += (s, a) =>
            {
                mainWin.progressBar.Value = a.ProgressPercentage;
            };
            bw.RunWorkerCompleted += (s, a) =>
            {
                if (a.Error != null)
                {
                    MessageBox.Show(a.Error.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Completed", "My Application", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                mainWin.savebtn.IsEnabled = true;
                mainWin.progressBar.Value = 0;

            };
            if (!this.bw.IsBusy)
            {
                bw.RunWorkerAsync();
                mainWin.savebtn.IsEnabled = false;

            }





        });





    }
}