using BigioHrServices.Db;
using BigioHrServices.Db.Entities;
using BigioHrServices.Model;
using BigioHrServices.Model.Datatable;
using BigioHrServices.Model.Employee;
using BigioHrServices.Model.Notification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Drawing.Printing;
using System.Globalization;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BigioHrServices.Services
{
    public interface INotificationService
    {
        public DatatableResponse GetListNotification(NotificationGetRequest request);
        public DatatableResponse GetListNotificationByEmployeeId(NotificationGetRequest request, string nik);
        public void UpdateStatusNotification(int id);
        public NotificationResponse GetDetailNotification(int id);
    }
    public class NotificationServices : INotificationService
    {
        private readonly ApplicationDbContext _db;
        public NotificationServices(ApplicationDbContext db)
        {
            _db = db;
        }

        public DatatableResponse GetListNotification(NotificationGetRequest request)
        {            
            var query = _db.Notifications
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrEmpty(request.Search))
            {
                query = query.Where(p => p.Title.ToLower() == request.Search.ToLower() || p.Body.ToLower() == request.Search.ToLower());
            }
            if (!string.IsNullOrEmpty(request.StartDate))
            {
                DateTime startDate = DateTime.ParseExact(request.StartDate, "yyyy-MM-dd HH:mm:sszzz", CultureInfo.InvariantCulture);
                NpgsqlTypes.NpgsqlDateTime startDateTimestampz = new NpgsqlTypes.NpgsqlDateTime(startDate);
                query = query.Where(p => NpgsqlTypes.NpgsqlDateTime.ToNpgsqlDateTime(p.CreatedDate.Value) > startDateTimestampz);
            }

            if (!string.IsNullOrEmpty(request.EndDate))
            {
                DateTime endDate = DateTime.ParseExact(request.EndDate, "yyyy-MM-dd HH:mm:sszzz", CultureInfo.InvariantCulture);
                NpgsqlTypes.NpgsqlDateTime endDateTimestampz = new NpgsqlTypes.NpgsqlDateTime(endDate);
                query = query.Where(p => NpgsqlTypes.NpgsqlDateTime.ToNpgsqlDateTime(p.CreatedDate.Value) < endDateTimestampz);
            }

            var data = query
                .Select(_notification => new NotificationResponse
                {
                    Id = _notification.Id,
                    Title = _notification.Title,
                    Body = _notification.Body,
                    IsRead = _notification.IsRead,
                    ReadDate = _notification.ReadDate,
                    CreatedDate = _notification.CreatedDate
                })
                .OrderByDescending(_notification => _notification.CreatedDate)
                .ToList();

            return new DatatableResponse()
            {
                Data = data.Skip(request.Page * request.PageSize)
                    .Take(request.PageSize)
                    .ToList(),
                TotalRecords = data.Count,
                PageSize = request.PageSize > data.Count ? data.Count : request.PageSize,
                NextPage = (request.PageSize * request.Page) < data.Count,
                PrevPage = request.Page > 1,
            };
        }

        public DatatableResponse GetListNotificationByEmployeeId(NotificationGetRequest request, string nik)
        {
            var query = _db.Notifications
                .Where(p => p.Nik == nik)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrEmpty(request.Search))
            {
                query = query.Where(p => p.Title.ToLower() == request.Search.ToLower() || p.Body.ToLower() == request.Search.ToLower());
            }
            if (!string.IsNullOrEmpty(request.StartDate))
            {
                query = query.Where(p => p.CreatedDate > DateTime.ParseExact(request.StartDate, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture));
            }

            if (!string.IsNullOrEmpty(request.EndDate))
            {
                query = query.Where(p => p.CreatedDate < DateTime.ParseExact(request.EndDate, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture));
            }

            var data = query
                .Select(_notification => new NotificationResponse
                {
                    Id = _notification.Id,
                    Title = _notification.Title,
                    Body = _notification.Body,
                    IsRead = _notification.IsRead,
                    ReadDate = _notification.ReadDate,
                    CreatedDate = _notification.CreatedDate
                })
                .OrderByDescending(_notification => _notification.CreatedDate)
                .ToList();

            return new DatatableResponse()
            {
                Data = data.Skip(request.Page * request.PageSize)
                    .Take(request.PageSize)
                    .ToList(),
                TotalRecords = data.Count,
                PageSize = request.PageSize > data.Count ? data.Count : request.PageSize,
                NextPage = (request.PageSize * request.Page) < data.Count,
                PrevPage = request.Page > 1,
            };
        }

        public void UpdateStatusNotification(int id)
        {
            var data = _db.Notifications.Where(p => !p.IsRead)
                .FirstOrDefault(p => p.Id == id);
            
            if (data != null)
            {
                data.IsRead = true;
                data.ReadDate = DateTime.UtcNow;

                _db.Update(data);
                _db.SaveChanges();
            }
        }

        public NotificationResponse GetDetailNotification(int id)
        {
            var data = _db.Notifications
                 .Select(_notification => new NotificationResponse
                 {
                     Id = _notification.Id,                   
                     Title = _notification.Title,
                     Body = _notification.Body,
                     IsRead = _notification.IsRead,
                     ReadDate = _notification.ReadDate,
                     CreatedDate = _notification.CreatedDate
                 })                 
                 .FirstOrDefault(p => p.Id == id);
            return data;
        }

        public void CreateNotification(NotificationAddRequest request)
        {
            _db.Notifications.Add(new Notification
            {
                Id = request.Id,
                Nik = request.Nik,
                Title = request.Title,
                Body = request.Body,
                IsRead = request.IsRead,
                ReadDate = request.ReadDate,
                CreatedDate = request.CreatedDate
            }); 

            _db.SaveChanges();
        }
    }
}
