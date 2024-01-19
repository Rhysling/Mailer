using System.Reflection.Metadata.Ecma335;

namespace Mailer.EventReader.Models;

public class EventBlock(string fromDomain)
{
	public List<EventItem> EventItems { get; set; } = [];
	public string NextUrl { get; set; } = "";

	public string FromDomain => fromDomain;

	public List<EventItem> EventsWithFromDomain() => EventItems.Select(a => { a.fromDomain = a.fromDomain ?? fromDomain; return a; }).ToList();
}
