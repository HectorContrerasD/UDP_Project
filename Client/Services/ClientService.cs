using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Client.Models.DTOs;

namespace Client.Services
{
    public class ClientService
    {
        private UdpClient _udpClient;
        private string _serverIp;
        private int _port;

        public event EventHandler<QuestionDto> QuestionReceived; 
        public event EventHandler<ResultDTO> ResultReceived; 

        public ClientService(string serverIp, int port)
        {
            _serverIp = serverIp;
            _port = port;
            _udpClient = new UdpClient();
            Task.Run(ReceiveMessagesAsync); 
        }

      
        public async Task SendAnswerAsync(AnswerMessageDTO answer)
        {
            var json = JsonSerializer.Serialize(answer);
            var buffer = Encoding.UTF8.GetBytes(json);

            var endpoint = new IPEndPoint(IPAddress.Parse(_serverIp), _port);
            await _udpClient.SendAsync(buffer, buffer.Length, endpoint);
        }


        private async Task ReceiveMessagesAsync()
        {
            while (true)
            {
                var result = await _udpClient.ReceiveAsync();
                var json = Encoding.UTF8.GetString(result.Buffer);

              
                if (json.Contains("Question"))
                {
                    var question = JsonSerializer.Deserialize<QuestionDto>(json);
                    QuestionReceived?.Invoke(this, question);
                }
                else if (json.Contains("CorrectAnswers"))
                {
                    var resultModel = JsonSerializer.Deserialize<ResultDTO>(json);
                    ResultReceived?.Invoke(this, resultModel);
                }
            }
        }
    }
}
