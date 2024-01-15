using NumpyDotNet;
using NumSharp.Utilities;
using ScottPlot;
using ScottPlot.Control;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MedSys
{
    partial class PlotSelect
    {
        [Plotting]
        public void GenderPlotting()
        {
            GenderPlot.Plot.Clear();
            List<med> data = BackingData;
            if (data == null || data.Count == 0)
            {
                return;
            }
            ConcurrentBag<string> genderBag = new ConcurrentBag<string>();
            Parallel.ForEach(data, med => {
                
                if(med.性别 == "男" || med.性别 == "女")
                {
                    genderBag.Add(med.性别);
                }
               
            });

            var result = np.unique(np.array(genderBag.ToArray()),return_counts:true);
            double[] bins = ((System.Int64[])result.counts.ToArray()).Select(x => (double)x).ToArray();
            string[] labels = (string[]) result.data.ToArray();
            double[] positions = ((int[])Enumerable.Range(0, labels.Length).ToArray()).Select(x => (double)x).ToArray(); 
            GenderPlot.Configuration.UseRenderQueue = true;
            GenderPlot.Plot.AddBar(bins, positions);
            GenderPlot.Plot.XTicks(positions, labels);
            GenderPlot.Plot.AxisAuto();
            GenderPlot.Refresh();                     
        }

        [Plotting]
        public void AgePlotting()
        {
            AgePlot.Plot.Clear();
            List<med> data = BackingData;
            if (data == null || data.Count == 0)
            {
                return;
            }
            ConcurrentBag<double> ageBag = new ConcurrentBag<double>();
            Parallel.ForEach(data, med => {
                double age = 0;
                if (double.TryParse(med.年龄, out age))
                {
                    if (med.年龄单位 == "月")
                    {
                        age /= 12;
                    }
                }
                ageBag.Add(age);
            });

            var result = np.histogram(np.array(ageBag.ToArray()),10,(0,100));
            double[] edges = (double[])result.bin_edges.ToArray();
            System.Int64[] bins = (System.Int64[])result.hist.ToArray();
            var dbins = bins.Select(x => (double)x).ToArray();
            for (int i = 0;i<edges.Length - 1; i++)
            {
                edges[i] = (edges[i] + edges[i + 1]) / 2;
            }
            AgePlot.Configuration.UseRenderQueue = true;
            edges = edges.Slice(0, edges.Length - 1);
            AgePlot.Plot.AddBar(dbins,edges);
            AgePlot.Refresh();
        }

        [Plotting]
        public void ReportRegionPlotting()
        {
            ReportRegionPlot.Plot.Clear();
            List<med> data = BackingData;
            if (data == null || data.Count == 0)
            {
                return;
            }
            ConcurrentBag<string> regionBag = new ConcurrentBag<string>();
            Parallel.ForEach(data, med => {

                if (med.报告地区名称 != string.Empty)
                {
                    regionBag.Add(med.报告地区名称);
                }

            });
            var result = np.unique(np.array(regionBag.ToArray()), return_counts: true);
            double[] bins = ((System.Int64[])result.counts.ToArray()).Select(x => (double)x).ToArray();
            string[] labels = (string[])result.data.ToArray();
            double[] positions = ((int[])Enumerable.Range(0, labels.Length).ToArray()).Select(x => (double)x).ToArray(); 
            ReportRegionPlot.Configuration.UseRenderQueue = true;
            ReportRegionPlot.Plot.AddBar(bins, positions);
            ReportRegionPlot.Plot.XTicks(positions, labels);
            ReportRegionPlot.Plot.AxisAuto();
            ReportRegionPlot.Refresh();
        }

        [Plotting]
        public void ReportTypePlotting()
        {
            ReportTypePlot.Plot.Clear();
            List<med> data = BackingData;
            if (data == null || data.Count == 0)
            {
                return;
            }
            ConcurrentBag<string> regionBag = new ConcurrentBag<string>();
            Parallel.ForEach(data, med => {

                if (med.报告类型 != string.Empty)
                {
                    regionBag.Add(med.报告类型);
                }

            });
            var result = np.unique(np.array(regionBag.ToArray()), return_counts: true);
            double[] bins = ((System.Int64[])result.counts.ToArray()).Select(x => (double)x).ToArray();
            string[] labels = (string[])result.data.ToArray();
            double[] positions = ((int[])Enumerable.Range(0, labels.Length).ToArray()).Select(x => (double)x).ToArray(); 
            ReportTypePlot.Configuration.UseRenderQueue = true;
            ReportTypePlot.Plot.AddBar(bins, positions);
            ReportTypePlot.Plot.XTicks(positions, labels);
            ReportTypePlot.Plot.AxisAuto();
            ReportTypePlot.Refresh();
        }

        [Plotting]
        public void ReportInstitutionTypePlotting()
        {
            ReportInstitutionTypePlot.Plot.Clear();
            List<med> data = BackingData;
            if (data == null || data.Count == 0)
            {
                return;
            }
            ConcurrentBag<string> regionBag = new ConcurrentBag<string>();
            Parallel.ForEach(data, med => {

                if (med.报告单位类别 != string.Empty)
                {
                    regionBag.Add(med.报告单位类别);
                }

            });
            var result = np.unique(np.array(regionBag.ToArray()), return_counts: true);
            double[] bins = ((System.Int64[])result.counts.ToArray()).Select(x => (double)x).ToArray();
            string[] labels = (string[])result.data.ToArray();
            double[] positions = ((int[])Enumerable.Range(0, labels.Length).ToArray()).Select(x => (double)x).ToArray();
            ReportInstitutionTypePlot.Configuration.UseRenderQueue = true;
            ReportInstitutionTypePlot.Plot.AddBar(bins, positions);
            ReportInstitutionTypePlot.Plot.XTicks(positions, labels);
            ReportInstitutionTypePlot.Plot.AxisAuto();
            ReportInstitutionTypePlot.Refresh();
        }

        [Plotting]
        public void ReportInstitutionPlotting()
        {
            ReportInstitutionPlot.Plot.Clear();
            List<med> data = BackingData;
            if (data == null || data.Count == 0)
            {
                return;
            }
            ConcurrentBag<string> regionBag = new ConcurrentBag<string>();
            Parallel.ForEach(data, med => {

                if (med.报告单位名称 != string.Empty)
                {
                    regionBag.Add(med.报告单位名称);
                }

            });
            var result = np.unique(np.array(regionBag.ToArray()), return_counts: true);
            double[] bins = ((System.Int64[])result.counts.ToArray()).Select(x => (double)x).ToArray();
            string[] labels = (string[])result.data.ToArray();
            double[] positions = ((int[])Enumerable.Range(0, labels.Length).ToArray()).Select(x => (double)x).ToArray();
            ReportInstitutionPlot.Configuration.UseRenderQueue = true;
            ReportInstitutionPlot.Plot.AddBar(bins, positions);
            ReportInstitutionPlot.Plot.XTicks(positions, labels);
            ReportInstitutionPlot.Plot.AxisAuto();
            ReportInstitutionPlot.Refresh();
        }


        [Plotting]
        public void ReporterProfessionPlotting()
        {
            ReporterProfessionPlot.Plot.Clear();
            List<med> data = BackingData;
            if (data == null || data.Count == 0)
            {
                return;
            }
            ConcurrentBag<string> regionBag = new ConcurrentBag<string>();
            Parallel.ForEach(data, med => {

                if (med.初始报告人职业 != string.Empty)
                {
                    regionBag.Add(med.初始报告人职业);
                }

            });
            var result = np.unique(np.array(regionBag.ToArray()), return_counts: true);
            double[] bins = ((System.Int64[])result.counts.ToArray()).Select(x => (double)x).ToArray();
            string[] labels = (string[])result.data.ToArray();
            double[] positions = ((int[])Enumerable.Range(0, labels.Length).ToArray()).Select(x => (double)x).ToArray();
            ReporterProfessionPlot.Configuration.UseRenderQueue = true;
            ReporterProfessionPlot.Plot.AddBar(bins, positions);
            ReporterProfessionPlot.Plot.XTicks(positions, labels);
            ReporterProfessionPlot.Plot.AxisAuto();
            ReporterProfessionPlot.Refresh();
        }

        [Plotting]
        public void AdverseReactionNamePlotting() {

            AdverseReactionNamePlot.Plot.Clear();
            List<med> data = BackingData;
            if (data == null || data.Count == 0)
            {
                return;
            }
            ConcurrentDictionary<string, int> buckets = new ConcurrentDictionary<string, int>();
            Parallel.ForEach(MedDRAEntry.PreferredTerms, pt =>
            {
                int acc = 0;
               
                foreach(var a in data
                    )
                {
                    if (a.系统不良反应术语.Contains(pt.Name)) acc++;
                }
                if(acc!=0)buckets.TryAdd(pt.Name, acc);
            });
            double[] bins = ((int[])buckets.Values.ToArray()).Select(x => (double)x).ToArray();
            string[] labels = (string[])buckets.Keys.ToArray();
            double[] positions = ((int[])Enumerable.Range(0, labels.Length).ToArray()).Select(x => (double)x).ToArray();
            AdverseReactionNamePlot.Configuration.UseRenderQueue = true;
            AdverseReactionNamePlot.Plot.AddBar(bins, positions);
            AdverseReactionNamePlot.Plot.XTicks(positions, labels);
            AdverseReactionNamePlot.Plot.AxisAuto();
            AdverseReactionNamePlot.Refresh();
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class Plotting : Attribute
    {
    }
}
