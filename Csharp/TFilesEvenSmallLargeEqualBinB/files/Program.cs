using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFilesEvenSLEbin {
	class Program {
		static public bool logToFile;

		static void Main(string[] args) {





			//http://msdn.microsoft.com/en-us/library/az4se3k1.aspx
			//http://stackoverflow.com/questions/14131435/how-to-pass-a-constant-string-reference-to-function
			//http://www.albahari.com/valuevsreftypes.aspx
			//By default, C# passes strings by reference, which means a pointer is efficiently passed under the hood.
			//http://msdn.microsoft.com/en-us/library/dd264739.aspx
			//http://social.msdn.microsoft.com/Forums/vstudio/en-US/a03952b1-36c5-4612-8308-85295eb51ab3/passing-parameter-as-byref-readonly
			//Value types (structs) are passed by value and reference types (classes) are passed by reference (i.e. you can't pass a reference type by value).  When you add the "ref" or "out" keyword to a parameter you're really saying pass the reference by reference, in the case of classes.  "ref" on a value type works as you've described for C++.



			//Console.BackgroundColor = ConsoleColor.Blue;
			//Console.ForegroundColor = ConsoleColor.White;
			//Console.Write("Trace on Odometer.");
			//Console.ForegroundColor = ConsoleColor.Red;
			//Console.WriteLine("Trace on Odometer."); // <-- This line is still white on blue.
			//Console.ResetColor();
			// fn(n) runs all possible forest for n functions, and saves them to text file.

			string fechaCompilado = " (Compiled " + DateStamp.BuildTime.ToString("dd MMMM yyyy HH:mm:ss") + ").";

			string programVersion = "\n*** Usage: TOdometer n optional_parameters " +
				"\n***   help (It draws the doubleTrees!) " +
				"\n***   standardBatches  creates batches n1.bat ... n15.bat with standard paramenters for a destop computer"+
				"\n***   bluff (will only show the commands that you input. It will not run the program)" +
				"\n***   threadAntal=3" +
				"\n***	 numberOfSubvolumes=0"+
				"\n***   minSizeSubvolume=0"+
				"\n***   maxNumberOfLinesInReadingBlock=100" +
				"\n***   maxNumberOfLinesInWritingBlock=100" +
				"\n***   srcPath=\"\"" +
				"\n***   destPath=\"\"" +
				"\n***   destInversesPath=\"\"" +
				"\n***   smallOrEqual='s'" +// 's'=small or 'e'=equal 'b'=both  
				"\n***   antalVolumesInput=8" +
				"\n***   antalVolumesOutput=8 (should be the length of volumesOutput_str)" +
				"\n***   volumesOutput_str=\"UWY^egio\"  (dictionary volumes with letters [..U][..W]...[..o] and anything above o is sent to the last volume)" +
				"\n***   append" +
				"\n***   mono=str (append the string in mono for mono purposes)" +
				"\n***   join (creates bashfile which joins files to compare with odometer counterpart, which is also created.(SLOW) Valid only for n>=2)" +
				"\n***   repartition" +
				"\n***   startWithVolume=0" +
				"\n***   numberOfLinesInSubvolume=0"+
				"\n***   subtract (computes int_dest =  int_src1 -q int_src2. )" +
				"\n***   subtr_src1 (subtr_src1=filename1 to subtract)" +
				"\n***   subtr_src2 (subtr_src2=filename2 to subtract)" +
				"\n***   subtr_dest (subtr_dest=filename3 to subtract)" +
				"\n***   q (q=integer to subtract cmd)" +
				"\n***   intersect (intersects int_src1 int_src2 and puts the innerproduct in int_dest. )" +
				"\n***   sumFiles=file1,file2,file3... (reads first line of each file and adds it using bigIntegers.)" +
				"\n***   range (gets the range and frequence of each line rzdzp freq (i.e. it drops domain))" +
				"\n***   range_src (for range cmd)" +
				"\n***   range_dest (for range cmd)" +
				"\n***   copytext (range_src=sourcefile, range_dest=destfile copies text sourcefile to destfile (lines in file should be <4000) " +
				"\n***   bashfile=intersect[=join;=sort;=freq;=range;=joinSorted;]" +
				"\n***   int_src1 (int_src1=filename1 to intersect)" +
				"\n***   int_src2 (int_src2=filename2 to intersect)" +
				"\n***   int_dest (int_dest=filename3 to intersect)" +
				"\n***   label_dest (adds label to the name of destfile)" +
				"\n***   label_src (adds label to the name of srcfile)" +
				"\n***   odometer (will create files erENn erEIn srENn srEIn using odometer instead of files. It does not partition into volumes.)" +
				"\n***   pause" +
				"\n***   log (will save log data to logfile.txt)" +
				"\n***   frequencies (adds the frequencies, counts the lines, and gives lenght of longest line)" +
				"\n***   freq_src (for frequencies)" +
				"\n***   freq_dest (for frequencies)" +
				"\n***   compress_src (to compress)" +
				"\n***   compress_dest (to compress)" +
				"\n***   compress (compress a sorted file by adding its frequencies)" +
				"\n*** TFilesEvenSLEbin goes through all the n-words of letters A0,A1,A2,A3,A4,A5. "+
				"Explanation of name: T=ThompsonT Files=input (n-1)-files, output n-files. Even=<(ab)^n,(ba)^n>=tau((ab)^2n). SLE=SmallLargeEqual it stores only w<ri(w) and w=ri(w) (but not w>ri(w)) ri=reverseinverse  " +
				"bin=the trees are saved in base 64."+
				"\n\nbat:"+ 
"\nTFilesEvenSLEbin.exe 2 volumesOutput_str=\"UWY^egio\"  antalVolumesOutput=8  antalVolumesInput=8 threadAntal=4 "+
"\n//it reads .dts, and compresses the results by adding a frequency if same doubletree is given."+
"\n//it creates .doubletree intersect2.bat n2.sh N2.stats sumintersections2.bat"+
"\n"+
"\nbash:"+
"\ncd /cygdrive/e/csharp/Projects/TOdometers/TFilesEvenSmallLargeEqualBin/files/bin/Debug/test"+
"\ncd /cygdrive/f/tests/test2"+
"\nsh n2.sh"+
"\n//it creates *.dts"+
"\n//delete *.doubletree"+
"\nbat:"+
"\nintersect2.bat"+
"\n//it creates erZeta.int srZeta.int"+
"\nsum_intersections2.bat"+
"\n//it creates erZeta.sum srZeta.sum"+
"\nImportant examples:"+
"\n>TFilesEvenSLEbin.exe 10 standardBatches" +
"\n>TFilesEvenSLEbin.exe 10 bashfile=joinSorted volumesOutput_str=\"UWY^egio\"  antalVolumesOutput=8  antalVolumesInput=8 " +
"\n>TFilesEvenSLEbin.exe 10 bashfile=range" +
				"\n*** " + fechaCompilado;


			if (args.Length == 0) {
				Console.WriteLine(programVersion); //Console.ReadKey();
				return;
			}

			int n = 1;
			if (int.TryParse(args[0], out n)) Console.WriteLine("n=" + n);
			else {
				Console.WriteLine("*** Could not understand " + args[0] + programVersion);
				//Console.ReadKey();
				return;
			}

			if (n > 20) {
				Console.WriteLine("WARNING!!!!: YOU MIGHT EXCEED THE MAXVALUE OF long integers");
				//long max value is 9.2x10^18
				//3*6^20 is 1.09x10^16  
			}
			int threadAntal = 3; int maxNumberOfLinesInReadingBlock = (int)Math.Pow(2, 19)/20;
			int maxNumberOfLinesInWritingBlock = (int)Math.Pow(2, 19)/20; string srcPath = ""; string destPath = "";
			string destInversesPath = "";
			char smallorequal = 'b';// 's'=small or 'e'=equal 'b'=both  
			int AntalVolumesInput = 8;
			int AntalVolumesOutput = 8;
			int AntalVolumesOutput_fromstr = 0;
			//string volumesOutput_str = @"0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\]^_`abcdefghijklmno";//"UWY^egio";
			string volumesOutput_str = "UWY^egio";
			bool showinfo = false;
			bool append = false;
			int startwithVolume = 0;
			bool pause = false;
			bool join = false;
			string compress_src = "", compress_dest = "";
			string freq_src = "", freq_dest="";
			string maincommand = "parallel";
			if (n == 1) maincommand = "odometer";
			logToFile = false;
			string intersect_src1 = "", intersect_src2 = "", intersect_dest="";
			string subtr_src1 = "", subtr_src2 = "", subtr_dest = "";
			string range_src = "", range_dest = "";

			string label_dest = "",label_src="";
			string makebash = "";
			string[] sumFiles = null;
			string sum_dest = "";
			string mono = "";
			long numberOfLinesInSubvolume = 0;
			int numberOfSubvolumes=0;
			long minSizeSubvolume=0;
			int q_number = 0;

			for (int i = 1; i < args.Length; i++) {
				string[] words = args[i].Split('=');
				int m;
				switch (words[0]) {
					case "numberOfLinesInSubvolume":
						if (words.Length != 2) { Console.WriteLine("Error: numberOfLinesInSubvolume=expecting a number"); return; }
						m = 0; if (int.TryParse(words[1], out m)) { numberOfLinesInSubvolume = m; Console.WriteLine("numberOfLinesInSubvolume=" + numberOfLinesInSubvolume); }
						else { Console.WriteLine("Error: numberOfLinesInSubvolume=could not read number"); return; }
						break;
					case "numberOfSubvolumes":
						if (words.Length != 2) { Console.WriteLine("Error: numberOfSubvolumes=expecting a number"); return; }
						m = 0; if (int.TryParse(words[1], out m)) { numberOfSubvolumes = m; Console.WriteLine("numberOfSubvolumes=" + numberOfSubvolumes); }
						else { Console.WriteLine("Error: numberOfSubvolumes=could not read number"); return; }
						break;
					case "minSizeSubvolume":
						if (words.Length != 2) { Console.WriteLine("Error: minSizeSubvolume=expecting a number"); return; }
						m = 0; if (int.TryParse(words[1], out m)) { minSizeSubvolume = m; Console.WriteLine("minSizeSubvolume=" + minSizeSubvolume); }
						else { Console.WriteLine("Error: minSizeSubvolume=could not read number"); return; }
						break;
					case "threadAntal":
						if (words.Length != 2) { Console.WriteLine("Error: threadAntal=expecting a number"); return; }
						m = 0; if (int.TryParse(words[1], out m)) { threadAntal = m; Console.WriteLine("threadAntal=" + threadAntal); }
						else { Console.WriteLine("Error: threadAntal=could not read number"); return; }
						break;
					case "maxNumberOfLinesInReadingBlock":
						if (words.Length != 2) { Console.WriteLine("Error: maxNumberOfLinesInReadingBlock=expecting a number"); return; }
						m = 0; if (int.TryParse(words[1], out m)) { maxNumberOfLinesInReadingBlock = m; Console.WriteLine("maxNumberOfLinesInReadingBlock=" + maxNumberOfLinesInReadingBlock); }
						else { Console.WriteLine("Error: maxNumberOfLinesInReadingBlock=could not read number"); return; }
						break;
					case "maxNumberOfLinesInWritingBlock":
						if (words.Length != 2) { Console.WriteLine("Error: maxNumberOfLinesInWritingBlock=expecting a number"); return; }
						m = 0; if (int.TryParse(words[1], out m)) { maxNumberOfLinesInWritingBlock = m; Console.WriteLine("maxNumberOfLinesInWritingBlock=" + maxNumberOfLinesInWritingBlock); }
						else { Console.WriteLine("Error: maxNumberOfLinesInWritingBlock=could not read number"); return; }
						break;
					case "sum_dest":
						if (words.Length != 2) { Console.WriteLine("Error: sum_dest=expecting a string"); return; }
						sum_dest = words[1]; Console.WriteLine("sum_dest=" + sum_dest);
						break;
					case "srcPath":
						if (words.Length != 2) { Console.WriteLine("Error: srcPath=expecting a string"); return; }
						srcPath = words[1]; Console.WriteLine("srcPath=" + srcPath);
						break;
					case "destPath":
						if (words.Length != 2) { Console.WriteLine("Error: destPath=expecting a string"); return; }
						destPath = words[1]; Console.WriteLine("destPath=" + destPath);
						break;
					case "mono":
						if (words.Length != 2) { Console.WriteLine("Error: mono=expecting a string"); return; }
						mono = words[1] + " "; Console.WriteLine("mono=" + mono);
						break;
					case "label_dest":
						if (words.Length != 2) { Console.WriteLine("Error: label_dest=expecting a string"); return; }
						label_dest = words[1]; Console.WriteLine("label_dest=" + label_dest);
						break;
					case "label_src":
						if (words.Length != 2) { Console.WriteLine("Error: label_src=expecting a string"); return; }
						label_src = words[1]; Console.WriteLine("label_src=" + label_src);
						break;
					case "destInversesPath":
						if (words.Length != 2) { Console.WriteLine("Error: destInversesPath=expecting a string"); return; }
						destInversesPath = words[1]; Console.WriteLine("destInversesPath=" + destInversesPath);
						break;
					case "smallOrEqual":
						if (words.Length != 2) { Console.WriteLine("Error: smallOrEqual=expecting a character s or e"); return; }
						if ((words[1][0] != 's') && (words[1][0] != 'e')) { Console.WriteLine("Error: smallOrEqual=s or e. but recieved: \'" + words[1][0] + "\'"); return; }
						smallorequal = words[1][0]; Console.WriteLine("smallOrEqual=" + smallorequal);
						break;
					case "antalVolumesInput":
						if (words.Length != 2) { Console.WriteLine("Error: antalVolumesInput=expecting a number"); return; }
						m = 0; if (int.TryParse(words[1], out m)) { AntalVolumesInput = m; Console.WriteLine("antalVolumesInput=" + AntalVolumesInput); }
						else { Console.WriteLine("Error: antalVolumesInput=could not read number"); return; }
						break;
					case "antalVolumesOutput":
						if (words.Length != 2) { Console.WriteLine("Error: antalVolumesOutput=expecting a number"); return; }
						m = 0; if (int.TryParse(words[1], out m)) { AntalVolumesOutput = m; Console.WriteLine("antalVolumesOutput=" + AntalVolumesOutput); }
						else { Console.WriteLine("Error: antalVolumesOutput=could not read number"); return; }
						break;
					case "volumesOutput_str":
						if (words.Length != 2) { Console.WriteLine("Error: volumesOutput_str=expecting a string"); return; }
						if (words[1].Length < 2) { Console.WriteLine("Error: volumesOutput_str=string length should be at least 2"); return; }
						volumesOutput_str = words[1]; Console.WriteLine("volumesOutput_str=" + volumesOutput_str);
						AntalVolumesOutput_fromstr = volumesOutput_str.Length;
						break;
					case "standardBatches":
						if (words.Length != 1) { Console.WriteLine("Error: standardBatches:: no extra parameters expected"); return; }
						maincommand = "standardBatches"; Console.WriteLine("standardBatches");
						break;
						
					case "pause":
						if (words.Length != 1) { Console.WriteLine("Error: pause:: no extra parameters expected"); return; }
						pause = true; Console.WriteLine("pause");
						break;
					case "range":
						if (words.Length != 1) { Console.WriteLine("Error: range:: no extra parameters expected"); return; }
						maincommand = "range"; Console.WriteLine("range");
						break;
					case "intersect":
						if (words.Length != 1) { Console.WriteLine("Error: intersect:: no extra parameters expected"); return; }
						maincommand = "intersect"; Console.WriteLine("intersect");
						break;
					case "copytext":
						if (words.Length != 1) { Console.WriteLine("Error: copytext:: no extra parameters expected"); return; }
						maincommand = "copytext"; Console.WriteLine("copytext");
						break;
					case "subtract":
						if (words.Length != 1) { Console.WriteLine("Error: subtract:: no extra parameters expected"); return; }
						maincommand = "subtract"; Console.WriteLine("subtract");
						break;
					case "q":
						if (words.Length != 2) { Console.WriteLine("Error: q=expecting a number"); return; }
						m = 0; if (int.TryParse(words[1], out m)) { q_number = m; Console.WriteLine("q=" + q_number); }
						else { Console.WriteLine("Error: q=could not read number"); return; }
						break;
					case "bashfile":
						if (words.Length != 2) { Console.WriteLine("Error: bashfile:: expecting a string"); return; }
						maincommand = "bashfile"; Console.WriteLine("bashfile");
						makebash = words[1];
						break;
					case "log":
						if (words.Length != 1) { Console.WriteLine("Error: log:: no extra parameters expected"); return; }
						logToFile = true; Console.WriteLine("log");
						break;
					case "frequencies":
						if (words.Length != 1) { Console.WriteLine("Error: frequencies:: no extra parameters expected"); return; }
						maincommand = "frequencies"; Console.WriteLine("frequencies");
						break;
					case "compress":
						if (words.Length != 1) { Console.WriteLine("Error: compress:: no extra parameters expected"); return; }
						Console.WriteLine("compress");
						maincommand = "compress";
						break;
					case "freq_src":
						if (words.Length != 2) { Console.WriteLine("Error: freq_src=expecting a string"); return; }
						freq_src = words[1]; Console.WriteLine("freq_src=" + freq_src);
						break;
					case "freq_dest":
						if (words.Length != 2) { Console.WriteLine("Error: freq_dest=expecting a string"); return; }
						freq_dest = words[1]; Console.WriteLine("freq_dest=" + freq_dest);
						break;
					case "range_src":
						if (words.Length != 2) { Console.WriteLine("Error: range_src=expecting a string"); return; }
						range_src = words[1]; Console.WriteLine("range_src=" + range_src);
						break;
					case "range_dest":
						if (words.Length != 2) { Console.WriteLine("Error: range_dest=expecting a string"); return; }
						range_dest = words[1]; Console.WriteLine("range_dest=" + range_dest);
						break;
					case "int_src1":
						if (words.Length != 2) { Console.WriteLine("Error: int_src1=expecting a string"); return; }
						intersect_src1 = words[1]; Console.WriteLine("int_src1=" + intersect_src1);
						break;
					case "int_src2":
						if (words.Length != 2) { Console.WriteLine("Error: int_src2=expecting a string"); return; }
						intersect_src2 = words[1]; Console.WriteLine("int_src2=" + intersect_src2);
						break;
					case "int_dest":
						if (words.Length != 2) { Console.WriteLine("Error: int_dest=expecting a string"); return; }
						intersect_dest = words[1]; Console.WriteLine("int_dest=" + intersect_dest);
						break;
					case "subtr_src1":
						if (words.Length != 2) { Console.WriteLine("Error: subtr_src1=expecting a string"); return; }
						subtr_src1 = words[1]; Console.WriteLine("subtr_src1=" + subtr_src1);
						break;
					case "subtr_src2":
						if (words.Length != 2) { Console.WriteLine("Error: subtr_src2=expecting a string"); return; }
						subtr_src2 = words[1]; Console.WriteLine("subtr_src2=" + subtr_src2);
						break;
					case "subtr_dest":
						if (words.Length != 2) { Console.WriteLine("Error: subtr_dest=expecting a string"); return; }
						subtr_dest = words[1]; Console.WriteLine("subtr_dest=" + subtr_dest);
						break;

					case "sumFiles":
						if (words.Length != 2) { Console.WriteLine("Error: sumFiles=expecting strings separeted by ,"); return; }
						sumFiles = words[1].Split(','); Console.WriteLine("sumFiles=" + string.Join(";", sumFiles));
						maincommand = "sumFiles";
						break;
					case "compress_src":
						if (words.Length != 2) { Console.WriteLine("Error: compress_src=expecting a string"); return; }
						compress_src = words[1]; Console.WriteLine("compress_src=" + compress_src);
						break;
					case "compress_dest":
						if (words.Length != 2) { Console.WriteLine("Error: compress_destc=expecting a string"); return; }
						compress_dest = words[1]; Console.WriteLine("compress_dest=" + compress_dest);
						break;

					case "odometer":
						if (words.Length != 1) { Console.WriteLine("Error: odometer:: no extra parameters expected"); return; }
						Console.WriteLine("odometer");
						maincommand = "odometer";
						break;

					case "help":
						if (words.Length != 1) { Console.WriteLine("Error: help:: no extra parameters expected"); return; }
						showinfo = true; Console.WriteLine("help");
						break;
					case "join":
						if (words.Length != 1) { Console.WriteLine("Error: join:: no extra parameters expected"); return; }
						join = true; Console.WriteLine("join (to compare with odometer output which is also created)");
						break;
					case "repartition":
						if (words.Length != 1) { Console.WriteLine("Error: repartition:: no extra parameters expected"); return; }
						Console.WriteLine("repartition");
						maincommand = "repartition";
						break;
					case "append":
						if (words.Length != 1) { Console.WriteLine("Error: append:: no extra parameters expected"); return; }
						append = true; Console.WriteLine("append");
						break;
					case "startWithVolume":
						if (words.Length != 2) { Console.WriteLine("Error: startWithVolume=expecting a number"); return; }
						m = 0; if (int.TryParse(words[1], out m)) { startwithVolume = m; Console.WriteLine("startWithVolume=" + startwithVolume); }
						else { Console.WriteLine("Error: startWithVolume=could not read number"); return; }
						break;
					case "bluff":
						if (words.Length != 1) { Console.WriteLine("Error: bluff:: no extra parameters expected"); return; }
						Console.WriteLine("bluff");
						maincommand = "bluff";
						break;

					default:
						Console.WriteLine("Error: could not read parameter:" + words[0]);
						return;
				}
			}

			if (AntalVolumesOutput_fromstr!=0 && ( AntalVolumesOutput != AntalVolumesOutput_fromstr)) {
				Console.WriteLine("Error: antalVolumesOutput("+AntalVolumesOutput+") does not match length of antalVolumes_str("+volumesOutput_str.Length+")");
			}


			Console.WriteLine("***MAINCOMMAND(" + maincommand + ")" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ***");
			log_WriteLine("***LOG ENTRY(" + maincommand + ")" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ***");

			if (showinfo) {
				Console.WriteLine("WARNING: wordfreq is space sensitive. That is, the format on each line of input files is assumed to be: word-space-freq-newline.");
			}

			switch (maincommand) {
				case "bluff": { Console.WriteLine("Bluffing!"); }
					break;
				case "sumFiles": {
						ExecuteParallelization executeParallelization = new ExecuteParallelization();
						executeParallelization.sum_Files(sumFiles,sum_dest,srcPath,append);
					}
					break;
				case "standardBatches": {
						ExecuteParallelization executeParallelization = new ExecuteParallelization();
						executeParallelization.standardBatches(n);
					}
					break;
				case "bashfile": 
					switch (makebash) {
						case "intersect": {
								ExecuteParallelization executeParallelization = new ExecuteParallelization();
							executeParallelization.bat_intersect(n: n, antalVolumesOutput: AntalVolumesOutput, label_dest: label_dest,mono:mono);
							}
							break;
						case "joinSorted": {
								ExecuteParallelization executeParallelization = new ExecuteParallelization();
								executeParallelization.bash_joinSorted(n_src: n, antalVolumesOutput: AntalVolumesOutput, label_dest: label_dest);
								break;
							}

						case "join": {
								ExecuteParallelization executeParallelization = new ExecuteParallelization();
								executeParallelization.bash_joinAndSort(n: n, antalVolumesOutput: AntalVolumesOutput, label_dest: label_dest);
							executeParallelization.bat_joinedCompress(n: n, label_dest: label_dest,mono:mono);
								executeParallelization.bash_joinedcompressedCompare(n: n, label_dest: label_dest);
							executeParallelization.bat_joinedIntersect(n: n, label_dest: label_dest,mono:mono);
							executeParallelization.bat_odomIntersect(n: n, label_dest: label_dest,mono:mono);
								break;
							}
						case "sort": {
								ExecuteParallelization executeParallelization = new ExecuteParallelization();
								executeParallelization.bash_sort(n: n, antalVolumesOutput: AntalVolumesOutput, label_dest: label_dest);
							executeParallelization.bat_intersect(n: n, antalVolumesOutput: AntalVolumesOutput, label_dest: label_dest,mono:mono);
								break;
							}
						case "freq": {
								ExecuteParallelization executeParallelization = new ExecuteParallelization();
							executeParallelization.bat_frequenciesCount(n: n, antalVolumesOutput: AntalVolumesOutput, label_dest: label_dest,mono:mono);
								break;
							}
						case "range": {
								ExecuteParallelization executeParallelization = new ExecuteParallelization();
								executeParallelization.bat_joinedRange(n_src: n, label_dest: label_dest, mono: mono);
							executeParallelization.bash_joinedCutSecondWord( n_src:n, label_dest:label_dest);
								break;
							}

						default: {
							Console.WriteLine("Error: Not found bashcommand=" + makebash);
							}
							break;
					}
					break;
				case "compress": {
						ExecuteParallelization executeParallelization = new ExecuteParallelization();

						executeParallelization.compressFilesBin_EvenSmallEqualwords(src: compress_src,
							dest: compress_dest,
							maxNumberOfLinesInReadingBlock: maxNumberOfLinesInReadingBlock,
							maxNumberOfLinesInWritingBlock: maxNumberOfLinesInWritingBlock,
								srcPath: srcPath,
								destPath: destPath,
								destInversesPath: destInversesPath,
							//FileLog: FileLog,
								showinfo: showinfo,
								append: append);
					}
					break;
				case "range": {
						ExecuteParallelization executeParallelization = new ExecuteParallelization();
						executeParallelization.getRangeAndFrequency(src: range_src, dest: range_dest, maxNumberOfLinesInReadingBlock: maxNumberOfLinesInReadingBlock,
							maxNumberOfLinesInWritingBlock: maxNumberOfLinesInWritingBlock, srcPath: srcPath, destPath: destPath, append: append); 
						break;
					}

				case "odometer": {
						if (n <= 1) Console.WriteLine("WARNING: n=0 or n=1 are not distributed in volumes nor are they sorted.");
						ExecuteOdometers odom = new ExecuteOdometers();
						//odometer.ExecuteCDonOdometerBinString(n, FileLog);
						//DoubleTreeFunctions.test();

						odom.ExecuteA0toA5onOdometerBinString_EvenSmallEqualwords(n,label_dest, showinfo);
					}
					break;
				case "parallel":
					if (n==0||n >= 2) {

						//ExecuteOdometers odometer = new ExecuteOdometers();
						//odometer.ExecuteCDonOdometerBinString(n, FileLog);
						//DoubleTreeFunctions.test();

						//odometer.ExecuteA0toA5onOdometerBinString_EvenSmallEqualwords(n, FileLog, showinfo);
						//LC_ALL=C sort srEI2.doubletree >srEI2.dts

						ExecuteParallelization executeParallelization = new ExecuteParallelization();
						switch (smallorequal) {
							case ('b'):
								executeParallelization.ExecuteA0toA5onFilesBin_EvenSmallEqualwords(n: n, threadAntal: threadAntal,
									maxNumberOfLinesInReadingBlock: maxNumberOfLinesInReadingBlock,
									maxNumberOfLinesInWritingBlock: maxNumberOfLinesInWritingBlock,
									srcPath: srcPath,
									label_src:label_src,
									destPath: destPath,
									label_dest: label_dest,
									destInversesPath: destInversesPath,
									smallorequal: 'e',
									join_bashcommand: join,
									mono:mono,
									antalVolumesInput: AntalVolumesInput,
									antalVolumesOutput: AntalVolumesOutput,
									volumesOutput_str: volumesOutput_str,
									showinfo: showinfo,
									append: false,
									startwithVolume: startwithVolume,
									numberOfLinesInSubvolume: numberOfLinesInSubvolume,
									numberOfSubvolumes:numberOfSubvolumes,
									minSizeSubvolume: minSizeSubvolume);
								executeParallelization.ExecuteA0toA5onFilesBin_EvenSmallEqualwords(n: n, threadAntal: threadAntal,
									maxNumberOfLinesInReadingBlock: maxNumberOfLinesInReadingBlock,
									maxNumberOfLinesInWritingBlock: maxNumberOfLinesInWritingBlock,
									srcPath: srcPath,
									label_src: label_src,
									destPath: destPath,
									label_dest: label_dest,
									destInversesPath: destInversesPath,
									smallorequal: 's',
									join_bashcommand: join,
									mono:mono,
									antalVolumesInput: AntalVolumesInput,
									antalVolumesOutput: AntalVolumesOutput,
									volumesOutput_str: volumesOutput_str,
									showinfo: showinfo,
									append: true,
									startwithVolume: startwithVolume,
									numberOfLinesInSubvolume: numberOfLinesInSubvolume,
									numberOfSubvolumes: numberOfSubvolumes,
									minSizeSubvolume: minSizeSubvolume);

								break;
							case ('e'):
								executeParallelization.ExecuteA0toA5onFilesBin_EvenSmallEqualwords(n: n, threadAntal: threadAntal,
									maxNumberOfLinesInReadingBlock: maxNumberOfLinesInReadingBlock,
									maxNumberOfLinesInWritingBlock: maxNumberOfLinesInWritingBlock,
									srcPath: srcPath,
									label_src: label_src,
									destPath: destPath,
									label_dest: label_dest,
									destInversesPath: destInversesPath,
									smallorequal: 'e',
									join_bashcommand: join,
									mono:mono,
									antalVolumesInput: AntalVolumesInput,
									antalVolumesOutput: AntalVolumesOutput,
									volumesOutput_str: volumesOutput_str,
									showinfo: showinfo,
									append: append,
									startwithVolume: startwithVolume,
									numberOfLinesInSubvolume: numberOfLinesInSubvolume,
									numberOfSubvolumes: numberOfSubvolumes,
									minSizeSubvolume: minSizeSubvolume);

								break;
							case ('s'):
								executeParallelization.ExecuteA0toA5onFilesBin_EvenSmallEqualwords(n: n, threadAntal: threadAntal,
									maxNumberOfLinesInReadingBlock: maxNumberOfLinesInReadingBlock,
									maxNumberOfLinesInWritingBlock: maxNumberOfLinesInWritingBlock,
									srcPath: srcPath,
									label_src: label_src,
									destPath: destPath,
									label_dest: label_dest,
									destInversesPath: destInversesPath,
									smallorequal: 's',
									join_bashcommand: join,
									mono:mono,
									antalVolumesInput: AntalVolumesInput,
									antalVolumesOutput: AntalVolumesOutput,
									volumesOutput_str: volumesOutput_str,
									showinfo: showinfo,
									append: append,
									startwithVolume: startwithVolume,
									numberOfLinesInSubvolume: numberOfLinesInSubvolume, 
									numberOfSubvolumes: numberOfSubvolumes,
									minSizeSubvolume: minSizeSubvolume);

								break;

						}
						if (join) {
							ExecuteOdometers odom = new ExecuteOdometers();
							odom.ExecuteA0toA5onOdometerBinString_EvenSmallEqualwords(n, label_dest, showinfo);
						}
					}
					break;
				case "repartition": {
						ExecuteParallelization executeParallelization = new ExecuteParallelization();

						executeParallelization.repartitionFilesBin_EvenSmallEqualwords(n: n,
							maxNumberOfLinesInReadingBlock: maxNumberOfLinesInReadingBlock,
							maxNumberOfLinesInWritingBlock: maxNumberOfLinesInWritingBlock,
							srcPath: srcPath,
							label_src: label_src,
							destPath: destPath,
							label_dest:label_dest,
							destInversesPath: destInversesPath,
							smallorequal: 'e',
							NorI: 'N',
							antalVolumesInput: AntalVolumesInput,
							antalVolumesOutput: AntalVolumesOutput,
							volumesOutput_str: volumesOutput_str,
							showinfo: showinfo,
							append: false,
							startwithVolume: startwithVolume);
						executeParallelization.repartitionFilesBin_EvenSmallEqualwords(n: n,
							maxNumberOfLinesInReadingBlock: maxNumberOfLinesInReadingBlock,
							maxNumberOfLinesInWritingBlock: maxNumberOfLinesInWritingBlock,
							srcPath: srcPath,
							label_src: label_src,
							destPath: destPath,
							label_dest: label_dest,
							destInversesPath: destInversesPath,
							smallorequal: 'e',
							NorI: 'I',
							antalVolumesInput: AntalVolumesInput,
							antalVolumesOutput: AntalVolumesOutput,
							volumesOutput_str: volumesOutput_str,
							showinfo: showinfo,
							append: false,
							startwithVolume: startwithVolume);
						executeParallelization.repartitionFilesBin_EvenSmallEqualwords(n: n,
							maxNumberOfLinesInReadingBlock: maxNumberOfLinesInReadingBlock,
							maxNumberOfLinesInWritingBlock: maxNumberOfLinesInWritingBlock,
							srcPath: srcPath,
							label_src: label_src,
							destPath: destPath,
							label_dest: label_dest,
							destInversesPath: destInversesPath,
							smallorequal: 's',
							NorI: 'N',
							antalVolumesInput: AntalVolumesInput,
							antalVolumesOutput: AntalVolumesOutput,
							volumesOutput_str: volumesOutput_str,
							showinfo: showinfo,
							append: false,
							startwithVolume: startwithVolume);
						executeParallelization.repartitionFilesBin_EvenSmallEqualwords(n: n,
							maxNumberOfLinesInReadingBlock: maxNumberOfLinesInReadingBlock,
							maxNumberOfLinesInWritingBlock: maxNumberOfLinesInWritingBlock,
							srcPath: srcPath,
							label_src: label_src,
							destPath: destPath,
							label_dest: label_dest,
							destInversesPath: destInversesPath,
							smallorequal: 's',
							NorI: 'I',
							antalVolumesInput: AntalVolumesInput,
							antalVolumesOutput: AntalVolumesOutput,
							volumesOutput_str: volumesOutput_str,
							showinfo: showinfo,
							append: false,
							startwithVolume: startwithVolume);
						break;
					}
				case "frequencies": {
						ExecuteParallelization executeParallelization = new ExecuteParallelization();
						executeParallelization.frequenciesAndCountingLines(
							src: freq_src,
							dest: freq_dest,
							maxNumberOfLinesInReadingBlock: maxNumberOfLinesInReadingBlock,
							srcPath: srcPath,
							destInversesPath: destInversesPath,
							showinfo: showinfo);
					}
					break;
				case "copytext": {
					ExecuteParallelization executeParallelization = new ExecuteParallelization();
					executeParallelization.copyTextFile(src: range_src, dest: range_dest);

					break;
					}

				case "intersect": {
						ExecuteParallelization executeParallelization = new ExecuteParallelization();
					executeParallelization.intersect(src1: intersect_src1, src2: intersect_src2,dest:intersect_dest,
							maxNumberOfLinesInReadingBlock: maxNumberOfLinesInReadingBlock,
							srcPath: srcPath,destPath:destPath,append:append,
							showinfo: showinfo);
					}
					break;
				case "subtract": {
						ExecuteParallelization executeParallelization = new ExecuteParallelization();
					//WARNING: we swap file1 with file2!!!
					executeParallelization.subtract(q: q_number,src1: subtr_src2, src2: subtr_src1, dest: subtr_dest,
								maxNumberOfLinesInReadingBlock: maxNumberOfLinesInReadingBlock,
								srcPath: srcPath, destPath: destPath, append: append,
								showinfo: showinfo);
					}
					break;
				default: Console.WriteLine("Not found main command " + maincommand);
					break;
			}

			if (pause) { Console.WriteLine("Press any key..."); Console.ReadKey(); }
		}//end Main


		public static void log_WriteLine(string str) {
			if (!logToFile) return;
			WBuffer FileLog = new WBuffer("logfile.txt", 1000, true);//Append to logfile.txt
			FileLog.WriteLine(str);
			FileLog.Close();
		}
		public static void log_Write(string str) {
			if (!logToFile) return;
			WBuffer FileLog = new WBuffer("logfile.txt", 1000, true);//Append to logfile.txt
			FileLog.Write(str);
			FileLog.Close();
		}
	}

}
