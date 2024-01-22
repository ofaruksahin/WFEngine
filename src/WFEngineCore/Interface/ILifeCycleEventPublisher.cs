using System;
using WFEngineCore.Models.LifeCycleEvents;

namespace WFEngineCore.Interface
{
    public interface ILifeCycleEventPublisher : IBackgroundTask
    {
        void PublishNotification(LifeCycleEvent evt);
    }
}
