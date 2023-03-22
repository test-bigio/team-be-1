namespace BigioHrServices.Model.Position
{
    public class PositionResponse
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
