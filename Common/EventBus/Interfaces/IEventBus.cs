using Common.EventBus.Events;
using Common.EventBus.RPCs;

namespace Common.EventBus.Interfaces;

public interface IEventBus
{
  void Publish<T>(T @event) where T : Event;
  void Subscribe<T, TH>() where T : Event
    where TH : IEventHandler<T>;

  public Task<TRPCResult> CallRP<TRPCResult>(RPC<TRPCResult> rpc);
  public void RegisterRPC<TRPC, TRPCResult>() where TRPC : RPC<TRPCResult>;
  public void RegisterRPCHandler<TRPC, TRPCHandler, TRPCResult>()
    where TRPC : RPC<TRPCResult>
    where TRPCHandler : IRPCHandler<TRPC, TRPCResult>;
}
