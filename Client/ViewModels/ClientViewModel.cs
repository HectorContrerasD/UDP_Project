using Client.Models.DTOs;
using Client.Services;
using Client.Views;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Client.ViewModels
{
    public class ClientViewModel
    {
        private ClientService _clientService;
        private string _serverIp;
        private string _currentQuestion;
        private string[] _currentOptions;
        private int _correctAnswers;
        public string ServerIp
        {
            get => _serverIp;
            set
            {
                _serverIp = value;
                OnPropertyChanged(nameof(ServerIp));
            }
        }

        public string CurrentQuestion
        {
            get => _currentQuestion;
            set
            {
                _currentQuestion = value;
                OnPropertyChanged(nameof(CurrentQuestion));
            }
        }

        public string[] CurrentOptions
        {
            get => _currentOptions;
            set
            {
                _currentOptions = value;
                OnPropertyChanged(nameof(CurrentOptions));
            }
        }

        public int CorrectAnswers
        {
            get => _correctAnswers;
            set
            {
                _correctAnswers = value;
                OnPropertyChanged(nameof(CorrectAnswers));
            }
        }

        public ICommand ConnectCommand { get; }
        public ICommand SendAnswerCommand { get; }

        public ClientViewModel()
        {
            
            ConnectCommand = new RelayCommand(Connect);
            SendAnswerCommand = new RelayCommand(SendAnswer);
         
            _clientService.QuestionReceived += OnQuestionReceived;
            _clientService.ResultReceived += OnResultReceived;
        }

        private void Connect()
        {

            
            if (string.IsNullOrWhiteSpace(ServerIp))
            {
                MessageBox.Show("Por favor, ingresa una dirección IP válida.");
                return;
            }

           
            var ClientView = new ClientView()
            {
                DataContext = new ClientView() 
            };
            ClientView.Show();

            
            Application.Current.MainWindow.Close();
        }

        private void SendAnswer()
        {
          
            var answer = new AnswerMessageDTO
            {
                UserName = Environment.UserName,
                SelectedOption = "Alola" 
            };

            _clientService.SendAnswerAsync(answer);
        }

        private void OnQuestionReceived(object sender, QuestionDto question)
        {

            CurrentQuestion = question.Question;
            CurrentOptions = question.Options;
        }

        private void OnResultReceived(object sender, ResultDTO result)
        {
           
            CorrectAnswers = result.CorrectAnswers;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
