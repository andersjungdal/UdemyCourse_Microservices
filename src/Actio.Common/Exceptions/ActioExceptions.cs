using System;
using System.Collections.Generic;
using System.Text;

namespace Actio.Common.Exceptions
{
    public class ActioExceptions : Exception
    {
        public string Code { get; }
        public ActioExceptions()
        {

        }
        public ActioExceptions(string code)
        {
            Code = code;
        }
        public ActioExceptions(string message, params object[] args) : this(string.Empty, message, args)
        {

        }
        public ActioExceptions(string code, string message, params object[] args) : this(null, code, message, args)
        {

        }
        public ActioExceptions(Exception innerException, string message, params object[] args) 
            : this(innerException, string.Empty, message, args)
        {

        }
        public ActioExceptions(Exception innerException, string code, string message, params object[] args) 
            : base(string.Format(message, args), innerException)
        {
            Code = code;
        }
    }
}
