namespace BigioHrServices.Model.Delegation
{
    public class DelegationDetailResponse
    {
        public string NIK { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DelegationResponse[] Backups { get; set; }
    }
}
