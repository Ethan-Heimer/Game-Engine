
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GameEngine.Engine.Settings
{
    public static class EngineSettings
    {
        static SettingList _settings;
        static SettingList Settings
        {
            get
            {
                if (_settings == null)
                {
                    string data = File.ReadAllText("..\\..\\engine\\Engine Settings\\Settings.json");
                    _settings = JsonSerializer.Deserialize<SettingList>(data);
                }

                return _settings;
            }
        }

        public static string GetString(string key) => Settings.GetSettingsOfType<string>().FirstOrDefault(x => x.Option == key).Value;
        public static Color GetColor(string key) 
        {
            Setting colorSetting = Settings.GetSettingsOfType<Color>().FirstOrDefault(x => x.Option == key);

            float R = int.Parse(colorSetting.X);
            float G = int.Parse(colorSetting.Y);
            float B = int.Parse(colorSetting.Z);

            return new Color(R, G, B);
        }
        public static float GetFloat(string key) => float.Parse(Settings.GetSettingsOfType<float>().FirstOrDefault(x => x.Option == key).Value);
        public static float GetInt(string key) => int.Parse(Settings.GetSettingsOfType<int>().FirstOrDefault(x => x.Option == key).Value);
        public static Vector2 GetVector2(string key)
        {
            Setting setting = Settings.GetSettingsOfType<Vector2>().FirstOrDefault(x => x.Option == key);

            float X = int.Parse(setting.X);
            float Y = int.Parse(setting.Y);

            return new Vector2(X, Y);
        }
        public static Vector3 GetVector3(string key)
        {
            Setting setting = Settings.GetSettingsOfType<Vector3>().FirstOrDefault(x => x.Option == key);

            float X = int.Parse(setting.X);
            float Y = int.Parse(setting.Y);
            float Z = int.Parse(setting.Z);

            return new Vector3(X, Y, Z);
        }

        public static bool GetBool(string key) => bool.Parse(Settings.GetSettingsOfType<bool>().FirstOrDefault(x => x.Option == key).Value);

        public static Setting[] GetSettings() => Settings.Settings;
        public static Section[] GetSections() => Settings.Sections.ToArray();

    }

    public class SettingList
    {
        public List<Section> Sections {get; set;}

        Setting[] _setting;
        public Setting[] Settings
        {
            get
            {
                if(_setting == null)
                {
                    List<Setting> s = new List<Setting>();
                    foreach (Section section in Sections) 
                    {
                        s.AddRange(section.Values);
                    }

                    _setting = s.ToArray();
                }
                return _setting;
            }
        }

       public Setting[] GetSettingsOfType<T>()
       {
            return Settings.Where(x => x.Type == typeof(T).FullName).ToArray();
       }
    }

    public struct Section
    {
        public string Title { get; set; }
        public List<Setting> Values { get; set; }
    }

    public struct Setting
    {
        public string Value { get; set; }
        public string Option { get; set; }
        public string X { get; set; }
        public string Y { get; set; }
        public string Z { get; set; }
        public string Type { get; set; }
    }
}
