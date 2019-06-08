using System;
namespace AppFinal.Models
{
    public class Response
    {

        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public object Result { get; set; }


        public Response()
        {
        }
    }
}
