using Microsoft.AspNetCore.SignalR;
using MyWebServer;

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

            // Device joins its own group
            await Groups.AddToGroupAsync(Context.ConnectionId, deviceId);

            // Notify all admins that the device list changed
            await Clients.All.SendAsync(
                "DeviceListUpdated",
                DeviceManager.Devices.Values.ToList());

            Console.WriteLine($"Device Registered: {computerName} ({deviceId})");
        }

        // Send current device list to one admin
        public async Task GetDevices()
        {
            await Clients.Caller.SendAsync(
                "DeviceListUpdated",
                DeviceManager.Devices.Values.ToList());
        }

        // Admin starts watching a device
        public async Task WatchDevice(string deviceId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, deviceId);

            Console.WriteLine($"Admin is watching {deviceId}");
        }

        // Receive screen frame from remote device
        public async Task SendFrame(string deviceId, byte[] frameBytes)
        {
            try
            {
                Console.WriteLine($"Frame received: {frameBytes.Length} bytes from {deviceId}");

                await Clients.Group(deviceId)
                    .SendAsync("ReceiveFrame", deviceId, frameBytes);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
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

                Console.WriteLine($"Device Disconnected: {device.ComputerName}");
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
