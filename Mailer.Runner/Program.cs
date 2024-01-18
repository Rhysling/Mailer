using Mailer;
using Mailer.Runner;
using Mailer.Runner.Run.IqaInvestors;
using Mailer.Runner.Run.TestMail;
using Microsoft.Extensions.Configuration;

IConfigurationRoot config = new ConfigurationBuilder()
	.AddEnvironmentVariables()
	.Build();

AppSettings? settings = config.Get<AppSettings>();
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
string fromDomain = settings?.Mailgun.FromDomain ?? throw new ArgumentNullException(nameof(fromDomain));
string authValue = settings?.Mailgun.AuthValue ?? throw new ArgumentNullException(nameof(authValue));
#pragma warning restore CA2208 // Instantiate argument exceptions correctly


//RunIqaInvestors runner = new(fromDomain, authValue);
RunTest runner = new(fromDomain, authValue);

await runner.Go();

Console.WriteLine("Done.");
Console.ReadKey();
