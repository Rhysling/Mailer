using CloudantDb.Services;
using Mailer.EventReader.Models;

namespace Mailer.EventReader;

public class App
{
	private readonly HttpClient mgEventClient;
	private readonly DbService dbService;

	public App(AppSettings appSettings)
	{
		string url = $"{appSettings.Mailgun.BaseUrl}{appSettings.Mailgun.FromDomain}/events";

		var socketsHandler = new SocketsHttpHandler
		{
			PooledConnectionLifetime = TimeSpan.FromMinutes(10),
			PooledConnectionIdleTimeout = TimeSpan.FromMinutes(5),
			MaxConnectionsPerServer = 10
		};
		mgEventClient = new HttpClient(socketsHandler)
		{
			BaseAddress = new Uri(url)
		};
		mgEventClient.DefaultRequestHeaders.Add("Authorization", appSettings.Mailgun.AuthValue);
	
		dbService = new(
			appSettings.CloudantDb.BaseUrl,
			appSettings.CloudantDb.DbName,
			appSettings.CloudantDb.AuthValue
		);
	}

	public HttpClient MgEventClient => mgEventClient;
	public DbService DbService => dbService;
}
