using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models.DTOs
{
    public class ClientMessage
    {
        public AnswerModel Answer { get; set; }
        public string Type { get; set; }
    }
}
