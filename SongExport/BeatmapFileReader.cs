using System;
using System.IO;
using System.Linq;

namespace SongExport
{
    class BeatmapFileReader
    {
        private static string file;

        // Some beatmaps will contain characters that cannot be in a file name (eg. PADORU / PADORU). This list contains characters that cannot be in a file name.
        private static string[] illegalCharacters = { "<", ">", ":", "\"", @"/", @"\", "|", "?", "*" };

        public BeatmapFileReader(string File) {
            file = File;
        }

        /// <summary>
        /// A function that returns the song's artist
        /// </summary>
        /// <returns>The song's artist (romanised)</returns>
        public string GetArtist() {
            try {
                var lines = File.ReadLines(file);
                foreach (var line in lines) {
                    if (line.StartsWith("Artist:")) {
                        string artist = line.Substring(line.IndexOf(':') + 1);

                        foreach (string character in illegalCharacters) {
                            if (artist.Contains(character)) {
                                artist = artist.Replace(character, "");
                            }
                        }

                        return artist;
                    }
                }
            } catch (Exception e) {
                Console.WriteLine(e.StackTrace);
            }
            return null;
        }

        /// <summary>
        /// A function that returns the song's artist (not romanised)
        /// </summary>
        /// <returns>The song's artist (not romanised)</returns>
        public string GetArtistUnicode() {
            int linesRead = 0;

            try {
                var lines = File.ReadLines(file);
                foreach (var line in lines) {
                    linesRead++;

                    if (line.StartsWith("ArtistUnicode:")) {
                        string artist = line.Substring(line.IndexOf(':') + 1);

                        foreach (string character in illegalCharacters) {
                            if (artist.Contains(character)) {
                                artist = artist.Replace(character, "");
                            }
                        }

                        if (artist == string.Empty) {
                            return GetTitle();
                        }

                        return artist;
                    }
                }

                /*
                 * For older osu! file formats it will return the artist instead of ArtistUnicode because it didn't exist back then.
                 * The function checks if it has gone through all lines of the beatmap's file. If it didn't find "ArtistUnicode",
                 * we can know the file is an older osu! file format. (Thanks bad apple).
                 */
                if (linesRead == lines.Count()) {
                    return GetArtist();
                }

            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
            return null;
        }

        /// <summary>
        /// A function that returns the song's title
        /// </summary>
        /// <returns>The song's title</returns>
        public string GetTitle() {
            try {
                var lines = File.ReadLines(file);
                foreach (var line in lines) {
                    if (line.StartsWith("Title:")) {
                        string title = line.Substring(line.IndexOf(':') + 1);
                        
                        foreach(string character in illegalCharacters) {
                            if (title.Contains(character)) {
                                /*
                                 * Remove illegal characters from the title
                                 * eg. PADORU / PADORU -> PADORU PADORU
                                */
                                title = title.Replace(character, "");
                            }
                        }

                        return title;
                    }
                }
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
            return null;
        }

        public string GetTitleUnicode() {
            int linesRead = 0;

            try {
                var lines = File.ReadLines(file);
                foreach (var line in lines) {
                    linesRead++;

                    if (line.StartsWith("TitleUnicode:")) {
                        string title = line.Substring(line.IndexOf(':') + 1);

                        foreach (string character in illegalCharacters) {
                            if (title.Contains(character)) {
                                /*
                                 * Remove illegal characters from the title
                                 * eg. PADORU / PADORU -> PADORU PADORU
                                */
                                title = title.Replace(character, "");
                            }
                        }

                        if(title == string.Empty) {
                            return GetTitle();
                        }

                        return title;
                    }
                }

                if(linesRead == lines.Count()) {
                    return GetTitle();
                }
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
            return null;
        }
    }
}
