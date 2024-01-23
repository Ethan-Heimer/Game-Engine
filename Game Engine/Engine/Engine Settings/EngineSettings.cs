
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Input;
using System.Windows.Markup;

namespace GameEngine.Engine.Settings
{
    public static class EngineSettings
    {
        const string path = "..\\..\\engine\\Engine Settings\\Settings.json";

        static SettingList _settings;
        static SettingList Settings
        {
            get
            {
                if (_settings == null)
                {
                    _settings = LoadSettings();
                }

                return _settings;
            }
        }

        public static string GetString(string key) => FetchSetting<string>(key).Value;
        public static Color GetColor(string key) 
        {
            Setting colorSetting = FetchSetting<Color>(key);

            float R = int.Parse(colorSetting.X);
            float G = int.Parse(colorSetting.Y);
            float B = int.Parse(colorSetting.Z);

            return new Color(R, G, B);
        }
        public static float GetFloat(string key) => float.Parse(FetchSetting<float>(key).Value);
        public static float GetInt(string key) => int.Parse(FetchSetting<int>(key).Value);
        public static Vector2 GetVector2(string key)
        {
            Setting setting = FetchSetting<Vector2>(key);

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

        public static bool GetBool(string key) => bool.Parse(FetchSetting<bool>(key).Value);


        public static void SetFloat(string key, float value) => FetchSetting<float>(key).Value = value.ToString();
        public static void SetString(string key, string value) => FetchSetting<string>(key).Value = value.ToString();

        public static Setting[] GetSettings() => Settings.Settings;
        public static Section[] GetSections() => Settings.Sections.ToArray();

        static SettingList LoadSettings()
        {
            string data = File.ReadAllText(path);
            SettingList settings = JsonSerializer.Deserialize<SettingList>(data);
            foreach(var o in settings.Settings)
            {
                o.OnValueChanged += (s, v) => SaveSettings();
            }

            return settings;
        }

        static void SaveSettings()
        {
            FileStream createStream = File.Create(path);
            JsonSerializer.Serialize(createStream, Settings);

            Console.WriteLine("Save");
        }

        static Setting FetchSetting<T>(string name) => Settings.GetSettingsOfType<T>().FirstOrDefault(s => s.Option == name);
        

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

    public class Setting
    {
        public event Action<Setting, string> OnValueChanged;

        string _value;
        public string Value 
        {
            get
            {
                return _value;
            }
            
            set
            {
                _value = value;
                OnValueChanged?.Invoke(this, value);
            }
        }

        public string Option 
        {
            get; set;
        }

        string _x;
        public string X 
        {
            get
            {
                return _x;
            }
            set
            {
                _x = value;
                OnValueChanged?.Invoke(this, value);
            } 
        }

        string _y;
        public string Y 
        {
            get
            {
                return _y;
            }
            set
            {
                _y = value;
                OnValueChanged?.Invoke(this, value);
            }
        }

        string _z;
        public string Z 
        {
            get
            {
                return _z;
            }
            set
            {
                _z = value;
                OnValueChanged?.Invoke(this, value);
            }
        }

        public string Type 
        {
            get; set;
        }


    }
}
