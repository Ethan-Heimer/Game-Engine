using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace GameEngine.Editor
{
    public class BinarySeriailizer
    {
        public void Serialize(object data, string filePath)
        {
            FileStream fileStream;
            BinaryFormatter formatter = new BinaryFormatter();

            if(File.Exists(filePath)) { File.Delete(filePath);}

            Console.WriteLine(filePath);
            fileStream = File.Create(filePath);
            formatter.Serialize(fileStream, data);
            fileStream.Close();
        }

        public object Deserialize(string filePath)
        {
            try
            {
                object data = null;

                FileStream fileStream;
                BinaryFormatter formatter = new BinaryFormatter();

                if (File.Exists(filePath))
                {
                    fileStream = File.OpenRead(filePath);
                    data = formatter.Deserialize(fileStream);
                    fileStream.Close();
                }

                return data;
            }
            catch (Exception e) 
            {
                throw e;
            }
        }

        public T Deserialize<T>(string filePath)
        {
            return (T)Deserialize(filePath);
        }
    }
}
