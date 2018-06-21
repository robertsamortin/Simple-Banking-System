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
        [DataType(DataType.Currency, ErrorMessage = "Not a number")]
        [Required(ErrorMessage = "Amount is required"), Range(200, 10000, ErrorMessage = "Min amount: 200, max amount: 10,000")]
        [Display(Name = "Amount")]
        public double Amount { get; set; }
        public string TransType { get; set; }
        public DateTime TransDate { get; set; }
        public string TransBy { get; set; }
        public double Balance { get; set; }
        public double RunningBalance { get; set; }
    }

}
