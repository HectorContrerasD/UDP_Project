using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models.DTOs
{
    public class QuestionDTO
    {
        public string Question { get; set; }
        public string[] Options { get; set; }
        public DateTime ExpirationTime { get; set; }

    }
}
