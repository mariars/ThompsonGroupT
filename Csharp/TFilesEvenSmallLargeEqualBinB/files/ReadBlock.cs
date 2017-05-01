using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TFilesEvenSLEbin
{

	class ReadBlock {

		//private FileStream fs;
		private ReadTextB sr;
		private int maxNumberOfLinesInBlock;
		public long fileLength;
		public long readSoFar;
		private WordFreq carryOverCompact;

		public ReadBlock(string filepath,bool fileshare, int maxNumberOfLinesInBlock = 4000) {
			this.maxNumberOfLinesInBlock = maxNumberOfLinesInBlock;
			sr = new ReadTextB(filepath,fileshare);
			fileLength=sr.fileLength;
			readSoFar = 0;
			carryOverCompact = null;

		}


		public BlockOfLines fillcompact() {
			if (sr.isDoneReadingFile()) return null;
			BlockOfLines blockOfLines = new BlockOfLines(maxNumberOfLinesInBlock + 1);//+1 to avoid EOF agree with maxnumberoflinesinblock
			if (carryOverCompact != null)
				blockOfLines.AddLineCompact(carryOverCompact.word, carryOverCompact.freq, carryOverCompact.smaller);
			string line;
			// it's important that length<maxNumberOfLinesInBlock comes before sr.readline. Otherwise it will still read the line, when length reached buffersize, but it wont be put in the buffer, so there will be missing lines
			while (blockOfLines.numberOfLines < maxNumberOfLinesInBlock && (line = sr.ReadLine()) != null) {
				string[] words = line.Split(' ');
				long freq = 1;
				if (words.Length == 2) freq = long.Parse(words[1]);
				blockOfLines.AddLineCompact(words[0], freq);
				readSoFar += line.Length;
			}
			if (sr.isDoneReadingFile()) blockOfLines.AddLastLineCompact();
			else
				carryOverCompact = blockOfLines.CarryOverCompact();
			return blockOfLines;
		}

		public void Close() {
			sr.Close();
			
		}


	}
}
