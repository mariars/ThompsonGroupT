using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace TFilesEvenSLEbin {

	class ReadstrLine {

		static public void Example() {
			string[] strarray = ReadstrLine.WordsMin(" 23434  asdfs23423;jkldsf ");
			for (int i = 0; i < strarray.Length; i++) {
				Console.WriteLine("Word[" + i + "]=" + strarray[i]);
			}
			strarray = ReadstrLine.WordsMin("      2 fred 2000");
			for (int i = 0; i < strarray.Length; i++) {
				Console.WriteLine("Word[" + i + "]=" + strarray[i]);
			}
			string st = strarray[2];
			Console.WriteLine(2 * ReadstrLine.StrToInteger(st));

			Console.ReadKey();

		}

		static public string[] Words(string strLine) {
			return strLine.Split(' ');
		}

		static public string[] WordsMin(string strLine) {
			string str = strLine.Trim();
			return str.Split(' ');
		}

	
		static public int StrToInteger(string strWord) {
			return int.Parse(strWord);
		}

/*
		static public string ExtractXXIntegerAsString(string strLine) {
			return Regex.Match(strLine, @"\d+(?!\D*\d)").Value;
		}


		//	( start a capture group
		//	\d a shorthand character class, which matches all numbers; it is the same as [0-9]
		//	+ one or more of the expression
		//	) end a capture group
		//	/ a literal forward slash
		//	( start a capture group
		//	\d a shorthand character class, which matches all numbers; it is the same as [0-9]
		//	+ one or more of the expression
		//	) end a capture group

		static public string ExtractYYIntegerAsString(string strLine) {

			String number = Regex.Match(strLine, @"\d+").Value;
			return number;
		}
*/


	}

}