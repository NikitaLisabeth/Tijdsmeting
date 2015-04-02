using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestRFID
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string IPADDRESS = "172.23.49.1";
        private const string LOGIN = "student";
        private const string PASSWORD = "niets";
        private BitmapImage _image;

        public BitmapImage Image
        {
            get { return _image; }
            set { _image = value; }
        }
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string code = txbCode.Text;
            byte[] asciiBytes = Encoding.ASCII.GetBytes(code);
            foreach(byte b in asciiBytes){
                txbAscii.Text = txbAscii.Text +" "+ b.ToString();
            }
            
        }
        private void SetCamera()
        {
            string link = string.Format("http://{0}/axis-cgi/com/ptz.cgi?pan=0&tilt=0", IPADDRESS);
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(link);
            CredentialCache cc = new CredentialCache();
            cc.Add(
                new Uri(link),
                "Basic",
                new NetworkCredential(LOGIN, PASSWORD));
            req.Credentials = cc;
            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
        }
        private void GetImage()
        {
            //while (true)
           // {

                string link = string.Format("http://{0}/axis-cgi/jpg/image.cgi", IPADDRESS);
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(link);
                CredentialCache cc = new CredentialCache();
                cc.Add(
                    new Uri(link),
                    "Basic",
                    new NetworkCredential(LOGIN, PASSWORD));
                req.Credentials = cc;
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                this.Dispatcher.Invoke((Action)(() =>
                {
                    using (BinaryReader reader = new BinaryReader(res.GetResponseStream()))
                    {
                        Byte[] lnByte = reader.ReadBytes(1 * 1024 * 1024 * 10);
                        Image = ToImage(lnByte);
                        imgScreen.Source = Image;
                    }
                }));
           // }

            /*    try
                {
                    if (imgScreen != null && txbAscii.Text!=null)
                    {
                        imgScreen.Save("c:\\myBitmap.bmp");
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("There was a problem saving the file." +
                        "Check the file permissions.");
                }*/
        }
        private BitmapImage ToImage(byte[] array)
        {
            using (var ms = new System.IO.MemoryStream(array))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad; // here
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            GetImage();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            SetCamera();
        }

    }
}
