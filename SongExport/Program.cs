using SongExport.Utils;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace SongExport
{
    class Program
    {
        // osu! song path
        public static string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\osu!\songs\";

        // destination to export the songs to
        public static string destination = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\exported songs\";

        // Instance of our BeatmapFileReader class
        public static BeatmapFileReader reader;

        private static bool unicodeTitle = false;
        private static bool addMetadata = true;
        private static bool metadataUnicode = true;
        static void Main(string[] args) {
            Console.SetWindowSize(150, 40);
            PrintWelcomeMessage();
            DoOptions();

            if (!Directory.Exists(path)) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("osu's song directory was not found. Do you even have osu installed?");
                Console.ReadKey();
                Environment.Exit(0);
            }

            // Create the destination directory if it doesn't exist
            if(!Directory.Exists(destination)) {
                Directory.CreateDirectory(destination);
            }

            // Goes through every beatmap's directory, gets the mp3 file from it and copies it to the destination folder.
            foreach(string dir in GetSongDirectories(path)) {
                if(!(File.Exists(GetSongFile(dir, "*.osu")) || File.Exists(GetSongFile(dir, ".mp3")))) {
                    continue;
                }


                reader = new BeatmapFileReader(GetSongFile(dir, "*.osu"));
                string file = destination + reader.GetTitle() + ".mp3";

                if (!File.Exists(destination + reader.GetTitle() + ".mp3")) {
                    if (unicodeTitle) {
                        Console.WriteLine("Copied: " + reader.GetArtistUnicode() + " - " + reader.GetTitle());
                    } else {
                        Console.WriteLine("Copied: " + reader.GetArtist() + " - " + reader.GetTitle());
                    }
                    File.Copy(GetSongFile(dir, "*.mp3"), destination + reader.GetTitle() + ".mp3", true);

                    if (addMetadata) {
                        if (metadataUnicode) {
                            MetadataUtils.AddMetadata(file, reader.GetArtistUnicode(), reader.GetTitleUnicode());
                        } else {
                            MetadataUtils.AddMetadata(file, reader.GetArtist(), reader.GetTitle());
                        }
                    }
                }
            }
            MessageBox.Show("Finished exporting.", "osu! song exporter", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Opens the folder with the exported songs.
            Process.Start(destination);
        }

        /// <summary>
        /// Gets all sub directories in the osu! directory.
        /// </summary>
        /// <param name="path">osu's path</param>
        /// <returns>An array of file paths.</returns>
        public static string[] GetSongDirectories(string path) {
            return Directory.GetDirectories(path);
        }

        /// <summary>
        /// Returns the first avilabile file in the directory under the specified search pattern.
        /// </summary>
        /// <param name="sDir">Beatmap's directory</param>
        /// <param name="pattern">Search pattern. *.osu for the beatmap's file and *.mp3 for the beatmap's song file</param>
        /// <returns>The first avilabile file in the directory</returns>
        public static string GetSongFile(string sDir, string pattern) {
            try {
                foreach (string f in Directory.GetFiles(sDir, pattern)) {
                    if (File.Exists(f)) {
                        return f;
                    }
                }

                foreach (string d in Directory.GetDirectories(sDir)) {
                    GetSongFile(d, pattern);
                }
            } catch (Exception) {
            }

            return "";
        }

        /// <summary>
        /// Prints the welcome message to the console.
        /// </summary>
        private static void PrintWelcomeMessage() {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("----------------------------------------------------------------");
            Console.WriteLine("osu! song exporter v1.0");
            Console.WriteLine("Due to console's encoding, non ASCII characters (such as japanese) will appear as question marks.");
            Console.WriteLine("Titles and artist names that contain illegal filename characters will have those characters removed.");
            Console.WriteLine("Please close the program if you don't have osu! installed because the program will not do anything.");
            Console.WriteLine("----------------------------------------------------------------");
            Console.ResetColor();
        }

        /// <summary>
        /// Does the options dialog.
        /// </summary>
        private static void DoOptions() {
            Console.WriteLine("Force unicode titles? (this will export the files under the names in their original language (if avilabile by the beatmap)");
            Console.Write("y/n : ");
            string character = Console.ReadLine();

            if (character == "y") {
                unicodeTitle = true;
            } else if (character  == "n") {
                unicodeTitle = false;
            } else {
                unicodeTitle = false;
            }

            Console.WriteLine("Add song metadata to the exported files? (this will somewhat slower the process but not much thanks to threading) (true by default)");
            Console.WriteLine("This is recommended for better organisation of your music");
            Console.Write("y/n : ");
            character = Console.ReadLine();

            if (character == "y") {
                addMetadata = true;
            } else if (character == "n") {
                addMetadata = false;
            } else {
                addMetadata = false;
            }

            if (!addMetadata) {
                return;
            }

            Console.WriteLine("Export the metadata under the non romanized title and artist? (true by default)");
            Console.Write("y/n : ");
            character = Console.ReadLine();

            if (character == "y") {
                metadataUnicode = true;
            } else if (character == "n") {
                metadataUnicode = false;
            } else {
                metadataUnicode = false;
            }
        }
    }
}
