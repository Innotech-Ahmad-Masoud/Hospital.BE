﻿namespace Hospital.Application.Results
{
    public class Response<T>
    {
        public string? Status { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
    }
}
