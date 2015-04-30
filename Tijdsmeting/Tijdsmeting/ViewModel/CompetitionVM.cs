using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Tijdsmeting.DAL;
using Tijdsmeting.Model;

namespace Tijdsmeting.ViewModel
{
    class CompetitionVM : ObservableObject, IPage
    {
        public string Name
        {
            get { return "Competition"; }
        }
        private const string IPADDRESS = "172.23.49.1";
        private const string LOGIN = "student";
        private const string PASSWORD = "niets";
        RunnerRepository rep = new RunnerRepository();
        public CompetitionVM()
        {
            CheckedRFID = "";
            getRunners();
            setTimer();
            IsFocussed = false;
            dispatcherTimerReadCode = new DispatcherTimer();
            dispatcherTimerReadCode.Interval = TimeSpan.FromMilliseconds(10);
            dispatcherTimerReadCode.Tick += dispatcherTimerReadCode_Tick;
        }

        void dispatcherTimerReadCode_Tick(object sender, EventArgs e)
        {
            if (CheckedRFID != "" && CheckedRFID.Count() == 8)
            {
                setCheckpoints();
            }
        }
        private BitmapImage _image;
        public BitmapImage Image
        {
            get { return _image; }
            set { _image = value; }
        }
        private bool _isFocussed;

        public bool IsFocussed
        {
            get { return _isFocussed; }
            set { _isFocussed = value; OnPropertyChanged("IsFocussed"); }
        }
        private string _checkedRFID;

        public string CheckedRFID
        {
            get { return _checkedRFID; }
            set
            {
                _checkedRFID = value; 
                OnPropertyChanged("CheckedRFID"); 
                //setCheckpoints(); 
            }
        }
        public DispatcherTimer dispatcherTimerReadCode { get; set; }

        public DispatcherTimer dispatcherTimerStopwatch { get; set; }

        private TimeSpan _Timer;
        public TimeSpan Timer
        {
            get { return _Timer; }
            set { _Timer = value; OnPropertyChanged("Timer"); }
        }

        private DateTime _startTime = DateTime.MinValue;

        private List<Runner> _runners;

        public List<Runner> Runners
        {
            get { return _runners; }
            set { _runners = value; OnPropertyChanged("Runners"); }
        }

        public void setCheckpoints()
        {
            string currentTime = Timer.ToString();
            if (CheckedRFID != "")
            {
                Runner checkedRunner = rep.GetRunnerByRFID(CheckedRFID);
                if (checkedRunner != null)
                {
                    if (checkedRunner.Checkpoint1 == null)
                    {
                        checkedRunner.Checkpoint1 = currentTime;
                    }
                    else if (checkedRunner.Checkpoint2 == null)
                    {
                        checkedRunner.Checkpoint2 = currentTime;
                    }
                    else
                    {
                        Console.WriteLine(checkedRunner.Name + " finish");
                        TakePicture(checkedRunner);
                        checkedRunner.Finish = currentTime;
                    }
                    rep.UpdateRunner(checkedRunner);
                    getRunners();
                    CheckedRFID = "";
                }
            }
        }

        private void TakePicture(Runner checkedRunner)
        {
            GetImage(checkedRunner);
        }

        private void getRunners()
        {
            Runners = rep.GetRunners();
        }

        private void Stop()
        {
            dispatcherTimerReadCode.Stop();
            dispatcherTimerStopwatch.Stop();
            ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
            appvm.ChangePage(new ResultVM());
        }
        public ICommand StopCommand
        {
            get { return new RelayCommand(Stop); }
        }
        
        #region Timer
        private void setTimer()
        {
            dispatcherTimerStopwatch = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimerStopwatch.Tick += dispatcherTimer_Tick;
            dispatcherTimerStopwatch.Interval = TimeSpan.FromSeconds(1);
        }

        void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            var timeSinceStartTime = DateTime.Now - _startTime;
            timeSinceStartTime = new TimeSpan(timeSinceStartTime.Hours,
                                              timeSinceStartTime.Minutes,
                                              timeSinceStartTime.Seconds);
            Timer = timeSinceStartTime;
            
        }
        public void StartTimer()
        {
            SetCamera();
            _startTime = DateTime.Now;
            dispatcherTimerReadCode.Start();
           dispatcherTimerStopwatch.Start();
           IsFocussed = true;
        }
        public ICommand StartCommand
        {
            get { return new RelayCommand(StartTimer); }
        }
#endregion
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

        private void GetImage(Runner checkedRunner)
        {
            Console.WriteLine("get image");

           //SetCamera();
            //string link = string.Format("https://{0}/axis-cgi/mjpg/video.cgi?nbrofframes=1", IPADDRESS);

            /*string link = string.Format("http://{0}/axis-cgi/jpg/image.cgi?resolution=320x240", IPADDRESS);
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(link);
            req.Timeout = Timeout.Infinite;
            req.KeepAlive = true;
            CredentialCache cc = new CredentialCache();
            cc.Add(
                new Uri(link),
                "Basic",
                new NetworkCredential(LOGIN, PASSWORD));
            req.Credentials = cc;

            req.Proxy = null;
           /* IWebProxy proxy = req.Proxy;
            if (req.Proxy != null)
            {
                Console.WriteLine("Removing proxy: {0}", proxy.GetProxy(req.RequestUri));
                IWebProxy proxyNew = new WebProxy("127.0.0.1",8888);
                req.Proxy = null;
            } 


            System.Net.ServicePointManager.Expect100Continue = false;
            HttpWebResponse res = null;

            try
            {
                res = (HttpWebResponse)req.GetResponse();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Niet gelukt");
                throw ex;
            }*/
            string link = string.Format("http://{0}/axis-cgi/jpg/image.cgi", IPADDRESS);
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(link);
            CredentialCache cc = new CredentialCache();
            cc.Add(
                new Uri(link),
                "Basic",
                new NetworkCredential(LOGIN, PASSWORD));
            req.Credentials = cc;
            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            using (BinaryReader reader = new BinaryReader(res.GetResponseStream()))
            {
                Byte[] lnByte = reader.ReadBytes(1 * 1024 * 1024 * 10);
                Image = ToImage(lnByte);

                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                string photoID = "Finish";
                String photolocation = photoID.ToString() + checkedRunner.Name + checkedRunner.Firstname + ".jpg";  //file name 

                encoder.Frames.Add(BitmapFrame.Create(Image));

                using (var filestream = new FileStream(photolocation, FileMode.Create))
                    encoder.Save(filestream);

                checkedRunner.Image = photolocation;
                rep.UpdateRunner(checkedRunner);

                Console.WriteLine("einde get image");
                reader.Read();
            }
            //res.GetResponseStream().EndRead(test);
            res.GetResponseStream().Close();
            res.Close();
            GC.Collect();
            
        }
        private void test(IAsyncResult e)
        { }

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
        #endregion
    }
}
