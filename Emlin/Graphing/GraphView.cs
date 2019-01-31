using System.Windows.Forms;
using System;
using System.Threading;
using Emlin;

namespace TestGraphing
{
    public partial class GraphView : UserControl ,  BaseObserver
    {
        private IGraphPlot graph;
        private Random random = new Random();
        private HealthSubject healthSubject;

        private const int NUMBER_OF_READINGS = 1000;

        public GraphView()
        {
            InitializeComponent();
            cartesianChart1.DisableAnimations = true;
            cartesianChart1.DataTooltip = null;
            graph = new LiveChartsPlot(cartesianChart1);
        }

        public void SetHealthSubject(HealthSubject healthSubject)
        {
            this.healthSubject = healthSubject;
        }

        private void GetSetpointNewThread()
        {
            Thread thread = new Thread(() =>
            { // Thread begins here
                for (int i = 0; i < NUMBER_OF_READINGS; i++)
                {
                    //graph.PushValue(deviceReader.GetSetpoint());
                }
            })
            {
                IsBackground = true
            }; // Thread ends here
            thread.Start();
        }

        #region Button listeners

        private void PushButtonOnClick(object sender, EventArgs e)
        {
            graph.PushValue(random.Next(-20, 20));
        }

        private void InsertButtonOnClick(object sender, EventArgs e)
        {
            graph.InsertValue(random.Next(-20, 20));
        }

        private void PopButtonClick(object sender, EventArgs e)
        {
            graph.PopValue();   
        }

        public void UpdateView()
        {
            graph.PushValue(healthSubject.GetValue());
        }


        #endregion

    }
}
