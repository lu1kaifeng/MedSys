using ScottPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MedSys
{
    partial class PlotSelect
    {
        private void DiseaseTypePlot()
        {
            List<med> data = BackingData;
            if (data == null || data.Count == 0)
            {
                return;
            }
            List<double> dataX = new List<double>();
            List<double> dataY = new List<double>();
            foreach (var med in data)
            {
                int age = 0;
                if (int.TryParse(med.年龄, out age))
                {
                    double weight = 0;
                    if (double.TryParse(med.体重kg, out weight))
                    {
                        dataX.Add(age);
                        dataY.Add(weight);
                    }
                }
            }
            WpfPlot.Plot.AddScatterPoints(dataX.ToArray(), dataY.ToArray());
            WpfPlot.Plot.AxisAuto();
            WpfPlot.Refresh();
        }
    }
}
