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
        public ServerService()
        {
            var thread = new Thread(new ThreadStart(Start))
            {
                IsBackground = true
            };
            thread.Start();
        }
        public event EventHandler<UserDTO>? ValidateAnswer;
        void Start()
        {
            UdpClient server = new(5001);
            while (true)
            {
                IPEndPoint remote = new(IPAddress.Any, 5001);
                byte[] buffer = server.Receive(ref remote);
                UserDTO? user = JsonSerializer.Deserialize<UserDTO>(Encoding.UTF8.GetString(buffer));
            
                if (user != null)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        ValidateAnswer?.Invoke(this, user);
                    });
                }
            }
        }
    }
}
