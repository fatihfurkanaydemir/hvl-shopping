using Common.EventBus.RPCs;

namespace Common.ApplicationRPCs;

public class TestRPC: RPC<string>
{
  public string message { get; set; }
}
