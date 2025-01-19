using System.Net;

namespace HackerNewsASP.Models.Exceptions;

public class HandledException(string message, HttpStatusCode statusCode) : Exception(message)
{
    public HttpStatusCode StatusCode { get; } = statusCode;
}