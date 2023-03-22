using BigioHrServices.Db.Entities;
using BigioHrServices.Db;
using BigioHrServices.Model.Datatable;
using BigioHrServices.Model.Employee;

namespace BigioHrServices.Services
{
    public class LogActivityServices
    {
        public interface ILogActivityServices
        {
            public void LogActivityAdd(DateTime date, string modul, string activity);
        }
        public class LogActivityService : ILogActivityServices
        {
            private readonly ApplicationDbContext _db;
            public LogActivityService(ApplicationDbContext db)
            {
                _db = db;
            }

            public void LogActivityAdd(DateTime date, string modul, string activity)
            {
                try
                {
                    _db.LogActivities.Add(new LogActivity
                    {
                        Date = date,
                        Modul = modul,
                        Activity = activity
                    }); ;
                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
}
