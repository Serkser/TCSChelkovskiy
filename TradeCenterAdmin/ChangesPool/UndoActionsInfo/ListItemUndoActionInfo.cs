using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeCenterAdmin.ChangesPool.UndoActionsInfo
{
    public class ListItemUndoActionInfo<T>
    {
        /// <summary>
        /// Объект при отмене действия, сюда передаём клон объекта
        /// </summary>
        public T ObjectAfter { get; set; }
        public List<Action> Callbacks { get; set; } = new List<Action>();
        public void SelectObjects(T objAfter, List<Action> callbacks = null)
        {
            ObjectAfter = objAfter;

            if (callbacks != null)
            {
                Callbacks = callbacks;
            }
        }
    }
}
