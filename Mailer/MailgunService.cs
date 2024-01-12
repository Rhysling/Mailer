using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;

namespace Mailer
{
	public class MailgunService
	{
		private readonly HttpClient client;
		private const string baseAddress = "https://api.mailgun.net/v3/{0}/messages";

		private readonly string fromAddress;


		public MailgunService(string fromAddress, string fromDomain, string authValue)
		{
			this.fromAddress = fromAddress;

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

		public async Task<HttpResponseMessage> SendAsync(MailMessage msg)
		{
			// Has Attachments ***
			if (msg.Attachments.Count > 0)
			{
				using MultipartFormDataContent multipartContent = new()
				{
					{ new StringContent(fromAddress, Encoding.UTF8, MediaTypeNames.Text.Plain), "from" },
					{ new StringContent(msg.ToEmails, Encoding.UTF8, MediaTypeNames.Text.Plain), "to" },
					{ new StringContent(msg.Subject, Encoding.UTF8, MediaTypeNames.Text.Plain), "subject" },
					{ new StringContent(msg.Body, Encoding.UTF8, MediaTypeNames.Text.Plain), "html" }
				};

				foreach (var a in msg.Attachments)
				{
					var fileContent = new StreamContent(File.OpenRead(a.FullPath));
					fileContent.Headers.ContentType = new MediaTypeHeaderValue(a.MimeType);
					multipartContent.Add(fileContent, "attachment", a.FileName);
				}

				using var response = await client.PostAsync("", multipartContent).ConfigureAwait(false);
				return response;
			}


			var parameters = new Dictionary<string, string> {
				{ "from", fromAddress },
				{ "to", msg.ToEmails },
				{ "subject", msg.Subject },
				{ "html", msg.Body }
			};

			using FormUrlEncodedContent encodedContent = new(parameters);
			using HttpResponseMessage res = await client.PostAsync("", encodedContent).ConfigureAwait(false);
			return res;
		}

	}
}