using System.Diagnostics;

namespace BusinessLogicLayer.Exceptions
{
    /// <summary>
    /// class which contains Driver exception
    /// </summary>
    public class DriverException : Exception
    {
        public DriverException() { }
        public DriverException(string msg) : base(msg) { Debug.WriteLine( msg ); }
        public DriverException(string msg, Exception innerException) : base(msg, innerException)
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