using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParkingLot_API.Middleware
{
    public class WebSocketMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly WebSocketConnectionManager _manager = new WebSocketConnectionManager();

        public WebSocketMiddleware (RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                WebSocket ws = await context.WebSockets.AcceptWebSocketAsync();
                string connID = _manager.AddSocket(ws);
                Console.WriteLine("WebSocket Connected!");
                Console.WriteLine("Current connections: " + _manager.GetAllSockets().Count.ToString());

                await ReceiveMessage(ws, async (result, buffer) =>
                {
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        //Console.WriteLine("Received Text!");
                        Console.WriteLine($"Message: {Encoding.UTF8.GetString(buffer, 0, result.Count)}");
                        var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        await RouteMessageAsync(message);
                        //await SendMessage(ws, message);
                        return;
                    }
                    else if (result.MessageType == WebSocketMessageType.Close)
                    {
                        string ID = _manager.GetAllSockets().FirstOrDefault(s => s.Value == ws).Key;
                        Console.WriteLine("Closing WebSocket");

                        _manager.GetAllSockets().TryRemove(ID, out WebSocket socket);
                        Console.WriteLine("Current connections: " + _manager.GetAllSockets().Count.ToString());

                        await socket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);


                        return;
                    }
                });
            }
            else
            {
                await _next(context);
            }
        }

        private async Task ReceiveMessage(WebSocket ws, Action<WebSocketReceiveResult, byte[]> handler)
        {
            var buffer = new byte[1024 * 4];

            while (ws.State == WebSocketState.Open)
            {
                var result = await ws.ReceiveAsync(buffer: new ArraySegment<byte>(buffer), cancellationToken: CancellationToken.None);

                handler(result, buffer);
            }
        }

        private async Task RouteMessageAsync(string message)
        {
            foreach(var ws in _manager.GetAllSockets())
            {
                if (ws.Value.State == WebSocketState.Open)
                {
                    await ws.Value.SendAsync(Encoding.UTF8.GetBytes(message.ToString()), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }
    }
}
