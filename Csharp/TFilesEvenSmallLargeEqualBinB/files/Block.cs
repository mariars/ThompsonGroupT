using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TFilesEvenSLEbin
{

	class WordFreq {
		public string word;
		public long freq;
		public bool smaller;
		public WordFreq(string word, long freq = 1,bool smaller=true) {
			this.word = word;
			this.freq = freq;
			this.smaller = smaller;//used for w<reverseinverse(w) or w=ri(w) when writing not when reading 
		}

		public string asString() {
			if (freq == 1) { return word; }
			return word + " " + freq;
		}

	}

	class BlockOfLines {

		private WordFreq[] Lines;
		public int MaxNumberOfLines;
		public int numberOfLines;//keeps track of the number of lines added to the buffer
		private int pos;//keeps track of the current read line
		private WordFreq curWordFreq;//reading current wordline. 

		public BlockOfLines(int MaxNumberOfLines = 3000) {
			this.MaxNumberOfLines = MaxNumberOfLines;
			Lines = new WordFreq[MaxNumberOfLines];
			pos = 0;
			numberOfLines = 0;
			curWordFreq = null;
		}

		public WordFreq ReadLine() {
			if (pos >= numberOfLines) return null;// all lines have been read
			if (numberOfLines == 0) return null;//no lines have been added
			return Lines[pos++];
		}




		public void AddLine(string line, long freq = 1, bool smaller = true) {
			//Console.WriteLine("adding line " + MaxNumberOfLines +" "+ numberOfLines + " "  + " " + line + " " + freq + " ");
			Lines[numberOfLines] = new WordFreq(line, freq, smaller);
			numberOfLines++;
		}

		public void AddLineCompact(string line, long freq = 1, bool smaller = true) {
			if (curWordFreq == null) { curWordFreq = new WordFreq(line, freq, smaller); return; }
			if (string.CompareOrdinal(line, curWordFreq.word) == 0) {
				curWordFreq.freq += freq;
			}
			else {
				//if (numberOfLines >= MaxNumberOfLines) { Console.WriteLine("Error:above line could not be written!!!"); return; }
				Lines[numberOfLines] = curWordFreq;
				numberOfLines++;
				curWordFreq = new WordFreq(line, freq, smaller);
			}

			//if (line == "dXzd`zX") {
			//	Console.WriteLine("dXzd`zX found ");
			//	Console.WriteLine("dXzd`zX found "); 
			//}

		}
		//this function must be called when the end of file has been reached.
		public void AddLastLineCompact() {
			if (curWordFreq != null) {
				Lines[numberOfLines] = curWordFreq;
				numberOfLines++;
				curWordFreq = null;
			}
		}

		public WordFreq CarryOverCompact() {
			return curWordFreq;
		}


	}
}
