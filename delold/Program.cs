using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace delold {
	class Program {
		static bool recursive;
		static bool remove_empty_directories;
		static bool test_mode;
		static bool creation_time;
		static int days;

		static int files_deleted;
		static int directories_removed;

		static void Main(string[] args) {
			string path = null;
			recursive = false;
			remove_empty_directories = false;
			test_mode = false;
			files_deleted = 0;
			directories_removed = 0;

			if (args.Length < 1) {
				Usage();
				return;
			}


			if (!Int32.TryParse(args[0], out days)) {
				Console.WriteLine("Could not parse numeric value '{0}'.", args[0]);
				Usage();
				return;
			}

			for (int i = 1; i < args.Length; i++) {
				string a = args[i].Replace("/", "-");
				if (a.Equals("-r", StringComparison.InvariantCultureIgnoreCase)) {
					recursive = true;
				} else if (a.Equals("-e", StringComparison.InvariantCultureIgnoreCase)) {
					recursive = true;
					remove_empty_directories = true;
				} else if (a.Equals("-t", StringComparison.InvariantCultureIgnoreCase)) {
					test_mode = true;
				} else if (a.Equals("-c", StringComparison.InvariantCultureIgnoreCase)) {
					creation_time = true;
				} else {
					path = args[i];
				}
			}

			if (path == null) {
				Console.WriteLine("No path specified.");
				Usage();
				return;
			}

			if (!Directory.Exists(path)) {
				Console.WriteLine("{0} does not exist, or is not a directory.", path);
				return;
			}

			if (test_mode) {
				Console.WriteLine("*** Test mode. Not deleting files. ***");
			}
			try {
				Dive(path, 1);
			} catch (Exception ex) {
				Console.WriteLine("{0}: {1}", ex.GetType().FullName, ex.Message);
			}
			Console.WriteLine("Files deleted: {0}", files_deleted);
			if (directories_removed > 0) {
				Console.WriteLine("Directories removed: {0}", directories_removed);
			}
			if (test_mode) {
				Console.WriteLine("*** Test mode. No files deleted. ***");
			}
		}

		static void Dive(string path, int depth) {
			string[] files = null;
			try {
				files = Directory.GetFiles(path);
			} catch (Exception ex) {
				Console.WriteLine(ex.Message);
				return;
			}

            foreach (string f in files) {
				int d = (DateTime.Now - (creation_time ? File.GetCreationTime(f) : File.GetLastWriteTime(f))).Days;
				if (d >= days) {
					Console.WriteLine("Deleting '{0}' ({1} days)", f, d);
					if (!test_mode) {
						try {
							File.Delete(f);
							files_deleted++;
						} catch (Exception ex) {
							Console.WriteLine("Failed to delete '{0}': {1}", f, ex.Message);
						}
					}
				}
			}

			if (recursive) {
				foreach (string d in Directory.GetDirectories(path)) {
					Dive(d, depth + 1);
				}
			}

			if (remove_empty_directories && depth > 1) {
				if (Directory.GetFileSystemEntries(path).Length == 0) {
					Console.WriteLine("Removing empty directory '{0}'", path);
					if (!test_mode) {
						try {
							Directory.Delete(path);
							directories_removed++;
						} catch (Exception ex) {
							Console.WriteLine("Failed to remove directory '{0}': {1}", path, ex.Message);
						}
					}
				}
			}
		}

		static void Usage() {
			Console.WriteLine("Usage:");
			Console.WriteLine("delold <days> <path> [-r|-e] [-c] [-t]");
			Console.WriteLine();
			Console.WriteLine("Deletes all files in specified directory that are at least a given number of days old.");
			Console.WriteLine();
			Console.WriteLine("  -r: Recurse into subdirectories.");
			Console.WriteLine("  -e: Remove all empty subdirectories (depth-first traversal). Implies -r. Will not remove starting directory specified by <path>.");
			Console.WriteLine("  -c: Use creation time instead of last modification time.");
			Console.WriteLine("  -t: Test mode. Display files that would be deleted, but don't delete anything.");
		}
	}
}
