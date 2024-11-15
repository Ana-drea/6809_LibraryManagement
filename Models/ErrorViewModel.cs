namespace LibraryManagement.Models;

public class ErrorViewModel
{
    public string? RequestId { get; set; }

    public int? ErrorCode { get; set; }
    public string? ErrorCategory { get; set; }
    public string? ErrorMessage { get; set; }
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
