using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeCenterAdmin.ChangesPool.RedoActionsInfo
{
    public class ListRedoActionInfo
    {
        public string MethodName { get; set; }
        public List<object> MethodArguments { get; set; } = new List<object>();
        public void AddMethodArgument(ICloneable obj)
        {
            var clone = obj.Clone();
            MethodArguments.Add(clone);
        }
        public void AddMethodArgument<T>(T obj) where T : struct
        {
            MethodArguments.Add(obj);
        }
        public List<Action> Callbacks { get; set; } = new List<Action>();

        public void MakeRemoveFromListMethod(object parameter, List<Action> callbacks = null)
        {
            MethodName = "Remove";
            MethodArguments = new List<object>() { parameter };
            if (callbacks != null)
            {
                Callbacks = callbacks;
            }
        }
        public void MakeAddToListMethod(object parameter, List<Action> callbacks = null)
        {
            MethodName = "Add";
            MethodArguments = new List<object>() { parameter };
            if (callbacks != null)
            {
                Callbacks = callbacks;
            }
        }

        public void MakeInsertToListMethod(int index, object obj, List<Action> callbacks = null)
        {
            MethodName = "Insert";
            MethodArguments = new List<object>() { index, obj };
            if (callbacks != null)
            {
                Callbacks = callbacks;
            }
        }
    }
}
