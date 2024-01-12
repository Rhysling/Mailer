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
			List<IqaInvestorModel> lst = new();

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
				IqaInvestorModel item = new();
				item.Seq = reader.GetInt32("Seq");
				item.StakeholderName = reader.GetString("StakeholderName");
				item.TotalShares = reader.GetInt32("TotalShares");
				item.TotalCash = reader.GetDouble("TotalCash");
				item.Email = reader.GetString("Email");
				item.FirstName = reader.GetString("FirstName");
				lst.Add(item);
			}

			connection.Close();

			return lst;
		}
	}
}
