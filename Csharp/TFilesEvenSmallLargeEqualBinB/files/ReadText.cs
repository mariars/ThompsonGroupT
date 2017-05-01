using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TFilesEvenSLEbin {
	class ReadText {
		private string srcLocation;
		private long bufferStart;
		private int bufferLength;
		private FileStream srcFile;
		public long fileLength;
		private bool DoneReadingFile;

		public ReadText(string srcLocation) {
			this.srcLocation = srcLocation;
			srcFile = new FileStream(srcLocation, FileMode.Open, FileAccess.Read);
			bufferStart = 0; //want to start at byte 0
			bufferLength = 1; //we only want to write one byte at a time
			fileLength = srcFile.Length; //long=9223372036854775807bytes =8589934592 GBytes.
			if (fileLength == 0)
				DoneReadingFile = true;
			else
				DoneReadingFile = false;

		}

		public bool isDoneReadingFile() {
			return DoneReadingFile;
		}
		public string ReadLine() {
			if (DoneReadingFile == true) return null;
			//now we want to loop through each byte and write it
			//IMPORTANT – put the FileLength in a variable, since FileStream.Length actually loops
			//through each time we call it, thus taking o(n) time. Whereas a variable is constant time
			string line = "";
			while (bufferStart < fileLength) {
				byte[] buffer = { (byte)bufferStart };
				//we pass our buffer, the start position to read from the buffer (0), and the length of buffer (bufferLength).
				srcFile.Read(buffer, 0, bufferLength);
				bufferStart++;

				if (buffer[0] == '\r') { buffer = null; continue; }
				if (buffer[0] == '\n') {
					if (bufferStart == fileLength) DoneReadingFile = true;
					return line;
				}
				else
					line +=(char)( buffer[0]);
				//for good measure, dereference buffer
				buffer = null;
			}
			DoneReadingFile = true;
			return line;
		}

		public void Close() {
			//close our file streams, and get rid of them
			srcFile.Close();
			srcFile.Dispose();
		}
	}
}
