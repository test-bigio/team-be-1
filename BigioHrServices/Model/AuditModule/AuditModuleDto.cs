namespace BigioHrServices.Model.AuditModule
{
    public class AuditModuleDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string IpAddress { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Module { get; set; } = string.Empty;
        public string Activity { get; set; } = string.Empty;
        public string Detail { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public static implicit operator AuditModuleDto(Db.Entities.AuditModul entity)
        {
            AuditModuleDto auditModuleDto = new AuditModuleDto();
            auditModuleDto.Id = entity.Id;
            auditModuleDto.IpAddress = entity.IpAddress;
            auditModuleDto.Username = entity.IpAddress;
            auditModuleDto.Module = entity.IpAddress;
            auditModuleDto.Activity = entity.IpAddress;
            auditModuleDto.Detail = entity.IpAddress;
            auditModuleDto.CreatedDate = entity.CreatedDate;

            return auditModuleDto;
        }
    }
}
