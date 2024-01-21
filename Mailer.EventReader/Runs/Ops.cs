using Mailer.EventReader.Db;
using Mailer.EventReader.Models;

namespace Mailer.EventReader.Runs;

public static class Ops
{
	public static async Task UpdateEventsDbAsync(App app, AppSettings appSettings)
	{
		using var db = new MailgunLogDb(app.DbService);
		var mges = new MailgunEventService(app, appSettings);

		var ids = await db.GetLatestEventIdsAsync(0, 300);

		long latestTs = 0;
		if (ids.Count > 0)
		{
			latestTs = long.Parse(ids[0].Substring(2, 10)) - 20 * 60; // 20 min earlier
		}

		var items = await mges.GetEventsAsync(latestTs);
		var newEventItems = items.Where(a => !ids.Contains(a._id)).ToList();

		Console.WriteLine($"Id count from db: {ids.Count}.");
		Console.WriteLine($"Latest ts in db: {latestTs}.");
		Console.WriteLine($"Mg event items since ts: {items.Count}.");
		Console.WriteLine($"New event items: {newEventItems.Count}.");

		if (newEventItems.Count != 0)
			_ = await db.SaveEventsAsync(newEventItems);
	}


	// *** Db Only ***

	public static async Task<List<MessageSummary>> GetMessageSummariesFromDbAsync(App app, DateTime? lastDate, DateTime? firstDate)
	{
		using var db = new MailgunLogDb(app.DbService);

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


	//public static async Task FromFilesAsync(App app, AppSettings appSettings, long ts, int fileCount)
	//{
	//	string basePath = $@"D:\UserData\Documents\AppDev\Mailer\Mailer.EventReader\Output\";
	//	var mgep = new MailgunEventParser(appSettings.Mailgun.FromDomain);

	//	var files = new List<string>();
	//	var eventBlocks = new List<EventBlock>();
	//	var eventItems = new List<EventItem>();

	//	Console.WriteLine($"TimeStamp:{ts} FileCount:{fileCount}.");

	//	for (int i = 0; i < fileCount; i += 1)
	//	{
	//		files.Add(File.ReadAllText($"{basePath}mgEvents_{ts}_{i}.json"));
	//		eventBlocks.Add(mgep.ParseBlock(files[i]));
	//		eventItems.AddRange(eventBlocks[i].EventsWithFromDomain());
	//		Console.WriteLine($"File:{i} ItemCount:{eventBlocks[i].EventItems.Count}.");
	//	}

	//	eventItems = eventItems.DistinctBy(a => a._id).ToList();
	//	Console.WriteLine($"DistinctNewItemCount:{eventItems.Count}.");

	//	using var db = new MailgunLogDb(app.DbService);
	//	var idsInDb = await db.GetLatestEventIdsAsync(0, 200);
	//	Console.WriteLine($"EventsFoundInDb:{idsInDb.Count}.");

	//	var newEventItems = eventItems.Where(a => !idsInDb.Contains(a._id)).ToList();
	//	Console.WriteLine($"NewItemCount:{newEventItems.Count}.");

	//	if (newEventItems.Count != 0)
	//		_ = await db.SaveEventsAsync(newEventItems);
	//}

	//public static async Task UpdateAsync(App app)
	//{
	//	using var db = new MailgunLogDb(app.DbService);
	//	var items = await db.GetEventsDescAsync(0D, 0D, 0);

	//	if (items is null) return;

	//	foreach (var item in items)
	//	{
	//		// Do something to each item.
	//	}


	//	//_ = await db.SaveEventsAsync(items);
	//}
}
