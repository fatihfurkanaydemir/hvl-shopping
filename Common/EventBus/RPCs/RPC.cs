namespace Common.EventBus.RPCs;

public abstract class RPC
{
  public DateTime TimeStamp { get; protected set; }

  public RPC()
  {
    TimeStamp = DateTime.Now; 
  }
}
