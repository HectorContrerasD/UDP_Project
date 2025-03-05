using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models.DTOs
{
    public class AnswerMessageDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Option { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
