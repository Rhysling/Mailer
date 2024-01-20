using Mailer.EventReader.Db;
using Mailer.EventReader.Models;

namespace Mailer.EventReader.Runs;

public static class GetEvents
{
	public static async Task FromMailgun(App app, AppSettings appSettings)
	{
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

	public static void FromFiles(AppSettings appSettings, long ts, int fileCount)
	{
		string outBasePath = $@"D:\UserData\Documents\AppDev\Mailer\Mailer.EventReader\Output\";
		var mgep = new MailgunEventParser(appSettings.Mailgun.FromDomain);

		var files = new List<string>();
		var leb = new List<EventBlock>();
		var items = new List<EventItem>();

		Console.WriteLine($"TimeStamp:{ts} FileCount:{fileCount}.");

		for (int i = 0; i < fileCount; i += 1) {
			files.Add(File.ReadAllText($"{outBasePath}mgEvents_{ts}_{i}.json"));
			leb.Add(mgep.ParseBlock(files[i]));
			items.AddRange(leb[i].EventItems);
			Console.WriteLine($"File:{i} ItemCount:{leb[i].EventItems.Count}.");
		}

		
		var di = items.DistinctBy(a => a._id).ToList();
		Console.WriteLine($"DistinctCount:{di.Count}.");
	}

	public static async Task FromDb(App app)
	{
		using var db = new MailgunLogDb(app.DbService);

		var evList = await db.GetLatestEventsAsync(100);

		Console.WriteLine($"EventCount:{evList.Count}.");
	}

}
