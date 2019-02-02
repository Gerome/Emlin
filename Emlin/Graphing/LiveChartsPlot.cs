using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Threading;
using System.Windows.Media;

namespace TestGraphing
{
    class LiveChartsPlot : IGraphPlot
    {
        private LiveCharts.WinForms.CartesianChart chart;
        private const int MAX_VALUES_ON_GRAPH = 65;
        public ChartValues<ObservableValue> Values { get; set; }


        public LiveChartsPlot(LiveCharts.WinForms.CartesianChart chart)
        {
            this.chart = chart;

            var r = new Random();

            Values = new ChartValues<ObservableValue>{};

            this.chart.LegendLocation = LegendLocation.Right;

            this.chart.Series.Add(new LineSeries
            {
                Values = Values,
                StrokeThickness = 1,
                PointGeometrySize = 0,
                Fill = Brushes.Transparent,
                DataLabels = true,
                
            });
        }


        public void LoadChart()
        {
            chart.Series.Add(new LineSeries
            {
                Values = new ChartValues<ObservableValue>
                {
                    new ObservableValue(new Random().Next(-20, 20))
                }
            });
        }

        public void InsertValue(double value)
        {
            var r = new Random();
            Values.Insert(r.Next(0, Values.Count), new ObservableValue(value)); 
        }

        public void PopValue()
        {
            if (Values.Count > 1)
            {
                Values.RemoveAt(0);
            }
        }

        public void PushValue(double value)
        {    
            Values.Add(new ObservableValue(value));

            if (Values.Count >= MAX_VALUES_ON_GRAPH)
            {
                PopValue();
            }

            Thread.Sleep(100);
        }

        public void RandomiseValues()
        {
            var r = new Random();
            foreach (var observable in Values)
            {
                observable.Value = r.Next(-20, 20);
            }
        }
    }
}
