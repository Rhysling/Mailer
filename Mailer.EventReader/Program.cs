using Mailer.EventReader;
using Mailer.EventReader.Models;
using Mailer.EventReader.Runs;
using Mailer.EventReader.Utilities;
using Microsoft.Extensions.Configuration;

IConfigurationRoot config = new ConfigurationBuilder()
	.AddJsonFile("appsettings.json")
	.Build();

AppSettings? settings = config.Get<AppSettings>() ?? throw new Exception("Settings are missing.");
var app = new App(settings);


//await GetEvents.FromMailgun(app, settings);
//GetEvents.FromFiles(settings, 240118, 2);
//await GetEvents.FromDb(app);
//await AddEvents.FromFiles(app, settings, 1705689393, 2);
//await AddEvents.UpdateAsync(app);
//await AddEvents.FromMailgunAsync(app, settings);
//_ = await GetEvents.FromDbByMessageId(app, "20240114152621.f0c2253667245b94@american-research-capital.net");
//_ = await GetEvents.FromDbByRecipient(app, "rpkummer@hotmail.com");


string outPath = @"D:\yy\tp2\EventSummary.csv";
var dbo = new DbOps(app);
var msl = await dbo.GetMessageSummaries(null, null);
string res = EventFormatter.MessageSummariesToCsv(msl);
File.WriteAllText(outPath, res);


app.MgEventClient.Dispose();
app.DbService.Dispose();

Console.WriteLine("Done.");
Console.ReadKey();