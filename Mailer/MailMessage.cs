namespace Mailer;

public class MailMessage
{
	private readonly string body;
	private readonly string subject;
	private readonly string toEmails; // ',' delimited
	private readonly string? bcc;
	private readonly bool isHtml = true;
	private readonly List<Attachment> attachments;

	private static readonly List<string> levelColors =
	[
		"#337ab7", //None = 0,
		"#5bc0de", //Info = 1,
		"#f0ad4e", //Warn = 2,
		"#d9534f", //Error = 3,
		"#d953ad"  //Fatal = 4
	];

	public MailMessage(
		IMergeable mergeObj,
		string template,
		string subject,
		string toEmails,
		string? bcc = null,
		bool isHtml = true,
		List<Attachment>? attachments = null,
		bool isTesting = false
	)
	{
		if (String.IsNullOrWhiteSpace(template))
			throw new ArgumentNullException(nameof(template));

		if (String.IsNullOrWhiteSpace(subject))
			throw new ArgumentNullException(nameof(subject));

		if (String.IsNullOrWhiteSpace(toEmails))
			throw new ArgumentNullException(nameof(toEmails));

		this.subject = isTesting ? $"TESTING -- {subject}" : subject;
		this.toEmails = toEmails;
		this.bcc = bcc;
		this.isHtml = isHtml;
		this.attachments = attachments ?? [];

		TimeZoneInfo pstZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
		DateTime pstNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, pstZone);

		var mergeFields = mergeObj.MergeFields();
		mergeFields.Add("DateNow", pstNow.ToShortDateString());
		mergeFields.Add("TimeNow", pstNow.ToLongTimeString() + (pstZone.IsDaylightSavingTime(pstNow) ? " PDT" : " PST"));
		mergeFields.Add("Title", subject);
		mergeFields.Add("LogLevelColor", levelColors[1]);

		string testingMsg = isTesting
			? """<p style="font-size:1.5em;font-weight:bold;color:red;margin:1em 0 1em 0;text-align:center;">* * * TEST MESSAGE * * *</p>"""
			: "";
		mergeFields.Add("TestingMsg", testingMsg);

		// Render Body ***

		foreach (var r in mergeFields)
			template = template.Replace($"[{r.Key}]", r.Value);

		//if (template.Contains('[') || template.Contains(']'))
		//	throw new ArgumentException("Not all fields replaced ('[' or ']' found).");

		body = template;
	}

	// *** properties ***
	public string Body => body;

	public string ToEmails => toEmails;

	public string? Bcc => bcc;

	public string Subject => subject;

	public bool IsHtml => isHtml;

	public List<Attachment> Attachments => attachments;
}
