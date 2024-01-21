namespace Mailer.EventReader.Models;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
public class EventSummary
{
	public double TimeStamp { get; set; }
	public string HumanDate { get; set; }
	public string EventType { get; set; }
	public string GeoSummary { get; set; }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

