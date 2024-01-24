
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Diagnostics;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Input;
using System.Windows.Markup;

namespace GameEngine.Engine.Settings
{
    public static class EngineSettings
    {
        const string path = "..\\..\\engine\\Engine Settings\\Settings.json";

        static List<Setting> _settings;
        public static List<Setting> settings
        {
            get
            {
                if (_settings == null)
                    _settings = LoadSettings();

                return _settings;
            }
        }

        public static string[] Sections
        {
            get
            {
                return settings.Select(x => x.Section).Distinct().ToArray();
            }
        }

        public static void SaveSettings()
        {
            FileStream createStream = File.Create(path);
            JsonSerializer.Serialize(createStream, settings);

            Console.WriteLine("Save");
        }

        static List<Setting> LoadSettings()
        {
            string data = File.ReadAllText(path);
            List<Setting> settings = JsonSerializer.Deserialize<List<Setting>>(data);
            foreach (var o in settings)
            {
               // o.OnValueChanged += (s, v) => SaveSettings();
            }

            return settings;
        }

        public static T GetSetting<T>(string name) =>
            (T)settings.FirstOrDefault(settings => settings.Name == name && Type.GetType(settings.Type) == typeof(T))?.Data;
        public static void SetSetting<T>(string name, T value) =>
            settings.FirstOrDefault(settings => settings.Name == name && Type.GetType(settings.Type) == typeof(T)).Data = value;


        public static Setting[] GetSettings() => settings.ToArray();
        public static Setting[] GetSettings(string section) => settings.Where(x => x.Section == section).ToArray();


        /*
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
        public static float GetFloat(string key) => float.Parse(FetchSetting<float>(key).Value);
        public static float GetInt(string key) => int.Parse(FetchSetting<int>(key).Value);
        public static bool GetBool(string key) => bool.Parse(FetchSetting<bool>(key).Value);
        public static Color GetColor(string key) 
        {
            Setting colorSetting = FetchSetting<Color>(key);

            float R = int.Parse(colorSetting.X);
            float G = int.Parse(colorSetting.Y);
            float B = int.Parse(colorSetting.Z);

            return new Color(R, G, B);
        }
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



        public static void SetFloat(string key, float value) => FetchSetting<float>(key).Value = value.ToString();
        public static void SetInt(string key, int value) => FetchSetting<int>(key).Value = value.ToString();
        public static void SetString(string key, string value) => FetchSetting<string>(key).Value = value.ToString();
        public static void SetBool(string key, bool value) => FetchSetting<bool>(key).Value = value.ToString();
        public static void SetColor(string key, Color value)
        {
            Setting colorSetting = FetchSetting<Color>(key);

            colorSetting.X = value.R.ToString();
            colorSetting.Y = value.G.ToString();
            colorSetting.Z = value.B.ToString();
        }

        public static void SetVector3(string key, Vector3 value)
        {
            Setting colorSetting = FetchSetting<Vector3>(key);

            colorSetting.X = value.X.ToString();
            colorSetting.Y = value.Y.ToString();
            colorSetting.Z = value.Z.ToString();
        }

        public static void SetVector2(string key, Vector2 value)
        {
            Setting colorSetting = FetchSetting<Vector3>(key);

            colorSetting.X = value.X.ToString();
            colorSetting.Y = value.Y.ToString();
        }

        public static Setting[] GetSettings() => Settings.GetAllSettings();
        public static Section[] GetSections() => Settings.Sections.ToArray();

        static SettingList LoadSettings()
        {
            string data = File.ReadAllText(path);
            SettingList settings = JsonSerializer.Deserialize<SettingList>(data);
            foreach(var o in settings.GetAllSettings())
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
        
        */
    }

    public class Setting 
    {
        public string Name { get; set; }
        public string Section { get; set; }

        object _data;
        public object Data
        {
            get
            {
                return _data;
            }

            set 
            {
                _data = value;
            }
        }

        public string Type
        {
            get
            {
                return Data.GetType().FullName;
            }
        }

        public Setting(string name, string settingSection, object val)
        {
            Name = name;
            Data = val;

            Section = settingSection;
        }

    }

    

    public interface ISetting<T> : ISetting
    {
        T Data { get; set; }
    }

    public interface ISetting
    {
        string Type { get; }
    }
}
