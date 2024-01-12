using Mailer;
using Mailer.Runner;
using Mailer.Runner.Db;
using Microsoft.Extensions.Configuration;

IConfigurationRoot config = new ConfigurationBuilder()
	.AddEnvironmentVariables()
	.Build();

AppSettings? settings = config.Get<AppSettings>();
string fromDomain = settings?.Mailgun.FromDomain ?? throw new ArgumentNullException(nameof(fromDomain));
string authValue = settings?.Mailgun.AuthValue ?? throw new ArgumentNullException(nameof(authValue));


string msgFullPath = @"D:\UserData\Documents\AppDev\Mailer\Mailer.Runner\Run\IqaInvestors\MessageText.html";
string msgHtml = File.ReadAllText(msgFullPath);
string subject = "IQ-Analog Investor Notice";


List<Attachment> attachments = [
	new(@"D:\UserData\Documents\AppDev\Mailer\Mailer.Runner\Attachments\ARC - IQA Agreement and Consent to Surrender Collateral_EXECUTED .pdf"),
	new(@"D:\UserData\Documents\AppDev\Mailer\Mailer.Runner\Attachments\IQANALOGCORPORATION-CA-Filed and Approved by Ca Secretary of State.pdf")
];

var mgs = new MailgunService("noreply@american-research-capital.net", fromDomain, authValue);


var db = new ReadDb();
var investors = db.GetIqaInvestors();

foreach (var inv in investors)
{
	var m = new MailMessage(inv, msgHtml, subject, inv.Email, isHtml: true, attachments);
	var res = await mgs.SendAsync(m);
	Console.WriteLine($"Email: {inv.Email}; Result: {res.StatusCode}");
}




//Console.WriteLine($"From: {settings?.Mailgun.FromDomain ?? "missing"}");
//Console.WriteLine($"Auth: {settings?.Mailgun.AuthValue ?? "missing"}");

Console.WriteLine("Done.");
Console.ReadKey();
