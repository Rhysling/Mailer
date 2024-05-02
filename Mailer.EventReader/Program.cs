using Mailer.EventReader;
using Mailer.EventReader.Models;
using Mailer.EventReader.Runs;
using Microsoft.Extensions.Configuration;

IConfigurationRoot config = new ConfigurationBuilder()
	.AddJsonFile("appsettings.json")
	.Build();

AppSettings? settings = config.Get<AppSettings>() ?? throw new Exception("Settings are missing.");
var app = new App(settings);


//await AddEvents.FromFiles(app, settings, 1705689393, 2);
//await AddEvents.UpdateAsync(app);
//await AddEvents.FromMailgunAsync(app, settings);
//_ = await GetEvents.FromDbByMessageId(app, "20240114152621.f0c2253667245b94@american-research-capital.net");
//_ = await GetEvents.FromDbByRecipient(app, "rpkummer@hotmail.com");


//await Ops.UpdateEventsDbAsync(app, settings);

//string outPath = @"D:\yy\tp2\EventSummary.csv";
//string csv = await Reports.CreateMessageSummariesCsv(app, lastDate: null, firstDate: null);
//File.WriteAllText(outPath, csv);


// Testing ***
await Testing.UpdateTestEventsDbAsync(app);


app.MgEventClient.Dispose();
app.DbService.Dispose();

Console.WriteLine("Done.");
Console.ReadKey();