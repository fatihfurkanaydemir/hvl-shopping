﻿using System.Globalization;

namespace DiscountService.Application.Exceptions;

public class ApiException : Exception
{
    public List<string> Errors { get; set; }
    public ApiException() : base() { }

    public ApiException(string message) : base(message) { }

    public ApiException(string message, params object[] args)
        : base(String.Format(CultureInfo.CurrentCulture, message, args))
    {
    }
}
