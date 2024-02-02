using ScottPlot;
using ScottPlot.Control;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace MedSys
{
    partial class PlotSelect
    {
        [Plotting]
        public void GenderPlotting()
        {
            GenderPlot.PlotData = ExactBuckets((med) => {
                if (med.性别 == "男" || med.性别 == "女")
                {
                    return med.性别;
                }
                else
                {
                    return string.Empty;
                }
            });
        }
        [Plotting]
        public void AgePlotting()
        {

            AgePlot.PlotData = ExactBuckets((med) => {
                double age = 0;
                if (double.TryParse(med.年龄, out age))
                {
                    if (med.年龄单位 == "月")
                    {
                        age /= 12;
                    }if(med.年龄单位 == "天")
                    {
                        age /= 356;
                    }
                    else if(med.年龄单位 != "年")
                    {
                        return string.Empty;
                    }
                }
                else
                {
                    return string.Empty;
                }
                return (((int)Math.Round((int)age / 10.0)) * 10).ToString();

            },  Comparer<string>.Create((k1, k2) =>
            {
                var ik1 = int.Parse(k1);
                var ik2 = int.Parse(k2);
                if(ik1 == ik2)
                {
                    return 0;
                }
                if(ik1 > ik2)
                {
                    return 1;
                }
                if (ik1 < ik2)
                {
                    return -1;
                }
                return 0;
            }));
        }

        [Plotting]
        public void ReportRegionPlotting()
        {

            ReportRegionPlot.PlotData = ExactBuckets((m) => m.报告地区名称, valComparer: Comparer<int>.Create((x, y) => x == y ? 0 : (x < y ? 1 : -1)));
        }

        [Plotting]
        public void ReportTypePlotting()
        {

            ReportTypePlot.PlotData = ExactBuckets((m)=>m.报告类型);
        }

        [Plotting]
        public void ReportInstitutionTypePlotting()
        {

            ReportInstitutionTypePlot.PlotData = ExactBuckets((m) => m.报告单位类别);
        }

        [Plotting]
        public void ReportInstitutionPlotting()
        { 
            ReportInstitutionPlot.PlotData = ExactBuckets((m) => m.报告单位名称,valComparer: Comparer<int>.Create((x,y)=>x==y?0:(x<y?1:-1)));
        }


        [Plotting]
        public void ReporterProfessionPlotting()
        {

            ReporterProfessionPlot.PlotData =  ExactBuckets((m) => m.初始报告人职业);
        }

        [Plotting]
        public void AdverseReactionNamePlotting()
        {

            //AdverseReactionNamePlot.Plot.Clear();
            List<med> data = BackingData.BackingData;
            if (data == null || data.Count == 0)
            {
                return;
            }
            ConcurrentDictionary<MedDRAEntry, int> buckets = new ConcurrentDictionary<MedDRAEntry, int>();
            Parallel.ForEach(MedDRAEntry.PreferredTerms, pt =>
            {
                int acc = 0;
                Object thisLock = new Object();
                Parallel.ForEach(data,
                        (a) =>
                    {
                        if (a.系统不良反应术语.Contains(pt.Name)) lock (thisLock) { acc++; };
                    });
                if (acc != 0) buckets.TryAdd(pt, acc);

            });
            var sorted = buckets.OrderBy(i => i.Value, Comparer<int>.Create((x, y) => x == y ? 0 : (x < y ? 1 : -1)));
            var vals = from s in sorted select s.Value;
            var keys = from s in sorted select s.Key;
            double[] bins = ((int[])vals.ToArray()).Select(x => (double)x).ToArray();
            string[] labels = (string[])keys.Select((r)=>r.Name).ToArray();
            double[] positions = ((int[])Enumerable.Range(0, labels.Length).ToArray()).Select(x => (double)x).ToArray();
            AdverseReactionNamePlot.PlotData = new PlotData(labels,bins,positions);
            //AdverseReactionNamePlot.Configuration.UseRenderQueue = true;
            //AdverseReactionNamePlot.Plot.AddBar(bins, positions);
            //AdverseReactionNamePlot.Plot.XTicks(positions, labels);
            //AdverseReactionNamePlot.Plot.AxisAuto();
            //AdverseReactionNamePlot.Refresh();
            

            ConcurrentDictionary<MedDRAEntry, int> socBuckets = new ConcurrentDictionary<MedDRAEntry, int>();
            Parallel.ForEach(buckets, (e) =>
            {
                var ek = e.Key;
                while (ek.Ancestor != null) ek = ek.Ancestor;
                if (!socBuckets.ContainsKey(ek))
                {
                    socBuckets.TryAdd(ek,e.Value);
                }
                else
                {
                    socBuckets[ek] += e.Value;
                }
            });
            sorted = socBuckets.OrderBy(i => i.Value, Comparer<int>.Create((x, y) => x == y ? 0 : (x < y ? 1 : -1)));
            vals = from s in sorted select s.Value;
            keys = from s in sorted select s.Key;
            bins = ((int[])vals.ToArray()).Select(x => (double)x).ToArray();
            labels = (string[])keys.Select((r) => r.Name).ToArray();
            positions = ((int[])Enumerable.Range(0, labels.Length).ToArray()).Select(x => (double)x).ToArray();
            AdverseReactionDamagedSystemPlot.PlotData = new PlotData(labels, bins, positions);
        }

        [Plotting]
        public void AdverseReactionResultPlotting()
        {
             AdverseReactionResultPlot.PlotData = ContainBuckets(Typing.AdverseEffectResultType.Skip(1), (a) => a.不良反应结果, valComparer: Comparer<int>.Create((x, y) => x == y ? 0 : (x < y ? 1 : -1)));
        }
        [Plotting]
        public void PreviousAdverseReactionPlotting()
        {
            //PlotBucketsIfExact((m) => m.既往药品不良反应事件, PreviousAdverseReactionPlot);
        }
        [Plotting]
        public void FamilyAdverseReactionPlotting()
        {
            //PlotBucketsIfExact((m) => m.家族药品不良反应事件, FamilyAdverseReactionPlot);
        }
        [Plotting]
        public void EffectOnPreexistingConditionPlotting()
        {
            EffectOnPreexistingConditionPlot.PlotData = ExactBuckets((m) => m.对原患疾病影响);
        }
        [Plotting]
        public void MedInfoPlottings()
        {
            GenericNameNoDosagePlot.PlotData = ExactBuckets((m)=>m.通用名称,valComparer: Comparer<int>.Create((x, y) => x == y ? 0 : (x < y ? 1 : -1)));
            ManufacturerPlot.PlotData = ExactBuckets(m => m.持有人或生产厂家,valComparer: Comparer<int>.Create((x, y) => x == y ? 0 : (x < y ? 1 : -1)));

        }

        private void PlotBucketsIfContain(IEnumerable<string> labelList,Func<med,string> selector,WpfPlot targetPlot)
        {
            targetPlot.Plot.Clear();
            List<med> data = BackingData.
                    BackingData;

            if (data == null || data.Count == 0)
            {
                return;
            }
            ConcurrentDictionary<string, int> buckets = new ConcurrentDictionary<string, int>();
            foreach (var s in labelList)
            {
                buckets.TryAdd(s, 0);
            }
            foreach (var s in buckets.Keys)
            {
                Parallel.ForEach(data,
                    (a) =>
                    {

                        if (selector(a).Contains(s))
                            lock (buckets)
                            {
                                buckets[s]++;
                            }
                        
                    });
            }
            double[] bins = ((int[])buckets.Values.ToArray()).Select(x => (double)x).ToArray();
            string[] labels = (string[])buckets.Keys.ToArray();
            double[] positions = ((int[])Enumerable.Range(0, labels.Length).ToArray()).Select(x => (double)x).ToArray();
            targetPlot.Configuration.UseRenderQueue = true;
            targetPlot.Plot.AddBar(bins, positions);
            targetPlot.Plot.XTicks(positions, labels);
            targetPlot.Plot.AxisAuto();
            targetPlot.Refresh();
        }

        private void PlotBucketsIfExact( Func<med, string> selector, WpfPlot targetPlot,IComparer<string> keyComparer=null, IComparer<int> valComparer = null)
        {
            targetPlot.Plot.Clear();
            List<med> data = BackingData.BackingData;

            if (data == null || data.Count == 0)
            {
                return;
            }
            ConcurrentDictionary<string, int> buckets = new ConcurrentDictionary<string, int>();
           
                Parallel.ForEach(data,
                    (a) =>
                    {
                        if (selector(a) == null) return;
                        if (selector(a).Trim() != string.Empty)
                            lock (buckets)
                            {
                                if (buckets.ContainsKey(selector(a).Trim()))
                                {
                                    buckets[selector(a).Trim()]++;
                                }
                                else
                                {
                                    buckets[selector(a).Trim()] = 0;
                                }             
                            }

                    });
            IEnumerable<int> vals;
            IEnumerable<string> keys;
            if (keyComparer != null || valComparer !=null)
            {
                IEnumerable<KeyValuePair<string, int>> sorted;
                if (valComparer != null)
                {
                    sorted = buckets.OrderBy(i => i.Value, valComparer);
                }
                else
                {
                    sorted = buckets.OrderBy(i => i.Key, keyComparer);
                }
                vals = from s in sorted select s.Value;
                keys = from s in sorted select s.Key;
            }
            else
            {
                vals = buckets.Values;
                keys = buckets.Keys;
            }
            double[] bins = ((int[])vals.ToArray()).Select(x => (double)x).ToArray();
            string[] labels = (string[])keys.ToArray();
            double[] positions = ((int[])Enumerable.Range(0, labels.Length).ToArray()).Select(x => (double)x).ToArray();
            targetPlot.Configuration.UseRenderQueue = true;
            targetPlot.Plot.AddBar(bins, positions);
            targetPlot.Plot.XTicks(positions, labels);
            targetPlot.Plot.AxisAuto();
            targetPlot.Refresh();
        }

        private PlotData ExactBuckets(Func<med, string> selector,  IComparer<string> keyComparer = null, IComparer<int> valComparer = null)
        {

            List<med> data = BackingData.BackingData;

            if (data == null || data.Count == 0)
            {
                return null;
            }
            ConcurrentDictionary<string, int> buckets = new ConcurrentDictionary<string, int>();

            Parallel.ForEach(data,
                (a) =>
                {
                    if (selector(a) == null) return;
                    if (selector(a).Trim() != string.Empty)
                        lock (buckets)
                        {
                            if (buckets.ContainsKey(selector(a).Trim()))
                            {
                                buckets[selector(a).Trim()]++;
                            }
                            else
                            {
                                buckets[selector(a).Trim()] = 0;
                            }
                        }

                });
            IEnumerable<int> vals;
            IEnumerable<string> keys;
            if (keyComparer != null || valComparer != null)
            {
                IEnumerable<KeyValuePair<string, int>> sorted;
                if (valComparer != null)
                {
                    sorted = buckets.OrderBy(i => i.Value, valComparer);
                }
                else
                {
                    sorted = buckets.OrderBy(i => i.Key, keyComparer);
                }
                vals = from s in sorted select s.Value;
                keys = from s in sorted select s.Key;
            }
            else
            {
                vals = buckets.Values;
                keys = buckets.Keys;
            }
            double[] bins = ((int[])vals.ToArray()).Select(x => (double)x).ToArray();
            string[] labels = (string[])keys.ToArray();
            double[] positions = ((int[])Enumerable.Range(0, labels.Length).ToArray()).Select(x => (double)x).ToArray();
            return new PlotData(labels, bins, positions);

        }

        private PlotData ContainBuckets(IEnumerable<string> labelList, Func<med, string> selector, IComparer<string> keyComparer = null, IComparer<int> valComparer = null)
        {

            List<med> data = BackingData.BackingData;

            if (data == null || data.Count == 0)
            {
                return null;
            }
            ConcurrentDictionary<string, int> buckets = new ConcurrentDictionary<string, int>();
            foreach (var s in labelList)
            {
                buckets.TryAdd(s, 0);
            }
            foreach (var s in buckets.Keys)
            {
                Parallel.ForEach(data,
                    (a) =>
                    {

                        if (selector(a).Contains(s))
                            lock (buckets)
                            {
                                buckets[s]++;
                            }

                    });
            }
            IEnumerable<int> vals;
            IEnumerable<string> keys;
            if (keyComparer != null || valComparer != null)
            {
                IEnumerable<KeyValuePair<string, int>> sorted;
                if (valComparer != null)
                {
                    sorted = buckets.OrderBy(i => i.Value, valComparer);
                }
                else
                {
                    sorted = buckets.OrderBy(i => i.Key, keyComparer);
                }
                vals = from s in sorted select s.Value;
                keys = from s in sorted select s.Key;
            }
            else
            {
                vals = buckets.Values;
                keys = buckets.Keys;
            }
            double[] bins = ((int[])vals.ToArray()).Select(x => (double)x).ToArray();
            string[] labels = (string[])keys.ToArray();
            double[] positions = ((int[])Enumerable.Range(0, labels.Length).ToArray()).Select(x => (double)x).ToArray();
            return new PlotData(labels, bins, positions);
            
        }

    }

    [AttributeUsage(AttributeTargets.Method)]
    public class Plotting : Attribute
    {
    }
}
