using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using Dapper;
using Microsoft.Win32;
using System.Windows.Media.Imaging;

namespace SupplyChain.ViewModels
{
    class ProductWindowViewModel : BaseViewModel
    {
        string strName, imageName;
        private string _connectionString;
        private ProductWindow mainWin = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is ProductWindow) as ProductWindow;
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
                foreach (var product in Products)
                {

                    byte[] data = product.Photo;
                    BitmapImage biImg = new BitmapImage();
                    MemoryStream ms = new MemoryStream(data);
                    biImg.BeginInit();
                    biImg.StreamSource = ms;
                    biImg.EndInit();
                    ImageSource imgSrc = biImg as ImageSource;
                    mainWin.image1.Source = imgSrc;

                }
            }

        }
        public ICommand SelectCommand => new CommandHandler(() =>
        {
            try
            {
                FileDialog fldlg = new OpenFileDialog();
                fldlg.InitialDirectory = Environment.SpecialFolder.MyPictures.ToString();
                fldlg.Filter = "Image File (*.jpg;*.bmp;*.gif)|*.jpg;*.bmp;*.gif";
                fldlg.ShowDialog();
                {
                    strName = fldlg.SafeFileName;
                    imageName = fldlg.FileName;
                    ImageSourceConverter isc = new ImageSourceConverter();
                    mainWin.image1.SetValue(Image.SourceProperty, isc.ConvertFromString(imageName));
                }
                fldlg = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        });

        public ICommand SaveCommand => new CommandHandler(() =>
        {
            try
            {
                if (imageName != "")
                {
                    FileStream fs = new FileStream(imageName, FileMode.Open, FileAccess.Read);

                    //Initialize a byte array with size of stream
                    byte[] imgByteArr = new byte[fs.Length];

                    //Read data from the file stream and put into the byte array
                    fs.Read(imgByteArr, 0, Convert.ToInt32(fs.Length));

                    //Close a file stream
                    fs.Close();
             
                    using (var connection = new SqlConnection(_connectionString))
                    {
                        connection.Open();
                        // 3. add new parameter to command object
                        var sql = @"UPDATE [Product] SET Name=@Name, Price=@Price, imgPath='" + imageName + "',Image=@Photo WHERE Id=@Id";
                        using (SqlCommand cmd = new SqlCommand(sql, connection))

                        {
                          
                            //Pass byte array into database
                            foreach (var product in Products)
                            {
                                product.Photo = imgByteArr;
                                int result = connection.Execute(sql, product);
                                if (result == 1)
                                {
                                    MessageBox.Show("Image added successfully.");
                                }
                            }

                        }



                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        });
    }
}
