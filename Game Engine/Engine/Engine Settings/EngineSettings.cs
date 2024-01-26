
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

            SetSettingsHook(settings, SaveSettings);
        }
        public static T GetSetting<T>(string name) =>
            (T)settings.FirstOrDefault(settings => settings.Name == name && settings.DataType() == typeof(T))?.Value;
        public static void SetSetting<T>(string name, T value) =>
            settings.FirstOrDefault(settings => settings.Name == name && settings.DataType() == typeof(T)).Value = value;

        public static Setting[] GetSettings() => settings.ToArray();
        public static Setting[] GetSettings(string section) => settings.Where(x => x.Section == section).ToArray();


        static List<Setting> LoadJson()
        {
            string data = File.ReadAllText(path);
            List<Setting> settings = JsonSerializer.Deserialize<List<Setting>>(data);

            return settings;
        }

        static void SetSettingsHook(Setting[] settings, Action action)
        {
            foreach(var o in settings)
            {
                o.OnDataChanged += action;
            }
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
        static Setting[] LoadSettings(IEnumerable<Setting> settings, FieldInfo[] foundSettings)
        {
            List<Setting> engineSettings = new List<Setting>();

            foreach (var o in foundSettings)
            {
                Setting setting = settings.FirstOrDefault(x => x.Name == o.Name);

                if (setting != null)
                    setting.Init(o);
                else
                    setting = new Setting(o);

                engineSettings.Add(setting);
            }

            return engineSettings.ToArray();
        }

        async static void SaveSettings()
        {
            Console.WriteLine("Save");

            FileStream createStream = File.Create(path);
            JsonSerializer.Serialize(createStream, settings);

            createStream.Close();
        }

        static Type[] FindSettingTypes() => Assembly.GetExecutingAssembly().GetTypes().Where(x => x.GetCustomAttribute(typeof(ContainsSettingsAttribute)) != null).ToArray();
    }

    public class Setting 
    {
        public event Action OnDataChanged;

        public string Name { get; set; }
        public string Section { get; set; }

        object _data;
        public object Value
        {
            get 
            {
                if(_data.GetType() == typeof(JsonElement))
                {
                    _data = ((JsonElement)_data).ToRuntimeObject(DataType());
                }

                return _data;
                
            }
            set
            {
                Console.WriteLine(isLoaded);

                _data = value;

                if (isLoaded)
                {
                    bindingField.SetValue(null, value);
                    OnDataChanged?.Invoke();
                }
                else
                    isLoaded = true;
            } 
        }
        public string StringRepresentedType { get; set; }

        FieldInfo bindingField;
        bool isLoaded = false;
        


        public Setting() 
        {
            isLoaded = false;
        }

        public Setting(FieldInfo field)
        {
            Name = field.Name;
            Value = field.GetValue(null);

            Section = ((EngineSettingsAttribute)field.GetCustomAttribute(typeof(EngineSettingsAttribute))).Section;
            StringRepresentedType = Value.GetType().AssemblyQualifiedName;

            bindingField = field;
        }

        public Setting(string name, string settingSection, object val)
        {
            Name = name;
            Value = val;

            Section = settingSection;
            StringRepresentedType = val.GetType().AssemblyQualifiedName;
        }

        public void Init(FieldInfo field)
        {
            BindField(field);
            GetSection();
        }

        public void BindField(FieldInfo field) => bindingField = field;
        public string GetSection() => ((EngineSettingsAttribute)bindingField.GetCustomAttribute(typeof(EngineSettingsAttribute))).Section;
        public Type DataType() => Type.GetType(StringRepresentedType);
    }

    public class ContainsSettingsAttribute : Attribute { }
    public class EngineSettingsAttribute : Attribute
    {
        public string Section;

        public EngineSettingsAttribute(string section)
        {
            Section = section;
        }
    }
}
