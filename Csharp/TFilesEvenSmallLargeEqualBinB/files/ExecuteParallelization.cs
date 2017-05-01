using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Numerics;


namespace TFilesEvenSLEbin {
	class ExecuteParallelization {

		private int strToFiles(WriteTextBuffer[] FilesForests, string str, int volumeslog2, string volumes_str) {
			if (str.Length == 0) {
				Console.WriteLine("Error in strToFiles: Attempting to save an empty doubleTree??");
				return 0;
			}
			int i = BinarySearcher.BinarySearch(str[0], volumeslog2, volumes_str);//i is between 0 and 7 by default. Run BinarySearcher.Example() to test it

			FilesForests[i].WriteLine(str);
			return i;
		}

		public void ExecuteA0toA5onFilesBin_EvenSmallEqualwords(int n, int threadAntal, int maxNumberOfLinesInReadingBlock,
			int maxNumberOfLinesInWritingBlock, string srcPath, string label_src,string destPath, string label_dest,string destInversesPath,
			char smallorequal, bool join_bashcommand, string mono,int antalVolumesInput = 1, int antalVolumesOutput = 0, string volumesOutput_str = "", bool showinfo = false,
			bool append = false, int startwithVolume = 0, long numberOfLinesInSubvolume=0, int numberOfSubvolumes=0, long minSizeSubvolume=0) {
			#region console information
			Console.WriteLine("Any key for progress info. x to stop program.");
			Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " Started");
			Console.WriteLine("Computing n(" + n + ") using " + threadAntal + " threads...");
			Program.log_WriteLine("Computing n(" + n + ") using " + threadAntal + " threads...");
			Console.WriteLine("srcPath= " + (srcPath == "" ? "same folder" : srcPath));
			Console.WriteLine("destPath= " + (destPath == "" ? "same folder" : destPath));
			Console.WriteLine("destInversesPath= " + (destInversesPath == "" ? "same folder" : destInversesPath));
			#endregion console information

			#region case n=0. Creating files for n=0
			if (n == 0) {  //CREATING files for n=0 EN0=(ab)^0={e} EIN=(ba)^0={0}
				string destfilename = "erEN0-0.dts";
				string destfilenameInverse = "erEI0-0.dts";
				WBuffer File_EN0 = new WBuffer(destfilename, 1, false);//does not append to file
				WBuffer File_EI0 = new WBuffer(destfilenameInverse, 1, false);//does not append to file
				Console.WriteLine("Saving to files: " + destfilename + ", " + destfilenameInverse);
				DoubleTree doubleTree = new DoubleTree();
				string str = "";
				string strinv = "";
				DoubleTreeFunctions.DoubleTreeToStringAndinvStringBin(doubleTree, ref str, ref strinv);
				File_EN0.WriteLine(str);
				File_EI0.WriteLine(strinv);
				File_EN0.Close();
				File_EI0.Close();
				return;
			}
			#endregion case n=0

			#region check if 2^volumenlog2 and volumesOutput_str.Length agree
			int volumeslog2 = (int)Math.Log(antalVolumesOutput, 2);
			if (antalVolumesOutput != 0 && (((int)Math.Pow(2, volumeslog2)) != volumesOutput_str.Length)) {
				Console.WriteLine("Error in ExecuteA0toA5..:: 2^volumeslog2 != volumes_str.length."
					+ "volumes=" + antalVolumesOutput + " volumeslog2=" + volumeslog2 + " volumes_str.length=" + volumesOutput_str.Length + " (use default:UWY^egio ).");
				return;
			}
			if (showinfo == true) { Console.WriteLine("volumes=" + antalVolumesOutput + " volumeslog2=" + volumeslog2 + "volumes_str=" + volumesOutput_str); }
			#endregion end check if 2^volumenlog2 and volumesOutput_str.Length agree

			#region declaring 4 destination files (for smallri and equalri and its inverses) and bash file

			string[] destfilesnames_s = new string[antalVolumesOutput];
			string[] destfilesnamesInverses_s = new string[antalVolumesOutput];
			WriteTextBuffer[] file_srENn = new WriteTextBuffer[antalVolumesOutput];
			WriteTextBuffer[] file_srEIn = new WriteTextBuffer[antalVolumesOutput];
			int[] antalSubVolumes_srENn = new int[antalVolumesOutput];
			int[] antalSubVolumes_srEIn = new int[antalVolumesOutput];
			long[] subVolume_srENn = new long[antalVolumesOutput];
			long[] subVolume_srEIn = new long[antalVolumesOutput];
			
			string[] destfilesnames_e = new string[antalVolumesOutput];
			string[] destfilesnamesInverses_e = new string[antalVolumesOutput];
			WriteTextBuffer[] file_erENn = new WriteTextBuffer[antalVolumesOutput];
			WriteTextBuffer[] file_erEIn = new WriteTextBuffer[antalVolumesOutput];

			if (numberOfLinesInSubvolume > 0 && numberOfSubvolumes > 0) {
				Console.WriteLine("Error:ExecuteA0toA5: Either numberOfLinesInSubvolume>0 ("+numberOfLinesInSubvolume+") or numberOfSubvolumes>0 ("+numberOfSubvolumes+") but not both. ");
				return;
			}
			bool makeSubvolumes = false;
			bool makeNumberOfSubvolumeLines = false;
			if (numberOfLinesInSubvolume > 0 ) makeSubvolumes = true;
			if (numberOfSubvolumes > 0) { makeSubvolumes = true; makeNumberOfSubvolumeLines = true; }
			
			
			//initialize files to where the doubletrees will go
			for (int i = 0; i < antalVolumesOutput; i++) {
				if (makeSubvolumes) {
					//small 
					destfilesnames_s[i] = "srEN" + n + label_dest + "-" + i + "-0.doubletree";
					destfilesnamesInverses_s[i] = "srEI" + n + label_dest + "-" + i + "-0.doubletree";
					Console.WriteLine("Saving to files: " + destfilesnames_s[i] + ", " + destfilesnamesInverses_s[i]);
					file_srENn[i] = new WriteTextBuffer(destPath + destfilesnames_s[i], append, maxNumberOfLinesInWritingBlock);
					file_srEIn[i] = new WriteTextBuffer(destInversesPath + destfilesnamesInverses_s[i], append, maxNumberOfLinesInWritingBlock);
				}
				else {
					//small 
					destfilesnames_s[i] = "srEN" + n + label_dest + "-" + i + ".doubletree";
					destfilesnamesInverses_s[i] = "srEI" + n + label_dest + "-" + i + ".doubletree";
					Console.WriteLine("Saving to files: " + destfilesnames_s[i] + ", " + destfilesnamesInverses_s[i]);
					file_srENn[i] = new WriteTextBuffer(destPath + destfilesnames_s[i], append, maxNumberOfLinesInWritingBlock);
					file_srEIn[i] = new WriteTextBuffer(destInversesPath + destfilesnamesInverses_s[i], append, maxNumberOfLinesInWritingBlock);
				}
				//equal
				destfilesnames_e[i] = "erEN" + n + label_dest + "-" + i + ".doubletree";
				destfilesnamesInverses_e[i] = "erEI" + n + label_dest + "-" + i + ".doubletree";
				Console.WriteLine("Saving to files: " + destfilesnames_e[i] + ", " + destfilesnamesInverses_e[i]);
				file_erENn[i] = new WriteTextBuffer(destPath + destfilesnames_e[i], append, maxNumberOfLinesInWritingBlock);
				file_erEIn[i] = new WriteTextBuffer(destInversesPath + destfilesnamesInverses_e[i], append, maxNumberOfLinesInWritingBlock);
			}
			bash_sort(n: n, label_dest: label_dest, antalVolumesOutput: antalVolumesOutput);
			bat_intersect(n: n, antalVolumesOutput: antalVolumesOutput, label_dest: label_dest,mono:mono);
			if (join_bashcommand) {
				bash_joinAndSort(n: n, antalVolumesOutput: antalVolumesOutput, label_dest: label_dest,bashappend:true);
				bat_joinedCompress(n: n, label_dest: label_dest,mono:mono);
				bash_joinedcompressedCompare(n: n, label_dest: label_dest);
			}

///////////////////////////////
			#endregion declaring destination files for smallri and equalri

			long maxfreq = 0;
			int volumesread = 0;
			long cumfreq=0,cumfreqinv=0;
			long linecount = 0;
			long longestlinelenght = 0;

			bool smaller = true; if (smallorequal == 'e') smaller = false;
			string srcfilesnames = "";

			using (Process p = Process.GetCurrentProcess())
				p.PriorityClass = ProcessPriorityClass.High;

