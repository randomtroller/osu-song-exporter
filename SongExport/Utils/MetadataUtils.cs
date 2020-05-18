using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SongExport.Utils
{
    public class MetadataUtils
    {
        /// <summary>
        /// Sets the artist attribute in the given file.
        /// </summary>
        /// <param name="file">Target file</param>
        /// <param name="artist">The song's artist</param>
        public static void AddArtist(string file, string artist) {
            var _file = ShellFile.FromFilePath(file);

            try {
                ShellPropertyWriter propertyWriter = _file.Properties.GetPropertyWriter();
                propertyWriter.WriteProperty(SystemProperties.System.Music.Artist, new string[] { artist });
                propertyWriter.Close();
            } catch (Exception e) {

            }
        }

        /// <summary>
        /// Sets the title attribute in the given file.
        /// </summary>
        /// <param name="file">Target file</param>
        /// <param name="title">The song's title</param>
        public static void AddTitle(string file, string title) {
            var _file = ShellFile.FromFilePath(file);

            try {
                ShellPropertyWriter propertyWriter = _file.Properties.GetPropertyWriter();
                propertyWriter.WriteProperty(SystemProperties.System.Title, new string[] { title });
                propertyWriter.Close();
            } catch (Exception e) {

            }
        }

        /// <summary>
        /// Adds the required metadata to the file (MUCH better and cleaner way of doing it).
        /// </summary>
        /// <param name="file">Target file</param>
        /// <param name="artist">The artist of the song</param>
        /// <param name="title">The title of the song</param>
        public static void AddMetadata(string file, string artist, string title) {
            /*
            Thread titleThread = new Thread(() => AddTitle(file, title));
            titleThread.Start();

            titleThread.Join();
            Thread artistThread = new Thread(() => AddArtist(file, artist));
            artistThread.Start();
            */

            AddTitle(file, title);
            AddArtist(file, artist);
        }
    }
}
