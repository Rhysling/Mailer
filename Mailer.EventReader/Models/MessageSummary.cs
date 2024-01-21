namespace Mailer.EventReader.Models;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
public class MessageSummary
{
	public string MessageId { get; set; }
	public double TimeStamp { get; set; }
	public string HumanDate { get; set; }
	public string Recipient { get; set; }
	public string Subject { get; set; }
	public List<EventSummary> EventSummaries { get; set; }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

