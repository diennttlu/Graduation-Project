namespace Devmoba.ToolClient.Models
{
    public class ReportMessage
    {
        public string ConnectionId { get; set; }

        public long? ToolId { get; set; }

        public bool IsSuccess { get; set; }

        public string ErrorMessage { get; set; }
    }
}
