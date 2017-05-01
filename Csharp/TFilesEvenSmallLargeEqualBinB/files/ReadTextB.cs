using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TFilesEvenSLEbin {
	class ReadTextB {
		private string srcLocation;
		private int array_length;
		private FileStream srcFile;
		public long fileLength;
		private bool DoneReadingFile;
		private BinaryReader bwread;
		private byte[] dataArray;
		private int bytesread;
		private int curByte;
		private string leftover_str;
		public ReadTextB(string srcLocation, bool fileShare) {
			array_length = (int)Math.Pow(2, 19);
			dataArray = new byte[array_length];
			this.srcLocation = srcLocation;
			if(fileShare==false)
				srcFile = new FileStream(srcLocation, FileMode.Open, FileAccess.Read, FileShare.None, array_length);
			else
				srcFile = new FileStream(srcLocation, FileMode.Open, FileAccess.Read, FileShare.Read,array_length);
			fileLength = srcFile.Length; //long=9223372036854775807bytes =8589934592 GBytes.
			bwread = new BinaryReader(srcFile);
			if (fileLength == 0) { DoneReadingFile = true; }
			else { DoneReadingFile = false;  }
			leftover_str = "";
			curByte = 0;
			bytesread = 0;
		
		}

		public bool isDoneReadingFile() {
			return DoneReadingFile;
		}


		public string ReadLine() {
			if (DoneReadingFile == true) return null;
			//now we want to loop through each byte and write it
			//IMPORTANT – put the FileLength in a variable, since FileStream.Length actually loops
			//through each time we call it, thus taking o(n) time. Whereas a variable is constant time

			while (!DoneReadingFile) {
				string line = leftover_str;
				while (curByte < bytesread) {
					byte b = dataArray[curByte++];
					if (b == '\r') continue;
					if (b == '\n') { leftover_str = ""; return line; }
					line += (char)b;
				}
				leftover_str = line;
				//dataArray = null;//dereferencing the array
				bytesread = bwread.Read(dataArray, 0, array_length);

				curByte = 0;
				if (bytesread == 0) {
					DoneReadingFile = true;
					if (leftover_str == "") return null;
					return leftover_str;
				}

			}
			return null;
		}



		public void Close() {
			//close our file streams, and get rid of them
			srcFile.Close();
			srcFile.Dispose();
		}
	}
}
