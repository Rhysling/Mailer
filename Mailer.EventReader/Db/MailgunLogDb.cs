using Mailer.EventReader.Models;

namespace Mailer.EventReader.Db
{
	public class MailgunLogDb : IDisposable
	{

		private readonly CloudantDb.Services.DbService db;


		public MailgunLogDb(CloudantDb.Services.DbService dbIn)
		{
			db = dbIn;
		}


		public async Task<List<EventItem>> GetLatestEventsAsync(int count)
		{
			var qp = new CloudantDb.Models.QueryParams
			{
				Include_Docs = true,
				Startkey = "e-999999",
				Endkey = $"e-000000",
				Descending = true,
				Limit = count
			};

			return await db.GetViewItemsAsync<EventItem>("app", "events-by-id", qp).ConfigureAwait(false);
		}

		public async Task<List<string>> GetLatestEventIdsAsync(int count)
		{
			var qp = new CloudantDb.Models.QueryParams
			{
				Include_Docs = false,
				Startkey = "e-999999",
				Endkey = $"e-000000",
				Descending = true,
				Limit = count
			};

			return (await db.GetViewIdsValues("app", "events-by-id", qp).ConfigureAwait(false)).Select(a => a.id).ToList();
		}

		public async Task<string> SaveEventAsync(EventItem item)
		{
			return await db.SaveItemAsync(item).ConfigureAwait(false);
		}

		public async Task<string> SaveEventsAsync(IEnumerable<EventItem> items)
		{
			return await db.SaveItemBatchAsync(items).ConfigureAwait(false);
		}


		//public async Task<List<PostItem>> GetPostsByIdsAsync(List<string> ids)
		//{
		//	return await db.GetViewItemsAsync<PostItem>("app", "posts-by-id", ids).ConfigureAwait(false);
		//}

		//public async Task<List<KVP>> GetPostCountByFeedIdAsync()
		//{
		//	var res = await db.GetViewKeysValues("app", "postcount-by-feedid", isGrouped: true).ConfigureAwait(false);
		//	return res.Select(a => new KVP { Key = a.key, Value = FormatNum(a.val) }).ToList();

		//	static string FormatNum(string str)
		//	{
		//		if (str.IsEmpty()) return "missing";

		//		if (int.TryParse(str, out var num))
		//		{
		//			return num.ToString("n0");
		//		}
		//		return "bad";
		//	}
		//}

		//public async Task<Dictionary<string, string>> GetLastPostForAllFeedsAsync()
		//{
		//	//var qp = new CloudantDb.Models.QueryParams { Group = true };

		//	return await db.GetViewDictionaryAsync("app", "feeds-latest-postid").ConfigureAwait(false);
		//}

		//public async Task<string> SavePostsAsync(IEnumerable<PostItem> posts)
		//{
		//	return await db.SaveItemBatchAsync(posts).ConfigureAwait(false);
		//}



		//public async Task<Feed> GetFeedById(string id)
		//{
		//	return await db.GetItemAsync<Feed>(id).ConfigureAwait(false);
		//}

		//public async Task<List<Feed>> GetFeedsByIds(IEnumerable<string> ids)
		//{
		//	return await db.GetItemsAsync<Feed>(ids).ConfigureAwait(false);
		//}


		//public async Task<List<Feed>> GetAllFeedsAsync(string feedType)
		//{
		//	var qp = new CloudantDb.Models.QueryParams
		//	{
		//		Include_Docs = true,
		//		Startkey = $"f-{feedType}-",
		//		Endkey = $"f-{feedType}-ZZZ",
		//		Descending = false
		//	};

		//	return await db.GetViewItemsAsync<Feed>("app", "feeds-by-id", qp).ConfigureAwait(false);
		//}

		//public async Task<List<Feed>> GetAllFeedsAsync()
		//{
		//	var qp = new CloudantDb.Models.QueryParams
		//	{
		//		Include_Docs = true
		//	};

		//	return await db.GetViewItemsAsync<Feed>("app", "feeds-all", qp).ConfigureAwait(false);
		//}

		//public async Task<List<string>> GetAllUserFeedIdsAsync()
		//{
		//	// Excludes isDisabled users.
		//	var res = await db.GetViewKeysValues("app", "feedids-all-users").ConfigureAwait(false);
		//	return res.Select(a => a.key).Distinct().ToList();
		//}

		//public async Task<List<Feed>> GetFeedsForUser(string userId)
		//{
		//	var qp = new CloudantDb.Models.QueryParams
		//	{
		//		Include_Docs = true,
		//		Key = userId
		//	};

		//	var feeds = await db.GetViewItemsAsync<Feed>("app", "feeds-by-user", qp).ConfigureAwait(false);
		//	feeds = feeds.Where(a => a is not null).ToList();

		//	// Trim ' - Go Comics' from Description
		//	feeds = feeds.Select(a => {
		//		a.Description = a.Description?.Replace(" - GoComics", "") ?? a.Title;
		//		return a;
		//	}).ToList();

		//	return feeds.OrderBy(a => a.Title).ToList();
		//}



		//public async Task<List<LogItemOut>> GetLogItemsAsync(DateTime asOfDate, int takeCount, bool isAllForDate)
		//{
		//	DateTime baseDate = new(asOfDate.Year, asOfDate.Month, asOfDate.Day);
		//	string startKey = "Log-" + baseDate.AddDays(1).ToJsTime().ToString().PadLeft(19, '0');

		//	var qp = new CloudantDb.Models.QueryParams
		//	{
		//		Include_Docs = true,
		//		Descending = true,
		//		Startkey = startKey
		//	};

		//	if (isAllForDate)
		//		qp.Endkey = "Log-" + baseDate.ToJsTime().ToString().PadLeft(19, '0');
		//	else
		//		qp.Limit = takeCount;

		//	return await db.GetViewItemsAsync<LogItemOut>("app", viewName: "log-all", queryParams: qp).ConfigureAwait(false);
		//}




		// IDisposable Implementation

		bool _disposed = false;

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~MailgunLogDb()
		{
			Dispose(false);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (_disposed)
				return;

			if (disposing)
			{
				// free other managed objects that implement IDisposable only
				db.Dispose();
			}

			// release any unmanaged objects
			// set the object references to null

			_disposed = true;
		}

	}
}
