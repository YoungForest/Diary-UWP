using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;


namespace Diary
{
    public class Library
    {
        public string getInformation(string key)
        {
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey(key) && ApplicationData.Current.LocalSettings.Values[key] != null)
            {
                return ApplicationData.Current.LocalSettings.Values[key].ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        public void saveInformation(string key, string value)
        {
            ApplicationData.Current.LocalSettings.Values[key] = value;
            ApplicationData.Current.LocalSettings.Values[key + "_time"] = DateTime.Now.ToLocalTime().ToString();
        }

        public void deleteInformation(string key)
        {
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey(key) && ApplicationData.Current.LocalSettings.Values[key] != null)
            {
                ApplicationData.Current.LocalSettings.Values[key] = null;
                ApplicationData.Current.LocalSettings.Values[key + "_time"] = null;
            }
        }

        public ICollection<string> getKeys()
        {
            var keys = ApplicationData.Current.LocalSettings.Values.Keys;
            return keys;
        }
    }
}
