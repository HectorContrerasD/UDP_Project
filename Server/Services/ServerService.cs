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
        public int Port { get; set; } = 5001;
        public bool IsRunning { get; set; }
        public UdpClient Server { get; set; }
        public ServerService(int port)
        {
            Server = new();
            Port = port;
            var thread = new Thread(new ThreadStart(Start))
            {
                IsBackground = true
            };
            thread.Start();
        }
        public event EventHandler<ServerMessageDTO>? MessageReceived;
        void Start()
        {
            while (true)
            {
                IPEndPoint remote = new(IPAddress.Any, Port);
                byte[] buffer = Server.Receive(ref remote);
                ServerMessageDTO? message = JsonSerializer.Deserialize<ServerMessageDTO>(Encoding.UTF8.GetString(buffer));
                if (message != null)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MessageReceived?.Invoke(this, message);
                    });
                }
            }
        }
        public event Action<ServerService> QuizEnding;
        async Task StartQuiz(QuestionModel model)
        {
            var experationTime = DateTime.UtcNow + model.TimeOut;
            ServerMessageDTO serverMessageDTO = new ServerMessageDTO();
            serverMessageDTO.Type = "Question";
            serverMessageDTO.Question = new QuestionDTO
            {
                Question = model.Question,
                Options = model.Options,
                ExpirationTime = experationTime
            };
            var json = JsonSerializer.Serialize(serverMessageDTO);
            byte[] data = Encoding.UTF8.GetBytes(json);
            await Server.SendAsync(data, data.Length, new IPEndPoint(IPAddress.Broadcast, Port));
            while (DateTime.UtcNow < experationTime)
            {
                await Task.Delay(100);
            }
            Application.Current.Dispatcher.Invoke(() =>
            {
                QuizEnding.Invoke(this);
            });
        }
    }
}
