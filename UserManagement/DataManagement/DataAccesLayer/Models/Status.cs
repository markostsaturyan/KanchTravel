namespace UserManagement.DataManagement.DataAccesLayer.Models
{
    public class Status
    {
        public int StatusCode { get; set; }

        public bool IsOk { get; set; }

        public string Message { get; set; }

        //Status codes

        //1000-all is ok
        //2000-2999 errors
        //3000-


    }
}
