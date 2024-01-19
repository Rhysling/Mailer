using Newtonsoft.Json;

namespace Mailer.EventReader.Models;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable IDE1006 // Naming Styles
public class EventItem : CloudantDb.Models.ICloudantObj
{

	public EventItem()
	{
		tbl = "events";
		_rev = null;
	}

	public string _id
	{
		get
		{
			return $"e-{timestamp.ToString("F7")}";
		}

		set
		{
			_ = value; //no op
		}
	}

	public string? _rev { get; set; }
	public string tbl { get; set; }

	public string fromDomain { get; set; }

	public MsgEnvelope envelope { get; set; }

	public double timestamp { get; set; }

	[JsonProperty("log-level")]
	public string logLevel { get; set; }
	public object[] campaigns { get; set; }
	public MsgFlags flags { get; set; }

	[JsonProperty("event")]
	public string eventType { get; set; }

	[JsonProperty("recipient-domain")]
	public string recipientDomain { get; set; }

	[JsonProperty("originating-ip")]
	public string originatingIp { get; set; }

	public string id { get; set; }

	[JsonProperty("delivery-status")]
	public MsgDeliveryStatus deliveryStatus { get; set; }
	public MsgInfo message { get; set; }
	public string recipient { get; set; }
	public MsgStorage storage { get; set; }
	public MsgGeolocation geolocation { get; set; }
}

public class MsgEnvelope
{
	public string targets { get; set; }
	public string sendingip { get; set; }
	public string transport { get; set; }
	public string sender { get; set; }
}

public class MsgFlags
{
	public bool isrouted { get; set; }
	public bool issystemtest { get; set; }
	public bool istestmode { get; set; }
	public bool isauthenticated { get; set; }
}

public class MsgDeliveryStatus
{
	public bool certificateverified { get; set; }
	public bool utf8 { get; set; }
	public bool tls { get; set; }
	public double sessionseconds { get; set; }
	public string description { get; set; }
	public string enhancedcode { get; set; }
	public string message { get; set; }
	public int code { get; set; }
	public int attemptno { get; set; }
	public string mxhost { get; set; }
}

public class MsgInfo
{
	public int size { get; set; }
	public MsgHeaders headers { get; set; }
	public List<MsgAttachment> attachments { get; set; }
}

public class MsgAttachment
{
	public string filename { get; set; }

	[JsonProperty("content-type")]
	public string contentType { get; set; }
	public int size { get; set; }
}

public class MsgHeaders
{
	[JsonProperty("message-id")]
	public string messageId { get; set; }
	public string subject { get; set; }
	public string to { get; set; }
	public string from { get; set; }
}

public class MsgStorage
{
	public string env { get; set; }
	public string region { get; set; }
	public string key { get; set; }
	public string url { get; set; }
}

public class MsgGeolocation
{
	public string region { get; set; }
	public string city { get; set; }
	public string country { get; set; }
}

#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
