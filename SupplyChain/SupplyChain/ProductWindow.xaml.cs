using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SupplyChain.ViewModels;
using Microsoft.Win32;
using System.Data;
using System.IO;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.Collections.ObjectModel;
using System.Configuration;
using Dapper;
namespace SupplyChain
{
    /// <summary>
    /// Interaction logic for ProductWindow.xaml
    /// </summary>
    public partial class ProductWindow : Window
    {
        DataSet ds;
        string strName, imageName;
        string constr = @"Data Source=(local);Initial Catalog=ITS;Integrated Security=True";
        public ProductWindow()
        {
            InitializeComponent();
            SqlConnection con = new SqlConnection(constr);
            con.Open();

            string da = "Select imgPath from Product";
            SqlCommand command = new SqlCommand(da, con);
            SqlDataReader reader = command.ExecuteReader();
            string count;
            if (reader.Read())
            {
                count = Convert.ToString(reader[0]);
                string uRl = count;
                ImageSourceConverter imgs = new ImageSourceConverter();
                image1.SetValue(Image.SourceProperty, imgs.ConvertFromString(uRl));
            }
            reader.Close();
            
           

            DataContext = new ProductWindowViewModel();
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

                    using (SqlConnection conn = new SqlConnection(constr))
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
