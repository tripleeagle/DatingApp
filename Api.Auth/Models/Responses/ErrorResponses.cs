using Newtonsoft.Json;

namespace Api.Auth.Models.Responses
{
    public class ErrorResponses
    {
        public class ErrorResponse
        {
            public int StatusCode { get; set; }
            public string Message { get; set; }

            public override string ToString()
            {
                return JsonConvert.SerializeObject(this);
            }
        }

    }
}