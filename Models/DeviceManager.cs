using System.Collections.Concurrent;

namespace SignalingServerWeb.Models
{
    public static class DeviceManager
    {
        public static ConcurrentDictionary<string, DeviceInfo> Devices
            = new();
    }
}
