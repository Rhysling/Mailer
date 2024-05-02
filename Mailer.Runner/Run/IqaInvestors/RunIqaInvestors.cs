using Mailer.Runner.Db;

namespace Mailer.Runner.Run.IqaInvestors;

internal class RunIqaInvestors(string fromDomain, string authValue, string? bcc = null)
{
	public async Task Go()
	{
		string msgFullPath = @"D:\UserData\Documents\AppDev\Mailer\Mailer.Runner\Run\IqaInvestors\MessageText.html";
		string msgHtml = File.ReadAllText(msgFullPath);
		string subject = "IQ-Analog Investor Notice";
		bool isHtml = true;
		bool isTesting = false;


		List<Attachment> attachments = [
			new(@"D:\UserData\Documents\AppDev\Mailer\Mailer.Runner\Attachments\ARC - IQA Agreement and Consent to Surrender Collateral_EXECUTED .pdf"),
			new(@"D:\UserData\Documents\AppDev\Mailer\Mailer.Runner\Attachments\IQANALOGCORPORATION-CA-Filed and Approved by Ca Secretary of State.pdf")
		];

		var mgs = new MailgunService("noreply@american-research-capital.net", fromDomain, authValue);

		var db = new ReadDb();
		var investors = db.GetIqaInvestors();

		foreach (var inv in investors)
		{
			var m = new MailMessage(inv, msgHtml, subject, inv.Email, bcc, isHtml, attachments, isTesting);
			var res = await mgs.SendAsync(m);
			Console.WriteLine($"Email: {inv.Email}; Result: {res.StatusCode}");
		}
	}

}
