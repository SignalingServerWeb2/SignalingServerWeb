namespace MyWebServer;
{
    public class DeviceInfo
    {
        public string DeviceId { get; set; } = string.Empty;
        public string ComputerName { get; set; } = string.Empty;
        public string ConnectionId { get; set; } = string.Empty;
        public bool Online { get; set; }
        public DateTime ConnectedAt { get; set; } = DateTime.UtcNow;
    }
}
