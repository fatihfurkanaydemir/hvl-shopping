using Common.EventBus.Events;
using Common.EventBus.RPCs;

namespace Common.EventBus.Interfaces;

public interface IEventBus
{
  void Publish<T>(T @event) where T : Event;
  void Subscribe<T, TH>() where T : Event
    where TH : IEventHandler<T>;

  public Task<TRPCResult> CallRP<TRPC, TRPCResult>(TRPC rpc) where TRPC : RPC;
  public void RegisterRPC<TRPC, TRPCResult>() where TRPC : RPC;
  public void RegisterRPCHandler<TRPC, TRPCHandler, TRPCResult>()
    where TRPC : RPC
    where TRPCHandler : IRPCHandler<TRPC, TRPCResult>;
}
