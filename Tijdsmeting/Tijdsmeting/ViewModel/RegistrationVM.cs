using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Tijdsmeting.ViewModel
{
    class RegistrationVM : ObservableObject, IPage
    {
        public string Name
        {
            get { return "Registration"; }
        }
        public RegistrationVM()
        {

            Runners = new Dictionary<string, string>();
            IsUserNameFocused = false;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
        }
        public DispatcherTimer timer { get; set; }
        private bool _isUserNameFocused;

        public bool IsUserNameFocused
        {
            get { return _isUserNameFocused; }
            set { _isUserNameFocused = value; OnPropertyChanged("IsUserNameFocused"); }
        }
        
        private string _barcode;

        public string Barcode
        {
            get { return _barcode; }
            set { _barcode = value; 
                StartTimer();  
                OnPropertyChanged("Barcode"); }
        }

        private string _rfid;

        public string RFID
        {
            get { return _rfid; }
            set { _rfid = value; OnPropertyChanged("RFID"); }
        }

        public Dictionary<string,string> Runners { get; set; }
        private void Save()
        {
            try
            {
                Console.WriteLine(RFID + " " + Barcode);
                Runners.Add(RFID, Barcode);
            }
            catch (Exception)
            {
                MessageBox.Show("Gelieve de waarden te controleren");
            }
            Barcode = "";
            RFID = "";
        }

        private void StartTimer()
        {
            if (timer.IsEnabled == false)
            {
                timer.Start();
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (Barcode != null && Barcode != "")
            {
                IsUserNameFocused = true;
            }
            timer.Stop();
        }
        public ICommand SaveCommand
        {
            get { return new RelayCommand(Save); }
        }
    }
}
