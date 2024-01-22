using Mailer.EventReader.Models;
using System.Text;

namespace Mailer.EventReader.Utilities
{
	public static class EventFormatter
	{
		public static string MessageSummariesToCsv(List<MessageSummary> list)
		{
			var evSums = list.SelectMany(a => a.EventSummaries);
			string fromDate = evSums.MinBy(a => a.TimeStamp)?.HumanDate ?? "none";
			string toDate = evSums.MaxBy(a => a.TimeStamp)?.HumanDate ?? "none";

			StringBuilder sb = new();

			sb.AppendLine("\"MESSAGE SUMMARIES\"");
			sb.AppendLine();
			sb.AppendLine($"""
				"From:","{fromDate}"
				""");
			sb.AppendLine($"""
				"To:","{toDate}"
				""");
			sb.AppendLine();
			sb.AppendLine("""
				"MessageId","","","Recipient","Subject"
				""");
			sb.AppendLine("""
				"","TimeStamp","HumanDate","EventType","GeoSummary"
				""");
			sb.AppendLine();

			foreach (MessageSummary s in list)
			{
				sb.AppendLine($"""
				"{s.MessageId}","","","{s.Recipient}","{s.Subject}"
				""");

				foreach(EventSummary e in s.EventSummaries)
				{
					sb.AppendLine($"""
					"","{e.TimeStamp}","{e.HumanDate}","{e.EventType}","{e.GeoSummary}"
					""");
				}

				sb.AppendLine();
			}

			return sb.ToString();
		}

	}
}
