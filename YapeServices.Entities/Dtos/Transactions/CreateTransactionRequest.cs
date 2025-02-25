using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YapeServices.Entities.Dtos.Transactions
{
    public class CreateTransactionRequest
    {
        [Required]
        public string SourceAccountId { get; set; }
        [Required]
        public string TargetAccountId { get; set; }
        [Required]
        public int TransferTypeId { get; set; }
        [Required]
        public decimal Value { get; set; }
    }
}
