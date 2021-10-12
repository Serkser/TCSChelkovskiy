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
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="D">Тип объекта для массива или листа</typeparam>
    public class ListItemActionPlaceInfo<D> 
    {
        public IList<D> ActionPlace { get; set; }
        public int Index { get; set; }
        public ListItemUndoActionInfo<D> UndoActionInfo { get; set; } = new ListItemUndoActionInfo<D>();
        public ListItemRedoActionInfo<D> RedoActionInfo { get; set; } = new ListItemRedoActionInfo<D>();
    }
}
