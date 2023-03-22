using BigioHrServices.Db;
using BigioHrServices.Db.Entities;
using BigioHrServices.Model;
using BigioHrServices.Model.Datatable;
using BigioHrServices.Model.Notification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

namespace BigioHrServices.Services
{
    public interface INotificationService
    {
        public DatatableResponse GetList(NotificationGetRequest request);
    }
    public class NotificationServices : INotificationService
    {
        private readonly ApplicationDbContext _db;
        public NotificationServices(ApplicationDbContext db)
        {
            _db = db;
        }

        public DatatableResponse GetList(NotificationGetRequest request)
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
                    Data = _notification.Data,
                    IsRead = _notification.IsRead,
                    ReadDate = _notification.ReadDate,
                    CreatedDate = _notification.CreatedDate
                })
                .OrderByDescending(_notification => _notification.CreatedDate)
                .ToList();

            return new DatatableResponse()
            {
                Data = data.ToArray(),
                TotalRecords = data.Count,
                PageSize = request.PageSize > data.Count ? data.Count : request.PageSize,
                NextPage = (request.PageSize * request.Page) < data.Count,
                PrevPage = request.Page > 1,
            };
        }
    }
}
