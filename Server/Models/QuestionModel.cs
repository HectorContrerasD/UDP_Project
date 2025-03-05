using Server.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models
{
    public class QuestionModel
    {
        public Guid Id { get; set; }
        public string Question { get; set; }
        public string[] Options { get; set; }
        public string Correct { get; set; } 
        public TimeSpan TimeOut { get; set; }
        public List<AnswerModel> Answers { get; set; }  
    }
}
