//using System;


namespace TFilesEvenSLEbin {
	class BinarySearcher {


		//string str = "0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmno";
		//char = Function [n, StringTake[str, {n, n}]]
		//Table[char[n], {n, 32, 64, 4}]
		//{32, 36, 40, 44, 48, 52, 56, 60, 64}
		//{"O", "S", "W", "[", "_", "c", "g", "k", "o"}
		//string str = "UWY^egio";//dictionary volumes with letters [..U][..W]...[..o] and anything above o is sent to the last volume
		//string str = @"[\]^_elo";//dictionary volumes for T good choice
		public static int BinarySearch(char key, int volumeslog2=0,string volumes_str="") {
			if (volumeslog2 == 0) { volumeslog2 = 3; volumes_str = @"UWY^egio"; }
			int low = 0;//T
			int high = volumes_str.Length - 1;//o
			int middle;
			int count = 0;
			while (low <= high && count < volumeslog2) {//length of str is assumed 2^volumes=8 and so the count is 3 as 2^3.
				middle = (low + high) / 2;
				if (key > volumes_str[middle])
					low = middle + 1;
				else if (key < volumes_str[middle])
					high = middle - 1;
				else
					return middle;
				++count;
			}
			return low;
		}
		/*
		public static void Example() {
			Console.WriteLine("UWY^egio");
			//For[i = 1, i <= 64, i++,Print["Console.WriteLine(BinarySearcher.BinarySearch('" <> char[i] <> "').ToString()+'" <> char[i] <> "');"]]
			Console.WriteLine(BinarySearcher.BinarySearch('0').ToString() + '0');
			Console.WriteLine(BinarySearcher.BinarySearch('1').ToString() + '1');
			Console.WriteLine(BinarySearcher.BinarySearch('2').ToString() + '2');
			Console.WriteLine(BinarySearcher.BinarySearch('3').ToString() + '3');
			Console.WriteLine(BinarySearcher.BinarySearch('4').ToString() + '4');
			Console.WriteLine(BinarySearcher.BinarySearch('5').ToString() + '5');
			Console.WriteLine(BinarySearcher.BinarySearch('6').ToString() + '6');
			Console.WriteLine(BinarySearcher.BinarySearch('7').ToString() + '7');
			Console.WriteLine(BinarySearcher.BinarySearch('8').ToString() + '8');
			Console.WriteLine(BinarySearcher.BinarySearch('9').ToString() + '9');
			Console.WriteLine(BinarySearcher.BinarySearch(':').ToString() + ':');
			Console.WriteLine(BinarySearcher.BinarySearch(';').ToString() + ';');
			Console.WriteLine(BinarySearcher.BinarySearch('<').ToString() + '<');
			Console.WriteLine(BinarySearcher.BinarySearch('=').ToString() + '=');
			Console.WriteLine(BinarySearcher.BinarySearch('>').ToString() + '>');
			Console.WriteLine(BinarySearcher.BinarySearch('?').ToString() + '?');
			Console.WriteLine(BinarySearcher.BinarySearch('@').ToString() + '@');
			Console.WriteLine(BinarySearcher.BinarySearch('A').ToString() + 'A');
			Console.WriteLine(BinarySearcher.BinarySearch('B').ToString() + 'B');
			Console.WriteLine(BinarySearcher.BinarySearch('C').ToString() + 'C');
			Console.WriteLine(BinarySearcher.BinarySearch('D').ToString() + 'D');
			Console.WriteLine(BinarySearcher.BinarySearch('E').ToString() + 'E');
			Console.WriteLine(BinarySearcher.BinarySearch('F').ToString() + 'F');
			Console.WriteLine(BinarySearcher.BinarySearch('G').ToString() + 'G');
			Console.WriteLine(BinarySearcher.BinarySearch('H').ToString() + 'H');
			Console.WriteLine(BinarySearcher.BinarySearch('I').ToString() + 'I');
			Console.WriteLine(BinarySearcher.BinarySearch('J').ToString() + 'J');
			Console.WriteLine(BinarySearcher.BinarySearch('K').ToString() + 'K');
			Console.WriteLine(BinarySearcher.BinarySearch('L').ToString() + 'L');
			Console.WriteLine(BinarySearcher.BinarySearch('M').ToString() + 'M');
			Console.WriteLine(BinarySearcher.BinarySearch('N').ToString() + 'N');
			Console.WriteLine(BinarySearcher.BinarySearch('O').ToString() + 'O');
			Console.WriteLine(BinarySearcher.BinarySearch('P').ToString() + 'P');
			Console.WriteLine(BinarySearcher.BinarySearch('Q').ToString() + 'Q');
			Console.WriteLine(BinarySearcher.BinarySearch('R').ToString() + 'R');
			Console.WriteLine(BinarySearcher.BinarySearch('S').ToString() + 'S');
			Console.WriteLine(BinarySearcher.BinarySearch('T').ToString() + 'T');
			Console.WriteLine(BinarySearcher.BinarySearch('U').ToString() + 'U');
			Console.WriteLine(BinarySearcher.BinarySearch('V').ToString() + 'V');
			Console.WriteLine(BinarySearcher.BinarySearch('W').ToString() + 'W');
			Console.WriteLine(BinarySearcher.BinarySearch('X').ToString() + 'X');
			Console.WriteLine(BinarySearcher.BinarySearch('Y').ToString() + 'Y');
			Console.WriteLine(BinarySearcher.BinarySearch('Z').ToString() + 'Z');
			Console.WriteLine(BinarySearcher.BinarySearch('[').ToString() + '[');
			Console.WriteLine(BinarySearcher.BinarySearch('\\').ToString() + '\\');
			Console.WriteLine(BinarySearcher.BinarySearch(']').ToString() + ']');
			Console.WriteLine(BinarySearcher.BinarySearch('^').ToString() + '^');
			Console.WriteLine(BinarySearcher.BinarySearch('_').ToString() + '_');
			Console.WriteLine(BinarySearcher.BinarySearch('`').ToString() + '`');
			Console.WriteLine(BinarySearcher.BinarySearch('a').ToString() + 'a');
			Console.WriteLine(BinarySearcher.BinarySearch('b').ToString() + 'b');
			Console.WriteLine(BinarySearcher.BinarySearch('c').ToString() + 'c');
			Console.WriteLine(BinarySearcher.BinarySearch('d').ToString() + 'd');
			Console.WriteLine(BinarySearcher.BinarySearch('e').ToString() + 'e');
			Console.WriteLine(BinarySearcher.BinarySearch('f').ToString() + 'f');
			Console.WriteLine(BinarySearcher.BinarySearch('g').ToString() + 'g');
			Console.WriteLine(BinarySearcher.BinarySearch('h').ToString() + 'h');
			Console.WriteLine(BinarySearcher.BinarySearch('i').ToString() + 'i');
			Console.WriteLine(BinarySearcher.BinarySearch('j').ToString() + 'j');
			Console.WriteLine(BinarySearcher.BinarySearch('k').ToString() + 'k');
			Console.WriteLine(BinarySearcher.BinarySearch('l').ToString() + 'l');
			Console.WriteLine(BinarySearcher.BinarySearch('m').ToString() + 'm');
			Console.WriteLine(BinarySearcher.BinarySearch('n').ToString() + 'n');
			Console.WriteLine(BinarySearcher.BinarySearch('o').ToString() + 'o');
			Console.ReadKey();
		}
		*/
	
	}
}