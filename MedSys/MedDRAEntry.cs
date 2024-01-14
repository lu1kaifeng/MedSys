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
    public class MedDRAEntry
    {
        [JsonProperty("Name")]
        public string Name { set; get; }
        [JsonProperty("NameEn")]
        public string NameEn { set; get; }
        [JsonProperty("Code")]
        public string Code { set; get; }
        [JsonProperty("Children")]
        MedDRAEntry[] Children { set; get; }
        public static MedDRAEntry[] Entries;
        public static TreeViewItem[] TreeViewItems;
        public static Task LoadHandle;
        public static HashSet<MedDRAEntry> PreferredTerms = new HashSet<MedDRAEntry>();

        private TreeViewItem ToTreeViewItem(int layers=4)
        {
            var tvi = new TreeViewItem();
            tvi.Focusable = false;
            tvi.Header = this.Name;
            if (Children == null || layers == 1) { 
                tvi.Focusable = true;
                PreferredTerms.Add(this);
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
