using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tijdsmeting.Model;

namespace Tijdsmeting.DAL
{
    class RunnerRepository
    {
        public List<Runner> GetRunners()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var query = (from r in context.Runners select r);

                return query.ToList<Runner>();
            }
        }

        public Runner GetRunnerByBarcode(string barcode)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var query = (from r in context.Runners where r.Barcode.Equals(barcode) select r);

                return query.SingleOrDefault<Runner>();
            }
        }

        public Runner GetRunnerByRFID(string rfid)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var query = (from r in context.Runners where r.RFID.Equals(rfid) select r);

                return query.SingleOrDefault<Runner>();
            }
        }

        public void UpdateRunner(Runner runner)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                Runner rnr = (from r in context.Runners where r.Id == runner.Id select r).SingleOrDefault<Runner>();

                rnr.Name = runner.Name;
                rnr.Firstname = runner.Firstname;
                rnr.Barcode = runner.Barcode;
                rnr.RFID = runner.RFID;
                rnr.Image = runner.Image;
                rnr.Checkpoint1 = runner.Checkpoint1;
                rnr.Checkpoint2 = runner.Checkpoint2;
                rnr.Finish = runner.Finish;

                context.SaveChanges();
            }
        }
        
    }
}
