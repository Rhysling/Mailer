namespace Mailer
{
	public class MailMessage
	{
		private readonly Dictionary<string,string> mergeFields;
		private readonly string body;
		private readonly string subject;
		private readonly string toEmails; // ',' delimited
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


		public MailMessage(IMergeable mergeObj, string body, string subject, string toEmails, bool isHtml = true, List<Attachment>? attachments = null)
		{
			mergeFields = mergeObj.MergeFields();
			this.body = body;
			this.subject = subject;
			this.toEmails = toEmails;
			this.isHtml = isHtml;
			this.attachments = attachments ?? [];

			TimeZoneInfo pstZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
			DateTime pstNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, pstZone);

			mergeFields.Add("DateNow", pstNow.ToShortDateString());
			mergeFields.Add("TimeNow", pstNow.ToLongTimeString() + (pstZone.IsDaylightSavingTime(pstNow) ? " PDT" : " PST"));
			mergeFields.Add("Title", subject);
			//mergeFields.Add("LogDateGmt", model.EventDateGmt);
			//mergeFields.Add("LogLevelName", model.LevelName ?? "Missing");
			mergeFields.Add("LogLevelColor", levelColors[1]);
		
			// Render Body ***

			if (String.IsNullOrWhiteSpace(body))
				throw new ArgumentNullException(nameof(body));

			foreach (var r in mergeFields)
				body = body.Replace("[" + r.Key + "]", r.Value);

			if (body.Contains('[') || body.Contains(']'))
				throw new ArgumentException("Not all fields replaced ('[' or ']' found).");

		}

		// *** properties ***
		public string Body => body;

		public string ToEmails
		{
			get
			{
				if (String.IsNullOrWhiteSpace(toEmails))
					throw new ArgumentNullException("ToEmails cannot be empty.");

				return toEmails;
			}
		}

		public string Subject
		{
			get
			{
				if (String.IsNullOrWhiteSpace(subject))
					throw new ArgumentNullException("Subject cannot be empty.");

				return subject;
			}
		}

		public bool IsHtml => isHtml;

		public List<Attachment> Attachments => attachments;
	}
}
