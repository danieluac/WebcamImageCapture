
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using WinFormCharpWebCam;
using System.Data.SqlClient;

namespace CameraCapture
{
    public partial class Form1 : Form
    {
        WebCam webCam;
        string ConnectionString = "Data Source=DESKTOP-TSH8ILJ;Initial Catalog=bd_photo;Integrated Security=True;User ID=sagres_user;Password=sagres_user";
        SqlConnection connection;
        public Form1()
        {
            InitializeComponent();
            connection = new SqlConnection(this.ConnectionString);
        }
  
        private void btnStart_Click(object sender, EventArgs e)
        {
            webCam = new WebCam();
            webCam.InitializeWebCam(ref pictureBox1);
            webCam.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {          
            
            
        }

        private void btnCapture_Click(object sender, EventArgs e)
        {

            SaveImageToDB(pictureBox1.Image);
            webCam.Stop();
            ImageShow();

        }
        public void  SaveImageToDB(Image img)
        {
            try
            {
                connection.Open();
                string QueryString = $"INSERT INTO photos (titlte, photo) VALUES (@title, @photo)";
                SqlCommand command = new SqlCommand(QueryString, connection);
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@title", "Capture photografer");
                MemoryStream stream = new MemoryStream();
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] pic = stream.ToArray();

                command.Parameters.AddWithValue("@photo", pic);

                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (SqlException exception1)
            {               
                MessageBox.Show("data not saved");
            }
        }
        public void ImageShow()
        {
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand($"Select photo from photos order by id desc", connection);
                byte[] image = (byte[])cmd.ExecuteScalar();
                if (image != null)
                {
                    MemoryStream ms = new MemoryStream(image);
                    pictureBox2.Image = Image.FromStream(ms);
                }

                connection.Close();
            }
            catch (SqlException exception1)
            {
                MessageBox.Show("no row affected...");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

           
        }
    }
}
