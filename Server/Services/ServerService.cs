using Server.Models;
using Server.Models.DTOs;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace Server.Services
{
    public class ServerService
    {
        private UdpClient _udpClient;
        private int _port;
        private Dictionary<string, int> _userScores = new Dictionary<string, int>(); 

        public event EventHandler<AnswerModel> AnswerReceived; 
        public event EventHandler QuizFinished;

        public ServerService(int port)
        {
            _port = port;
            _udpClient = new UdpClient(port);
            Task.Run(ReceiveAnswersAsync); 
        }

       
        public async Task SendQuestionAsync(QuestionModel question)
        {
            var json = JsonSerializer.Serialize(question);
            var buffer = Encoding.UTF8.GetBytes(json);

           
            var endpoint = new IPEndPoint(IPAddress.Broadcast, _port);
            await _udpClient.SendAsync(buffer, buffer.Length, endpoint);
        }

      
        private async Task ReceiveAnswersAsync()
        {
            while (true)
            {
                var result = await _udpClient.ReceiveAsync();
                var json = Encoding.UTF8.GetString(result.Buffer);
                var answer = JsonSerializer.Deserialize<AnswerModel>(json);

                if (answer != null)
                {
                   
                    AnswerReceived?.Invoke(this, answer);
                }
            }
        }

      
        public async Task SendResultsAsync()
        {
            foreach (var userScore in _userScores)
            {
                var result = new UserScoreModel
                {
                    UserName = userScore.Key,
                    CorrectAnswers = userScore.Value
                };

                var json = JsonSerializer.Serialize(result);
                var buffer = Encoding.UTF8.GetBytes(json);

            
                var endpoint = new IPEndPoint(IPAddress.Broadcast, _port);
                await _udpClient.SendAsync(buffer, buffer.Length, endpoint);
            }

        
            QuizFinished?.Invoke(this, EventArgs.Empty);
        }

       
        public void UpdateUserScore(string userName, bool isCorrect)
        {
            if (_userScores.ContainsKey(userName))
            {
                if (isCorrect)
                {
                    _userScores[userName]++;
                }
            }
            else
            {
                _userScores[userName] = isCorrect ? 1 : 0;
            }
        }
    }
}
