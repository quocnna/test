using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTest.Core
{
    public static class RecentFileHelper
    {
        private const string RegistryKey = "Software\\Neogov\\AutoTest\\RecentFileList";
        private const int MaxNumberOfFiles = 10;

        public static List<string> GetRecentFiles()
        {
            RegistryKey k = Registry.CurrentUser.OpenSubKey(RegistryKey);
            if (k == null)
                k = Registry.CurrentUser.CreateSubKey(RegistryKey);

            List<string> list = new List<string>();
            for (int i = 0; i < MaxNumberOfFiles; i++)
            {
                string filename = (string)k.GetValue(i.ToString());
                if (string.IsNullOrEmpty(filename))
                    break;
                list.Add(filename);
            }
            list.Reverse();
            return list;
        }

        public static void AddFile(string path)
        {
            RegistryKey k = Registry.CurrentUser.OpenSubKey(RegistryKey, true);
            if (k == null)
            {
                _ = Registry.CurrentUser.CreateSubKey(RegistryKey);
                k = Registry.CurrentUser.OpenSubKey(RegistryKey, true);
            }

            RemoveFile(path);

            k.SetValue(k.ValueCount.ToString(), path);

            if (k.ValueCount > MaxNumberOfFiles)
            {
                int length = k.ValueCount;
                int diff = k.ValueCount - MaxNumberOfFiles;
                for (int i = 0; i < diff; i++)
                {
                    k.DeleteValue(i.ToString());
                }

                for (int i = diff; i <= length - 1; i++)
                {
                    object val = k.GetValue(i.ToString());
                    k.SetValue((i - diff).ToString(), val.ToString());
                    k.DeleteValue(i.ToString());
                }
            }
        }

        public static void RemoveFile(string path)
        {
            RegistryKey k = Registry.CurrentUser.OpenSubKey(RegistryKey, true);
            if (k != null)
            {
                int index = -1;
                for (int i = 0; i < k.ValueCount; i++)
                {
                    string s = k.GetValue(i.ToString()).ToString();
                    if (path.Equals(s, StringComparison.OrdinalIgnoreCase))
                    {
                        index = i;
                        k.DeleteValue(i.ToString(), false);
                        break;
                    }
                }
                if (index >= 0)
                {
                    for (int i = index + 1; i <= k.ValueCount; i++)
                    {
                        object val = k.GetValue(i.ToString());
                        k.SetValue((i - 1).ToString(), val.ToString());
                        k.DeleteValue(i.ToString());
                    }
                }
            }
        }
    }
}
