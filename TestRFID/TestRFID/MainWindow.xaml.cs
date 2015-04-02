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
using System.Threading;
using System.Windows.Threading;

namespace TestRFID
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    { 
        public MainWindow()
        {
            InitializeComponent();
            txbBarcode.Focus();           
        }

        private const string IPADDRESS = "172.23.49.1";
        private const string LOGIN = "student";
        private const string PASSWORD = "niets";
        DispatcherTimer timer = new DispatcherTimer();
        Dictionary<string, string> Runners = new Dictionary<string, string>();
        private BitmapImage _image;
        public BitmapImage Image
        {
            get { return _image; }
            set { _image = value; }
        }

        private void Save()
        {
            string barcode = txbBarcode.Text;
            string rfid = txbRFID.Text;
            try
            {
                Runners.Add(rfid,barcode);
            }
            catch (Exception)
            {
                MessageBox.Show("Gelieve de waarden te controleren");
            }
           
            txbBarcode.Text = "";
            txbRFID.Text = "";
            txbBarcode.Focus();
        }

        private void EndRegistration()
        {
            foreach (KeyValuePair<string, string> Runner in Runners)
            {
                lstListRunners.Items.Add(Runner.Value);
            }
            //Runners.Clear();
        }

        #region Camera
        private void SetCamera()
        {
            string link = string.Format("http://{0}/axis-cgi/com/ptz.cgi?pan=80&tilt=-10&zoom=20", IPADDRESS);
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


                        JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                        string photoID = "Finish";
                        String photolocation = photoID.ToString() + ".jpg";  //file name 

                        encoder.Frames.Add(BitmapFrame.Create(Image));

                        using (var filestream = new FileStream(photolocation, FileMode.Create))
                            encoder.Save(filestream);
                    }
                }));
        }

        void Image_DownloadCompleted(object sender, EventArgs e)
        {
            Console.WriteLine("download completed");
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


        //void objImage_DownloadCompleted(object sender, EventArgs e)
        //{
        //    JpegBitmapEncoder encoder = new JpegBitmapEncoder();
        //    Guid photoID = System.Guid.NewGuid();
        //    String photolocation = photoID.ToString() + ".jpg";  //file name 

        //    encoder.Frames.Add(BitmapFrame.Create((BitmapImage)sender));

        //    using (var filestream = new FileStream(photolocation, FileMode.Create))
        //        encoder.Save(filestream);
        //}

        #endregion

        #region Button Click
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            GetImage();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            SetCamera();
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            EndRegistration();
        }
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            string link = string.Format("http://{0}/axis-cgi/jpg/image.cgi", IPADDRESS);
            //SaveImage(link);
        }
        #endregion

        private void txbBarcode_TextChanged(object sender, TextChangedEventArgs e)
        {
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (txbBarcode.Text != null && txbBarcode.Text != "")
            {
                txbRFID.Focus();
            }
            timer.Stop();
        }

       

       

       
    }
}
