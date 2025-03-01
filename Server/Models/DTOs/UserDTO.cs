using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models.DTOs
{
    public class UserDTO
    {
        public string? Name { get; set; }
        public string? Answer { get; set; }
        public bool IsCorrect { get; set; }
    }
}
