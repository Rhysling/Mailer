using Mailer.Runner.Run.IqaInvestors;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;

namespace Mailer.Runner.Db
{
	internal class ReadDb
	{
		readonly string cs = "Data Source=localhost;Initial Catalog=TestingPocos;Integrated Security=True;Trust Server Certificate=True";
		public ReadDb()
		{

		}

		public List<IqaInvestorModel> GetIqaInvestors()
		{
			List<IqaInvestorModel> lst = [];

			using var connection = new SqlConnection(cs);
			connection.Open();


			using var command = new SqlCommand();
			command.Connection = connection;
			command.CommandType = CommandType.Text;
			command.CommandText = """  
				SELECT
					Seq,
					StakeholderName,
					TotalShares,
					TotalCash,
					Email,
					FirstName
				FROM [TestingPocos].[dbo].[IqaInvestors];
				""";

			SqlDataReader reader = command.ExecuteReader();

			while (reader.Read())
			{
				IqaInvestorModel item = new()
				{
					Seq = reader.GetInt32("Seq"),
					StakeholderName = reader.GetString("StakeholderName"),
					TotalShares = reader.GetInt32("TotalShares"),
					TotalCash = reader.GetDouble("TotalCash"),
					Email = reader.GetString("Email"),
					FirstName = reader.GetString("FirstName")
				};
				lst.Add(item);
			}

			connection.Close();

			return lst;
		}
	}
}
