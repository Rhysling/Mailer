using Mailer.EventReader.Models;

namespace Mailer.EventReader;

public class MailgunEventService(App app)
{
	private readonly HttpClient client = app.MgEventClient;

	public async Task<(long ts, string json)> GetAsync()
	{
		long tsNow = (long)(((DateTimeOffset)DateTime.UtcNow).ToUnixTimeMilliseconds() / 1000.0);

		using HttpResponseMessage res = await client.GetAsync($"?begin={tsNow}&ascending=no&limit=200").ConfigureAwait(false);

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