using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmetic.Model.Dto.Exceptions
{
    public class BusinessException : ExceptionBase
    {
        public BusinessException(string responseCode) : base(responseCode)
        {
        }

        public BusinessException(string responseCode, Exception exception) : base(responseCode, exception)
        {
        }

        public BusinessException(string responseCode, string Message) : base(responseCode, Message)
        {
        }

        public BusinessException(string responseCode, string Message, Exception exception) : base(responseCode, Message, exception)
        {
        }


        public BusinessException(string responseCode, Exception exception, params object[] parameters) : base(responseCode, exception, parameters)
        {
        }
    }
}
