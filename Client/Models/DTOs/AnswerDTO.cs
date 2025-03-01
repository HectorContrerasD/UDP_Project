using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models.DTOs
{
    public class AnswerDTO
    {
        public string? Name { get; set; }
        public string? Answer { get; set; }
        public bool IsCorrect { get; set; }
    }
}
