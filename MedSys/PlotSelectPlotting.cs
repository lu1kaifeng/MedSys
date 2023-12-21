using NumpyDotNet;
using NumSharp.Utilities;
using ScottPlot;
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
        public void WeightPlotting()
        {
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
            double[] positions = ((int[])Enumerable.Range(0, labels.Length).ToArray()).Select(x => (double)x).ToArray(); ;
            GenderPlot.Plot.AddBar(bins, positions);
            GenderPlot.Plot.XTicks(positions, labels);
            GenderPlot.Plot.AxisAuto();
            GenderPlot.Refresh();                     
        }

        [Plotting]
        public void AgePlotting()
        {
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
            edges = edges.Slice(0, edges.Length - 1);
            AgePlot.Plot.AddBar(dbins,edges);
            AgePlot.Refresh();
        }

        [Plotting]
        public void ReportRegionPlotting()
        {
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
            double[] positions = ((int[])Enumerable.Range(0, labels.Length).ToArray()).Select(x => (double)x).ToArray(); ;
            ReportRegionPlot.Plot.AddBar(bins, positions);
            ReportRegionPlot.Plot.XTicks(positions, labels);
            ReportRegionPlot.Plot.AxisAuto();
            ReportRegionPlot.Refresh();
        }

        [Plotting]
        public void ReportTypePlotting()
        {
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
            double[] positions = ((int[])Enumerable.Range(0, labels.Length).ToArray()).Select(x => (double)x).ToArray(); ;
            ReportTypePlot.Plot.AddBar(bins, positions);
            ReportTypePlot.Plot.XTicks(positions, labels);
            ReportTypePlot.Plot.AxisAuto();
            ReportTypePlot.Refresh();
        }

    }

    [AttributeUsage(AttributeTargets.Method)]
    public class Plotting : Attribute
    {
    }
}
