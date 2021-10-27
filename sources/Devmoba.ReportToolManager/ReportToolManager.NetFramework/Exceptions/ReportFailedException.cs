using System;
using System.Net;

namespace ReportToolManager.NetFramework.Exceptions
{
    public class ReportFailedException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public string Details { get; }

        public ReportFailedException(string resourcePath, HttpStatusCode statusCode, string details, Exception innerException = null)
            : base($"Request to resource '{resourcePath}' has failed!", innerException)
        {
            StatusCode = statusCode;
            Details = details;
        }
    }
}
