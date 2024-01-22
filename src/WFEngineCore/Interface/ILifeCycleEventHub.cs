using System;
using System.Threading.Tasks;
using WFEngineCore.Models.LifeCycleEvents;

namespace WFEngineCore.Interface
{
    public interface ILifeCycleEventHub
    {
        Task PublishNotification(LifeCycleEvent evt);
        void Subscribe(Action<LifeCycleEvent> action);
        Task Start();
        Task Stop();
    }
}
