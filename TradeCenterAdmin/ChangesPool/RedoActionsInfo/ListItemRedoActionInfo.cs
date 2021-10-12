using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeCenterAdmin.ChangesPool.RedoActionsInfo
{
    public class ListItemRedoActionInfo<T>
    {
        /// <summary>
        /// Объект при возврате действия, сюда передаём клон объекта
        /// </summary>
        public T ObjectAfter;
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
