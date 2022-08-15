using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.EventBus.Events;

public abstract class Event
{
  public DateTime TimeStamp { get; protected set; }

  public Event()
  {
    TimeStamp = DateTime.Now; 
  }
}