			//reading 8 files. The files with inverses i.e. srEI or erEI are not read because they are computed from the N's.
			for (int j = startwithVolume; j < antalVolumesInput; j++) {
				string srcfilename = smallorequal + "rEN" + (n - 1)+label_src + "-" + j + ".dts";

				Console.WriteLine("Reading File: " + srcfilename);
				srcfilename = srcPath + srcfilename;

				if (File.Exists(srcfilename) == true) {
					long srcFileLength=new FileInfo(srcfilename).Length;
					if (srcFileLength == 0) continue;//ignores empty files
					srcfilesnames += srcfilename + " ";
					volumesread++;
					ReadBlock currentSnFile = new ReadBlock(srcfilename, fileshare:false,maxNumberOfLinesInBlock:maxNumberOfLinesInReadingBlock);
					ulong blockcounter = 0;//unnecessary counter

					bool doneReadingFile = false;

					BlockJob[] tasks = new BlockJob[threadAntal];
					Thread[] threads = new Thread[threadAntal];
					bool[] launchedTask = new bool[threadAntal];
					BlockOfLines[] blocks = new BlockOfLines[threadAntal];
					Array.Clear(tasks, 0, threadAntal);
					Array.Clear(launchedTask, 0, threadAntal);
					Array.Clear(threads, 0, threadAntal);

					//Thread.CurrentThread.Priority = ThreadPriority.Highest;


					if (makeNumberOfSubvolumeLines ) {
						numberOfLinesInSubvolume = srcFileLength / ((5+n)*numberOfSubvolumes);//each line is assumed to be about 5+n
						if (numberOfLinesInSubvolume < minSizeSubvolume) numberOfLinesInSubvolume = minSizeSubvolume/(5+n);
						if (numberOfLinesInSubvolume < 100) {
							Console.WriteLine("Error:ExecuteA0toA5: srcFileLength /((n+5)*numberOfSubvolumes)=" + numberOfLinesInSubvolume +
								"is too small. It should be at least 100.");
							return;
						}
						Console.WriteLine("makenumberOfSubvolumes:NumberOfLinesInSubvolume=" + numberOfLinesInSubvolume);
					}
					while (doneReadingFile == false) {
						for (int i = 0; i < threadAntal; i++) {
							if (launchedTask[i] == false) {
								blocks[i] = currentSnFile.fillcompact();
								if (blocks[i]==null) {
									//Console.WriteLine("blocks" + i);
									doneReadingFile = true;
								}
								else {
									++blockcounter;
									launchedTask[i] = true;
									tasks[i] = new BlockJob(blocks[i], smaller);
									threads[i] = new Thread(tasks[i].ApplyAiToBlock);
									threads[i].Priority = ThreadPriority.Highest;
									threads[i].Start();
									
									//Console.WriteLine("Launched Thread " + i + " with block " + blockcounter);
								}
							}
							else {//Task i has been launched
								if (threads[i].IsAlive == false) {
									//collect the results and send them to file
									//if I comment following line the processor jumps to 80% with 3 threads
									for (int k = 0; k < tasks[i].blockOutput.numberOfLines; k++) {
										WordFreq wordfreq = tasks[i].blockOutput.ReadLine(); cumfreq += wordfreq.freq; linecount++;
										if (maxfreq < wordfreq.freq) maxfreq = wordfreq.freq;
										if (longestlinelenght < wordfreq.word.Length) longestlinelenght = wordfreq.word.Length;
										if (wordfreq.smaller) {
											int vol=strToFiles(file_srENn, wordfreq.asString(), volumeslog2, volumesOutput_str);
											if (makeSubvolumes) {
												subVolume_srENn[vol]++;
												if (subVolume_srENn[vol] > numberOfLinesInSubvolume) {//close subvolume and start new one
													//small 
													file_srENn[vol].Close();
													destfilesnames_s[vol] = "srEN" + n + label_dest + "-" + vol + "-" + (++antalSubVolumes_srENn[vol]) + ".doubletree";
													Console.WriteLine("Saving to file: " + destfilesnames_s[vol]);
													file_srENn[vol] = new WriteTextBuffer(destPath + destfilesnames_s[vol], append, maxNumberOfLinesInWritingBlock);
													subVolume_srENn[vol] = 0;							
												}
											}
										}
										else
											strToFiles(file_erENn, wordfreq.asString(), volumeslog2, volumesOutput_str);
										WordFreq wordfreqinv = tasks[i].blockOutputInverses.ReadLine(); cumfreqinv += wordfreqinv.freq;
										if (wordfreqinv.smaller) {
											int vol=strToFiles(file_srEIn, wordfreqinv.asString(), volumeslog2, volumesOutput_str);
											if (makeSubvolumes) {
												subVolume_srEIn[vol]++;
												if (subVolume_srEIn[vol] > numberOfLinesInSubvolume) {//close subvolume and start new one
													//small 
													file_srEIn[vol].Close();
													destfilesnamesInverses_s[vol] = "srEI" + n + label_dest + "-" + vol + "-" + (++antalSubVolumes_srEIn[vol]) + ".doubletree";
													Console.WriteLine("Saving to file: " + destfilesnamesInverses_s[vol]);
													file_srEIn[vol] = new WriteTextBuffer(destInversesPath + destfilesnamesInverses_s[vol], append, maxNumberOfLinesInWritingBlock);
													subVolume_srEIn[vol] = 0;
												}
											}
										}
										else
											strToFiles(file_erEIn, wordfreqinv.asString(), volumeslog2, volumesOutput_str);
									}

									launchedTask[i] = false;




								}
							}
						}
						// check if key has been pressed. Exit if x has been pressed
						if (Console.KeyAvailable) {
							float percent = ((float)currentSnFile.readSoFar) / (float)currentSnFile.fileLength * 100;
							Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " % read= " + percent + ". Working on block  " + blockcounter);
							ConsoleKeyInfo cki = Console.ReadKey(true);
							if (cki.Key == ConsoleKey.X) break;
						}

					}
					//Wait for all threads to be done.
					Console.Write("Done reading file " + srcfilename + ". Waiting for threads to finish...");
					for (int i = 0; i < threadAntal; i++) {
						if (launchedTask[i] == true) threads[i].Join();
					}
					Console.WriteLine("done.");
					//collect the results and send them to file
					for (int i = 0; i < threadAntal; i++) {
						if (launchedTask[i] == true)
							for (int k = 0; k < tasks[i].blockOutput.numberOfLines; k++) {
								WordFreq wordfreq = tasks[i].blockOutput.ReadLine(); cumfreq += wordfreq.freq; linecount++;
								if (maxfreq < wordfreq.freq) maxfreq = wordfreq.freq;
								if (longestlinelenght < wordfreq.word.Length) longestlinelenght = wordfreq.word.Length;
								if (wordfreq.smaller)
									strToFiles(file_srENn, wordfreq.asString(), volumeslog2, volumesOutput_str);
								else
									strToFiles(file_erENn, wordfreq.asString(), volumeslog2, volumesOutput_str);
								WordFreq wordfreqinv = tasks[i].blockOutputInverses.ReadLine(); cumfreqinv += wordfreqinv.freq;
								if (wordfreqinv.smaller)
									strToFiles(file_srEIn, wordfreqinv.asString(), volumeslog2, volumesOutput_str);
								else
									strToFiles(file_erEIn, wordfreqinv.asString(), volumeslog2, volumesOutput_str);
							}
					}

					currentSnFile.Close();
				}
			}

			Console.WriteLine("Freq = " + cumfreq + " for " + smallorequal + "rN" + n);
			Console.WriteLine("Freq = " + cumfreqinv + " for " + smallorequal + "rI" + n);
			Console.WriteLine("maxfreq = " + maxfreq);
			Program.log_WriteLine("Freq = " + cumfreq + " for " + smallorequal + "rN" + n);
			Program.log_WriteLine("Freq = " + cumfreqinv + " for " + smallorequal + "rI" + n);
			string dest = "N" + n + ".stats";
			Console.WriteLine("Writing file: " + dest);
			WriteTextB stats_file = new WriteTextB(dest, true);
			stats_file.WriteLine("cumfreq(n=" + n + ") = " + cumfreq);
			stats_file.WriteLine("maxfreq(n=" + n + ") = " + maxfreq);
			//stats_file.WriteLine(cumfreq.ToString());
			stats_file.WriteLine("cumFreq = " + cumfreq + ", lines = " + linecount + ", longest line length= " + longestlinelenght + " from prefiles(n="+n+")" +srcfilesnames);
			stats_file.Close();


