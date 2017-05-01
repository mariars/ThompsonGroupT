using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TFilesEvenSLEbin {

	class BlockJob {

		public BlockOfLines blockInput;
		public BlockOfLines blockOutput;
		public BlockOfLines blockOutputInverses;
		private bool smaller;//the input block of lines is smaller (true) or equal(false). for output blocks it is meaningless.


		public BlockJob(BlockOfLines blockInput, bool smaller) {
			this.blockInput = blockInput;
			blockOutput = new BlockOfLines(blockInput.numberOfLines * 6);//For each line we will be applying A1 A2 A3 A4 A5 A6
			blockOutputInverses = new BlockOfLines(blockInput.numberOfLines * 6);
			blockOutput.numberOfLines = 0;
			this.smaller = smaller;
		}

		//DumpToConsole applies x0 x1 x01 x11 to all the lines.Length stringForests in the string of lines "lines".
		//public void DumpToConsole() {
		//	for (int i = 0; i < blockInput.numberOfLines; i++) {
		//		//Console.WriteLine(i + "  " + block.ReadLine());
		//		blockOutput.Lines[blockOutput.numberOfLines] = blockInput.ReadLine() + " " + i;
		//		blockOutput.numberOfLines++;
		//	}
		//}

		public void ApplyAiToBlock() {

			if (smaller) {//assuming w<ri(w)
				for (int i = 0; i < blockInput.numberOfLines; i++) {

					WordFreq wordfreq = blockInput.ReadLine();

					//A1..A6:
					for (int j = 1; j <= 6; j++) {
						DoubleTree doubleTree = DoubleTreeFunctions.StringBinToDoubleTreeT(wordfreq.word);
						DoubleTreeFunctions.functionA1toA6(ref doubleTree, j);
						//abri(w)=abl1^-1l2^-1l3^-1=a^-1b^-1l1^-1l2^-1l3^-1=ri(abw) because a*=a and b*=b
						DoubleTree doubleTreeReversei = DoubleTreeFunctions.reverseInverse_ofdoubleTreeT(doubleTree);
						string str = "", strinv = "";
						DoubleTreeFunctions.DoubleTreeToStringAndinvStringBin(doubleTree, ref str, ref strinv);
						string strReversei = "", strinvReversei = "";
						DoubleTreeFunctions.DoubleTreeToStringAndinvStringBin(doubleTreeReversei, ref strReversei, ref strinvReversei);

						int cmpr = string.CompareOrdinal(str, strReversei);
						if (cmpr < 0) { blockOutput.AddLine(str, wordfreq.freq, smaller: true);  } //w<ri(w) not saving: File_lrENn.WriteLine(strReversei); // the frequency is spread to all words 
						if (cmpr == 0) { blockOutput.AddLine(str, wordfreq.freq, smaller: false); }//w==ri(w) half saving: File_erENn.WriteLine(str); 
						if (cmpr > 0) { blockOutput.AddLine(strReversei, wordfreq.freq, smaller: true); }//w>ri(w) not saving: File_lrENn.WriteLine(str);  Console.WriteLine(j+" "+strReversei + " " + wordfreq.freq);

						int cmpr_i = string.CompareOrdinal(strinv, strinvReversei);
						if (cmpr_i < 0) { blockOutputInverses.AddLine(strinv, wordfreq.freq, smaller: true); }//the inverses have same freq as the normals.  w<ri(w)  not saving: File_lrEIn.WriteLine(strinvReversei);
						if (cmpr_i == 0) { blockOutputInverses.AddLine(strinv, wordfreq.freq, smaller: false); }//w==ri(w) half saving: File_erEIn.WriteLine(strinv);
						if (cmpr_i > 0) { blockOutputInverses.AddLine(strinvReversei, wordfreq.freq, smaller: true); }//w>ri(w) not saving: File_lrEIn.WriteLine(strinv);
					}
				}
			}
			else {//assuming w=ri(w). Words are assumed to be in file erENn and these are counted below with multiplicity 2.
				//ab w = ab ri(w) because a=a* b=b*.
				//CD w < ri(CDw)=C^-1D^-1w    <=>   C^-1D^-1ri(w)=C^-1D-1w< CDw
 				//in other words if CDw is s then C^-1D^-1w is l
				//
				for (int i = 0; i < blockInput.numberOfLines; i++) {

					WordFreq wordfreq = blockInput.ReadLine();
					

					//A1..A6:
					for (int j = 1; j <= 6; j++) {
						DoubleTree doubleTree = DoubleTreeFunctions.StringBinToDoubleTreeT(wordfreq.word);
						DoubleTreeFunctions.functionA1toA6(ref doubleTree, j);
						DoubleTree doubleTreeReversei = DoubleTreeFunctions.reverseInverse_ofdoubleTreeT(doubleTree);//ab w = ab ri(w) = ri(ab w) because a=a* b=b* ; see above
						string str = "", strinv = "";
						DoubleTreeFunctions.DoubleTreeToStringAndinvStringBin(doubleTree, ref str, ref strinv);
						string strReversei = "", strinvReversei = "";
						DoubleTreeFunctions.DoubleTreeToStringAndinvStringBin(doubleTreeReversei, ref strReversei, ref strinvReversei);

						int cmpr = string.CompareOrdinal(str, strReversei);
						if (cmpr < 0) { blockOutput.AddLine(str, 2 * wordfreq.freq, smaller: true); } //w<ri(w) not saving: File_lrENn.WriteLine(strReversei);  
						if (cmpr == 0) { blockOutput.AddLine(str, wordfreq.freq, smaller: false);  }//w==ri(w) half saving: File_erENn.WriteLine(str); 
						//if (cmpr > 0) { blockOutput.AddLine(strReversei, wordfreq.freq, smaller: true); Console.WriteLine("e>::::::::::" + strReversei + " " + freq2); }//w>ri(w) not saving: File_lrENn.WriteLine(str);

						int cmpr_i = string.CompareOrdinal(strinv, strinvReversei);
						if (cmpr_i < 0) { blockOutputInverses.AddLine(strinv, 2 * wordfreq.freq, smaller: true); }//the inverses have same freq as the normals.  w<ri(w)  not saving: File_lrEIn.WriteLine(strinvReversei);
						if (cmpr_i == 0) { blockOutputInverses.AddLine(strinv, wordfreq.freq, smaller: false); }//w==ri(w) half saving: File_erEIn.WriteLine(strinv);
						//if (cmpr_i > 0) { blockOutputInverses.AddLine(strinvReversei, wordfreq.freq, smaller: true); }//w>ri(w) not saving: File_lrEIn.WriteLine(strinv);
					}
				}
			}//end else

		}
	}
}


