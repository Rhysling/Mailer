using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Mailer.EventReader;

public class MailgunEventService
{
	private readonly HttpClient client;
	private const string baseAddress = "https://api.mailgun.net/v3/{0}/events";

	public MailgunEventService(string fromDomain, string authValue)
	{
		var socketsHandler = new SocketsHttpHandler
		{
			PooledConnectionLifetime = TimeSpan.FromMinutes(10),
			PooledConnectionIdleTimeout = TimeSpan.FromMinutes(5),
			MaxConnectionsPerServer = 10
		};
		client = new HttpClient(socketsHandler)
		{
			BaseAddress = new Uri(String.Format(baseAddress, fromDomain))
		};
		client.DefaultRequestHeaders.Add("Authorization", authValue);
	}

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
		url = url.Replace(baseAddress, "");

		using HttpResponseMessage res = await client.GetAsync(url).ConfigureAwait(false);

		if (res.IsSuccessStatusCode)
		{
			return await res.Content.ReadAsStringAsync().ConfigureAwait(false);
		}

		return "";
	}

}