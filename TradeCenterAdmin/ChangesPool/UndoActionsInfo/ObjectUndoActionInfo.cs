using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TradeCenterAdmin.ChangesPool.UndoActionsInfo
{
    public class ObjectUndoActionInfo
    {   
        /// <summary>
        /// Объект при отмене действия, сюда передаём клон объекта
        /// </summary>
        public object ObjectAfter { get; set; }
        public List<Action> Callbacks { get; set; } = new List<Action>();
        public void SelectObjects(object objAfter, List<Action> callbacks = null)
        {
            ObjectAfter = objAfter;
          
            if (callbacks != null)
            {
                Callbacks = callbacks;
            }
        }

    }
}
