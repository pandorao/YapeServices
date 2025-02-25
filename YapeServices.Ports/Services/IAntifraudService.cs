using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yape.Entities.Base;

namespace YapeServices.Ports.Services
{
    public interface IAntifraudService
    {
        Task<ServiceResult> ExecuteTransaction(string id);
    }
}
