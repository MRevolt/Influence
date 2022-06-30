using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Cosmetic.Model.Dto.Exceptions
{
    public class ExceptionBase : Exception
    {
        public string ResponseCode { get; set; }
        public string _Message { get; set; }
        public object[] Parameters { get; set; }


        public ExceptionBase(string message)
            : base(message)
        {
            this.ResponseCode = ExceptionErrorEnum.NotFound;
            this._Message = message;
        }

        public ExceptionBase(string responseCode, Exception exception)
            : base(responseCode, exception)
        {
            this.ResponseCode = responseCode;

        }
        public override string Message
        {
            get { return _Message; }
        }
        public ExceptionBase(string responseCode, string message, Exception innerException)
            : base(message, innerException)
        {
            this.ResponseCode = responseCode;
            this._Message = message;
        }

        public ExceptionBase(string responseCode, string message)
            : base(responseCode)
        {
            this.ResponseCode = responseCode;
            this._Message = message;
        }

        public ExceptionBase(string responseCode, Exception exception, params object[] parameters)
            : base(responseCode, exception)
        {
            this.ResponseCode = responseCode;
            this.Parameters = parameters;
        }
    }
}
