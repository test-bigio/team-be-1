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
        public Pageable<NotificationResponse> GetListNotification(NotificationGetRequest request);
        public Pageable<NotificationResponse> GetListNotificationByEmployeeId(NotificationGetRequest request, string nik);
        public void UpdateStatusNotification(int id);
        public NotificationResponse GetDetailNotification(int id);
        public void CreateNotification(NotificationAddRequest request);
        public void DeleteNotification(int id);
    }
    public class NotificationServices : INotificationService
    {
        private readonly ApplicationDbContext _db;
        public NotificationServices(ApplicationDbContext db)
        {
            _db = db;
        }

        public Pageable<NotificationResponse> GetListNotification(NotificationGetRequest request)
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
                DateTime startDate = DateTime.ParseExact(request.StartDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                query = query.Where(p => p.CreatedDate > startDate);
            }

            if (!string.IsNullOrEmpty(request.EndDate))
            {
                DateTime endDate = DateTime.ParseExact(request.EndDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                query = query.Where(p => p.CreatedDate < endDate);
            }

            var data = query
                .Select(_notification => new NotificationResponse
                {
                    Id = _notification.Id,
                    Nik = _notification.Nik,
                    Title = _notification.Title,
                    Body = _notification.Body,
                    IsRead = _notification.IsRead,
                    ReadDate = _notification.ReadDate,
                    CreatedDate = _notification.CreatedDate
                })
                .OrderByDescending(_notification => _notification.CreatedDate)
                .ToList();

            return new Pageable<NotificationResponse>(data, request.Page, request.PageSize);
        }

        public Pageable<NotificationResponse> GetListNotificationByEmployeeId(NotificationGetRequest request, string nik)
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
                DateTime startDate = DateTime.ParseExact(request.StartDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                query = query.Where(p => p.CreatedDate > startDate);
            }

            if (!string.IsNullOrEmpty(request.EndDate))
            {
                DateTime endDate = DateTime.ParseExact(request.EndDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                query = query.Where(p => p.CreatedDate < endDate);
            }

            var data = query
                .Select(_notification => new NotificationResponse
                {
                    Id = _notification.Id,
                    Nik = _notification.Nik,
                    Title = _notification.Title,
                    Body = _notification.Body,
                    IsRead = _notification.IsRead,
                    ReadDate = _notification.ReadDate,
                    CreatedDate = _notification.CreatedDate
                })
                .OrderByDescending(_notification => _notification.CreatedDate)
                .ToList();

            return new Pageable<NotificationResponse>(data, request.Page, request.PageSize);
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
                     Nik = _notification.Nik,
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
                Nik = request.Nik,
                Title = request.Title,
                Body = request.Body,
                CreatedDate = request.CreatedDate
            }); 

            _db.SaveChanges();
        }

        public void DeleteNotification(int id)
        {
            var data = _db.Notifications.FirstOrDefault(p => p.Id == id);

            if (data != null) {

                _db.Notifications.Remove(data);
                _db.SaveChanges();
            }
        }
    }
}
