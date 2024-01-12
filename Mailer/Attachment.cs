namespace Mailer;

public class Attachment(string fullPath)
{
	//private readonly string fullPath = fullPath;

	public string FileName => Path.GetFileName(fullPath);
	public string FullPath => fullPath;

	public string MimeType
	{
		get
		{
			string ext = Path.GetExtension(fullPath);

			return ext switch
			{
				".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
				".jpeg" => "image/jpeg",
				".jpg" => "image/jpeg",
				".png" => "image/png",
				".pdf" => "application/pdf",
				".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
				_ => throw new NotImplementedException($"'{ext}' MIME type not implemented.")
			};
		}
	}

}
