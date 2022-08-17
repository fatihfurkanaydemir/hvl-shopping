using Common.ApplicationRPCs;
using Common.EventBus.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountService.Application.Features.Test;

public class TestRPCHandler: IRPCHandler<TestRPC, string>
{
  public async Task<string> Handle(TestRPC rpc)
  {
    return rpc.message + " Completed!!" + DateTime.UtcNow.ToString();
  }
}
