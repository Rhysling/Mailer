using Mailer.EventReader.Models;

namespace Mailer.EventReader;

public class MailgunEventService(App app, AppSettings appSettings)
{
	private readonly HttpClient client = app.MgEventClient;

	public async Task<List<EventBlock>> GetEventBlocksAsync(long tsSince)
	{
		var mgep = new MailgunEventParser(appSettings.Mailgun.FromDomain);

		var leb = new List<EventBlock>();
		int i = 0;

		var res = await GetSinceAsync(tsSince);
		if (res.json == "") return leb;

		leb.Add(mgep.ParseBlock(res.json));


		while (true)
		{
			if (leb[i].EventItems.Count == 0) break;
			string json = await GetNextAsync(leb[i].NextUrl);
			leb.Add(mgep.ParseBlock(json));
			i += 1;
		}

		leb = leb.Where(a => a.EventItems.Count > 0).ToList();
		return leb;
	}

	public async Task<List<EventItem>> GetEventsAsync(long tsSince)
	{
		var leb = await GetEventBlocksAsync(tsSince);

		return leb.SelectMany(a => a.EventItems).ToList();
	}


	public async Task<(long ts, string json)> GetSinceAsync(long tsSince)
	{
		long tsNow = (long)(((DateTimeOffset)DateTime.UtcNow).ToUnixTimeMilliseconds() / 1000.0);

		string qs = (tsSince > 1.0)
			? $"?begin={tsSince}&ascending=yes"
			: $"?begin={tsNow}&ascending=no&limit=200";

		using HttpResponseMessage res = await client.GetAsync(qs).ConfigureAwait(false);

		if (res.IsSuccessStatusCode)
		{
			string json = await res.Content.ReadAsStringAsync().ConfigureAwait(false);
			return (tsNow, json);
		}

		return (tsNow, "");
	}

	public async Task<string> GetNextAsync(string url)
	{
		using HttpResponseMessage res = await client.GetAsync(url).ConfigureAwait(false);

		if (res.IsSuccessStatusCode)
		{
			return await res.Content.ReadAsStringAsync().ConfigureAwait(false);
		}

		return "";
	}

}