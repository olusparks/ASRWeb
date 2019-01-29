using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASRWebMVC.Helper
{
	public static class HelperMtds
	{
		public static string IDGenerator(string userType)
		{
			string r = string.Empty;

			if (String.IsNullOrEmpty(userType))
				return r;

			if (userType.ToLower().Contains("e"))
			{
				Random generator = new Random();
				r = $"e{generator.Next(0, 99999).ToString("D5")}";
			}
			else if (userType.ToLower().Contains("s"))
			{
				Random generator = new Random();
				r = $"s{generator.Next(0, 9999999).ToString("D7")}";
			}
			return r;
		}

		public static string IDGenerator()
		{
			string[] alphabets = { "a", "b", "c", "d", "e", "f" };
			Random generator = new Random();
			return generator.Next(1, 999).ToString();
		}

		/// <summary>
		/// Get string value after [last] a.
		/// </summary>
		public static string After(this string value, string a)
		{
			int posA = value.LastIndexOf(a);
			if (posA == -1)
			{
				return "";
			}
			int adjustedPosA = posA + a.Length;
			if (adjustedPosA >= value.Length)
			{
				return "";
			}
			return value.Substring(adjustedPosA);
		}
	}
}
