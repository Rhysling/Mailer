using Mailer.EventReader.Utilities;

namespace Mailer.EventReader.Runs
{
	public static class Reports
	{
		public static async Task<string> CreateMessageSummariesCsv(App app, DateTime? lastDate, DateTime? firstDate)
		{
			var msl = await Ops.GetMessageSummariesFromDbAsync(app, lastDate, firstDate);
			return EventFormatter.MessageSummariesToCsv(msl);
		}
		
	}
}
