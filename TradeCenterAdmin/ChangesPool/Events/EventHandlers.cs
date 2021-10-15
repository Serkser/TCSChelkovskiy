using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeCenterAdmin.ChangesPool.Events.EventArgs;

namespace TradeCenterAdmin.ChangesPool.Events
{
   public delegate void UndoEventHandler(UndoEventArgs args);

   public delegate void RedoEventHandler(RedoEventArgs args);

   public delegate void EntryAddedEventHandler(EntryAddedEventArgs args);
}
