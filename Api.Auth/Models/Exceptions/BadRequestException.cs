using System;

namespace Api.Auth.Models.Exceptions
{
    public class BadRequestException: Exception
    {
        public BadRequestException() : base("Bad request")
        {
        }
    }

}