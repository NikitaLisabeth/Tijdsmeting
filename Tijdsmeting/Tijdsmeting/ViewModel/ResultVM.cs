using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tijdsmeting.DAL;
using Tijdsmeting.Model;

namespace Tijdsmeting.ViewModel
{
    class ResultVM : ObservableObject, IPage
    {
        public string Name
        {
            get { return "Result"; }
        }
        RunnerRepository rep = new RunnerRepository();
        public ResultVM()
        {
            getRunners();

        }
        private List<Runner> _runners;

        public List<Runner> Runners
        {
            get { return _runners; }
            set { _runners = value; OnPropertyChanged("Runners"); }
        }

        private void getRunners()
        {
            Runners = rep.GetRunners();
        }

    }
}
