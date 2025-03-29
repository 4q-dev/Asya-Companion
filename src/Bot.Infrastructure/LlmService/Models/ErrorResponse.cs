namespace Bot.Infrastructure.LlmService.Models
{
    internal class ErrorResponse
    {
        public required Error Error { get; set; }
    }

    internal class Error
    {
        public required string Message { get; set; }
        public required string Type { get; set; }
    }
}
