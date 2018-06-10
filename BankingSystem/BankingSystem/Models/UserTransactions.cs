using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BankingSystem.Models
{
    public class UserTransactions
    {
        public int ID { get; set; }
        public string AccountNumber { get; set; }
        [Required]
        public double Amount { get; set; }
        public string TransType { get; set; }
        public DateTime TransDate { get; set; }
        public string TransBy { get; set; }
        public double Balance { get; set; }
        public double RunningBalance { get; set; }
    }

}
