using GameEngine.Engine;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms.VisualStyles;

namespace GameEngine.Editor
{
    public static class AssetManager
    {
        const string folderName = "Content";
        const string contentPath = "..\\..\\game\\" + folderName;

        const string iconPath = "..\\..\\Engine\\Asset Management\\Icons";

        static ContentManager contentManager;

        public static void Init(ContentManager content)
        {
            contentManager = content;
            contentManager.RootDirectory = "game\\" + folderName;
        }
       
        #region Load
        public static T LoadContent<T>(string name)
        {
            try
            {
                return contentManager.Load<T>(name);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return default(T);
            }
        }

        public static T LoadFile<T>(string name)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            BinarySeriailizer binarySeriailizer = new BinarySeriailizer();
            var res = binarySeriailizer.Deserialize<T>(contentPath + "\\" + name);
            stopwatch.Stop();
            var elaps = stopwatch.Elapsed;

            return res;
        }

        public static T[] FindAndLoadFiles<T>(string extension) => FindFiles(extension).Select(x => LoadFile<T>(x)).ToArray();

        public static string ReadRead(string name)
        {
            if (File.Exists(contentPath + "\\" + name))
            {
                return File.ReadAllText(contentPath + "\\" + name);
            }

            throw new Exception("File " + name + " Not Found");
        }
        #endregion

        #region Save
        public static void SaveAsset<T>(T asset, string fileName)
        {
            BinarySeriailizer seriailizer = new BinarySeriailizer();
            seriailizer.Serialize(asset, contentPath + "\\" + fileName);
        }

        public static void SaveFile<T>(T asset, string filePath)
        {
            BinarySeriailizer seriailizer = new BinarySeriailizer();
            seriailizer.Serialize(asset, contentPath + filePath);
        }
        #endregion

        public static string[] FindFiles(string extention)
        {
            string[] directories = Directory.GetDirectories(contentPath);
            List<string> files = Directory.GetFiles(contentPath).Where(x => x.Contains(extention)).ToList();

            foreach (string o in directories)
            {
                files.AddRange(Directory.GetFiles(o).Where(x => x.Contains(extention)));
            }

            return files.ToArray();
        }

        public static bool GetIcon(string name, out Icon icon)
        {
            icon = null;
            string file = iconPath + "\\" + name + ".png";

            if (File.Exists(file))
            {
                icon = new Icon(file);
                return true;
            }
            
            return false;
        }

        public static string[] GetIconNames()
        {
            return Directory.GetFiles(iconPath).Select(x => Path.GetFileNameWithoutExtension(x)).ToArray();
        }

        public static string GetAssetFolderDirectory() => contentPath;
        public static string GetAssetDirectory(string name) => contentPath + "\\" + name;

        public static string GetFilePath(string path)
        {
            int contentIndex = path.IndexOf(folderName);
            return path.Substring(contentIndex + folderName.Length);
        }
    }
}
