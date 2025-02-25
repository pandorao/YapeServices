using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YapeServices.Entities.Dtos.Transactions
{
    public class CreateTransactionResponse
    {
        public string TransactionExternalId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
