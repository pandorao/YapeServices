using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YapeServices.Core.Exceptions
{
    public class TransactionAlreadyExecutedException : ApplicationException
    {
        public TransactionAlreadyExecutedException()
            : base("Transaction was executed") { }
    }
}
