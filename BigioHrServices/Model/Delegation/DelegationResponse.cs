namespace BigioHrServices.Model.Delegation
{
    public class DelegationResponse
    {
        public string NIK { get; set; } = string.Empty;
        public string ParentNIK { get; set; } = string.Empty;
        public int Priority { get; set; }
    }
}
