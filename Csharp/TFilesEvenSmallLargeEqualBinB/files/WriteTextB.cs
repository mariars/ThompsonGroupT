using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TFilesEvenSLEbin {
	class WriteTextB {
		private string destLocation;
		private FileStream destFile;
		private int curByte;
		private byte[] dataArray;
		private int array_length;
		private BinaryWriter bwwrite;
		public WriteTextB(string destLocation, bool append ) {
			this.destLocation = destLocation;
			if (append == false) {
				destFile = new FileStream(destLocation, FileMode.Create, FileAccess.Write);
			}
			else {
				destFile = new FileStream(destLocation, FileMode.Append, FileAccess.Write);
			}
			bwwrite = new BinaryWriter(destFile);
			array_length = (int)Math.Pow(2, 19);
			dataArray = new byte[array_length];
			curByte = 0;
		}

		public void WriteLine_Bash(string line) {
			line += "\n";
			int length = line.Length;
			for (int i = 0; i < length; i++) {
				dataArray[curByte++] =  (byte)line[i];
				if (curByte == array_length) {
					bwwrite.Write(dataArray, 0, array_length);
					curByte = 0;
				}
			}
		}

		public void WriteLine(string line) {
			line += "\r\n";
			int length = line.Length;
			for (int i = 0; i < length; i++) {
				dataArray[curByte++] = (byte)line[i];
				if (curByte == array_length) {
					bwwrite.Write(dataArray, 0, array_length);
					curByte = 0;
				}
			}
		}

		public void Write(string line) {
			int length = line.Length;
			for (int i = 0; i < length; i++) {
				dataArray[curByte++] = (byte)line[i];
				if (curByte == array_length) {
					bwwrite.Write(dataArray, 0, array_length);
					curByte = 0;
				}
			}
		}

		public void Close() {
			bwwrite.Write(dataArray, 0, curByte);
			//close our file streams, and get rid of them
			destFile.Close();
			destFile.Dispose();
		}
	}
}
