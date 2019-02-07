namespace Emlin
{
    public interface ITimerInterface
    {
        bool Enabled { get; set; }

        void Dispose();

        void Start();

        void Stop();      
    }
}
