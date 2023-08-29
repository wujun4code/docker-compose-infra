namespace BetterCoding.MessagePubSubCenter.Entity.ContinuousSlicingTask
{
    public interface ICSTMessage
    {
        string Model { get; set; }
        DateTime? StartDate { get; set; }
    }
}
