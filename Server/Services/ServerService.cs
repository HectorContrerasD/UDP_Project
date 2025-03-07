using Server.Models;
using Server.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace Server.Services
{
    public class ServerService
    {
        public int Port { get; }
        private UdpClient Client { get; }

        public ServerService(int port)
        {
            Port = port;
            Client = new UdpClient(port);
            var thread = new Thread(Start);
            thread.IsBackground = true;
            thread.Start();
        }

        public EventHandler<ClientMessageDTO>? MessageReceived;

        private async void Start()
        {
            while (true)
            {
                var result = await Client.ReceiveAsync();
                var json = Encoding.UTF8.GetString(result.Buffer);
                var message = JsonSerializer.Deserialize<ClientMessageDTO>(json);
                if (message != null)
                {
                    // Invocar el evento en el hilo principal (UI thread)
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageReceived?.Invoke(this, message);
                    });
                }
            }
        }

        public async Task SendQuestion(QuestionModel question)
        {
            var message = new ServerMessageDTO
            {
                Type = "QUESTION",
                Question = new QuestionMessageDTO
                {
                    Id = Guid.NewGuid(), 
                    Question = question.Question,
                    Options = new[] { "True", "False" }, 
                    Expiration = DateTime.UtcNow + TimeSpan.FromSeconds(10) //
                }
            };
            var json = JsonSerializer.Serialize(message);
            var buffer = Encoding.UTF8.GetBytes(json);
            await Client.SendAsync(buffer, buffer.Length, new IPEndPoint(IPAddress.Broadcast, Port));
        }
    }
}
