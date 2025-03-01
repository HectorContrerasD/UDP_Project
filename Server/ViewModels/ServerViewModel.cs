using GalaSoft.MvvmLight.Command;
using Server.Models;
using Server.Models.DTOs;
using Server.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Server.ViewModels
{
    public class ServerViewModel : INotifyPropertyChanged
    {
        public string Ip { get; set; }
        public int Port { get; set; }
        public ObservableCollection<AnswerModel> Answers { get; set; } = new ObservableCollection<AnswerModel>();
        public QuestionModel question { get; set; } = new QuestionModel();
        public ICommand StartQuiz = new RelayCommand(start);
        ServerService server = new();
        private static void start()
        {
                
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
