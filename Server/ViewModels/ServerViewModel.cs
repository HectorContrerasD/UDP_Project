using GalaSoft.MvvmLight.Command;
using Server.Models;
using Server.Models.DTOs;
using Server.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Server.ViewModels
{
    public class ServerViewModel : INotifyPropertyChanged
    {
        public IPAddress IpAddress { get; }
        public int Port { get; set; }
        public ServerService? Service { get; set; }

        public string Question { get; set; }
        public string[] Options { get; set; }
        public string Correct { get; set; }
        public int Seconds { get; set; }
        public int Counter { get; set; }

        public List<QuestionModel> Questions { get; } = new List<QuestionModel>();
        public System.Timers.Timer Timer { get; set; }
        public event EventHandler<QuestionModel>? QuizFinished;

        public async void StartQuiz()
        {
            if (Service == null)
            {
                Service = new ServerService(Port);
                Service.MessageReceived += MessageReceived;
            }

            var question = new QuestionModel
            {
                Id = Guid.NewGuid(),
                Question = Question,
                Options = Options,
                Correct = Correct,
                TimeOut = TimeSpan.FromSeconds(Seconds),
                Answers = new List<AnswerModel>()
            };

            Questions.Add(question);
            await Service.SendQuestion(question);
            Counter = Seconds;
        }

        private void MessageReceived(object? sender, ClientMessageDTO message)
        {
            if (message.Type == "ANSWER" && message.Answer != null)
            {
                var lastQuestion = Questions.Last();
                var lastAnswer = lastQuestion.Answers.FirstOrDefault(x => x.Id == message.Answer.Id);
                if (lastAnswer != null || Counter == 0) return;
                lastQuestion.Answers.Add(new AnswerModel
                {
                    Id = message.Answer.Id,
                    Name = message.Answer.Name,
                    Correct = message.Answer.Option == lastQuestion.Correct
                });
                Console.WriteLine($"Answer received: {message.Answer.Name} --> {message.Answer.Option}");
            }
        }

        private void TimerMethod(object? sender, EventArgs e)
        {
            if (Counter > 0)
            {
                Counter -= 1;
                Console.WriteLine($"Counter: {Counter}");
                if (Counter == 0)
                {
                    QuizFinished?.Invoke(this, Questions.Last());
                }
            }
        }

        public ServerViewModel()
        {
            IpAddress = Dns.GetHostAddresses(Dns.GetHostName()).Where(x => x.AddressFamily == AddressFamily.InterNetwork).First();
            Port = 5001;
            Timer = new System.Timers.Timer(TimeSpan.FromSeconds(1));
            Timer.Elapsed += TimerMethod;
            Timer.Start();
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
