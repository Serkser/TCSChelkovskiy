using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeCenterAdmin.ChangesPool.Events.EventArgs
{
    public class UndoEventArgs
    {
        public string Message { get; }
        public UndoEventArgs(string mes)
        {
            Message = mes;
        }
    }
}
