using Client.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Client.Services
{
    public class ClientService
    {
        private UdpClient client = new();
        public string Server { get; set; } = "0.0.0.0";
        public void SendAnswer(AnswerDTO answerDTO)
        {
            var ipEndPoint = new IPEndPoint(IPAddress.Parse(Server), 5001);
            answerDTO.Name = Dns.GetHostName();
            var json = JsonSerializer.Serialize(answerDTO);
            byte[] buffer= Encoding.UTF8.GetBytes(json);
            client.Send(buffer,buffer.Length, ipEndPoint);
        }
    }
}
