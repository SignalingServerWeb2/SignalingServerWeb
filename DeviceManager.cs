using System.Collections.Concurrent;

namespace MyWebServer.Models
{
    public static class DeviceManager
    {
        public static ConcurrentDictionary<string, DeviceInfo> Devices = new();
    }
}
