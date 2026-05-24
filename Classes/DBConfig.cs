using System.IO;
using Newtonsoft.Json;

public class DBConfig
{
    public string Server { get; set; }
    public string Database { get; set; }
    public bool sqlAuthentication { get; set; }
    public string User { get; set; }
    public string Password { get; set; }

    // تحميل الإعدادات
    public static DBConfig Load()
    {
        if (!File.Exists("config.json"))
        {
            return new DBConfig(); // يرجع فاضي
        }
        return JsonConvert.DeserializeObject<DBConfig>(File.ReadAllText("config.json"));
    }

    // حفظ الإعدادات
    public void Save()
    {
        File.WriteAllText("config.json", JsonConvert.SerializeObject(this, Formatting.Indented));
    }
}
