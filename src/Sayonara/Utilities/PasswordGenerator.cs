using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sayonara.Utilities
{
  public class PasswordGenerator
  {
		public static string Generate(int passwordLength)
		{
			string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?_-";
			string specialChars = "!@$?_-";
			char[] chars = new char[passwordLength];
			Random rd = new Random();

			for (int i = 0; i < passwordLength - 1; i++)			
				chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];

			chars[passwordLength - 2] = specialChars[rd.Next(0, specialChars.Length)];

			return new string(chars);
		}
	}		
}
