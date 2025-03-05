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
                    // NOTE: This event must be invoked in the main thread through the Dispatcher class
                    MessageReceived?.Invoke(this, message);
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
                    Id = question.Id,
                    Question = question.Question,
                    Options = question.Options,
                    Expiration = DateTime.UtcNow + question.TimeOut
                }
            };
            var json = JsonSerializer.Serialize(message);
            var buffer = Encoding.UTF8.GetBytes(json);
            await Client.SendAsync(buffer, new IPEndPoint(IPAddress.Broadcast, Port));
        }
    }
}
