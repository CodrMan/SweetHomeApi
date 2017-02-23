namespace SweetHome.Api.Models
{
    public class ApiResponse
    {
        public int State { get; set; }
        public object Data { get; set; }
        public string Message { get; set; }
    }
}