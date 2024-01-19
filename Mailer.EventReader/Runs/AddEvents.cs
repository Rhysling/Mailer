using Mailer.EventReader.Db;
using Mailer.EventReader.Models;
using System;

namespace Mailer.EventReader.Runs;

public static class AddEvents
{
	public static async Task FromMailgun(App app, AppSettings appSettings)
	{
		//TODO:  Fix this ***


		string outBasePath = $@"D:\UserData\Documents\AppDev\Mailer\Mailer.EventReader\Output\";
		var mges = new MailgunEventService(app);
		var mgep = new MailgunEventParser(appSettings.Mailgun.FromDomain);

		var files = new List<string>();
		var leb = new List<EventBlock>();
		int i = 0;

		var res = await mges.GetAsync();
		files.Add(res.json);
		leb.Add(mgep.ParseBlock(res.json));


		while (true)
		{
			if (leb[i].EventItems.Count == 0) break;
			string json = await mges.GetNextAsync(leb[i].NextUrl);
			files.Add(json);
			leb.Add(mgep.ParseBlock(json));
			i += 1;
		}

		i = 0;
		foreach (var f in files)
		{
			File.WriteAllText($"{outBasePath}mgEvents_{res.ts}_{i}.json", f);
			i += 1;
		}


		//string path = $@"D:\UserData\Documents\AppDev\Mailer\Mailer.EventReader\Output\mgEvents_{res.ts}_0.json";

		//string url = "https://api.mailgun.net/v3/american-research-capital.net/events/WzMseyJiIjoiMjAyNC0wMS0xOFQxNzo0OTozMy40ODUrMDA6MDAiLCJlIjoiMjAyNC0wMS0xM1QxNzo0OTozMy41MDgrMDA6MDAifSx7ImIiOiIyMDI0LTAxLTE0VDE3OjU4OjEwLjYxOSswMDowMCIsImUiOiIyMDI0LTAxLTEzVDE3OjQ5OjMzLjUwOCswMDowMCJ9LCJfZG9jI2ZpZEFVeUw3UUlXRzhXUHkyUGFIUFEiLFsiZiJdLG51bGwsW1siYWNjb3VudC5pZCIsIjU1MzI4OTEyNzhmYTE2NjNlNmZmNGE1OSJdLFsiZG9tYWluLm5hbWUiLCJhbWVyaWNhbi1yZXNlYXJjaC1jYXBpdGFsLm5ldCJdXSwxMDBd";
		//var json = await mgs.GetNextAsync(url);
		//string path = $@"D:\UserData\Documents\AppDev\Mailer\Mailer.EventReader\Output\mgEvents_next_1.json";

	}

	public static async Task FromFiles(App app, AppSettings appSettings, long ts, int fileCount)
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
		var idsInDb = await db.GetLatestEventIdsAsync(200);
		Console.WriteLine($"EventsFoundInDb:{idsInDb.Count}.");

		var newEventItems = eventItems.Where(a => !idsInDb.Contains(a._id)).ToList();
		Console.WriteLine($"NewItemCount:{newEventItems.Count}.");

		if (newEventItems.Any())
			_ = await db.SaveEventsAsync(newEventItems);
	}

}
