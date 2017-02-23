namespace SweetHome.Core.Entities.log4net
{
    public class Log
    {
        public long UserId { get; set; }
        public string RequestType { get; set; }
        public string InputParameters { get; set; }
        public string Response { get; set; }
        public string ErrorSource { get; set; }
        public string ErrorMessage { get; set; }
        public string InnerErrorMessage { get; set; }
        public string StackTrace { get; set; }
        public double ExecutionTime { get; set; }
    }
}
