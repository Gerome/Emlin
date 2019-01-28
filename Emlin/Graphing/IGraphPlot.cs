namespace TestGraphing
{
    public interface IGraphPlot
    {

        void LoadChart();
        void PushValue(double value);
        void InsertValue(double value);
        void PopValue();
        void RandomiseValues();
    }
}
