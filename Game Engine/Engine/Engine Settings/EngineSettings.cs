
using GameEngine.Engine.Events;
using GameEngine.Engine.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
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
        static Setting[] settings;
        
        public static string[] Sections
        {
            get
            {
                return settings.Select(x => x.Section).Distinct().ToArray();
            }
        }

        public static void Init()
        {
            var json = LoadJson();
            FieldInfo[] foundSettings = FindSettings();

            settings = LoadSettings(json, foundSettings);
            InjectEngineSettings(settings, foundSettings);

            SaveSettings();
        }
        public static T GetSetting<T>(string name) =>
            (T)settings.FirstOrDefault(settings => settings.Name == name && Type.GetType(settings.Type) == typeof(T))?.Data;
        public static void SetSetting<T>(string name, T value) =>
            settings.FirstOrDefault(settings => settings.Name == name && Type.GetType(settings.Type) == typeof(T)).Data = value;

        public static Setting[] GetSettings() => settings.ToArray();
        public static Setting[] GetSettings(string section) => settings.Where(x => x.Section == section).ToArray();


        static List<Setting> LoadJson()
        {
            string data = File.ReadAllText(path);
            List<Setting> settings = JsonSerializer.Deserialize<List<Setting>>(data);

            return settings;
        }


        static void InjectEngineSettings(Setting[] settings, FieldInfo[] settingFields)
        {
            for(int i = 0; i < settings.Length; i++)
            {
                Setting setting = settings[i];
                FieldInfo field = settingFields[i];

                object value = ((JsonElement)setting.Data).ToRuntimeObject(field.FieldType);

                field.SetValue(null, value);
            }
        }
        
        static Setting[] LoadSettings(IEnumerable<Setting> settings, FieldInfo[] foundSettings)
        {
            List<Setting> engineSettings = new List<Setting>();

            foreach (var o in foundSettings)
            {
                Setting setting = settings.FirstOrDefault(x => x.Name == o.Name);

                if (setting != null)
                    setting.Section = ((EngineSettingsAttribute)o.GetCustomAttribute(typeof(EngineSettingsAttribute))).Section;
                else
                    setting = new Setting(o);

                engineSettings.Add(setting);
            }

            return engineSettings.ToArray();
        }

        static FieldInfo[] FindSettings()
        {
            Type[] typesWithSettings = FindSettingTypes();
            List<FieldInfo> setting = new List<FieldInfo>();

            foreach(var o in typesWithSettings) 
            {
                setting.AddRange(o.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).Where(x => x.GetCustomAttribute(typeof(EngineSettingsAttribute)) != null));
            }

            return setting.ToArray();
        }
        static void SaveSettings()
        {
            FileStream createStream = File.Create(path);
            JsonSerializer.Serialize(createStream, settings);
        }

        static Type[] FindSettingTypes() => Assembly.GetExecutingAssembly().GetTypes().Where(x => x.GetCustomAttribute(typeof(ContainsSettingsAttribute)) != null).ToArray();
    }

    public class Setting 
    {
        public string Name { get; set; }
        public string Section { get; set; }
        public object Data { get; set; }
        
        public string Type { get; set; }
        

        public Setting() { }

        public Setting(FieldInfo field)
        {
            Name = field.Name;
            Data = field.GetValue(null);

            Section = ((EngineSettingsAttribute)field.GetCustomAttribute(typeof(EngineSettingsAttribute))).Section;
            Type = Data.GetType().AssemblyQualifiedName;
        }

        public Setting(string name, string settingSection, object val)
        {
            Name = name;
            Data = val;

            Section = settingSection;
            Type = val.GetType().AssemblyQualifiedName;
        }

    }

    public class ContainsSettingsAttribute : Attribute { }
    public class EngineSettingsAttribute : Attribute
    {
        public string Section;
    }
}
