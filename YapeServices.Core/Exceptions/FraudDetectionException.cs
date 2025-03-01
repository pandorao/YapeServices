using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YapeServices.Core.Exceptions
{
    public class FraudDetectionException : ApplicationException
    {
        public FraudDetectionException(string message) : base(message) { }
    }
}
