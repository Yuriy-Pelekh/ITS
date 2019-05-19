using Microsoft.Win32;
using SupplyChain.ViewModels;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
namespace SupplyChain
{
    /// <summary>
    /// Interaction logic for ProductWindow.xaml
    /// </summary>
    public partial class ProductWindow : Window
    {
        DataSet ds;
        string strName, imageName;
        private readonly string _connectionString;
        public ProductWindow()
        {
            InitializeComponent();

            _connectionString = ConfigurationManager.ConnectionStrings["Main"].ConnectionString;

            SqlConnection con = new SqlConnection(_connectionString);
            con.Open();

            string da = "Select * from Product";
            SqlCommand command = new SqlCommand(da, con);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                byte[] data = (byte[])reader.GetValue(3);
                image1.Source = ByteToImage(data);

                /* ImageSourceConverter imgs = new ImageSourceConverter();
                 image1.SetValue(Image.SourceProperty, imgs.ConvertFromString(uRl));*/
            }
            reader.Close();
            DataContext = new ProductWindowViewModel();
        }

            public static ImageSource ByteToImage(byte[] imageData)
            {
                BitmapImage biImg = new BitmapImage();
                MemoryStream ms = new MemoryStream(imageData);
                biImg.BeginInit();
                biImg.StreamSource = ms;
                biImg.EndInit();

                ImageSource imgSrc = biImg as ImageSource;

                return imgSrc;
            }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (imageName != "")
                {
                    //Initialize a file stream to read the image file
                    FileStream fs = new FileStream(imageName, FileMode.Open, FileAccess.Read);

                    //Initialize a byte array with size of stream
                    byte[] imgByteArr = new byte[fs.Length];

                    //Read data from the file stream and put into the byte array
                    fs.Read(imgByteArr, 0, Convert.ToInt32(fs.Length));

                    //Close a file stream
                    fs.Close();

                    using (SqlConnection conn = new SqlConnection(_connectionString))
                    {
                        conn.Open();
                        string sql = "update Product set imgPath='" + imageName + "',Image=@img";
                        using (SqlCommand cmd = new SqlCommand(sql, conn))
                        {
                            //Pass byte array into database
                            cmd.Parameters.Add(new SqlParameter("img", imgByteArr));
                            int result = cmd.ExecuteNonQuery();
                            if (result == 1)
                            {
                                MessageBox.Show("Image added successfully.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnBrowse_Click(object sender, RoutedEventArgs e)
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
                    image1.SetValue(Image.SourceProperty, isc.ConvertFromString(imageName));
                }
                fldlg = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

    }
}