			if (volumesread == 0) Console.WriteLine("***WARNING: No volumes read (they were either empty or non-existent.)(n=" + n + ").");
			//close the doubletree files (srENn srEIn erENn erEIn )
			for (int i = 0; i < antalVolumesOutput; i++) {
				file_srENn[i].Close();
				file_srEIn[i].Close();
				file_erENn[i].Close();
				file_erEIn[i].Close();
			}
		}


		public void repartitionFilesBin_EvenSmallEqualwords(int n, int maxNumberOfLinesInReadingBlock,
			int maxNumberOfLinesInWritingBlock, string srcPath,string label_src, string destPath,string label_dest, string destInversesPath,
			char smallorequal, char NorI, int antalVolumesInput = 1, int antalVolumesOutput = 0, string volumesOutput_str = "", bool showinfo = false,
			bool append = false, int startwithVolume = 0) {
			#region console information
			Console.WriteLine("Any key for progress info. x to stop program.");
			Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " Started");
			Console.WriteLine("Repartitioning n(" + n + ").");
			Console.WriteLine("srcPath= " + (srcPath == "" ? "same folder" : srcPath));
			Console.WriteLine("destPath= " + (destPath == "" ? "same folder" : destPath));
			Console.WriteLine("destInversesPath= " + (destInversesPath == "" ? "same folder" : destInversesPath));
			#endregion console information

			#region check if 2^volumenlog2 and volumesOutput_str.Length agree
			int volumeslog2 = (int)Math.Log(antalVolumesOutput, 2);
			if (antalVolumesOutput != 0 && (((int)Math.Pow(2, volumeslog2)) != volumesOutput_str.Length)) {
				Console.WriteLine("Error in ExecuteA0toA5..:: 2^volumeslog2 != volumes_str.length."
					+ "volumes=" + antalVolumesOutput + " volumeslog2=" + volumeslog2 + " volumes_str.length=" + volumesOutput_str.Length + " (use default:UWY^egio ).");
				return;
			}
			if (showinfo == true) { Console.WriteLine("volumes=" + antalVolumesOutput + " volumeslog2=" + volumeslog2 + "volumes_str=" + volumesOutput_str); }
			#endregion end check if 2^volumenlog2 and volumesOutput_str.Length agree

			#region declaring 4 destination files (for smallri and equalri and its inverses)

			string[] destfilesnames_s = new string[antalVolumesOutput];
			string[] destfilesnamesInverses_s = new string[antalVolumesOutput];
			WriteTextBuffer[] file_srENn = new WriteTextBuffer[antalVolumesOutput];
			WriteTextBuffer[] file_srEIn = new WriteTextBuffer[antalVolumesOutput];
			string[] destfilesnames_e = new string[antalVolumesOutput];
			string[] destfilesnamesInverses_e = new string[antalVolumesOutput];
			WriteTextBuffer[] file_erENn = new WriteTextBuffer[antalVolumesOutput];
			WriteTextBuffer[] file_erEIn = new WriteTextBuffer[antalVolumesOutput];
			//initialize files to where the doubletrees will go
			for (int i = 0; i < antalVolumesOutput; i++) {
				//small 
				destfilesnames_s[i] = "srEN" + n + label_dest + "-" + i + ".doubletree";
				destfilesnamesInverses_s[i] = "srEI" + n + label_dest + "-" + i + ".doubletree";
				Console.WriteLine("Saving to files: " + destfilesnames_s[i] + ", " + destfilesnamesInverses_s[i]);
				file_srENn[i] = new WriteTextBuffer(destPath + destfilesnames_s[i], append, maxNumberOfLinesInWritingBlock);
				file_srEIn[i] = new WriteTextBuffer(destInversesPath + destfilesnamesInverses_s[i], append, maxNumberOfLinesInWritingBlock);
				//equal
				destfilesnames_e[i] = "erEN" + n + label_dest + "-" + i + ".doubletree";
				destfilesnamesInverses_e[i] = "erEI" + n + label_dest + "-" + i + ".doubletree";
				Console.WriteLine("Saving to files: " + destfilesnames_e[i] + ", " + destfilesnamesInverses_e[i]);
				file_erENn[i] = new WriteTextBuffer(destPath + destfilesnames_e[i], append, maxNumberOfLinesInWritingBlock);
				file_erEIn[i] = new WriteTextBuffer(destInversesPath + destfilesnamesInverses_e[i], append, maxNumberOfLinesInWritingBlock);
			}
			#endregion declaring destination files for smallri and equalri

			int volumesread = 0;
			//reading 8 files.
			for (int j = startwithVolume; j < antalVolumesInput; j++) {
				string srcfilename = smallorequal + "rE"+NorI + n+label_src + "-" + j + ".doubletree";

				Console.WriteLine("Reading File: " + srcfilename);
				srcfilename = srcPath + srcfilename;

				if (File.Exists(srcfilename) == true) {
					if (new FileInfo(srcfilename).Length == 0) continue;//ignores empty files
					volumesread++;
					ReadBlock currentSnFile = new ReadBlock(srcfilename,fileshare:false, maxNumberOfLinesInBlock:maxNumberOfLinesInReadingBlock);

					BlockOfLines block;


					while ((block = currentSnFile.fillcompact())!=null) {
						for (int k = 0; k < block.numberOfLines; k++) {
							WordFreq wordfreq = block.ReadLine();
							if (wordfreq.smaller)
								strToFiles(file_srENn, wordfreq.asString(), volumeslog2, volumesOutput_str);
							else
								strToFiles(file_erENn, wordfreq.asString(), volumeslog2, volumesOutput_str);
						}

						// check if key has been pressed. Exit if x has been pressed
						if (Console.KeyAvailable) {
							float percent = ((float)currentSnFile.readSoFar) / (float)currentSnFile.fileLength * 100;
							Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " % read= " + percent);
							ConsoleKeyInfo cki = Console.ReadKey(true);
							if (cki.Key == ConsoleKey.X) break;
						}

					}

					currentSnFile.Close();
				}
			}

			if (volumesread == 0) Console.WriteLine("***WARNING: No volumes read (they were either empty or non-existent.)(n=" + n + ").");
			//close the doubletree files (srENn srEIn erENn erEIn )
			for (int i = 0; i < antalVolumesOutput; i++) {
				file_srENn[i].Close();
				file_srEIn[i].Close();
				file_erENn[i].Close();
				file_erEIn[i].Close();
			}
		}

		public void compressFilesBin_EvenSmallEqualwords(string src, string dest, int maxNumberOfLinesInReadingBlock,
			int maxNumberOfLinesInWritingBlock, string srcPath, string destPath, string destInversesPath,
			bool showinfo = false, bool append = false) {
			#region console information
			Console.WriteLine("Any key for progress info. x to stop program.");
			Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " Started");
			Console.WriteLine("Compressing:" + src + ".");
			Console.WriteLine("srcPath= " + (srcPath == "" ? "same folder" : srcPath));
			Console.WriteLine("destPath= " + (destPath == "" ? "same folder" : destPath));
			#endregion console information

			if (dest == "") { Console.WriteLine("Error:destFile was not given:" + dest.Length + "(compress_dest=destfilename)."); return; }
			string destfilename = destPath + dest;
			WriteTextB file_dest = new WriteTextB(destfilename, append);
			Console.WriteLine("Reading File: " + src);
			string srcfilename = srcPath + src;

			if (File.Exists(srcfilename) == false) { Console.WriteLine("Error: srcFile does not exists:" + srcfilename + "(compress_src=srcfilename)."); return; }

			if (new FileInfo(srcfilename).Length == 0) { Console.WriteLine("Warning: srcFile " + src + " is empty"); };//ignores empty files
			//in the future change ReadBlock to read a certain size of bytes rather than lines. As of now, the lines should not be greater than 4000
			ReadBlock currentSnFile = new ReadBlock(srcfilename, fileshare: true, maxNumberOfLinesInBlock:maxNumberOfLinesInReadingBlock);
			Console.WriteLine("Writing File: " +dest);

			BlockOfLines block;
			
			long cumfreq = 0;
			while ((block = currentSnFile.fillcompact())!=null) {				
				for (int k = 0; k < block.numberOfLines; k++) {
					WordFreq wordfreq = block.ReadLine();
					cumfreq += wordfreq.freq;
					file_dest.WriteLine(wordfreq.asString());
				}
				// check if key has been pressed. Exit if x has been pressed
				if (Console.KeyAvailable) {
					float percent = ((float)currentSnFile.readSoFar) / (float)currentSnFile.fileLength * 100;
					Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "% read= " + percent);
					ConsoleKeyInfo cki = Console.ReadKey(true);
					if (cki.Key == ConsoleKey.X) break;
				}
			}
			currentSnFile.Close();
			Console.WriteLine("Freq = " + cumfreq +" for file "+dest);
			Program.log_WriteLine("Freq = " + cumfreq + " for file " + dest);
			//close the files
			file_dest.Close();
		}

		public void frequenciesAndCountingLines(string src,string dest, int maxNumberOfLinesInReadingBlock,
			 string srcPath, string destInversesPath,
			bool showinfo = false) {
			#region console information
			Console.WriteLine("Any key for progress info. x to stop program.");
			Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " Started");
			Console.WriteLine("FrequenciesOfFile: " + src + ".");
			Console.WriteLine("srcPath= " + (srcPath == "" ? "same folder" : srcPath));
			#endregion console information

			//Console.WriteLine("Reading File: " + src);
			string srcfilename = srcPath + src;

			if (File.Exists(srcfilename) == false) { Console.WriteLine("Error: srcFile does not exists:" + srcfilename + "(freq_src=srcfilename)."); return; }

			if (new FileInfo(srcfilename).Length == 0) { Console.WriteLine("Warning: srcFile " + src + " is empty"); };//ignores empty files
			ReadBlock currentSnFile = new ReadBlock(srcfilename, fileshare:true,maxNumberOfLinesInBlock:maxNumberOfLinesInReadingBlock);

			BlockOfLines block;
			long cumfreq = 0;
			long linecount = 0;
			long longestlinelenght = 0;
			while ((block = currentSnFile.fillcompact()) != null) {
				for (int k = 0; k < block.numberOfLines; k++) {
					WordFreq wordfreq = block.ReadLine();
					cumfreq += wordfreq.freq;
					linecount++;
					if (longestlinelenght < wordfreq.word.Length) longestlinelenght = wordfreq.word.Length;
				}
				// check if key has been pressed. Exit if x has been pressed
				if (Console.KeyAvailable) {
					float percent = ((float)currentSnFile.readSoFar) / (float)currentSnFile.fileLength * 100;
					Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " % read= " + percent);
					ConsoleKeyInfo cki = Console.ReadKey(true);
					if (cki.Key == ConsoleKey.X) break;
				}
			}
			//lines gives you the geodesics
			//freq gives you the total number of words
			Console.WriteLine("freq::Freq = " + cumfreq + ", lines = " + linecount + ", longest line length= " + longestlinelenght + " in file: " + src);
			Program.log_WriteLine("freq::Freq = " + cumfreq + ", lines = " + linecount + ", longest line length= " + longestlinelenght + " in file: " + src);
			//close the files
			currentSnFile.Close();
			if (dest == "") {
				Console.WriteLine("Warning:: freq:: No destination file was given. (freq_dest=destfilename)");
				return;
			}
			Console.WriteLine("Writing file: " + dest);
			WriteTextB freq_file = new WriteTextB(dest, false);
			freq_file.WriteLine(cumfreq.ToString());
			freq_file.WriteLine("freq::Freq = " + cumfreq + ", lines = " + linecount + ", longest line length= " + longestlinelenght + " in file: " + src);
			freq_file.Close();
		}

		public void getRangeAndFrequency(string src, string dest, int maxNumberOfLinesInReadingBlock, int maxNumberOfLinesInWritingBlock, string srcPath, string destPath, bool append) {
			string srcfilename = srcPath + src;
			if (File.Exists(srcfilename) == false) { Console.WriteLine("Error: srcFile does not exists:" + srcfilename + "(range_src=srcfilename)."); return; }
			ReadTextB srcFile = new ReadTextB(srcfilename,fileShare:true);
			WriteTextBuffer file_dest = new WriteTextBuffer(destPath + dest, append, maxNumberOfLinesInWritingBlock);
			string line;
			Console.WriteLine("Writing file: " + dest);
			while ((line = srcFile.ReadLine()) != null) {
				string range, freq;
				string[] words = line.Split('z');
				string[] freqwords = line.Split(' ');
				if (freqwords.Length == 2) freq = " " + freqwords[1]; else freq = "";
				if (words.Length != 0) range = words[0]; else range = "z";
				file_dest.WriteLine(range + freq);
			}
			srcFile.Close();
			file_dest.Close();
		}
		
		
		//dest=-q src1+src2
		public void subtract(int q,string src1, string src2, string dest, int maxNumberOfLinesInReadingBlock,
			 string srcPath, string destPath, bool append,
			bool showinfo = false) {
			#region console information
			Console.WriteLine("Any key for progress info. x to stop program.");
			Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " Started");
			Console.WriteLine("subtractFiles: " + src1 + " - "+q+" " + src2);
			Console.WriteLine("srcPath= " + (srcPath == "" ? "same folder" : srcPath));
			#endregion console information

			//Console.WriteLine("Reading File: " + src);
			string srcfilename1 = srcPath + src1;
			string srcfilename2 = srcPath + src2;

			if (File.Exists(srcfilename1) == false) { Console.WriteLine("Error: srcFile1 does not exists:" + srcfilename1 + "(subtr_src1=srcfilename)."); return; }
			if (File.Exists(srcfilename2) == false) { Console.WriteLine("Error: srcFile2 does not exists:" + srcfilename2 + "(subtr_src2=srcfilename)."); return; }

			if (new FileInfo(srcfilename1).Length == 0) { Console.WriteLine("Error: srcFile1 " + src1 + " is empty. Nothing to compare."); return; };
			if (new FileInfo(srcfilename2).Length == 0) { Console.WriteLine("Error: srcFile2 " + src2 + " is empty. Nothing to compare."); return; };
			////
			if (dest == "") { Console.WriteLine("Error:destFile was not given:" + dest.Length + "(subtr_dest=destfilename)."); return; }
			string destfilename = destPath + dest;
			WriteTextB file_dest = new WriteTextB(destfilename, append);
			Console.WriteLine("Writing File: " + dest);
			//////
			using (Process p = Process.GetCurrentProcess())
				p.PriorityClass = ProcessPriorityClass.High;


			ReadBlock currentSnFile1 = new ReadBlock(srcfilename1, fileshare:true, maxNumberOfLinesInBlock:maxNumberOfLinesInReadingBlock);
			ReadBlock currentSnFile2 = new ReadBlock(srcfilename2, fileshare: true, maxNumberOfLinesInBlock:maxNumberOfLinesInReadingBlock);

			BlockOfLines block1, block2;
			long cumfreq1 = 0, cumfreq2 = 0;
			long linecount1 = 0, linecount2 = 0;
			long longestlinelenght1 = 0, longestlinelenght2 = 0;
			int cmp;
			long subtractionproduct = 0;


			block1 = currentSnFile1.fillcompact();
			block2 = currentSnFile2.fillcompact();
			WordFreq wordfreq1 = null;
			WordFreq wordfreq2 = null;
			if (block1 != null) {//read w1
				wordfreq1 = block1.ReadLine();
				cumfreq1 += wordfreq1.freq;
				linecount1++;
				if (longestlinelenght1 < wordfreq1.word.Length) longestlinelenght1 = wordfreq1.word.Length;
			}
			if (block2 != null) {//read w2
				wordfreq2 = block2.ReadLine();
				cumfreq2 += wordfreq2.freq;
				linecount2++;
				if (longestlinelenght2 < wordfreq2.word.Length) longestlinelenght2 = wordfreq2.word.Length;
			}

			bool donereadingfile2 = false;
			while (block1 != null && block2 != null) {
				while ((cmp = string.CompareOrdinal(wordfreq1.word, wordfreq2.word)) >= 0) { //read w2 while w1>=w2
					if (cmp == 0) {//intersection found i.e.w1=w2
						subtractionproduct = -q*wordfreq1.freq + wordfreq2.freq;
						if(subtractionproduct!=0)file_dest.WriteLine((new WordFreq(wordfreq2.word,subtractionproduct)).asString());
						if (showinfo) Console.WriteLine("subtract:: " + wordfreq1.asString() + " == " + wordfreq2.asString() + " ::lines(compact): " + linecount1 + ", " + linecount2 + ".");
					}
					else {//w1>w2
						file_dest.WriteLine(wordfreq2.asString());
						if (showinfo) Console.WriteLine("subtract:: " + wordfreq1.asString() + " > " + wordfreq2.asString() + " ::lines(compact): " + linecount1 + ", " + linecount2 + ".");


					}
					#region read w2
					if ((wordfreq2 = block2.ReadLine()) == null) {//reads w2 and fills buffer if necessary
						if ((block2 = currentSnFile2.fillcompact()) == null) { donereadingfile2 = true; break; }//done reading file2(and w2=null) 
						if ((wordfreq2 = block2.ReadLine()) == null) { Console.WriteLine("Something went wrong....read empty block!"); break; }
					}
					cumfreq2 += wordfreq2.freq;
					linecount2++;
					if (longestlinelenght2 < wordfreq2.word.Length) longestlinelenght2 = wordfreq2.word.Length;
					// check if key has been pressed. Exit if x has been pressed
					if (Console.KeyAvailable) {
						float percent = ((float)currentSnFile2.readSoFar) / (float)currentSnFile2.fileLength * 100;
						Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "File2: % read= " + percent);
						ConsoleKeyInfo cki = Console.ReadKey(true);
						if (cki.Key == ConsoleKey.X) break;
					}
					#endregion read w2
				}
				if (donereadingfile2) break;// done because file2 has been read completely.
				//w1< w2
				#region read w1
				if ((wordfreq1 = block1.ReadLine()) == null) {//reads w1 and fills buffer if necessary
					if ((block1 = currentSnFile1.fillcompact()) == null) { break; }//done reading file1(and w1=null) 
					if ((wordfreq1 = block1.ReadLine()) == null) { Console.WriteLine("Something went wrong file1....read empty block!"); break; }
				}
				cumfreq1 += wordfreq1.freq;
				linecount1++;
				if (longestlinelenght1 < wordfreq1.word.Length) longestlinelenght1 = wordfreq1.word.Length;
				// check if key has been pressed. Exit if x has been pressed
				if (Console.KeyAvailable) {
					float percent = ((float)currentSnFile1.readSoFar) / (float)currentSnFile1.fileLength * 100;
					Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "File1: % read= " + percent);
					ConsoleKeyInfo cki = Console.ReadKey(true);
					if (cki.Key == ConsoleKey.X) break;
				}
				#endregion read w1
			}
			if (!donereadingfile2) {//done reading file1. dumping rest of file2 into destfile.
				if(wordfreq2!=null)file_dest.WriteLine(wordfreq2.asString());
				if (wordfreq2 != null &&showinfo) Console.WriteLine("subtract:: fil2:" + wordfreq2.asString() + "::lines(compact), " + linecount2 + ".");
				
				while ((wordfreq2 = block2.ReadLine()) != null) {
					file_dest.WriteLine(wordfreq2.asString());
						if (showinfo) Console.WriteLine("subtract:: fil2:" + wordfreq2.asString() + "::lines(compact), " + linecount2 + ".");
				}
				while ((block2 = currentSnFile2.fillcompact()) != null) {
					while ((wordfreq2 = block2.ReadLine()) != null) {
						file_dest.WriteLine(wordfreq2.asString());
						if (showinfo) Console.WriteLine("subtract:: fil2:" + wordfreq2.asString() + " ::lines(compact): "   + linecount2 + ".");
					}

				}
			}
			//close the files
			currentSnFile1.Close();
			currentSnFile2.Close();
			file_dest.Close();

			//lines gives you the geodesics of either file1 or file2 (only one of the files is read completely)
			//freq gives you the total number of words of either file 1 or file 2 (only one of the files is read completely)
			//Console.WriteLine("intersection= " + subtractionproduct + "  for files " + src1 + " " + src2);
			//Console.WriteLine("intersect::Freq1 = " + cumfreq1 + ", lines = " + linecount1 + ", longest line length= " + longestlinelenght1 + " in file: " + src1);
			//Console.WriteLine("intersect::Freq2 = " + cumfreq2 + ", lines = " + linecount2 + ", longest line length= " + longestlinelenght2 + " in file: " + src2);
			//if (dest == "") {
			//	Console.WriteLine("Warning:intersection: no destination file was given "); return;
			//}
			//WriteText intersection_file = new WriteText(destPath + dest, append);
			//intersection_file.WriteLine(subtractionproduct.ToString());
			//intersection_file.WriteLine("intersection= " + subtractionproduct + "\t\t\t\t for files " + src1 + " " + src2);
			//intersection_file.WriteLine("intersect::Freq1 = " + cumfreq1 + ", lines = " + linecount1 + ", longest line length= " + longestlinelenght1 + " in file: " + src1);
			//intersection_file.WriteLine("intersect::Freq2 = " + cumfreq2 + ", lines = " + linecount2 + ", longest line length= " + longestlinelenght2 + " in file: " + src2);
			//intersection_file.Close();
		}

		public void intersect(string src1, string src2,string dest, int maxNumberOfLinesInReadingBlock,
			 string srcPath,string destPath,bool append,
			bool showinfo = false) {
			#region console information
			Console.WriteLine("Any key for progress info. x to stop program.");
			Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " Started");
			Console.WriteLine("intersectFiles: " + src1 + " " + src2);
			Console.WriteLine("srcPath= " + (srcPath == "" ? "same folder" : srcPath));
			#endregion console information

			//Console.WriteLine("Reading File: " + src);
			string srcfilename1 = srcPath + src1;
			string srcfilename2 = srcPath + src2;

			if (File.Exists(srcfilename1) == false) { Console.WriteLine("Error: srcFile1 does not exists:" + srcfilename1 + "(int_src1=srcfilename)."); return; }
			if (File.Exists(srcfilename2) == false) { Console.WriteLine("Error: srcFile2 does not exists:" + srcfilename2 + "(int_src2=srcfilename)."); return; }

			if (new FileInfo(srcfilename1).Length == 0) { Console.WriteLine("Error: srcFile1 " + src1 + " is empty. Nothing to compare."); return; };
			if (new FileInfo(srcfilename2).Length == 0) { Console.WriteLine("Error: srcFile2 " + src2 + " is empty. Nothing to compare."); return; };

			using (Process p = Process.GetCurrentProcess())
				p.PriorityClass = ProcessPriorityClass.High;

			
			ReadBlock currentSnFile1 = new ReadBlock(srcfilename1, fileshare:true, maxNumberOfLinesInBlock:maxNumberOfLinesInReadingBlock);
			ReadBlock currentSnFile2 = new ReadBlock(srcfilename2, fileshare: true, maxNumberOfLinesInBlock: maxNumberOfLinesInReadingBlock);

			BlockOfLines block1, block2;
			long cumfreq1 = 0, cumfreq2 = 0;
			long linecount1 = 0, linecount2 = 0;
			long longestlinelenght1 = 0, longestlinelenght2 = 0;
			int cmp;
			BigInteger intersectionproduct = 0;


			block1 = currentSnFile1.fillcompact();
			block2 = currentSnFile2.fillcompact();
			WordFreq wordfreq1 = null;
			WordFreq wordfreq2 = null;
			if (block1 != null) {//read w1
				wordfreq1 = block1.ReadLine();
				cumfreq1 += wordfreq1.freq;
				linecount1++;
				if (longestlinelenght1 < wordfreq1.word.Length) longestlinelenght1 = wordfreq1.word.Length;
			}
			if (block2 != null) {//read w2
				wordfreq2 = block2.ReadLine();
				cumfreq2 += wordfreq2.freq;
				linecount2++;
				if (longestlinelenght2 < wordfreq2.word.Length) longestlinelenght2 = wordfreq2.word.Length;
			}

			bool donereadingfile2 = false;
			while (block1 != null && block2 != null) {
				while ((cmp = string.CompareOrdinal(wordfreq1.word, wordfreq2.word)) >= 0) { //read w2 while w1>=w2
					if (cmp == 0) {//intersection found
						intersectionproduct += wordfreq1.freq * (BigInteger)wordfreq2.freq;
						if (showinfo) Console.WriteLine("intersection:: " + wordfreq1.asString() + " :: " + wordfreq2.asString() + " ::lines(compact): " + linecount1 + ", " + linecount2 + ".");
					}
					#region read w2
					if ((wordfreq2 = block2.ReadLine()) == null) {//reads w2 and fills buffer if necessary
						if ((block2 = currentSnFile2.fillcompact()) == null) { donereadingfile2 = true; break; }//done reading file2(and w2=null) 
						if ((wordfreq2 = block2.ReadLine()) == null) { Console.WriteLine("Something went wrong....read empty block!"); break; }
					}
					cumfreq2 += wordfreq2.freq;
					linecount2++;
					if (longestlinelenght2 < wordfreq2.word.Length) longestlinelenght2 = wordfreq2.word.Length;
					// check if key has been pressed. Exit if x has been pressed
					if (Console.KeyAvailable) {
						float percent = ((float)currentSnFile2.readSoFar) / (float)currentSnFile2.fileLength * 100;
						Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "File2: % read= " + percent);
						ConsoleKeyInfo cki = Console.ReadKey(true);
						if (cki.Key == ConsoleKey.X) break;
					}
					#endregion read w2
				}
				if (donereadingfile2) break;// done because file2 has been read completely.
				//w1< w2
				#region read w1
				if ((wordfreq1 = block1.ReadLine()) == null) {//reads w1 and fills buffer if necessary
					if ((block1 = currentSnFile1.fillcompact()) == null) { break; }//done reading file1(and w1=null) 
					if ((wordfreq1 = block1.ReadLine()) == null) { Console.WriteLine("Something went wrong file1....read empty block!"); break; }
				}
				cumfreq1 += wordfreq1.freq;
				linecount1++;
				if (longestlinelenght1 < wordfreq1.word.Length) longestlinelenght1 = wordfreq1.word.Length;
				// check if key has been pressed. Exit if x has been pressed
				if (Console.KeyAvailable) {
					float percent = ((float)currentSnFile1.readSoFar) / (float)currentSnFile1.fileLength * 100;
					Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "File1: % read= " + percent);
					ConsoleKeyInfo cki = Console.ReadKey(true);
					if (cki.Key == ConsoleKey.X) break;
				}
				#endregion read w1
			}
			//close the files
			currentSnFile1.Close();
			currentSnFile2.Close();

			//lines gives you the geodesics of either file1 or file2 (only one of the files is read completely)
			//freq gives you the total number of words of either file 1 or file 2 (only one of the files is read completely)
			Console.WriteLine("intersection= " + intersectionproduct + "  for files " + src1 + " " + src2);
			Console.WriteLine("intersect::Freq1 = " + cumfreq1 + ", lines = " + linecount1 + ", longest line length= " + longestlinelenght1 + " in file: " + src1);
			Console.WriteLine("intersect::Freq2 = " + cumfreq2 + ", lines = " + linecount2 + ", longest line length= " + longestlinelenght2 + " in file: " + src2);
			if (dest == "") {
				Console.WriteLine("Warning:intersection: no destination file was given "); return;
			} 
			WriteTextB intersection_file = new WriteTextB(destPath+dest, append);
			intersection_file.WriteLine(intersectionproduct.ToString());
			intersection_file.WriteLine("intersection= " + intersectionproduct + "\t\t\t\t for files " + src1 + " " + src2);
			intersection_file.WriteLine("intersect::Freq1 = " + cumfreq1 + ", lines = " + linecount1 + ", longest line length= " + longestlinelenght1 + " in file: " + src1);
			intersection_file.WriteLine("intersect::Freq2 = " + cumfreq2 + ", lines = " + linecount2 + ", longest line length= " + longestlinelenght2 + " in file: " + src2);
			intersection_file.Close();
		}

		private void intersection_WriteLine(string str) {
			WBuffer FileIntersections = new WBuffer("interserctions.txt", 1000, true);//Append to logfile.txt
			FileIntersections.WriteLine(str);
			FileIntersections.Close();
		}



		public void bash_sort(int n, string label_dest, int antalVolumesOutput) {
			string[] destfilesnames_s = new string[antalVolumesOutput];
			string[] destfilesnamesInverses_s = new string[antalVolumesOutput];
			string[] destfilesnames_e = new string[antalVolumesOutput];
			string[] destfilesnamesInverses_e = new string[antalVolumesOutput];

			for (int i = 0; i < antalVolumesOutput; i++) {
				//small 
				destfilesnames_s[i] = "srEN" + n + label_dest + "-" + i + ".doubletree";
				destfilesnamesInverses_s[i] = "srEI" + n + label_dest + "-" + i + ".doubletree";
				//equal
				destfilesnames_e[i] = "erEN" + n + label_dest + "-" + i + ".doubletree";
				destfilesnamesInverses_e[i] = "erEI" + n + label_dest + "-" + i + ".doubletree";
			}

			WriteTextB bashfile_sort = new WriteTextB("n" + n + label_dest + ".sh", false);
			Console.WriteLine("Writing bashfile: " + "n" + n + label_dest + ".sh");
			//bashfile.WriteLine_Bash(@"#!/bin/bash");
			bashfile_sort.WriteLine_Bash("# \"To run it type command: sh n" + n + ".sh");
			bashfile_sort.WriteLine_Bash("# \"If nothing else is displayed besides the echo commands then everything is good.");
			bashfile_sort.WriteLine_Bash("echo \"Sorting volumes...\"");
			for (int i = 0; i < antalVolumesOutput; i++) { //Normals equals
				bashfile_sort.WriteLine_Bash("LC_ALL=C sort " + destfilesnames_e[i] + " >" + "erEN" + n + label_dest + "-" + i + ".dts");
			}
			for (int i = 0; i < antalVolumesOutput; i++) {//Inverses equals
				bashfile_sort.WriteLine_Bash("LC_ALL=C sort " + destfilesnamesInverses_e[i] + " >" + "erEI" + n + label_dest + "-" + i + ".dts");
			}
			for (int i = 0; i < antalVolumesOutput; i++) {//Normals smalls
				bashfile_sort.WriteLine_Bash("LC_ALL=C sort " + destfilesnames_s[i] + " >" + "srEN" + n + label_dest + "-" + i + ".dts");
			}
			for (int i = 0; i < antalVolumesOutput; i++) {//Inverses smalls
				bashfile_sort.WriteLine_Bash("LC_ALL=C sort " + destfilesnamesInverses_s[i] + " >" + "srEI" + n + label_dest + "-" + i + ".dts");
			}
			bashfile_sort.Close();

		}

		public void bat_intersect(int n, int antalVolumesOutput,string label_dest,string mono){
			WriteTextB bat_intersect = new WriteTextB("intersect" + n + label_dest + ".bat", false);
			Console.WriteLine("Writing batfile: " + "intersect" + n + label_dest + ".bat");
			//bashfile.WriteLine_Bash(@"#!/bin/bash");
			if(mono=="") bat_intersect.WriteLine("REM \"To run it type command: intersect" + n + ".bat");
			else
				if (mono == "") bat_intersect.WriteLine("# \"To run it type command: intersect" + n + ".bat");
			bat_intersect.WriteLine("echo \"intersecting volumes n(" + n + ")...\"");
			for (int i = 0; i < antalVolumesOutput; i++) { //Normals equals INTERSECT Inverses equals even-odd
				if (n >= 3) bat_intersect.WriteLine(mono + "START CMD /C CALL TFilesEvenSLEbin 0 intersect " + "int_src1=erEN" + n + label_dest + "-" + i + ".dts " +
						"int_src2=erEI" + (n - 1) + label_dest + "-" + i + ".dts int_dest=erZeta" + (2 * n - 1) + label_dest + "-" + i + ".int");
			}
			for (int i = 0; i < antalVolumesOutput; i++) { //Normals equals INTERSECT Inverses equals even-even
				bat_intersect.WriteLine(mono + "START CMD /C CALL TFilesEvenSLEbin 0 intersect " + "int_src1=erEN" + n + label_dest + "-" + i + ".dts " +
					   "int_src2=erEI" + n + label_dest + "-" + i + ".dts int_dest=erZeta" + (2 * n) + label_dest + "-" + i + ".int");
			}
			for (int i = 0; i < antalVolumesOutput; i++) { //Normals smallers INTERSECT Inverses smallers even-odd
				if (n >= 3) bat_intersect.WriteLine(mono + "START CMD /C CALL TFilesEvenSLEbin 0 intersect " + "int_src1=srEN" + n + label_dest + "-" + i + ".dts " +
						"int_src2=srEI" + (n - 1) + label_dest + "-" + i + ".dts int_dest=srZeta" + (2 * n - 1) + label_dest + "-" + i + ".int");
			}
			for (int i = 0; i < antalVolumesOutput; i++) { //Normals smallers INTERSECT Inverses smallers even-even
				bat_intersect.WriteLine(mono + "START CMD /C CALL TFilesEvenSLEbin 0 intersect " + "int_src1=srEN" + n + label_dest + "-" + i + ".dts " +
					   "int_src2=srEI" + n + label_dest + "-" + i + ".dts int_dest=srZeta" + (2 * n) + label_dest + "-" + i + ".int");
			}
			bat_intersect.Close();

			{
				string[] destZetaEven_s = new string[antalVolumesOutput];
				string[] destZetaOdd_s = new string[antalVolumesOutput];
				string[] destZetaEven_e = new string[antalVolumesOutput];
				string[] destZetaOdd_e = new string[antalVolumesOutput];

				for (int i = 0; i < antalVolumesOutput; i++) {
					//small 
					destZetaEven_e[i] = "erZeta" + (2 * n) + label_dest + "-" + i + ".int";
					destZetaOdd_e[i] = "erZeta" + (2 * n - 1) + label_dest + "-" + i + ".int";
					//equal
					destZetaEven_s[i] = "srZeta" + (2 * n) + label_dest + "-" + i + ".int";
					destZetaOdd_s[i] = "srZeta" + (2 * n - 1) + label_dest + "-" + i + ".int";
				}

				WriteTextB batfile_sum = new WriteTextB("sum_intersections" + n + label_dest + ".bat",append:false);
				Console.WriteLine("sumIntersection:Writing file= " + "sum_intersections" + n + label_dest + ".bat");
				batfile_sum.WriteLine("echo \"Adding intersections  n(" + n + ") from all volumes...(to compute zeta numbers)\"");
				string join;
				join = "";
				join += destZetaOdd_e[0];
				for (int i = 1; i < antalVolumesOutput; i++) join += "," + destZetaOdd_e[i];
				batfile_sum.WriteLine(mono+"TFilesEvenSLEbin 0 sumFiles=" + join + " sum_dest=erZeta" + (2 * n - 1) + label_dest + ".sum");
				join = "";
				join += destZetaEven_e[0];
				for (int i = 1; i < antalVolumesOutput; i++) join += "," + destZetaEven_e[i];
				batfile_sum.WriteLine(mono+"TFilesEvenSLEbin 0 sumFiles=" + join + " sum_dest=erZeta" + (2*n) + label_dest + ".sum");
				join = "";
				join += destZetaOdd_s[0];
				for (int i = 1; i < antalVolumesOutput; i++) join += "," + destZetaOdd_s[i];
				batfile_sum.WriteLine(mono+"TFilesEvenSLEbin 0 sumFiles=" + join + " sum_dest=srZeta" + (2 * n - 1) + label_dest + ".sum");
				join = "";
				join += destZetaEven_s[0];
				for (int i = 1; i < antalVolumesOutput; i++) join += "," + destZetaEven_s[i];
				batfile_sum.WriteLine(mono+"TFilesEvenSLEbin 0 sumFiles=" + join + " sum_dest=srZeta" + (2*n) + label_dest + ".sum");
				batfile_sum.Close();
			}



		}

		public void bat_joinedIntersect(int n, string label_dest,string mono) {
			WriteTextB bat_intersect = new WriteTextB("intersect" + n + label_dest + "-joined.bat", false);
			Console.WriteLine("Writing batfile: " + "intersect" + n + label_dest + "-joined.bat");
			//bashfile.WriteLine_Bash(@"#!/bin/bash");
			if(mono=="")bat_intersect.WriteLine("REM \"To run it type command: intersect" + n + "-joined.bat");
			else bat_intersect.WriteLine("# \"To run it type command: intersect" + n + "-joined.bat");
			bat_intersect.WriteLine("echo \"intersecting volumes n(" + n + ")...\"");
			bat_intersect.WriteLine(mono+"TFilesEvenSLEbin 0 intersect " + "int_src1=erEN" + n + label_dest + "-joined.dts " +
				   "int_src2=erEI" + (n - 1) + label_dest + "-joined.dts ");
			bat_intersect.WriteLine(mono+"TFilesEvenSLEbin 0 intersect " + "int_src1=erEN" + n + label_dest + "-joined.dts " +
				   "int_src2=erEI" + n + label_dest + "-joined.dts ");
			if (n >= 3) bat_intersect.WriteLine(mono+"TFilesEvenSLEbin 0 intersect " + "int_src1=srEN" + n + label_dest + "-joined.dts " +
					"int_src2=srEI" + (n - 1) + label_dest + "-joined.dts ");
			bat_intersect.WriteLine(mono+"TFilesEvenSLEbin 0 intersect " + "int_src1=srEN" + n + label_dest + "-joined.dts " +
				   "int_src2=srEI" + n + label_dest + "-joined.dts ");
			bat_intersect.Close();
		}
		public void bat_odomIntersect(int n, string label_dest,string mono) {
			WriteTextB bat_intersect = new WriteTextB("intersect" + n + label_dest + "-odom.bat", false);
			Console.WriteLine("Writing batfile: " + "intersect" + n + label_dest + "-odom.bat");
			//bashfile.WriteLine_Bash(@"#!/bin/bash");
			if (mono == "") bat_intersect.WriteLine("REM \"To run it type command: intersect" + n + "-odom.bat");
			else bat_intersect.WriteLine("# \"To run it type command: intersect" + n + "-odom.bat");
			bat_intersect.WriteLine("echo \"intersecting volumes n(" + n + ")...\"");
			bat_intersect.WriteLine(mono+"TFilesEvenSLEbin 0 intersect " + "int_src1=erEN" + n + label_dest + "-odom.dts " +
				   "int_src2=erEI" + (n - 1) + label_dest + "-odom.dts ");
			bat_intersect.WriteLine(mono+"TFilesEvenSLEbin 0 intersect " + "int_src1=erEN" + n + label_dest + "-odom.dts " +
				   "int_src2=erEI" + n + label_dest + "-odom.dts ");
			if (n >= 3) bat_intersect.WriteLine(mono+"TFilesEvenSLEbin 0 intersect " + "int_src1=srEN" + n + label_dest + "-odom.dts " +
					"int_src2=srEI" + (n - 1) + label_dest + "-odom.dts ");
			bat_intersect.WriteLine(mono+"TFilesEvenSLEbin 0 intersect " + "int_src1=srEN" + n + label_dest + "-odom.dts " +
				   "int_src2=srEI" + n + label_dest + "-odom.dts ");
			bat_intersect.Close();
		}

		

		public void bash_joinAndSort(int n, int antalVolumesOutput,string label_dest,bool bashappend=false) {
			string[] destfilesnames_s = new string[antalVolumesOutput];
			string[] destfilesnamesInverses_s = new string[antalVolumesOutput];
			string[] destfilesnames_e = new string[antalVolumesOutput];
			string[] destfilesnamesInverses_e = new string[antalVolumesOutput];

			for (int i = 0; i < antalVolumesOutput; i++) {
				//small 
				destfilesnames_s[i] = "srEN" + n + label_dest + "-" + i + ".doubletree";
				destfilesnamesInverses_s[i] = "srEI" + n + label_dest + "-" + i + ".doubletree";
				//equal
				destfilesnames_e[i] = "erEN" + n + label_dest + "-" + i + ".doubletree";
				destfilesnamesInverses_e[i] = "erEI" + n + label_dest + "-" + i + ".doubletree";
			}
			WriteTextB bashfile = new WriteTextB("n" + n + label_dest + "-joined.sh", bashappend);

			bashfile.WriteLine_Bash("echo \"Joining volumes, sorting odometer counterparts...(run n[number]c.bat and n[number]c.sh to compare them)\"");
			string join = "";//Normals equals
			for (int i = 0; i < antalVolumesOutput; i++) join += destfilesnames_e[i] + " ";
			bashfile.WriteLine_Bash("cat " + join + "> erEN" + n + label_dest + "-joined.doubletree");
			bashfile.WriteLine_Bash("LC_ALL=C sort erEN" + n + label_dest + "-joined.doubletree" + " >" + "erEN" + n + label_dest + "-joined.dts");
			bashfile.WriteLine_Bash("LC_ALL=C sort erEN" + n + label_dest + "-odom.doubletree" + " >" + "erEN" + n + label_dest + "-odom.dts");
			join = "";//Inverses equals
			for (int i = 0; i < antalVolumesOutput; i++) join += destfilesnamesInverses_e[i] + " ";
			bashfile.WriteLine_Bash("cat " + join + "> erEI" + n + label_dest + "-joined.doubletree");
			bashfile.WriteLine_Bash("LC_ALL=C sort erEI" + n + label_dest + "-joined.doubletree" + " >" + "erEI" + n + label_dest + "-joined.dts");
			bashfile.WriteLine_Bash("LC_ALL=C sort erEI" + n + label_dest + "-odom.doubletree" + " >" + "erEI" + n + label_dest + "-odom.dts");
			join = "";//Normals smallers
			for (int i = 0; i < antalVolumesOutput; i++) join += destfilesnames_s[i] + " ";
			bashfile.WriteLine_Bash("cat " + join + "> srEN" + n + label_dest + "-joined.doubletree");
			bashfile.WriteLine_Bash("LC_ALL=C sort srEN" + n + label_dest + "-joined.doubletree" + " >" + "srEN" + n + label_dest + "-joined.dts");
			bashfile.WriteLine_Bash("LC_ALL=C sort srEN" + n + label_dest + "-odom.doubletree" + " >" + "srEN" + n + label_dest + "-odom.dts");
			join = "";//Inverses smallers
			for (int i = 0; i < antalVolumesOutput; i++) join += destfilesnamesInverses_s[i] + " ";
			bashfile.WriteLine_Bash("cat " + join + "> srEI" + n + label_dest + "-joined.doubletree");
			bashfile.WriteLine_Bash("LC_ALL=C sort srEI" + n + label_dest + "-joined.doubletree" + " >" + "srEI" + n + label_dest + "-joined.dts");
			bashfile.WriteLine_Bash("LC_ALL=C sort srEI" + n + label_dest + "-odom.doubletree" + " >" + "srEI" + n + label_dest + "-odom.dts");

		}

		public void bat_frequenciesCount(int n, int antalVolumesOutput, string label_dest, string mono,bool bashappend = false) {
			string[] destfilesnames_s = new string[antalVolumesOutput];
			string[] destfilesnamesInverses_s = new string[antalVolumesOutput];
			string[] destfilesnames_e = new string[antalVolumesOutput];
			string[] destfilesnamesInverses_e = new string[antalVolumesOutput];

			for (int i = 0; i < antalVolumesOutput; i++) {
				//small 
				destfilesnames_s[i] = "srEN" + n + label_dest + "-" + i;
				destfilesnamesInverses_s[i] = "srEI" + n + label_dest + "-" + i;
				//equal
				destfilesnames_e[i] = "erEN" + n + label_dest + "-" + i;
				destfilesnamesInverses_e[i] = "erEI" + n + label_dest + "-" + i;
			}
			{
				WriteTextB batfile_freq = new WriteTextB("freq" + n + label_dest + ".bat", bashappend);
				Console.WriteLine("Freq:Writing file= " + "freq" + n + label_dest + ".bat");
				batfile_freq.WriteLine("echo \"Frequencies count n(" + n + ") for each volumes...(for checking purposes:Should add up to 3*6^n)\"");
				for (int i = 0; i < antalVolumesOutput; i++)
					batfile_freq.WriteLine(mono+"TFilesEvenSLEbin 0 frequencies freq_src=" + destfilesnames_e[i] + ".doubletree" + " freq_dest=" + destfilesnames_e[i] + ".freq");
				for (int i = 0; i < antalVolumesOutput; i++)
					batfile_freq.WriteLine(mono+"TFilesEvenSLEbin 0 frequencies freq_src=" + destfilesnamesInverses_e[i] + ".doubletree" + " freq_dest=" + destfilesnamesInverses_e[i] + ".freq");
				for (int i = 0; i < antalVolumesOutput; i++)
					batfile_freq.WriteLine(mono+"TFilesEvenSLEbin 0 frequencies freq_src=" + destfilesnames_s[i] + ".doubletree" + " freq_dest=" + destfilesnames_s[i] + ".freq");
				for (int i = 0; i < antalVolumesOutput; i++)
					batfile_freq.WriteLine(mono+"TFilesEvenSLEbin 0 frequencies freq_src=" + destfilesnamesInverses_s[i] + ".doubletree" + " freq_dest=" + destfilesnamesInverses_s[i] + ".freq");
			}
			{
				WriteTextB batfile_sum = new WriteTextB("sum_freq" + n + label_dest + ".bat", bashappend);
				Console.WriteLine("sumFreq:Writing file= " + "sum_freq" + n + label_dest + ".bat");
				batfile_sum.WriteLine("echo \"Adding the frequencies  n(" + n + ") from all volumes...(for checking purposes:Should add up to 3*6^n)\"");
				string join;
				join = "";
				join += destfilesnames_e[0] + ".freq";
				for (int i = 1; i < antalVolumesOutput; i++) join += "," + destfilesnames_e[i] + ".freq";
				batfile_sum.WriteLine(mono+"TFilesEvenSLEbin 0 sumFiles=" + join + " sum_dest=erEN" + n + label_dest + ".sum");
				join = "";
				join += destfilesnamesInverses_e[0] + ".freq";
				for (int i = 1; i < antalVolumesOutput; i++) join += "," + destfilesnamesInverses_e[i] + ".freq";
				batfile_sum.WriteLine(mono+"TFilesEvenSLEbin 0 sumFiles=" + join + " sum_dest=erEI" + n + label_dest + ".sum");
				join = "";
				join += destfilesnames_s[0] + ".freq";
				for (int i = 1; i < antalVolumesOutput; i++) join += "," + destfilesnames_s[i] + ".freq";
				batfile_sum.WriteLine(mono+"TFilesEvenSLEbin 0 sumFiles=" + join + " sum_dest=srEN" + n + label_dest + ".sum");
				join = "";
				join += destfilesnamesInverses_s[0] + ".freq";
				for (int i = 1; i < antalVolumesOutput; i++) join += "," + destfilesnamesInverses_s[i] + ".freq";
				batfile_sum.WriteLine(mono+"TFilesEvenSLEbin 0 sumFiles=" + join + " sum_dest=srEI" + n + label_dest + ".sum");
			}


		}

	

		//reads first line of each file and adds it.
		//designed for small files only!
		public void sum_Files(string[] srcFiles, string dest,string srcPath,bool append) {
			BigInteger sum=0;
			string comment ="";
			string error = "";
			for (int i = 0; i < srcFiles.Length; i++) {
				BigInteger m = number_file(srcPath + srcFiles[i],fileshare:true);
				if (m < 0) { Console.WriteLine("\n***ERROR: OVERFLOW!!!!!"); error = "\n***ERROR: OVERFLOW!!!!!"; }
				if (m != 0) { sum += m; comment += " " + srcFiles[i]; };
			}
			Console.WriteLine("Writing to file= " + dest);
			Console.WriteLine("Sum="+sum+" "+comment);
			if (dest == "") {
				Console.WriteLine("Warning:: sum:: No destination file was given. (sum_dest=filename)");
				return;
			}
			Console.WriteLine("Writing file: " + dest);
			WriteTextB sum_file = new WriteTextB(dest, append);
			sum_file.WriteLine(sum.ToString());
			sum_file.WriteLine(comment);
			if(error!="") sum_file.WriteLine(error);
			sum_file.Close();
		}

		public BigInteger number_file(string srcfilename,bool fileshare) {
			BigInteger m = 0;
			if (File.Exists(srcfilename) == true) {
				if (new FileInfo(srcfilename).Length == 0) return 0;//ignores empty files
				ReadTextB src_file = new ReadTextB(srcfilename,fileshare);
				string str = src_file.ReadLine();
				BigInteger.TryParse(str, out m);
				src_file.Close();
			}

			return m;
		}

			
		public void standardBatches(int n_src){
			if (n_src == 0) n_src = 15;
			for(int n=0;n<=n_src;n++){
				Console.WriteLine("writing bat file " + "n" + n + ".bat");
			WriteTextB batfile_cmp = new WriteTextB("n" + n  + ".bat", false);
			batfile_cmp.WriteLine("TfilesEvenSLEbin.exe "+n+" volumesOutput_str=\"UWY^egio\"  antalVolumesOutput=8  antalVolumesInput=8 threadAntal=4 ");
			batfile_cmp.Close();
			}
			
		}

		public void bat_joinedCompress(int n, string label_dest,string mono) {
			////compressing
			WriteTextB batfile_cmp = new WriteTextB("compress" + n + label_dest + "-joined.bat", false);
			Console.WriteLine("Writing batfile: " + "compress" + n + label_dest + "-joined.bat");
			//Normals equals
			batfile_cmp.WriteLine(mono+"TFilesEvenSLEbin 0  compress compress_src=erEN" + n + label_dest + "-joined.dts" + " compress_dest=erEN" + n + label_dest + "-joined.dtsc");
			//Inverses equals
			batfile_cmp.WriteLine(mono+"TFilesEvenSLEbin 0  compress compress_src=erEI" + n + label_dest + "-joined.dts" + " compress_dest=erEI" + n + label_dest + "-joined.dtsc");
			//Normals smallers
			batfile_cmp.WriteLine(mono+"TFilesEvenSLEbin 0  compress compress_src=srEN" + n + label_dest + "-joined.dts" + " compress_dest=srEN" + n + label_dest + "-joined.dtsc");
			//Inverses smallers
			batfile_cmp.WriteLine(mono+"TFilesEvenSLEbin 0  compress compress_src=srEI" + n + label_dest + "-joined.dts" + " compress_dest=srEI" + n + label_dest + "-joined.dtsc");
			//Normals equals
			batfile_cmp.WriteLine(mono+"TFilesEvenSLEbin 0  compress compress_src=erEN" + n + label_dest + "-odom.dts" + " compress_dest=erEN" + n + label_dest + "-odom.dtsc");
			//Inverses equals
			batfile_cmp.WriteLine(mono+"TFilesEvenSLEbin 0  compress compress_src=erEI" + n + label_dest + "-odom.dts" + " compress_dest=erEI" + n + label_dest + "-odom.dtsc");
			//Normals smallers
			batfile_cmp.WriteLine(mono+"TFilesEvenSLEbin 0  compress compress_src=srEN" + n + label_dest + "-odom.dts" + " compress_dest=srEN" + n + label_dest + "-odom.dtsc");
			//Inverses smallers
			batfile_cmp.WriteLine(mono+"TFilesEvenSLEbin 0  compress compress_src=srEI" + n + label_dest + "-odom.dts" + " compress_dest=srEI" + n + label_dest + "-odom.dtsc");
			batfile_cmp.Close();
		}

		public void bash_joinedcompressedCompare(int n, string label_dest) {
			WriteTextB bashfile_cmp = new WriteTextB("cmp" + n + label_dest + "-joined.sh", false);
			Console.WriteLine("Writing bashfile: " + "cmp" + n + label_dest + "-joined.sh");
			bashfile_cmp.WriteLine_Bash("echo \"Comparing joined volumes with their odometer counterparts (sorted and compressed)...\"");
			//Normals equals
			bashfile_cmp.WriteLine_Bash("cmp erEN" + n + label_dest + "-joined.dtsc" + " " + "erEN" + n + label_dest + "-odom.dtsc");
			//Inverses equals
			bashfile_cmp.WriteLine_Bash("cmp erEI" + n + label_dest + "-joined.dtsc" + " " + "erEI" + n + label_dest + "-odom.dtsc");
			//Normals smallers
			bashfile_cmp.WriteLine_Bash("cmp srEN" + n + label_dest + "-joined.dtsc" + " " + "srEN" + n + label_dest + "-odom.dtsc");
			//Inverses smallers
			bashfile_cmp.WriteLine_Bash("cmp srEI" + n + label_dest + "-joined.dtsc" + " " + "srEI" + n + label_dest + "-odom.dtsc");
			bashfile_cmp.Close();
		}

		public void bash_joinSorted(int n_src, int antalVolumesOutput, string label_dest, bool bashappend = false) {
			for (int n = 1; n <= n_src; n++) {
				string[] destfilesnames_s = new string[antalVolumesOutput];
				string[] destfilesnamesInverses_s = new string[antalVolumesOutput];
				string[] destfilesnames_e = new string[antalVolumesOutput];
				string[] destfilesnamesInverses_e = new string[antalVolumesOutput];

				for (int i = 0; i < antalVolumesOutput; i++) {
					//small 
					destfilesnames_s[i] = "srEN" + n + label_dest + "-" + i + ".dts";
					destfilesnamesInverses_s[i] = "srEI" + n + label_dest + "-" + i + ".dts";
					//equal
					destfilesnames_e[i] = "erEN" + n + label_dest + "-" + i + ".dts";
					destfilesnamesInverses_e[i] = "erEI" + n + label_dest + "-" + i + ".dts";
				}
				WriteTextB bashfile = new WriteTextB("n" + n + label_dest + "-sJoined.sh", bashappend);
				Console.WriteLine("writing bashfile " + "n" + n + label_dest + "-sJoined.sh");
				bashfile.WriteLine_Bash("echo \"Joining volumes which are already sorted...\"");
				string join = "";//Normals equals
				for (int i = 0; i < antalVolumesOutput; i++) join += destfilesnames_e[i] + " ";
				bashfile.WriteLine_Bash("cat " + join + "> erEN" + n + label_dest + "-joined.dts");
				join = "";//Inverses equals
				for (int i = 0; i < antalVolumesOutput; i++) join += destfilesnamesInverses_e[i] + " ";
				bashfile.WriteLine_Bash("cat " + join + "> erEI" + n + label_dest + "-joined.dts");
				join = "";//Normals smallers
				for (int i = 0; i < antalVolumesOutput; i++) join += destfilesnames_s[i] + " ";
				bashfile.WriteLine_Bash("cat " + join + "> srEN" + n + label_dest + "-joined.dts");
				join = "";//Inverses smallers
				for (int i = 0; i < antalVolumesOutput; i++) join += destfilesnamesInverses_s[i] + " ";
				bashfile.WriteLine_Bash("cat " + join + "> srEI" + n + label_dest + "-joined.dts");
			}
		}
		//$ cut -f 2  -d' ' srEN2-joined.rangec
		public void bash_joinedCutSecondWord(int n_src, string label_dest) {
			for (int n = 1; n <= n_src; n++) {
				WriteTextB bashfile_cmp = new WriteTextB("rangeFreq" + n + label_dest + "-joined.sh", false);
				Console.WriteLine("Writing bashfile: " + "rangeFreq" + n + label_dest + "-joined.sh");
				bashfile_cmp.WriteLine_Bash("echo \"reading second word (the frequency) ...\"");
				//Normals equals
				bashfile_cmp.WriteLine_Bash("cut -f 2 -d' ' erEN" + n + label_dest + "-joined.rangec" + " > " + "erEN" + n + label_dest + "-joined.rangef");
				//Inverses equals
				bashfile_cmp.WriteLine_Bash("cut -f 2 -d' ' erEI" + n + label_dest + "-joined.rangec" + " > " + "erEI" + n + label_dest + "-joined.rangef");
				//Normals smallers
				bashfile_cmp.WriteLine_Bash("cut -f 2 -d' ' srEN" + n + label_dest + "-joined.rangec" + " > " + "srEN" + n + label_dest + "-joined.rangef");
				//Inverses smallers
				bashfile_cmp.WriteLine_Bash("cut -f 2 -d' ' srEI" + n + label_dest + "-joined.rangec" + " > " + "srEI" + n + label_dest + "-joined.rangef");
				bashfile_cmp.Close();
			}
		}

		public void copyTextFile(string src, string dest) {
			DateTime start_time = DateTime.Now;
			copyTextFileMainB(src, dest);
			long size = new FileInfo(dest).Length;
			int milliseconds = 1 + (int)((DateTime.Now - start_time).TotalMilliseconds);
			// size time in milliseconds per hour
			long tsize = size * 3600000 / milliseconds;
			tsize = tsize / (int)Math.Pow(2, 30);
			Console.WriteLine(tsize + "GB/hour");
			Console.WriteLine("now copying it using nonbinary version:");
			copyTextFileA(src, dest);
		}

		public void copyTextFileA(string src, string dest) {
			DateTime start_time = DateTime.Now;
			copyTextFileMain(src, dest);
			long size = new FileInfo(dest).Length;
			int milliseconds = 1 + (int)((DateTime.Now - start_time).TotalMilliseconds);
			// size time in milliseconds per hour
			long tsize = size * 3600000 / milliseconds;
			tsize = tsize / (int)Math.Pow(2, 30);
			Console.WriteLine(tsize + "GB/hour");
		}

		//warning the lines in the source file should have length <4000
		public void copyTextFileMainB(string src, string dest) {
			dest = "B" + dest;
			Console.WriteLine("copying source file=" + src);
			Console.WriteLine("destination file=" + dest);

			ReadTextB src_file = new ReadTextB(src, false);
			WriteTextB dest_file = new WriteTextB(dest, false);
			string line="";
			while((line=src_file.ReadLine())!=null){
				dest_file.WriteLine(line);
			}
			src_file.Close();
			dest_file.Close();
		}

		public void copyTextFileMain(string src, string dest) {
			dest = "A" + dest;
			Console.WriteLine("copying source file=" + src);
			Console.WriteLine("destination file=" + dest);

			ReadText src_file = new ReadText(src);
			WriteText dest_file = new WriteText(dest,false);
			string line = "";
			while ((line = src_file.ReadLine()) != null) {
				dest_file.WriteLine(line);
			}
			src_file.Close();
			dest_file.Close();
		}

		public void bat_joinedRange(int n_src, string label_dest, string mono) {
			////rangeing
			for (int n = 1; n <= n_src; n++) {
				WriteTextB batfile_cmp = new WriteTextB("range" + n + label_dest + "-joined.bat", false);
				Console.WriteLine("Writing bashfile: " + "range" + n + label_dest + "-joined.bat");
				//Normals equals
				batfile_cmp.WriteLine(mono + "TFilesEvenSLEbin 0  range range_src=erEN" + n + label_dest + "-joined.dts" + " range_dest=erEN" + n + label_dest + "-joined.range");
				//Inverses equals
				batfile_cmp.WriteLine(mono + "TFilesEvenSLEbin 0  range range_src=erEI" + n + label_dest + "-joined.dts" + " range_dest=erEI" + n + label_dest + "-joined.range");
				//Normals smallers
				batfile_cmp.WriteLine(mono + "TFilesEvenSLEbin 0  range range_src=srEN" + n + label_dest + "-joined.dts" + " range_dest=srEN" + n + label_dest + "-joined.range");
				//Inverses smallers
				batfile_cmp.WriteLine(mono + "TFilesEvenSLEbin 0  range range_src=srEI" + n + label_dest + "-joined.dts" + " range_dest=srEI" + n + label_dest + "-joined.range");
				//Normals equals
				batfile_cmp.WriteLine(mono + "TFilesEvenSLEbin 0  compress compress_src=erEN" + n + label_dest + "-joined.range" + " compress_dest=erEN" + n + label_dest + "-joined.rangec");
				//Inverses equals
				batfile_cmp.WriteLine(mono + "TFilesEvenSLEbin 0  compress compress_src=erEI" + n + label_dest + "-joined.range" + " compress_dest=erEI" + n + label_dest + "-joined.rangec");
				//Normals smallers
				batfile_cmp.WriteLine(mono + "TFilesEvenSLEbin 0  compress compress_src=srEN" + n + label_dest + "-joined.range" + " compress_dest=srEN" + n + label_dest + "-joined.rangec");
				//Inverses smallers
				batfile_cmp.WriteLine(mono + "TFilesEvenSLEbin 0  compress compress_src=srEI" + n + label_dest + "-joined.range" + " compress_dest=srEI" + n + label_dest + "-joined.rangec");
				batfile_cmp.Close();
			}
		}


	}//end of class
}
