using System.Windows.Forms;
using System;
using System.Threading;

namespace TestGraphing
{
    public partial class GraphView : UserControl
    {
        private IGraphPlot graph;
        private Random random = new Random();
        //private DeviceReader deviceReader;

        private const int NUMBER_OF_READINGS = 1000;

        public GraphView()
        {
            InitializeComponent();
            graph = new LiveChartsPlot(cartesianChart1);
           
        }

        internal void SetDeviceReader()//DeviceReader deviceReader)
        {
           // this.deviceReader = deviceReader;
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


        #endregion

    }
}
