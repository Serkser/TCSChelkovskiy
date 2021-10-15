using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TradeCenterAdmin.ChangesPool.RedoActionsInfo
{
    public class ObjectRedoActionInfo
    {
        /// <summary>
        /// Объект при возврате действия, сюда передаём клон объекта
        /// </summary>
        public object ObjectAfter;      
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
