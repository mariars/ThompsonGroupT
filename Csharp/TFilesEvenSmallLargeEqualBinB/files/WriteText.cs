using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TFilesEvenSLEbin {
	class WriteText {
		private string destLocation;
		private FileStream destFile;
		private int bufferLength;
		public WriteText(string destLocation, bool append ) {
			this.destLocation = destLocation;
			if (append == false) {
				destFile = new FileStream(destLocation, FileMode.Create, FileAccess.Write);
			}
			else {
				destFile = new FileStream(destLocation, FileMode.Append, FileAccess.Write);
			}


			bufferLength = 1; //we only want to write one byte at a time
		}

		public void WriteLine_Bash(string line) {
			line += "\n";
			int length = line.Length;
			for (int i = 0; i < length; i++) {
				byte[] buffer = { (byte)line[i] };
				destFile.Write(buffer, 0, bufferLength);
				//for good measure, dereference buffer
				buffer = null;
			}
		}

		public void WriteLine(string line) {
			line += "\r\n";
			int length = line.Length;
			for (int i = 0; i < length; i++) {
				byte[] buffer = { (byte)line[i] };
				destFile.Write(buffer, 0, bufferLength);
				//for good measure, dereference buffer
				buffer = null;
			}
		}

		public void Write(string line) {
			int length = line.Length;
			for (int i = 0; i < length; i++) {
				byte[] buffer = { (byte)line[i] };
				destFile.Write(buffer, 0, bufferLength);
				//for good measure, dereference buffer
				buffer = null;
			}
		}

		public void Close() {
			//close our file streams, and get rid of them
			destFile.Close();
			destFile.Dispose();
		}
	}
}
