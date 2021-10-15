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
    /// Класс предназначен для манипуляций со свойствами объекта, на который указывает ссылка
    /// </summary>
    public class ObjectChangeEntry : ChangeEntry
    {
        public ObjectChangeEntry()
        {
        }
        public ObjectChangeEntry(ObjectActionPlaceInfo actionPlaceInfo)
        {
            ActionPlaceInfo = actionPlaceInfo;
        }
        public ObjectActionPlaceInfo ActionPlaceInfo { get; set; }

        public override void Redo()
        {      
           Type objectType = ActionPlaceInfo.Object.GetType();
           var objectTypeProps =  objectType.GetProperties(System.Reflection.BindingFlags.Public | 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var objectTypeFields = objectType.GetFields(System.Reflection.BindingFlags.Public |
             System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            Type newObjectType = ActionPlaceInfo.RedoActionInfo.ObjectAfter.GetType();
            var newObjectTypeProps = newObjectType.GetProperties(System.Reflection.BindingFlags.Public |
                 System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var newObjectTypeFields = newObjectType.GetFields(System.Reflection.BindingFlags.Public |
              System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            for (int i=0;i< objectTypeProps.Length; i++)
            {
                for (int j=0;j< newObjectTypeProps.Length; j++)
                {
                    if (objectTypeProps[i].Name == newObjectTypeProps[j].Name)
                    {
                        var newValue = newObjectTypeProps[j].GetValue(ActionPlaceInfo.RedoActionInfo.ObjectAfter);
                        if (objectTypeProps[i].CanWrite)
                        {
                            objectTypeProps[i].SetValue(ActionPlaceInfo.Object, newValue);
                        }
                       
                        break;
                    }          
                }
            }

            for (int i = 0; i < objectTypeFields.Length; i++)
            {
                for (int j = 0; j < newObjectTypeFields.Length; j++)
                {
                    if (objectTypeFields[i].Name == newObjectTypeFields[j].Name)
                    {
                        var newValue = newObjectTypeFields[j].GetValue(ActionPlaceInfo.RedoActionInfo.ObjectAfter);
                        objectTypeFields[i].SetValue(ActionPlaceInfo.Object, newValue);
                        break;
                    }
                }
            }

            foreach (var callback in ActionPlaceInfo.RedoActionInfo.Callbacks)
            {
                callback?.Invoke();
            }
        }
        public override void Undo()
        {
            Type objectType = ActionPlaceInfo.Object.GetType();
            var objectTypeProps = objectType.GetProperties(System.Reflection.BindingFlags.Public |
                 System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var objectTypeFields = objectType.GetFields(System.Reflection.BindingFlags.Public |
         System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);



            Type newObjectType = ActionPlaceInfo.UndoActionInfo.ObjectAfter.GetType();
            var newObjectTypeProps = newObjectType.GetProperties(System.Reflection.BindingFlags.Public |
                 System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var newObjectTypeFields = newObjectType.GetFields(System.Reflection.BindingFlags.Public |
         System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            for (int i = 0; i < objectTypeProps.Length; i++)
            {
                for (int j = 0; j < newObjectTypeProps.Length; j++)
                {
                    if (objectTypeProps[i].Name == newObjectTypeProps[j].Name)
                    {                       
                        var newValue = newObjectTypeProps[j].GetValue(ActionPlaceInfo.UndoActionInfo.ObjectAfter);
                        if (objectTypeProps[i].CanWrite)
                        {
                            objectTypeProps[i].SetValue(ActionPlaceInfo.Object, newValue);
                        }
                     
                        break;
                    }
                }
            }

            for (int i = 0; i < objectTypeFields.Length; i++)
            {
                for (int j = 0; j < newObjectTypeFields.Length; j++)
                {
                    if (objectTypeFields[i].Name == newObjectTypeFields[j].Name)
                    {
                        var newValue = newObjectTypeFields[j].GetValue(ActionPlaceInfo.UndoActionInfo.ObjectAfter);
                        objectTypeFields[i].SetValue(ActionPlaceInfo.Object, newValue);
                        break;
                    }
                }
            }

            foreach (var callback in ActionPlaceInfo.UndoActionInfo.Callbacks)
            {
                callback?.Invoke();
            }
        }
    }
}
