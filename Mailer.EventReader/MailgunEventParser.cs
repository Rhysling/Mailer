using Mailer.EventReader.Models;
using Newtonsoft.Json.Linq;

namespace Mailer.EventReader;

public class MailgunEventParser(string fromDomain)
{
	public EventBlock ParseBlock(string inp)
	{
		var eb = new EventBlock(fromDomain);

		JObject o = JObject.Parse(inp);
		eb.EventItems = ((JArray)(o["items"] ?? JObject.Parse("[]"))).Select(a => a.ToObject<EventItem>() ?? new EventItem()).Where(a => a.timestamp > 1.0).ToList();
		eb.NextUrl = o["paging"]?["next"]?.ToString() ?? "";

		return eb;
	}
}
