using NavigationMap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TradeCenterAdmin.ChangesPool.Abstractions;
using TradeCenterAdmin.ChangesPool.ActionPlaceInfo;

namespace TradeCenterAdmin.ChangesPool.Entries
{
    /// <summary>
    /// Класс предназначен для совершения действий в листе или в массиве с объектом по индексу.
    /// Для совершения действий в листе или массиве при помощи их методов нужно использовать класс ListChangeEntry
    /// 
    /// Параметр D означает тип объекта
    /// </summary>
    public class ListItemChangeEntry<D> : ChangeEntry
    {
        public ListItemChangeEntry(ListItemActionPlaceInfo<D> actionPlaceInfo)
        {
            ActionPlaceInfo = actionPlaceInfo;
        }
        public ListItemChangeEntry() { }

        public ListItemActionPlaceInfo<D> ActionPlaceInfo { get; set; }
        public override void Redo()
        {
            AreaPoint pointBefore = ActionPlaceInfo.ActionPlace[ActionPlaceInfo.Index] as AreaPoint;
            ActionPlaceInfo.ActionPlace[ActionPlaceInfo.Index] = ActionPlaceInfo.RedoActionInfo.ObjectAfter;

         
            foreach (var callback in ActionPlaceInfo.UndoActionInfo.Callbacks)
            {
                callback?.Invoke();
            }
        }

        public override void Undo()
        {
            AreaPoint pointBefore = ActionPlaceInfo.ActionPlace[ActionPlaceInfo.Index] as AreaPoint;
            ActionPlaceInfo.ActionPlace[ActionPlaceInfo.Index] = ActionPlaceInfo.UndoActionInfo.ObjectAfter;

         
            foreach (var callback in ActionPlaceInfo.UndoActionInfo.Callbacks)
            {
                callback?.Invoke();
            }
        }
    }
}
