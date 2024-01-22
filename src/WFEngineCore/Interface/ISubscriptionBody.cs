namespace WFEngineCore.Interface
{
    public interface ISubscriptionBody : IStepBody
    {
        object EventData { get; set; }        
    }
}
