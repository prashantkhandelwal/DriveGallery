using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Biggy;
using Biggy.JSON;


public class Settings
{
    public int SettingID { get; set; }
    public string SettingValue { get; set; }
    public Settings()
    {
        IBiggyStore<Settings> settings_store = new JsonStore<Settings>();
        var emp = new BiggyList<Settings>(settings_store);

        var s = new Settings { SettingID = 1, SettingValue = "Prashant" };

        emp.Add(s);

        emp.Update(s);

    }


}