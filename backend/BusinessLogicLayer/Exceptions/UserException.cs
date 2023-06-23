using System.Diagnostics;

namespace BusinessLogicLayer.Exceptions
{
    public class UserException : Exception
    {
        public UserException() { }
        public UserException(string msg) : base(msg) { Debug.WriteLine(msg); }
        public UserException(string msg, Exception innerException) : base(msg, innerException)
        {
            Debug.WriteLine(
                msg + "<br/>" +
                "innerException.Message  : " + innerException.Message + "<br/>" +
                "innerException.StackTrace : " + innerException.StackTrace + "<br/>" +
                "innerException.Source  : " + innerException.Source + "<br/>" +
                "innerException.Data : " + innerException.Data + "<br/>" +
                "innerException.InnerException : " + innerException.InnerException
                );
        }
    }
}
