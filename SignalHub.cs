using Microsoft.AspNetCore.SignalR;

namespace MyWebServer
{
    public class SignalHub : Hub
    {
        // Register a remote device
        public async Task RegisterDevice(string deviceId, string computerName)
        {
            DeviceManager.Devices[deviceId] = new DeviceInfo
            {
                DeviceId = deviceId,
                ComputerName = computerName,
                ConnectionId = Context.ConnectionId,
                Online = true,
                ConnectedAt = DateTime.UtcNow
            };

            await Groups.AddToGroupAsync(Context.ConnectionId, deviceId);

            await Clients.All.SendAsync(
                "DeviceListUpdated",
                DeviceManager.Devices.Values.ToList());

            Console.WriteLine($"Device Registered: {computerName} ({deviceId})");
        }

        public async Task GetDevices()
        {
            await Clients.Caller.SendAsync(
                "DeviceListUpdated",
                DeviceManager.Devices.Values.ToList());
        }

        public async Task WatchDevice(string deviceId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, deviceId);

            Console.WriteLine($"Admin is watching {deviceId}");
        }

        public async Task SendFrame(string deviceId, byte[] frameBytes)
        {
            await Clients.Group(deviceId)
                .SendAsync("ReceiveFrame", deviceId, frameBytes);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var device = DeviceManager.Devices.Values
                .FirstOrDefault(d => d.ConnectionId == Context.ConnectionId);

            if (device != null)
            {
                DeviceManager.Devices.TryRemove(device.DeviceId, out _);

                await Clients.All.SendAsync(
                    "DeviceListUpdated",
                    DeviceManager.Devices.Values.ToList());
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
