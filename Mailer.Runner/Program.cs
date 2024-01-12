using Mailer;
using Mailer.Runner;
using Mailer.Runner.Db;
using Microsoft.Extensions.Configuration;

IConfigurationRoot config = new ConfigurationBuilder()
	.AddEnvironmentVariables()
	.Build();

AppSettings? settings = config.Get<AppSettings>();
string fromDaomain = settings?.Mailgun.FromDomain ?? throw new ArgumentNullException(nameof(fromDaomain));
string authValue = settings?.Mailgun.AuthValue ?? throw new ArgumentNullException(nameof(authValue));



List<Attachment> attachments = [
	new(@"D:\UserData\Documents\AppDev\Mailer\Mailer.Runner\Attachments\ARC - IQA Agreement and Consent to Surrender Collateral_EXECUTED .pdf"),
	new(@"D:\UserData\Documents\AppDev\Mailer\Mailer.Runner\Attachments\IQANALOGCORPORATION-CA-Filed and Approved by Ca Secretary of State.pdf")
];

var mgs = new MailgunService("noreply@american-research-capital.net", fromDaomain, authValue);


var db = new ReadDb();
var investors = db.GetIqaInvestors();

foreach (var inv in investors)
{

}




//Console.WriteLine($"From: {settings?.Mailgun.FromDomain ?? "missing"}");
//Console.WriteLine($"Auth: {settings?.Mailgun.AuthValue ?? "missing"}");

Console.WriteLine("Done.");
Console.ReadKey();
