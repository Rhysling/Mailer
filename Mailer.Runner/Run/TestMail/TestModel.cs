namespace Mailer.Runner.Run.TestMail;

public class TestModel : IMergeable
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public int Seq { get; set; }
	public string DummyName { get; set; }
	public string Email { get; set; }

	public Dictionary<string, string> MergeFields()
	{
		return new Dictionary<string, string> {
			{ "DummyName", DummyName },
			{ "Email", Email }
		};
	}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}

