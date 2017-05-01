using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace TFilesEvenSLEbin {
	class ExecuteOdometers {
		//Base64: "0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmno"
		//string str = "0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmno";
		//char = Function [n, StringTake[str, {n, n}]]
		//Table[char[n], {n, 32, 64, 4}]
		//{32, 36, 40, 44, 48, 52, 56, 60, 64}
		//{"O", "S", "W", "[", "_", "c", "g", "k", "o"}

		

		public void ExecuteX0X1onOdometerBinString(int n, WBuffer FileLog) {

			Console.WriteLine("Any key for progress info. x to stop program.");
			Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " Started");
			FileLog.WriteLine("Computing S(" + n + ")");

			string filename = "n" + n + ".forest";
			string fileInversename = "i" + n + ".forest";

			// open file for writing. fills buffer with 1000 chars and then writes it.
			WBuffer FileForests = new WBuffer(filename, 1, false);//do not append to file
			WBuffer FileInverseForests = new WBuffer(fileInversename, 1, false);//do not append to file

			FileLog.WriteLine(FileForests.info);


			Odom odometer = new Odom(TupleSize: n);//initializes the odometer with tuple=00...0(n times).

			Console.WriteLine("Saving to file: " + filename);
			Console.WriteLine("Any key for progress info. x to stop program.");
			Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " Started");
			//Console.ReadKey();
			odometer.Next();
			while (odometer.isAlive == true) {
				DoubleTree doubleTree = new DoubleTree();
				Console.WriteLine(string.Join("", odometer.tuple));
				DoubleTreeFunctions.functionX0X1onTuple(ref odometer.tuple, ref doubleTree);// apply the tuple to the forest.
				string str = "";// = DoubleTreeFunctions.DoubleTreeToStringBin(doubleTree);
				string strinv = "";// = DoubleTreeFunctions.DoubleTreeToInverseToString(doubleTree);
				DoubleTreeFunctions.DoubleTreeToStringAndinvStringBin(doubleTree, ref str, ref strinv);
				Console.WriteLine(str);
				DoubleTree dtree = DoubleTreeFunctions.StringBinToDoubleTreeT(str);
				string teststr = "", teststrinv = "";
				DoubleTreeFunctions.DoubleTreeToStringAndinvStringBin(dtree, ref teststr, ref teststrinv);
				Console.WriteLine(String.Equals(str, teststr, StringComparison.Ordinal) + "  " + str + " " + teststr);
				Console.WriteLine(DoubleTreeFunctions.InfoF(doubleTree));
				DoubleTreeFunctions.Draw(doubleTree);
				Console.ReadKey();
				FileForests.WriteLine(str);
				FileInverseForests.WriteLine(strinv);


				// check if key has been pressed. Exit if x has been pressed
				if (Console.KeyAvailable) {
					Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "Progress: " + string.Join("", odometer.tuple));

					ConsoleKeyInfo cki = Console.ReadKey(true);
					if (cki.Key == ConsoleKey.X) break;
				}
				odometer.Next();

			}

			//close file
			FileForests.Close();
			FileInverseForests.Close();
		}


		public void ExecuteCDonOdometerBinString(int n, WBuffer FileLog) {
			DoubleTree doubleTree1 = new DoubleTree();
			DoubleTreeFunctions.functionD(ref doubleTree1);
			Console.WriteLine("XXXXXXXXXXXX=" + DoubleTreeFunctions.DoubleTreeToStringBin(doubleTree1));
			Console.WriteLine(DoubleTreeFunctions.Draw(doubleTree1));
			Console.WriteLine(DoubleTreeFunctions.InfoT(doubleTree1));
			string str1="", strinv1 = "";
			DoubleTreeFunctions.DoubleTreeToStringAndinvStringBin(doubleTree1, ref str1, ref strinv1);
			Console.WriteLine("doubleTree= " + str1 + " doubleTreeInv= " + strinv1 + "(format:DOMAINzRANGEzPOINT)");
			Console.ReadKey();




			FileLog.WriteLine("ExecuteCDonOdometerBinString n=" + n  );

			string filename = "n" + n + ".forest";
			string fileInversename = "i" + n + ".forest";
			// open file for writing. fills buffer with 1000 chars and then writes it.
			WBuffer FileForests = new WBuffer(filename, 1, false);//does not append to file
			WBuffer FileInverseForests = new WBuffer(fileInversename, 1, false);//does not append to file

			FileLog.WriteLine(FileForests.info);

			Odom odometer = new Odom(TupleSize: n,constantbase: 2);//initializes the odometer with tuple=00...0(n times).

			Console.WriteLine("Saving to file: " + filename);
			Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ".CDodometer Started");
			Console.WriteLine("Any key for progress info. x to stop program.");
			//Console.ReadKey();
			odometer.Next();
			while (odometer.isAlive == true) {
				DoubleTree doubleTree = new DoubleTree();

				Console.WriteLine("TUPLE="+string.Join("", odometer.tuple));
				DoubleTreeFunctions.functionCDonTuple(ref odometer.tuple, ref doubleTree);// apply the tuple to the forest.
				string str = "";// = DoubleTreeFunctions.DoubleTreeToStringBin(doubleTree);
				string strinv = "";// = DoubleTreeFunctions.DoubleTreeToInverseToString(doubleTree);
				DoubleTreeFunctions.DoubleTreeToStringAndinvStringBin(doubleTree, ref str, ref strinv);
				Console.WriteLine("doubleTree= " + str + " doubleTreeInv= " + strinv + "(format:DOMAINzRANGEzPOINT)");

				DoubleTree dtree = DoubleTreeFunctions.StringBinToDoubleTreeT(strinv);
				string teststr = "", teststrinv = "";
				DoubleTreeFunctions.DoubleTreeToStringAndinvStringBin(dtree, ref teststr, ref teststrinv);
				Console.WriteLine(String.Equals(strinv, teststr, StringComparison.Ordinal) + "  " + strinv + " " + teststr);
				Console.WriteLine("doubleTree :");
				Console.WriteLine(DoubleTreeFunctions.Draw(doubleTree));
				Console.WriteLine("inverse(doubleTree):");
				Console.WriteLine(DoubleTreeFunctions.Draw(dtree));
			    Console.WriteLine(DoubleTreeFunctions.InfoT(doubleTree));
				Console.ReadKey();
				FileForests.WriteLine(str);
				FileInverseForests.WriteLine(strinv);


				// check if key has been pressed. Exit if x has been pressed
				if (Console.KeyAvailable) {
					Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "Progress: " + string.Join("", odometer.tuple));

					ConsoleKeyInfo cki = Console.ReadKey(true);
					if (cki.Key == ConsoleKey.X) break;
				}
				odometer.Next();

			}

			//close file
			FileForests.Close();
			FileInverseForests.Close();
		}

		public void ExecuteA0toA5onOdometerBinString_EvenSmallEqualwords(int n, string label_dest,bool showinfo = false) {


			Program.log_WriteLine("ExecuteA0toAnonOdometerBinString n=" + n);

			//string strfilename_ENn = "EN" + n + ".doubletree";
			//string strfilename_EIn = "EI" + n + ".doubletree";
			//string strfilename_allrENn = "allrEN" + n + ".doubletree";
			//string strfilename_allrEIn = "allrEI" + n + ".doubletree";
			//string strfilename_s_erENn = "s_erEN" + n + ".doubletree";
			//string strfilename_l_erENn = "l_erEN" + n + ".doubletree";
			//string strfilename_e_erENn = "e_erEN" + n + ".doubletree";
			//string strfilename_s_reEIn = "s_erEI" + n + ".doubletree";
			//string strfilename_l_erEIn = "l_erEI" + n + ".doubletree";
			//string strfilename_e_erEIn = "e_erEI" + n + ".doubletree";
			string strfilename_srENn = "srEN" + n + label_dest + "-0.dts";
			//string strfilename_lrENn = "lrEN" + n + "-0.doubletree";
			string strfilename_erENn = "erEN" + n + label_dest + "-0.dts";
			string strfilename_srEIn = "srEI" + n + label_dest + "-0.dts";
			//string strfilename_lrEIn = "lrEI" + n + "-0.doubletree";
			string strfilename_erEIn = "erEI" + n + label_dest + "-0.dts";
			if (n >= 2) {
				strfilename_srENn = "srEN" + n + label_dest + "-odom.doubletree";
				strfilename_erENn = "erEN" + n + label_dest + "-odom.doubletree";
				strfilename_srEIn = "srEI" + n + label_dest + "-odom.doubletree";
				strfilename_erEIn = "erEI" + n + label_dest + "-odom.doubletree";
			}
			// open file for writing. fills buffer with 1000 chars and then writes it.
			//WBuffer File_ENn = new WBuffer(strfilename_ENn, 1, false);//does not append to file
			//WBuffer File_EIn = new WBuffer(strfilename_EIn, 1, false);//does not append to file
			//WBuffer File_allrENn = new WBuffer(strfilename_allrENn, 1, false);//does not append to file
			//WBuffer File_allrEIn = new WBuffer(strfilename_allrEIn, 1, false);//does not append to file
			//WBuffer File_s_erENn = new WBuffer(strfilename_s_erENn, 1, false);//does not append to file
			//WBuffer File_l_erENn = new WBuffer(strfilename_l_erENn, 1, false);//does not append to file
			//WBuffer File_e_erENn = new WBuffer(strfilename_e_erENn, 1, false);//does not append to file
			//WBuffer File_s_erEIn = new WBuffer(strfilename_s_reEIn, 1, false);//does not append to file
			//WBuffer File_l_erEIn = new WBuffer(strfilename_l_erEIn, 1, false);//does not append to file
			//WBuffer File_e_erEIn = new WBuffer(strfilename_e_erEIn, 1, false);//does not append to file
			WBuffer File_srENn = new WBuffer(strfilename_srENn, 1, false);//does not append to file
			//WBuffer File_lrENn = new WBuffer(strfilename_lrENn, 1, false);//does not append to file
			WBuffer File_erENn = new WBuffer(strfilename_erENn, 1, false);//does not append to file
			WBuffer File_srEIn = new WBuffer(strfilename_srEIn, 1, false);//does not append to file
			//WBuffer File_lrEIn = new WBuffer(strfilename_lrEIn, 1, false);//does not append to file
			WBuffer File_erEIn = new WBuffer(strfilename_erEIn, 1, false);//does not append to file

			Program.log_WriteLine(File_srENn.info);

			Odom odometer = new Odom(TupleSize: n, constantbase: 6);//initializes the odometer with tuple=00...0(n times).

			Console.WriteLine("Saving to files: " + strfilename_srENn + ", " + strfilename_erENn);
			Console.WriteLine("StartTime: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " (CDodometer Small Equal words (no Large) started)");
			Console.WriteLine("Any key for progress info. x to stop program.");
			//Console.ReadKey();
			odometer.Next();
			while (odometer.isAlive == true) {
				//OIn= a(ba)^n  where a=c+c^-1  and b=D+D^-1+D^2  A0=CD A1=CD^2 A2=CD^-1      A3=C^-1D A4=C^-1D^2 A5=C^-1D^-1

				#region OIn 'e'
				{
					DoubleTree doubleTree = new DoubleTree();
					DoubleTreeFunctions.functionA0toA5onTuple(ref odometer.tuple, ref doubleTree);// apply the tuple to the forest.
					string str = "";
					string strinv = "";
					DoubleTreeFunctions.DoubleTreeToStringAndinvStringBin(doubleTree, ref str, ref strinv);

					//File_ENn.WriteLine(str);
					//File_EIn.WriteLine(strinv);

					if (odometer.tuple[0] < 3) {//Looking at first half of the odometer and computing its reverseinverse
						string strReversei = "";
						string strinvReversei = "";
						DoubleTree doubleTreeReversei = DoubleTreeFunctions.reverseInverse_ofdoubleTreeT(doubleTree);
						DoubleTreeFunctions.DoubleTreeToStringAndinvStringBin(doubleTreeReversei, ref strReversei, ref strinvReversei);
						{//rONn
							//File_allrENn.WriteLine(str);
							//File_allrENn.WriteLine(strReversei);//this correspond to the second half of the odometer
							int cmpr = string.CompareOrdinal(str, strReversei);
							if (cmpr < 0) { File_srENn.WriteLine(str); } //w<ri(w)  not saving: File_lrENn.WriteLine(strReversei);
							if (cmpr == 0) { File_erENn.WriteLine(str); }//w==ri(w) half saving: File_erENn.WriteLine(str); 
							if (cmpr > 0) { File_srENn.WriteLine(strReversei); }//w>ri(w) not saving: File_lrENn.WriteLine(str);
 							//if (cmpr == 0) {//w=ri(w) thus w^-1=ri(w)^-1=ri(w^-1)
							//	int cmp = string.CompareOrdinal(str, strinv);
							//	if (cmp < 0) { File_s_erENn.WriteLine(str); File_s_erENn.WriteLine(str); } //w<w^-1
							//	if (cmp == 0) { File_e_erENn.WriteLine(str); File_e_erENn.WriteLine(str); }//w==w^-1
							//	if (cmp > 0) { File_l_erENn.WriteLine(str); File_l_erENn.WriteLine(str); }//w>w^-1
							//}
						}
						{//rOIn
							//File_allrEIn.WriteLine(strinv);
							//File_allrEIn.WriteLine(strinvReversei);//this correspond to the second half of the odometer
							int cmpr = string.CompareOrdinal(strinv, strinvReversei);
							if (cmpr < 0) { File_srEIn.WriteLine(strinv); }//w<ri(w)  not saving: File_lrEIn.WriteLine(strinvReversei);
							if (cmpr == 0) { File_erEIn.WriteLine(strinv); }//w==ri(w) half saving: File_erEIn.WriteLine(strinv);
							if (cmpr > 0) { File_srEIn.WriteLine(strinvReversei); }//w>ri(w) not saving: File_lrEIn.WriteLine(strinv);
							//if (cmpr == 0) {//w^-1=ri(w)^-1=ri(w^-1) thus w=ri(w^-1)^-1=ri(w)
							//	int cmp = string.CompareOrdinal(strinv, str);
							//	if (cmp < 0) { File_s_erEIn.WriteLine(strinv); File_s_erEIn.WriteLine(strinv); } //w^-1<(w^-1)^-1=w
							//	if (cmp == 0) { File_e_erEIn.WriteLine(strinv); File_e_erEIn.WriteLine(strinv); }//w^-1==w
							//	if (cmp > 0) { File_l_erEIn.WriteLine(strinv); File_l_erEIn.WriteLine(strinv); } //w^-1>w
							//}
						}

					}

					if (showinfo == true) {
						Console.WriteLine("TUPLE=" + string.Join("", odometer.tuple));
						Console.WriteLine(DoubleTreeFunctions.Draw(doubleTree));
						Console.WriteLine(str + " inv= " + strinv);
						string strsoren = "", strinvsoren = "";
						DoubleTreeFunctions.DoubleTreeToDoubleStringBinCompareWithSoren(doubleTree, ref strsoren, ref strinvsoren);
						Console.WriteLine(strsoren + "\ninv= " + strinvsoren);
					}
				}
				#endregion OIN 'id'


				//ONn= b(ab)^n  where a=c+c^-1  and b=D+D^-1+D^2  A0=CD A1=CD^2 A2=CD^-1      A3=C^-1D A4=C^-1D^2 A5=C^-1D^-1

				//D^2='K'  D^-1='J'






				// check if key has been pressed. Exit if x has been pressed
				if (Console.KeyAvailable) {
					Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "Executing tuple: " + string.Join("", odometer.tuple));

					ConsoleKeyInfo cki = Console.ReadKey(true);
					if (cki.Key == ConsoleKey.X) break;
				}
				odometer.Next();
				//Console.ReadKey();
			}

			Console.WriteLine("Done. (Time:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ")");
			//close file
			//File_ENn.Close();
			//File_EIn.Close();
			//File_allrENn.Close();
			//File_allrEIn.Close();
			File_srENn.Close();
			//File_lrENn.Close();
			File_erENn.Close();
			File_srEIn.Close();
			//File_lrEIn.Close();
			File_erEIn.Close();

			//File_s_erEIn.Close();
			//File_l_erEIn.Close();
			//File_e_erEIn.Close();
			//File_s_erENn.Close();
			//File_l_erENn.Close();
			//File_e_erENn.Close();

		}

		//////////////////////////////////////////////////////////////////////////////////////////

		
		public void ExecuteA0toA5onOdometerString_EvenSmallEqualwords(int n, WBuffer FileLog, bool showinfo = false) {


			FileLog.WriteLine("ExecuteA0toAnonOdometerBinString n=" + n);

			string strfilename_ENn = "EN" + n + ".doubletree";
			string strfilename_EIn = "EI" + n + ".doubletree";
			string strfilename_allrENn = "allrEN" + n + ".doubletree";
			string strfilename_allrEIn = "allrEI" + n + ".doubletree";
			string strfilename_s_erENn = "s_erEN" + n + ".doubletree";
			string strfilename_l_erENn = "l_erEN" + n + ".doubletree";
			string strfilename_e_erENn = "e_erEN" + n + ".doubletree";
			string strfilename_s_reEIn = "s_erEI" + n + ".doubletree";
			string strfilename_l_erEIn = "l_erEI" + n + ".doubletree";
			string strfilename_e_erEIn = "e_erEI" + n + ".doubletree";
			string strfilename_srENn = "srEN" + n + ".doubletree";
			string strfilename_lrENn = "lrEN" + n + ".doubletree";
			string strfilename_erENn = "erEN" + n + ".doubletree";
			string strfilename_srEIn = "srEI" + n + ".doubletree";
			string strfilename_lrEIn = "lrEI" + n + ".doubletree";
			string strfilename_erEIn = "erEI" + n + ".doubletree";
			// open file for writing. fills buffer with 1000 chars and then writes it.
			WBuffer File_ENn = new WBuffer(strfilename_ENn, 1, false);//does not append to file
			WBuffer File_EIn = new WBuffer(strfilename_EIn, 1, false);//does not append to file
			WBuffer File_allrENn = new WBuffer(strfilename_allrENn, 1, false);//does not append to file
			WBuffer File_allrEIn = new WBuffer(strfilename_allrEIn, 1, false);//does not append to file
			WBuffer File_s_erENn = new WBuffer(strfilename_s_erENn, 1, false);//does not append to file
			WBuffer File_l_erENn = new WBuffer(strfilename_l_erENn, 1, false);//does not append to file
			WBuffer File_e_erENn = new WBuffer(strfilename_e_erENn, 1, false);//does not append to file
			WBuffer File_s_erEIn = new WBuffer(strfilename_s_reEIn, 1, false);//does not append to file
			WBuffer File_l_erEIn = new WBuffer(strfilename_l_erEIn, 1, false);//does not append to file
			WBuffer File_e_erEIn = new WBuffer(strfilename_e_erEIn, 1, false);//does not append to file
			WBuffer File_srENn = new WBuffer(strfilename_srENn, 1, false);//does not append to file
			WBuffer File_lrENn = new WBuffer(strfilename_lrENn, 1, false);//does not append to file
			WBuffer File_erENn = new WBuffer(strfilename_erENn, 1, false);//does not append to file
			WBuffer File_srEIn = new WBuffer(strfilename_srEIn, 1, false);//does not append to file
			WBuffer File_lrEIn = new WBuffer(strfilename_lrEIn, 1, false);//does not append to file
			WBuffer File_erEIn = new WBuffer(strfilename_erEIn, 1, false);//does not append to file

			FileLog.WriteLine(File_ENn.info);

			Odom odometer = new Odom(TupleSize: n, constantbase: 6);//initializes the odometer with tuple=00...0(n times).

			Console.WriteLine("Saving to files: " + strfilename_ENn);
			Console.WriteLine("StartTime: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " (CDodometer Small Equal words (no Large) started)");
			Console.WriteLine("Any key for progress info. x to stop program.");
			//Console.ReadKey();
			odometer.Next();
			while (odometer.isAlive == true) {
				//OIn= a(ba)^n  where a=c+c^-1  and b=D+D^-1+D^2  A0=CD A1=CD^2 A2=CD^-1      A3=C^-1D A4=C^-1D^2 A5=C^-1D^-1

				#region OIn 'e'
				{
					DoubleTree doubleTree = new DoubleTree();
					DoubleTreeFunctions.functionA0toA5onTuple(ref odometer.tuple, ref doubleTree);// apply the tuple to the forest.
					string str = "";
					string strinv = "";
					DoubleTreeFunctions.DoubleTreeToStringAndinvStringVSLOW(doubleTree, ref str, ref strinv);

					File_ENn.WriteLine(str);
					File_EIn.WriteLine(strinv);

					if (odometer.tuple[0] < 3) {//Looking at first half of the odometer and computing its reverseinverse
						string strReversei = "";
						string strinvReversei = "";
						DoubleTree doubleTreeReversei = DoubleTreeFunctions.reverseInverse_ofdoubleTreeT(doubleTree);
						DoubleTreeFunctions.DoubleTreeToStringAndinvStringVSLOW(doubleTreeReversei, ref strReversei, ref strinvReversei);
						{//rONn
							File_allrENn.WriteLine(str);
							File_allrENn.WriteLine(strReversei);//this correspond to the second half of the odometer
							int cmpr = string.CompareOrdinal(str, strReversei);
							if (cmpr < 0) { File_srENn.WriteLine(str); File_lrENn.WriteLine(strReversei); }//w<ri(w)
							if (cmpr == 0) { File_erENn.WriteLine(str); File_erENn.WriteLine(str); }//w==ri(w)
							if (cmpr > 0) { File_lrENn.WriteLine(str); File_srENn.WriteLine(strReversei); }//w>ri(w)
							if (cmpr == 0) {//w=ri(w) thus w^-1=ri(w)^-1=ri(w^-1)
								int cmp = string.CompareOrdinal(str, strinv);
								if (cmp < 0) { File_s_erENn.WriteLine(str); File_s_erENn.WriteLine(str); } //w<w^-1
								if (cmp == 0) { File_e_erENn.WriteLine(str); File_e_erENn.WriteLine(str); }//w==w^-1
								if (cmp > 0) { File_l_erENn.WriteLine(str); File_l_erENn.WriteLine(str); }//w>w^-1
							}
						}
						{//rOIn
							File_allrEIn.WriteLine(strinv);
							File_allrEIn.WriteLine(strinvReversei);//this correspond to the second half of the odometer
							int cmpr = string.CompareOrdinal(strinv, strinvReversei);
							if (cmpr < 0) { File_srEIn.WriteLine(strinv); File_lrEIn.WriteLine(strinvReversei); }//w<ri(w)
							if (cmpr == 0) { File_erEIn.WriteLine(strinv); File_erEIn.WriteLine(strinv); }//w==ri(w)
							if (cmpr > 0) { File_lrEIn.WriteLine(strinv); File_srEIn.WriteLine(strinvReversei); }//w>ri(w)
							if (cmpr == 0) {//w^-1=ri(w)^-1=ri(w^-1) thus w=ri(w^-1)^-1=ri(w)
								int cmp = string.CompareOrdinal(strinv, str);
								if (cmp < 0) { File_s_erEIn.WriteLine(strinv); File_s_erEIn.WriteLine(strinv); } //w^-1<(w^-1)^-1=w
								if (cmp == 0) { File_e_erEIn.WriteLine(strinv); File_e_erEIn.WriteLine(strinv); }//w^-1==w
								if (cmp > 0) { File_l_erEIn.WriteLine(strinv); File_l_erEIn.WriteLine(strinv); } //w^-1>w
							}
						}

					}

					if (showinfo == true) {
						Console.WriteLine("TUPLE=" + string.Join("", odometer.tuple));
						Console.WriteLine(DoubleTreeFunctions.Draw(doubleTree));
						Console.WriteLine(str + " inv= " + strinv);
						string strsoren = "", strinvsoren = "";
						DoubleTreeFunctions.DoubleTreeToDoubleStringBinCompareWithSoren(doubleTree, ref strsoren, ref strinvsoren);
						Console.WriteLine(strsoren + "\ninv= " + strinvsoren);
					}
				}
				#endregion OIN 'id'


				//ONn= b(ab)^n  where a=c+c^-1  and b=D+D^-1+D^2  A0=CD A1=CD^2 A2=CD^-1      A3=C^-1D A4=C^-1D^2 A5=C^-1D^-1

				//D^2='K'  D^-1='J'






				// check if key has been pressed. Exit if x has been pressed
				if (Console.KeyAvailable) {
					Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "Executing tuple: " + string.Join("", odometer.tuple));

					ConsoleKeyInfo cki = Console.ReadKey(true);
					if (cki.Key == ConsoleKey.X) break;
				}
				odometer.Next();
				//Console.ReadKey();
			}

			Console.WriteLine("Done. (Time:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ")");
			//close file
			File_ENn.Close();
			File_EIn.Close();
			File_allrENn.Close();
			File_allrEIn.Close();
			File_srENn.Close();
			File_lrENn.Close();
			File_erENn.Close();
			File_srEIn.Close();
			File_lrEIn.Close();
			File_erEIn.Close();

			File_s_erEIn.Close();
			File_l_erEIn.Close();
			File_e_erEIn.Close();
			File_s_erENn.Close();
			File_l_erENn.Close();
			File_e_erENn.Close();

		}

		//////////////////////////////////////////////////////////////////////////////////////////

		public void ExecuteA0toA5onOdometerString_OddSmallEqualwords(int n, WBuffer FileLog, bool showinfo = false) {



			FileLog.WriteLine("ExecuteA0toAnonOdometerBinString n=" + n);

			string strfilename_ONn = "ON" + n + ".doubletree";
			string strfilename_OIn = "OI" + n + ".doubletree";
			string strfilename_allrONn = "allrON" + n + ".doubletree";
			string strfilename_allrOIn = "allrOI" + n + ".doubletree";
			string strfilename_sONn = "sON" + n + ".doubletree";
			string strfilename_lONn = "lON" + n + ".doubletree";
			string strfilename_eONn = "eON" + n + ".doubletree";
			string strfilename_sr_eONn = "sr_eON" + n + ".doubletree";
			string strfilename_lr_eONn = "lr_eON" + n + ".doubletree";
			string strfilename_er_eONn = "er_eON" + n + ".doubletree";
			string strfilename_sOIn = "sOI" + n + ".doubletree";
			string strfilename_lOIn = "lOI" + n + ".doubletree";
			string strfilename_eOIn = "eOI" + n + ".doubletree";
			string strfilename_sr_eOIn = "sr_eOI" + n + ".doubletree";
			string strfilename_lr_eOIn = "lr_eOI" + n + ".doubletree";
			string strfilename_er_eOIn = "er_eOI" + n + ".doubletree";
			string strfilename_srONn = "srON" + n + ".doubletree";
			string strfilename_lrONn = "lrON" + n + ".doubletree";
			string strfilename_erONn = "erON" + n + ".doubletree";
			string strfilename_srOIn = "srOI" + n + ".doubletree";
			string strfilename_lrOIn = "lrOI" + n + ".doubletree";
			string strfilename_erOIn = "erOI" + n + ".doubletree";
			// open file for writing. fills buffer with 1000 chars and then writes it.
			WBuffer File_ONn = new WBuffer(strfilename_ONn, 1, false);//does not append to file
			WBuffer File_OIn = new WBuffer(strfilename_OIn, 1, false);//does not append to file
			WBuffer File_allrONn = new WBuffer(strfilename_allrONn, 1, false);//does not append to file
			WBuffer File_allrOIn = new WBuffer(strfilename_allrOIn, 1, false);//does not append to file
			WBuffer File_sONn = new WBuffer(strfilename_sONn, 1, false);//does not append to file
			WBuffer File_lONn = new WBuffer(strfilename_lONn, 1, false);//does not append to file
			WBuffer File_eONn = new WBuffer(strfilename_eONn, 1, false);//does not append to file
			WBuffer File_sr_eONn = new WBuffer(strfilename_sr_eONn, 1, false);//does not append to file
			WBuffer File_lr_eONn = new WBuffer(strfilename_lr_eONn, 1, false);//does not append to file
			WBuffer File_er_eONn = new WBuffer(strfilename_er_eONn, 1, false);//does not append to file
			WBuffer File_sOIn = new WBuffer(strfilename_sOIn, 1, false);//does not append to file
			WBuffer File_lOIn = new WBuffer(strfilename_lOIn, 1, false);//does not append to file
			WBuffer File_eOIn = new WBuffer(strfilename_eOIn, 1, false);//does not append to file
			WBuffer File_sr_eOIn = new WBuffer(strfilename_sr_eOIn, 1, false);//does not append to file
			WBuffer File_lr_eOIn = new WBuffer(strfilename_lr_eOIn, 1, false);//does not append to file
			WBuffer File_er_eOIn = new WBuffer(strfilename_er_eOIn, 1, false);//does not append to file
			WBuffer File_srONn = new WBuffer(strfilename_srONn, 1, false);//does not append to file
			WBuffer File_lrONn = new WBuffer(strfilename_lrONn, 1, false);//does not append to file
			WBuffer File_erONn = new WBuffer(strfilename_erONn, 1, false);//does not append to file
			WBuffer File_srOIn = new WBuffer(strfilename_srOIn, 1, false);//does not append to file
			WBuffer File_lrOIn = new WBuffer(strfilename_lrOIn, 1, false);//does not append to file
			WBuffer File_erOIn = new WBuffer(strfilename_erOIn, 1, false);//does not append to file

			FileLog.WriteLine(File_sONn.info);

			Odom odometer = new Odom(TupleSize: n, constantbase: 6);//initializes the odometer with tuple=00...0(n times).

			Console.WriteLine("Saving to files: " + strfilename_sONn);
			Console.WriteLine("StartTime: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " (CDodometer Small Equal words (no Large) started)");
			Console.WriteLine("Any key for progress info. x to stop program.");
			//Console.ReadKey();
			odometer.Next();
			while (odometer.isAlive == true) {
				//OIn= a(ba)^n  where a=c+c^-1  and b=D+D^-1+D^2  A0=CD A1=CD^2 A2=CD^-1      A3=C^-1D A4=C^-1D^2 A5=C^-1D^-1

				#region OIn 'C'
				{
					DoubleTree doubleTree = new DoubleTree('C');
					DoubleTreeFunctions.functionA0toA5onTuple(ref odometer.tuple, ref doubleTree);// apply the tuple to the forest.
					string str = "";
					string strinv = "";
					DoubleTreeFunctions.DoubleTreeToStringAndinvStringVSLOW(doubleTree, ref str, ref strinv);
					
					File_OIn.WriteLine(str);
					{//Partitioning wrt str<strinv .  The union of sOIn and eOIn and lOIn should be OIn
						int cmp = string.CompareOrdinal(str, strinv);
						if (cmp < 0) File_sOIn.WriteLine(str);//str is smaller than strinv
						if (cmp == 0) File_eOIn.WriteLine(str); //str is equal to strinv
						if (cmp > 0) File_lOIn.WriteLine(str);//str is larger than strinv
					}

					if (odometer.tuple[0] < 3) {//Looking at first half of the odometer and computing its reverseinverse
						string strReversei = "";
						string strinvReversei = "";
						DoubleTree doubleTreeReversei = DoubleTreeFunctions.reverseInverse_ofdoubleTreeT(doubleTree);
						DoubleTreeFunctions.DoubleTreeToStringAndinvStringVSLOW(doubleTreeReversei, ref strReversei, ref strinvReversei);
						File_allrOIn.WriteLine(str);
						File_allrOIn.WriteLine(strReversei);//this correspond to the second half of the odometer
						int cmp = string.CompareOrdinal(str, strReversei);
						if (cmp < 0) { File_srOIn.WriteLine(str); File_lrOIn.WriteLine(strReversei); }//w<ri(w)
						if (cmp == 0) { File_erOIn.WriteLine(str); File_erOIn.WriteLine(str); }//w==ri(w)
						if (cmp > 0) { File_lrOIn.WriteLine(str); File_srOIn.WriteLine(strReversei); }//w>ri(w)
						if (string.CompareOrdinal(str, strinv) == 0) {//w=w^-1.  
							if (cmp < 0) File_sr_eOIn.WriteLine(str); //w<ri(w)
							if (cmp == 0) File_er_eOIn.WriteLine(str);//w==ri(w)
							if (cmp > 0) File_lr_eOIn.WriteLine(str); //w>ri(w)
						}
						if (string.CompareOrdinal(strReversei, strinvReversei) == 0) {//ri(w)=ri(w)^-1.  
							if (cmp > 0) File_sr_eOIn.WriteLine(strReversei); //ri(w)<ri(ri(w))=w note the opposite direction
							if (cmp == 0) File_er_eOIn.WriteLine(strReversei); //ri(w)==ri(ri(w))=w
							if (cmp < 0) File_lr_eOIn.WriteLine(strReversei); //ri(w)>ri(ri(w))=w
						}
					}

					if (showinfo == true) {
						Console.WriteLine("TUPLE=" + string.Join("", odometer.tuple));
						Console.WriteLine(DoubleTreeFunctions.Draw(doubleTree));
						Console.WriteLine(str + " inv= " + strinv);
						string strsoren = "", strinvsoren = "";
						DoubleTreeFunctions.DoubleTreeToDoubleStringBinCompareWithSoren(doubleTree, ref strsoren, ref strinvsoren);
						Console.WriteLine(strsoren + "\ninv= " + strinvsoren);
					}
				}
				#endregion OIN 'C'



				#region OIn 'Cinv'
				{
					DoubleTree doubleTree = new DoubleTree('I');//C^-1
					DoubleTreeFunctions.functionA0toA5onTuple(ref odometer.tuple, ref doubleTree);// apply the tuple to the forest.
					string str = "";
					string strinv = "";
					DoubleTreeFunctions.DoubleTreeToStringAndinvStringVSLOW(doubleTree, ref str, ref strinv);

					File_OIn.WriteLine(str);
					{//Partitioning wrt str<strinv .  The union of sOIn and eOIn and lOIn should be OIn
						int cmp = string.CompareOrdinal(str, strinv);
						if (cmp < 0) File_sOIn.WriteLine(str);//str is smaller than strinv
						if (cmp == 0) File_eOIn.WriteLine(str); //str is equal to strinv
						if (cmp > 0) File_lOIn.WriteLine(str);//str is larger than strinv
					}

					if (odometer.tuple[0] < 3) {//Looking at first half of the odometer and computing its reverseinverse
						string strReversei = "";
						string strinvReversei = "";
						DoubleTree doubleTreeReversei = DoubleTreeFunctions.reverseInverse_ofdoubleTreeT(doubleTree);
						DoubleTreeFunctions.DoubleTreeToStringAndinvStringVSLOW(doubleTreeReversei, ref strReversei, ref strinvReversei);
						File_allrOIn.WriteLine(str);
						File_allrOIn.WriteLine(strReversei);
						int cmp = string.CompareOrdinal(str, strReversei);
						if (cmp < 0) { File_srOIn.WriteLine(str); File_lrOIn.WriteLine(strReversei); }//w<ri(w)
						if (cmp == 0) { File_erOIn.WriteLine(str); File_erOIn.WriteLine(str); }//w==ri(w)
						if (cmp > 0) { File_lrOIn.WriteLine(str); File_srOIn.WriteLine(strReversei); }//w>ri(w)
						if (string.CompareOrdinal(str, strinv) == 0) {//w=w^-1.  
							if (cmp < 0) File_sr_eOIn.WriteLine(str); //w<ri(w)
							if (cmp == 0) File_er_eOIn.WriteLine(str);//w==ri(w)
							if (cmp > 0) File_lr_eOIn.WriteLine(str); //w>ri(w)
						}
						if (string.CompareOrdinal(strReversei, strinvReversei) == 0) {//ri(w)=ri(w)^-1.  
							if (cmp > 0) File_sr_eOIn.WriteLine(strReversei); //ri(w)<ri(ri(w))=w note the opposite direction
							if (cmp == 0) File_er_eOIn.WriteLine(strReversei); //ri(w)==ri(ri(w))=w
							if (cmp < 0) File_lr_eOIn.WriteLine(strReversei); //ri(w)>ri(ri(w))=w
						}

					}

					if (showinfo == true) {
						Console.WriteLine("TUPLE=" + string.Join("", odometer.tuple));
						Console.WriteLine(DoubleTreeFunctions.Draw(doubleTree));
						Console.WriteLine(str + " inv= " + strinv);
						string strsoren = "", strinvsoren = "";
						DoubleTreeFunctions.DoubleTreeToDoubleStringBinCompareWithSoren(doubleTree, ref strsoren, ref strinvsoren);
						Console.WriteLine(strsoren + "\ninv= " + strinvsoren);
					}
				}
				#endregion OIN 'Cinv'

				//ONn= b(ab)^n  where a=c+c^-1  and b=D+D^-1+D^2  A0=CD A1=CD^2 A2=CD^-1      A3=C^-1D A4=C^-1D^2 A5=C^-1D^-1


				#region ONn 'D'
				{
					DoubleTree doubleTree = new DoubleTree('D');
					DoubleTreeFunctions.functionA0invtoA5invOnTuple(ref odometer.tuple, ref doubleTree);// apply the tuple to the forest.
					string str = "";
					string strinv = "";
					DoubleTreeFunctions.DoubleTreeToStringAndinvStringVSLOW(doubleTree, ref str, ref strinv);

					File_ONn.WriteLine(str);
					{//Partitioning wrt str<strinv
						int cmp = string.CompareOrdinal(str, strinv);
						if (cmp < 0) File_sONn.WriteLine(str);//str is smaller than strinv
						if (cmp == 0) File_eONn.WriteLine(str);//str is equal to strinv
						if (cmp > 0) File_lONn.WriteLine(str);//str is larger than strinv
					}

					if (odometer.tuple[0] == 5 || odometer.tuple[0] == 2 || odometer.tuple[0] == 4) {//Looking at first half of the odometer and computing its reverseinverse
						string strReversei = "";
						string strinvReversei = "";
						DoubleTree doubleTreeReversei = DoubleTreeFunctions.reverseInverse_ofdoubleTreeT(doubleTree);
						DoubleTreeFunctions.DoubleTreeToStringAndinvStringVSLOW(doubleTreeReversei, ref strReversei, ref strinvReversei);
						File_allrONn.WriteLine(str);
						File_allrONn.WriteLine(strReversei);
						int cmp = string.CompareOrdinal(str, strReversei);
						if (cmp < 0) { File_srONn.WriteLine(str); File_lrONn.WriteLine(strReversei); }//w<ri(w)
						if (cmp == 0) { File_erONn.WriteLine(str); File_erONn.WriteLine(str); }//w==ri(w)
						if (cmp > 0) { File_lrONn.WriteLine(str); File_srONn.WriteLine(strReversei); }//w>ri(w)
						if (string.CompareOrdinal(str, strinv) == 0) {//w=w^-1.  
							if (cmp < 0) File_sr_eONn.WriteLine(str); //w<ri(w)
							if (cmp == 0) File_er_eONn.WriteLine(str);//w==ri(w)
							if (cmp > 0) File_lr_eONn.WriteLine(str); //w>ri(w)
						}
						if (string.CompareOrdinal(strReversei, strinvReversei) == 0) {//ri(w)=ri(w)^-1.  
							if (cmp > 0) File_sr_eONn.WriteLine(strReversei); //ri(w)<ri(ri(w))=w note the opposite direction
							if (cmp == 0) File_er_eONn.WriteLine(strReversei); //ri(w)==ri(ri(w))=w
							if (cmp < 0) File_lr_eONn.WriteLine(strReversei); //ri(w)>ri(ri(w))=w
						}

					}

					if (showinfo == true) {
						Console.WriteLine("TUPLE=" + string.Join("", odometer.tuple));
						Console.WriteLine(DoubleTreeFunctions.Draw(doubleTree));
						Console.WriteLine(str + " inv= " + strinv);
						string strsoren = "", strinvsoren = "";
						DoubleTreeFunctions.DoubleTreeToDoubleStringBinCompareWithSoren(doubleTree, ref strsoren, ref strinvsoren);
						Console.WriteLine(strsoren + "\ninv= " + strinvsoren);
					}
				}
				#endregion ONn 'D'

//D^2='K'  D^-1='J'

				#region ONn 'D2'
				{
					DoubleTree doubleTree = new DoubleTree('K');
					DoubleTreeFunctions.functionA0invtoA5invOnTuple(ref odometer.tuple, ref doubleTree);// apply the tuple to the forest.
					string str = "";
					string strinv = "";
					DoubleTreeFunctions.DoubleTreeToStringAndinvStringVSLOW(doubleTree, ref str, ref strinv);

					File_ONn.WriteLine(str);
					{//Partitioning wrt str<strinv
						int cmp = string.CompareOrdinal(str, strinv);
						if (cmp < 0) File_sONn.WriteLine(str);//str is smaller than strinv
						if (cmp == 0) File_eONn.WriteLine(str);//str is equal to strinv
						if (cmp > 0) File_lONn.WriteLine(str);//str is larger than strinv
					}

					if (odometer.tuple[0] == 5 || odometer.tuple[0] == 2 || odometer.tuple[0] == 4) {//Looking at first half of the odometer and computing its reverseinverse
						string strReversei = "";
						string strinvReversei = "";
						DoubleTree doubleTreeReversei = DoubleTreeFunctions.reverseInverse_ofdoubleTreeT(doubleTree);
						DoubleTreeFunctions.DoubleTreeToStringAndinvStringVSLOW(doubleTreeReversei, ref strReversei, ref strinvReversei);
						File_allrONn.WriteLine(str);
						File_allrONn.WriteLine(strReversei);
						int cmp = string.CompareOrdinal(str, strReversei);
						if (cmp < 0) { File_srONn.WriteLine(str); File_lrONn.WriteLine(strReversei); }//w<ri(w)
						if (cmp == 0) { File_erONn.WriteLine(str); File_erONn.WriteLine(str); }//w==ri(w)
						if (cmp > 0) { File_lrONn.WriteLine(str); File_srONn.WriteLine(strReversei); }//w>ri(w)
						if (string.CompareOrdinal(str, strinv) == 0) {//w=w^-1.  
							if (cmp < 0) File_sr_eONn.WriteLine(str); //w<ri(w)
							if (cmp == 0) File_er_eONn.WriteLine(str);//w==ri(w)
							if (cmp > 0) File_lr_eONn.WriteLine(str); //w>ri(w)
						}
						if (string.CompareOrdinal(strReversei, strinvReversei) == 0) {//ri(w)=ri(w)^-1.  
							if (cmp > 0) File_sr_eONn.WriteLine(strReversei); //ri(w)<ri(ri(w))=w note the opposite direction
							if (cmp == 0) File_er_eONn.WriteLine(strReversei); //ri(w)==ri(ri(w))=w
							if (cmp < 0) File_lr_eONn.WriteLine(strReversei); //ri(w)>ri(ri(w))=w
						}

					}

					if (showinfo == true) {
						Console.WriteLine("TUPLE=" + string.Join("", odometer.tuple));
						Console.WriteLine(DoubleTreeFunctions.Draw(doubleTree));
						Console.WriteLine(str + " inv= " + strinv);
						string strsoren = "", strinvsoren = "";
						DoubleTreeFunctions.DoubleTreeToDoubleStringBinCompareWithSoren(doubleTree, ref strsoren, ref strinvsoren);
						Console.WriteLine(strsoren + "\ninv= " + strinvsoren);
					}
				}
				#endregion ONn 'D2'


				#region ONn 'D^-1'
				{
					DoubleTree doubleTree = new DoubleTree('J');
					DoubleTreeFunctions.functionA0invtoA5invOnTuple(ref odometer.tuple, ref doubleTree);// apply the tuple to the forest.
					string str = "";
					string strinv = "";
					DoubleTreeFunctions.DoubleTreeToStringAndinvStringVSLOW(doubleTree, ref str, ref strinv);

					File_ONn.WriteLine(str);
					{//Partitioning wrt str<strinv
						int cmp = string.CompareOrdinal(str, strinv);
						if (cmp < 0) File_sONn.WriteLine(str);//str is smaller than strinv
						if (cmp == 0) File_eONn.WriteLine(str);//str is equal to strinv
						if (cmp > 0) File_lONn.WriteLine(str);//str is larger than strinv
					}

					if (odometer.tuple[0] == 5 || odometer.tuple[0] == 2 || odometer.tuple[0] == 4) {//Looking at first half of the odometer and computing its reverseinverse
						string strReversei = "";
						string strinvReversei = "";
						DoubleTree doubleTreeReversei = DoubleTreeFunctions.reverseInverse_ofdoubleTreeT(doubleTree);
						DoubleTreeFunctions.DoubleTreeToStringAndinvStringVSLOW(doubleTreeReversei, ref strReversei, ref strinvReversei);
						File_allrONn.WriteLine(str);
						File_allrONn.WriteLine(strReversei);
						int cmp = string.CompareOrdinal(str, strReversei);
						if (cmp < 0) { File_srONn.WriteLine(str); File_lrONn.WriteLine(strReversei); }//w<ri(w)
						if (cmp == 0) { File_erONn.WriteLine(str); File_erONn.WriteLine(str); }//w==ri(w)
						if (cmp > 0) { File_lrONn.WriteLine(str); File_srONn.WriteLine(strReversei); }//w>ri(w)
						if (string.CompareOrdinal(str, strinv) == 0) {//w=w^-1.  
							if (cmp < 0) File_sr_eONn.WriteLine(str); //w<ri(w)
							if (cmp == 0) File_er_eONn.WriteLine(str);//w==ri(w)
							if (cmp > 0) File_lr_eONn.WriteLine(str); //w>ri(w)
						}
						if (string.CompareOrdinal(strReversei, strinvReversei) == 0) {//ri(w)=ri(w)^-1.  
							if (cmp > 0) File_sr_eONn.WriteLine(strReversei); //ri(w)<ri(ri(w))=w note the opposite direction
							if (cmp == 0) File_er_eONn.WriteLine(strReversei); //ri(w)==ri(ri(w))=w
							if (cmp < 0) File_lr_eONn.WriteLine(strReversei); //ri(w)>ri(ri(w))=w
						}

					}

					if (showinfo == true) {
						Console.WriteLine("TUPLE=" + string.Join("", odometer.tuple));
						Console.WriteLine(DoubleTreeFunctions.Draw(doubleTree));
						Console.WriteLine(str + " inv= " + strinv);
						string strsoren = "", strinvsoren = "";
						DoubleTreeFunctions.DoubleTreeToDoubleStringBinCompareWithSoren(doubleTree, ref strsoren, ref strinvsoren);
						Console.WriteLine(strsoren + "\ninv= " + strinvsoren);
					}
				}
				#endregion ONn 'D^-1'





				// check if key has been pressed. Exit if x has been pressed
				if (Console.KeyAvailable) {
					Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "Executing tuple: " + string.Join("", odometer.tuple));

					ConsoleKeyInfo cki = Console.ReadKey(true);
					if (cki.Key == ConsoleKey.X) break;
				}
				odometer.Next();
				//Console.ReadKey();
			}

			Console.WriteLine("Done. (Time:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ")");
			//close file
			File_ONn.Close();
			File_OIn.Close();
			File_allrONn.Close();
			File_allrOIn.Close();
			File_sONn.Close();
			File_lONn.Close();
			File_eONn.Close();
			File_sOIn.Close();
			File_lOIn.Close();
			File_eOIn.Close();
			File_srONn.Close();
			File_lrONn.Close();
			File_erONn.Close();
			File_srOIn.Close();
			File_lrOIn.Close();
			File_erOIn.Close();

			File_sr_eOIn.Close();
			File_lr_eOIn.Close();
			File_er_eOIn.Close();
			File_sr_eONn.Close();
			File_lr_eONn.Close();
			File_er_eONn.Close();

		}

		//////////////////////////////////////////////////////////////////////////////////////////


		public void ExecuteA0toA5onOdometerBinString(int n, WBuffer FileLog,bool showinfo=false) {

			FileLog.WriteLine("ExecuteA0toAnonOdometerBinString n=" + n);

			string filename = "n" + n + ".forest";
			string fileInversename = "i" + n + ".forest";
			// open file for writing. fills buffer with 1000 chars and then writes it.
			WBuffer FileForests = new WBuffer(filename, 1, false);//does not append to file
			WBuffer FileInverseForests = new WBuffer(fileInversename, 1, false);//does not append to file

			FileLog.WriteLine(FileForests.info);

			Odom odometer = new Odom(TupleSize: n, constantbase: 6);//initializes the odometer with tuple=00...0(n times).

			Console.WriteLine("Saving to file: " + filename);
			Console.WriteLine("StartTime: "+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " (A0toA5odometer started)");
			Console.WriteLine("Any key for progress info. x to stop program.");
			//Console.ReadKey();
			odometer.Next();
			while (odometer.isAlive == true) {
				DoubleTree doubleTree = new DoubleTree();
				DoubleTreeFunctions.functionA0toA5onTuple(ref odometer.tuple, ref doubleTree);// apply the tuple to the forest.
				string str = "";
				string strinv = "";
				DoubleTreeFunctions.DoubleTreeToStringAndinvStringBin(doubleTree, ref str, ref strinv);
				if (showinfo == true) {
					Console.WriteLine("TUPLE=" + string.Join("", odometer.tuple));
					Console.WriteLine(DoubleTreeFunctions.Draw(doubleTree));
					Console.WriteLine(str + " inv= " + strinv);
					string strsoren = "", strinvsoren = "";
					DoubleTreeFunctions.DoubleTreeToDoubleStringBinCompareWithSoren(doubleTree, ref strsoren, ref strinvsoren);
					Console.WriteLine(strsoren + "\ninv= " + strinvsoren);

				}
				FileForests.WriteLine(str);
				FileInverseForests.WriteLine(strinv);


				// check if key has been pressed. Exit if x has been pressed
				if (Console.KeyAvailable) {
					Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "Executing tuple: " + string.Join("", odometer.tuple));

					ConsoleKeyInfo cki = Console.ReadKey(true);
					if (cki.Key == ConsoleKey.X) break;
				}
				odometer.Next();
				//Console.ReadKey();
			}

			Console.WriteLine("Done. (Time:"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+")");
			//close file
			FileForests.Close();
			FileInverseForests.Close();
		}



		private void strToFiles(WBuffer[] FilesForests, string str) {

			if (str.Length == 0) {
				Console.WriteLine("Error in strToFiles: Attempting to save an empty forest??");
				return;
			}
			int i = BinarySearcher.BinarySearch(str[0]);//i is between 0 and 7. Run BinarySearcher.Example() to test it
			FilesForests[i].WriteLine(str);
		}


	}
}
