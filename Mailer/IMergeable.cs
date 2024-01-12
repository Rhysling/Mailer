namespace Mailer;

public interface IMergeable
{
	Dictionary<string, string> MergeFields();
}