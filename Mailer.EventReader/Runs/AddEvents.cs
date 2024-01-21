using Mailer.EventReader.Db;
using Mailer.EventReader.Models;

namespace Mailer.EventReader.Runs;

public static class AddEvents
{
	public static async Task FromMailgunAsync(App app, AppSettings appSettings)
	{
		//TODO:  Fix this ***
		// Grab IDs from db (last 300)
		// Get most recent timestamp, deduct 20 min.
		// Query MG for all after that ts
		// Remove dupes
		// Save to db

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

	public static async Task FromFilesAsync(App app, AppSettings appSettings, long ts, int fileCount)
	{
		string basePath = $@"D:\UserData\Documents\AppDev\Mailer\Mailer.EventReader\Output\";
		var mgep = new MailgunEventParser(appSettings.Mailgun.FromDomain);

		var files = new List<string>();
		var eventBlocks = new List<EventBlock>();
		var eventItems = new List<EventItem>();

		Console.WriteLine($"TimeStamp:{ts} FileCount:{fileCount}.");

		for (int i = 0; i < fileCount; i += 1)
		{
			files.Add(File.ReadAllText($"{basePath}mgEvents_{ts}_{i}.json"));
			eventBlocks.Add(mgep.ParseBlock(files[i]));
			eventItems.AddRange(eventBlocks[i].EventsWithFromDomain());
			Console.WriteLine($"File:{i} ItemCount:{eventBlocks[i].EventItems.Count}.");
		}

		eventItems = eventItems.DistinctBy(a => a._id).ToList();
		Console.WriteLine($"DistinctNewItemCount:{eventItems.Count}.");

		using var db = new MailgunLogDb(app.DbService);
		var idsInDb = await db.GetLatestEventIdsAsync(0, 200);
		Console.WriteLine($"EventsFoundInDb:{idsInDb.Count}.");

		var newEventItems = eventItems.Where(a => !idsInDb.Contains(a._id)).ToList();
		Console.WriteLine($"NewItemCount:{newEventItems.Count}.");

		if (newEventItems.Count != 0)
			_ = await db.SaveEventsAsync(newEventItems);
	}

	public static async Task UpdateAsync(App app)
	{
		using var db = new MailgunLogDb(app.DbService);
		var items = await db.GetEventsDescAsync(0D, 0D, 200);

		if (items is null) return;

		int i = 0;
		foreach (var item in items)
		{
			Console.WriteLine($"{i:d3} Timestamp: {item.timestamp:f7} | Human: {item.humanDate}");
			i += 1;
		}

		//items.RemoveAt(i - 1);

		//_ = await db.SaveEventsAsync(items);
	}
}
