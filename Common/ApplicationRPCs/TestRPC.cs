using Common.EventBus.RPCs;

namespace Common.ApplicationRPCs;

public class TestRPC: RPC
{
  public string message { get; set; }
}
