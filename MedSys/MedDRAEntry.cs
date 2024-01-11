using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MedSys
{
    internal class MedDRAEntry
    {
        [JsonProperty("Name")]
        string Name;
        [JsonProperty("NameEn")]
        string NameEn;
        [JsonProperty("Code")]
        string Code;
        [JsonProperty("Children")]
        MedDRAEntry[] Children;
        public static MedDRAEntry[] Entries;
        public static TreeViewItem[] TreeViewItems;
        public static Task LoadHandle;
        
        private TreeViewItem ToTreeViewItem(int layers=4)
        {
            var tvi = new TreeViewItem();
            tvi.Focusable = false;
            tvi.Header = this.Name;
            if (Children == null || layers == 1) { 
                tvi.Focusable = true;
                return tvi; 
            }
            foreach(var mde in Children)
            {

                tvi.Items.Add(mde.ToTreeViewItem(layers - 1));
            }
            
            return tvi;
        }

        static MedDRAEntry()
        {
            LoadHandle = Task.Run(() =>
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                var rs = new StreamReader(assembly.GetManifestResourceStream("MedSys.meddra.json")).ReadToEnd();
                Entries = JsonConvert.DeserializeObject<MedDRAEntry[]>(rs);
                Application.Current.Dispatcher.Invoke(()=> { TreeViewItems = Entries.Select((x) => x.ToTreeViewItem()).ToArray(); });
            });

        }
    }
}
