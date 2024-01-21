using Mailer.EventReader.Models;
using System.Text;

namespace Mailer.EventReader.Utilities
{
	public static class EventFormatter
	{
		public static string MessageSummariesToCsv(List<MessageSummary> list)
		{
			StringBuilder sb = new();

			sb.AppendLine("\"MESSAGE SUMMARIES\"");
			sb.AppendLine();
			sb.AppendLine($"""
				"From:","{list[0].HumanDate}"
				""");
			sb.AppendLine($"""
				"To:","{list[^1].HumanDate}"
				""");
			sb.AppendLine();
			sb.AppendLine("""
				"MessageId","TimeStamp","HumanDate","Recipient","Subject"
				""");
			sb.AppendLine("""
				"","TimeStamp","HumanDate","EventType","GeoSummary"
				""");
			sb.AppendLine();

			foreach (MessageSummary s in list)
			{
				sb.AppendLine($"""
				"{s.MessageId}","{s.TimeStamp}","{s.HumanDate}","{s.Recipient}","{s.Subject}"
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
