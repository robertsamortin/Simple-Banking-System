using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BankingSystem.Models
{
    public class Users
    {
        public int ID { get; set; }
        public string AccountNumber { get; set; }
        [Required]
        public string LoginName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public double Balance { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
