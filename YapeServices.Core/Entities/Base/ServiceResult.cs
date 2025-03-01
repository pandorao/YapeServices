using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Yape.Entities.Base
{
    public class ServiceResult : AbstractServiceResult
    {
        public ServiceResult()
        {
            Errors = new Dictionary<string, string[]>();
        }
    }

    public class ServiceResult<T> : ServiceResult
    {
        public T ResponseObject { get; set; }
    }

    public abstract class AbstractServiceResult
    {
        public Dictionary<string, string[]> Errors { get; set; }

        public void AddModelError(List<ValidationResult> validationResults)
        {
            foreach (var validation in validationResults)
            {
                foreach (var member in validation.MemberNames)
                {
                    AddModelError(member, validation.ErrorMessage);
                }
            }
        }

        public void AddModelError(string key, string message)
        {
            if (!Errors.TryGetValue(key, out string[] value))
            {
                Errors[key] = new string[] { message };
            }
            else
            {
                var existingMessages = value.ToList();
                existingMessages.Add(message);
                Errors[key] = existingMessages.ToArray();
            }
        }

        public bool Succeeded 
        {
            get {
                return Errors.Count == 0;
            }
        }
    }
}
