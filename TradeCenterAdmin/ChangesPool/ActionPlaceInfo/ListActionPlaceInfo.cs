using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeCenterAdmin.ChangesPool.RedoActionsInfo;
using TradeCenterAdmin.ChangesPool.UndoActionsInfo;
using TradeCenterAdmin.Storage;

namespace TradeCenterAdmin.ChangesPool.ActionPlaceInfo
{
    public class ListActionPlaceInfo
    {
        public dynamic ActionPlace { get; set; } 
        public ListUndoActionInfo UndoActionInfo { get; set; } = new ListUndoActionInfo();
        public ListRedoActionInfo RedoActionInfo { get; set; } = new ListRedoActionInfo();
    }
}
