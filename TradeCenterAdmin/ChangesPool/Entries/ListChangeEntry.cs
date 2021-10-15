using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeCenterAdmin.ChangesPool.Abstractions;
using TradeCenterAdmin.ChangesPool.ActionPlaceInfo;
using TradeCenterAdmin.ChangesPool.Enums;
using TradeCenterAdmin.Storage;

namespace TradeCenterAdmin.ChangesPool.Entries
{
    /// <summary>
    /// Класс предназначен для совершения действий в листе или в массиве при помощи их методов.
    /// Для манипуляций с объектом по индексу нужно использовать класс ListItemChangeEntry
    /// </summary>
    public class ListChangeEntry : ChangeEntry
    {
        public ListChangeEntry(ListActionPlaceInfo actionPlaceInfo)
        {
            ActionPlaceInfo = actionPlaceInfo;
        }
        public ListChangeEntry() { }

        public ListActionPlaceInfo ActionPlaceInfo { get; set; }

        public override void Undo()
        {
            string methodName = ActionPlaceInfo.UndoActionInfo.MethodName;
            if (ActionPlaceInfo.ActionPlace == null){ return; }
            Type type = ActionPlaceInfo.ActionPlace.GetType();
            var methodInfo = type.GetMethod(methodName);
            if (methodInfo != null)
            {
                methodInfo.Invoke(ActionPlaceInfo.ActionPlace, ActionPlaceInfo.UndoActionInfo.MethodArguments.ToArray());
            }
            else
            {
                throw new Exception("Данного метода нет в месте действия");
            }

            foreach (var callback in ActionPlaceInfo.UndoActionInfo.Callbacks)
            {
                callback?.Invoke();
            }


        }
        public override void Redo()
        {
            string methodName = ActionPlaceInfo.RedoActionInfo.MethodName;
            Type type = ActionPlaceInfo.ActionPlace.GetType();
            var methodInfo = type.GetMethod(methodName);

            if (methodInfo != null)
            {
                methodInfo.Invoke(ActionPlaceInfo.ActionPlace, ActionPlaceInfo.RedoActionInfo.MethodArguments.ToArray());
            }
            else
            {
                throw new Exception("Данного метода нет в месте действия");
            }

            foreach (var callback in ActionPlaceInfo.RedoActionInfo.Callbacks)
            {
                callback?.Invoke();
            }
        }
    }
}
