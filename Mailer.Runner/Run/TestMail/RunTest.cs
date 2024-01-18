using Mailer.Runner.Db;

namespace Mailer.Runner.Run.TestMail;

internal class RunTest(string fromDomain, string authValue)
{
	public async Task Go()
	{
		string msgFullPath = @"D:\UserData\Documents\AppDev\Mailer\Mailer.Runner\Run\TestMail\Tpl_Alert_compiled.html";
		string msgHtml = File.ReadAllText(msgFullPath);
		string subject = "Test Notice";
		bool isHtml = true;
		bool isTesting = false;

		Console.WriteLine($"Sending message(s) for: {subject}");

		var mgs = new MailgunService("noreply@american-research-capital.net", fromDomain, authValue);

		var item = new TestModel { DummyName = "Bob Tester", Email = "rpkummer@hotmail.com" };

		
		var m = new MailMessage(item, msgHtml, subject, item.Email, isHtml, attachments: null, isTesting);
		var res = await mgs.SendAsync(m);
		Console.WriteLine($"Email: {item.Email}; Result: {res.StatusCode}");
	}
}

