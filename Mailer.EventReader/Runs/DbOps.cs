using Mailer.EventReader.Db;
using Mailer.EventReader.Models;

namespace Mailer.EventReader.Runs
{
	public class DbOps(App app)
	{
		private readonly MailgunLogDb db = new(app.DbService);

		public async Task<List<MessageSummary>> GetMessageSummaries(DateTime? lastDate, DateTime? firstDate)
		{
			var evList = await db.GetEventsDescAsync(lastDate, firstDate);
			var msl = new List<MessageSummary>();

			var messageIds = evList.Select(a => a.message?.headers?.messageId ?? "missing id").Distinct().ToList();

			foreach (var id in messageIds)
			{
				var esl = evList.Where(a => (a.message?.headers?.messageId ?? "missing id") == id)
					.Select(a => new EventSummary
					{
						TimeStamp = a.timestamp,
						HumanDate = a.humanDate,
						EventType = a.eventType,
						GeoSummary = $"{a.geolocation?.country ?? "missing"} / {a.geolocation?.region ?? "missing"} / {a.geolocation?.city ?? "missing"}"
					})
					.OrderBy(a => a.TimeStamp)
					.ToList();
				msl.Add(new MessageSummary
				{
					MessageId = id,
					TimeStamp = esl[0].TimeStamp,
					HumanDate = esl[0].HumanDate,
					Recipient = evList.FirstOrDefault(a => ((a.message?.headers?.messageId ?? "xxx") == id) && !string.IsNullOrWhiteSpace(a.recipient))?.recipient ?? "missing",
					Subject = evList.FirstOrDefault(a => ((a.message?.headers?.messageId ?? "xxx") == id) && !string.IsNullOrWhiteSpace(a.message?.headers?.subject))?.message.headers.subject ?? "missing",
					EventSummaries = esl
				});
			}

			msl = [.. msl.OrderBy(a => a.TimeStamp)];

			return msl;
		}


	}
}
