using Mailer.EventReader;
using Mailer.EventReader.Models;
using Mailer.EventReader.Runs;
using Microsoft.Extensions.Configuration;
using System.Text;

IConfigurationRoot config = new ConfigurationBuilder()
	.AddJsonFile("appsettings.json")
	.Build();

AppSettings? settings = config.Get<AppSettings>() ?? throw new Exception("Settings are missing.");
var app = new App(settings);


//await GetEvents.FromMailgun(app, settings);
//GetEvents.FromFiles(settings, 240118, 2);
//await GetEvents.FromDb(app);
//await AddEvents.FromFiles(app, settings, 1705689393, 2);
await AddEvents.Update(app);


//string outPath = $@"D:\UserData\Documents\AppDev\Mailer\Mailer.EventReader\Output\mgEvents_comp.csv";

//var sb = new StringBuilder();
//sb.AppendLine("""
//	"TimeStamp","EventType","Recipient"
//	""");

//foreach (var item in eb.EventItems)
//{
//	sb.AppendLine($"{item.timestamp},\"{item.eventType ?? "missing"}\",\"{item.recipient ?? "missing"}\"");
//}

//File.WriteAllText(outPath, sb.ToString());


app.MgEventClient.Dispose();
app.DbService.Dispose();

Console.WriteLine("Done.");
Console.ReadKey();