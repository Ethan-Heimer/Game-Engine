﻿using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
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
    public class AssetManager
    {
        const string folderName = "Content";
        const string contentPath = "..\\..\\game\\" + folderName;

        static ContentManager contentManager;

        public AssetManager(ContentManager content)
        {
            contentManager = content;
            contentManager.RootDirectory = "game\\" + folderName;
        }
       
        #region Load
        public static T LoadContent<T>(string name)
        {
            return contentManager.Load<T>(name);
        }

        public static T LoadFile<T>(string name)
        {
            BinarySeriailizer binarySeriailizer = new BinarySeriailizer();
            return binarySeriailizer.Deserialize<T>(contentPath + "\\" + name);
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

        public static string GetAssetFolderDirectory() => contentPath;
        public static string GetAssetDirectory(string name) => contentPath + "\\" + name;

        public static string GetFilePath(string path)
        {
            int contentIndex = path.IndexOf(folderName);
            Console.WriteLine(contentIndex);
            return path.Substring(contentIndex + folderName.Length);
        }
    }
}
