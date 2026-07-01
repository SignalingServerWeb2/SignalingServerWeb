using System.Collections.Concurrent;

namespace MyWebServer
{
    public static class DeviceManager
    {
        public static ConcurrentDictionary<string, DeviceInfo> Devices = new();
    }
}
