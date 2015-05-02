using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Tijdsmeting.DAL;
using Tijdsmeting.Model;

namespace Tijdsmeting.ViewModel
{
    class RegistrationVM : ObservableObject, IPage
    {
        #region "Properties"
        public RunnerRepository Rep { get; set; }
        public bool GreenLight { get; set; }
        public string Name
        {
            get { return "Registration"; }
        }

        private bool _isRFIDFocused;

        public bool IsRFIDFocused
        {
            get { return _isRFIDFocused; }
            set { _isRFIDFocused = value; OnPropertyChanged("IsRFIDFocused"); }
        }

        private bool _isBarcodeFocused;

        public bool IsBarcodeFocused
        {
            get { return _isBarcodeFocused; }
            set { _isBarcodeFocused = value; OnPropertyChanged("IsBarcodeFocused"); }
        }

        private string _barcode;

        public string Barcode
        {
            get { return _barcode; }
            set
            {
                _barcode = value;
                OnPropertyChanged("Barcode");
                GreenLight = true;
                if (_barcode.Count() == 9)
                { 
                    IsRFIDFocused = true;
                    IsBarcodeFocused = false;
                }
                    
            }
        }

        private string _rfid;

        public string RFID
        {
            get { return _rfid; }
            set 
            { 
                _rfid = value; 
                OnPropertyChanged("RFID");
                GreenLight = true;
                if (_rfid.Count() == 8 && GreenLight == true)
                { 
                    IsBarcodeFocused = true;
                    IsRFIDFocused = false;
                    Save();
                }
                    
            }
        }

#endregion 
        public RegistrationVM()
        {
            Rep = new RunnerRepository();
            IsBarcodeFocused = true;
            IsRFIDFocused = false;
            GreenLight = true;
        }

        private void Save()
        {
            try
            {

                Console.WriteLine(RFID + " " + Barcode);
                Runner runner = new Runner();                
                runner = Rep.GetRunnerByBarcode(Barcode);
                if (runner.RFID != null)
                {
                    MessageBox.Show("U bent al geregistreerd!");
                }
                else
                {
                    runner.RFID = RFID;
                    Rep.UpdateRunner(runner);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Gelieve de waarden te controleren");
                GreenLight = false;
                return;
            }
            
            Barcode = "";
            RFID = "";
        }

        public ICommand SaveCommand
        {
            get { return new RelayCommand(Save); }
        }
    }
}
