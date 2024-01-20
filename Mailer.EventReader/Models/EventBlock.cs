namespace Mailer.EventReader.Models;

public class EventBlock(string fromDomain)
{
	public List<EventItem> EventItems { get; set; } = [];
	public string NextUrl { get; set; } = "";

	public string FromDomain => fromDomain;

	public List<EventItem> EventsWithFromDomain() => EventItems.Select(a => { a.fromDomain ??= fromDomain; return a; }).ToList();
}
