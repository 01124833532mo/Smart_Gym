﻿namespace SmartGym.Shared.Errors.Models
{
    public class BadRequestExeption : ApplicationException
    {
        public BadRequestExeption(string Message) : base(Message)
        {

        }
    }

}
