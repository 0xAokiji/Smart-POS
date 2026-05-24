using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

public static class SettingsHelper
{
    private static readonly string SettingsFolder = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "SmartPos"
    );

    private static readonly string SettingsFile = Path.Combine(SettingsFolder, "Settings.config");

    static SettingsHelper()
    {
        // لو المجلد مش موجود نعمله
        if (!Directory.Exists(SettingsFolder))
            Directory.CreateDirectory(SettingsFolder);

        // لو الملف مش موجود نعمله
        if (!File.Exists(SettingsFile))
        {
            new XDocument(new XElement("Settings")).Save(SettingsFile);
        }
    }

    public static void SaveSetting(string key, string value)
    {
        XDocument doc = XDocument.Load(SettingsFile);
        XElement root = doc.Element("Settings");
        XElement setting = null;

        foreach (var el in root.Elements("Setting"))
        {
            if (el.Attribute("Key").Value == key)
            {
                setting = el;
                break;
            }
        }

        if (setting != null)
        {
            setting.Attribute("Value").Value = value;
        }
        else
        {
            root.Add(new XElement("Setting",
                new XAttribute("Key", key),
                new XAttribute("Value", value)));
        }

        doc.Save(SettingsFile);
    }

    public static string GetSetting(string key, string defaultValue = "")
    {
        XDocument doc = XDocument.Load(SettingsFile);
        XElement root = doc.Element("Settings");

        foreach (var el in root.Elements("Setting"))
        {
            if (el.Attribute("Key").Value == key)
                return el.Attribute("Value").Value;
        }

        return defaultValue;
    }
}
