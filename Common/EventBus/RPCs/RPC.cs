namespace Common.EventBus.RPCs;

public abstract class RPC<TRPCResult>: IBaseRPC
{
  public DateTime TimeStamp { get; protected set; }

  public RPC()
  {
    TimeStamp = DateTime.Now; 
  }
}
