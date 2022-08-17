using Common.EventBus.RPCs;

namespace Common.EventBus.Interfaces;

public interface IRPCHandler<TRPC, TRPCResult> where TRPC : RPC
{
  Task<TRPCResult> Handle(TRPC rpc);
}
