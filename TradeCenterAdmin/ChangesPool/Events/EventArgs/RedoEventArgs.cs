using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeCenterAdmin.ChangesPool.Events.EventArgs
{
    public class RedoEventArgs
    {
        public string Message { get; }
        public RedoEventArgs(string mes)
        {
            Message = mes;
        }
    }
}
