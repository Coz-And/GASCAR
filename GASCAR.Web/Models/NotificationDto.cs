namespace GASCAR.Web.Models;

public class NotificationDto
{
    public string Title { get; set; } = "";
    public string Message { get; set; } = "";
    public string Type { get; set; } = "";
    public DateTime Date { get; set; }
}