using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TFilesEvenSLEbin
{

	class ReadTextBuffer {

		//private FileStream fs;
		private ReadTextB sr;
		private string[] buffer;
		private int buffersizeinLines;
		private int pos;
		private int length;

		public ReadTextBuffer(string srcLocation, bool fileshare, int buffersizeinLines = 52428) {//524288/10=524288
			this.buffersizeinLines = buffersizeinLines;
			sr = new ReadTextB(srcLocation,fileshare);

			fillBuffer();
		}

		public string ReadLine() {
			if (pos >= length) fillBuffer();
			if (length == 0) return null;
			return buffer[pos++];
		}

		private void fillBuffer() {
			buffer = new string[buffersizeinLines];
			length = 0;
			pos = 0;
			string line;

			// it's important that length<buffersize comes before sr.readline. Otherwise it will still read the line, when length reached buffersize, but it wont be put in the buffer, so there will be missing lines
			while (length < buffersizeinLines && (line = sr.ReadLine()) != null) {
				buffer[length] = line;
				length++;
			}
		}

		public void Close() {
			sr.Close();
		}


	}
}
