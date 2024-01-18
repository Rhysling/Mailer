using Mailer.EventReader;
using Mailer.EventReader.Models;
using Microsoft.Extensions.Configuration;
using System.Text;

IConfigurationRoot config = new ConfigurationBuilder()
	.AddEnvironmentVariables()
	.Build();

AppSettings? settings = config.Get<AppSettings>();
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
string fromDomain = settings?.Mailgun.FromDomain ?? throw new ArgumentNullException(nameof(fromDomain));
string authValue = settings?.Mailgun.AuthValue ?? throw new ArgumentNullException(nameof(authValue));
#pragma warning restore CA2208 // Instantiate argument exceptions correctly

var mgs = new MailgunEventService(fromDomain, authValue);

//var res = await mgs.GetAsync();
//string path = $@"D:\UserData\Documents\AppDev\Mailer\Mailer.EventReader\Output\mgEvents_{res.ts}_0.json";

//string url = "https://api.mailgun.net/v3/american-research-capital.net/events/WzMseyJiIjoiMjAyNC0wMS0xOFQxNzo0OTozMy40ODUrMDA6MDAiLCJlIjoiMjAyNC0wMS0xM1QxNzo0OTozMy41MDgrMDA6MDAifSx7ImIiOiIyMDI0LTAxLTE0VDE3OjU4OjEwLjYxOSswMDowMCIsImUiOiIyMDI0LTAxLTEzVDE3OjQ5OjMzLjUwOCswMDowMCJ9LCJfZG9jI2ZpZEFVeUw3UUlXRzhXUHkyUGFIUFEiLFsiZiJdLG51bGwsW1siYWNjb3VudC5pZCIsIjU1MzI4OTEyNzhmYTE2NjNlNmZmNGE1OSJdLFsiZG9tYWluLm5hbWUiLCJhbWVyaWNhbi1yZXNlYXJjaC1jYXBpdGFsLm5ldCJdXSwxMDBd";
//var json = await mgs.GetNextAsync(url);
//string path = $@"D:\UserData\Documents\AppDev\Mailer\Mailer.EventReader\Output\mgEvents_next_1.json";

//File.WriteAllText(path, json);

string path = $@"D:\UserData\Documents\AppDev\Mailer\Mailer.EventReader\Output\mgEvents_1705601212_0.json";
string json = File.ReadAllText(path);
var mgep = new MailgunEventParser("american-research-capital.net");
var eb = mgep.ParseBlock(json);

string outPath = $@"D:\UserData\Documents\AppDev\Mailer\Mailer.EventReader\Output\mgEvents_comp.csv";

var sb = new StringBuilder();
sb.AppendLine("""
	"TimeStamp","EventType","Recipient"
	""");

foreach (var item in eb.EventItems)
{
	sb.AppendLine($"{item.timestamp},\"{item.eventType ?? "missing"}\",\"{item.recipient ?? "missing"}\"");
}

File.WriteAllText(outPath, sb.ToString());

Console.WriteLine("Done.");
Console.ReadKey();