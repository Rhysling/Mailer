﻿using System.Web;
using System;

namespace Mailer.Runner.Run.IqaInvestors;

public class IqaInvestorModel : IMergeable
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public int Seq { get; set; }
	public string StakeholderName { get; set; }
	public int TotalShares { get; set; }
	public double TotalCash { get; set; }
	public string Email { get; set; }
	public string DearName { get; set; }

	public Dictionary<string, string> MergeFields()
	{
		return new Dictionary<string, string> {
			{ "StakeholderName", StakeholderName },
			{ "TotalShares", TotalShares.ToString("n0") },
			{ "TotalCash", TotalCash.ToString("c0") },
			{ "Email", Email },
			{ "DearName", DearName },
			{ "EmailUrl", HttpUtility.UrlEncode(Email) }
		};
	}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
