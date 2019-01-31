using System.Collections.Generic;

namespace Emlin
{
    public abstract class BaseSubject
    {
        private List<BaseObserver> observers = new List<BaseObserver>();

        public void Attach(BaseObserver observer)
        {
            observers.Add(observer);
        }

        public void Detach(BaseObserver observer)
        {
            observers.Remove(observer);
        }

        public void Notify()
        {
            foreach (BaseObserver observer in observers)
            {
                observer.UpdateView();
            }
        }
    }
}
