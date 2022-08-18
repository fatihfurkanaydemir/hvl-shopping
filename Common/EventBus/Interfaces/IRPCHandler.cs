using Common.EventBus.RPCs;

namespace Common.EventBus.Interfaces;

public interface IRPCHandler<in TRPC, TRPCResult> where TRPC : RPC<TRPCResult>
{
  Task<TRPCResult> Handle(TRPC rpc);
}
