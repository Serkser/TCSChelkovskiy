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
    public class ObjectActionPlaceInfo
    {
        /// <summary>
        /// Ссылка на оригинальный объект
        /// </summary>
        public object Object { get; set; }
        public ObjectUndoActionInfo UndoActionInfo { get; set; } = new ObjectUndoActionInfo();
        public ObjectRedoActionInfo RedoActionInfo { get; set; } = new ObjectRedoActionInfo();
    }
}
