using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TradeCenterAdmin.ChangesPool.Abstractions;
using TradeCenterAdmin.ChangesPool.Events;
using TradeCenterAdmin.ChangesPool.Events.EventArgs;

namespace TradeCenterAdmin.ChangesPool
{
 
    public class ChangesPool
    {
        private List<ChangeEntry> Changes { get; set; } = new List<ChangeEntry>();
        public int CurrentChangesIndex = -1;
        public void Undo()
        {
            //MessageBox.Show(CurrentChangesIndex.ToString());
            if (CurrentChangesIndex > -1 && CurrentChangesIndex < Changes.Count)
            {
                Changes[CurrentChangesIndex].Undo();
                OnUndoing?.Invoke(new UndoEventArgs(Changes[CurrentChangesIndex].UndoText));
                CurrentChangesIndex--;
                
            }       
        }
        public void Redo()
        {
            //MessageBox.Show(CurrentChangesIndex.ToString());
            if (CurrentChangesIndex > -2 && CurrentChangesIndex < Changes.Count)
            {          
                if (CurrentChangesIndex+1 == Changes.Count)
                {
                    return;
                }              
                Changes[CurrentChangesIndex+1].Redo();
                
                CurrentChangesIndex++;
                OnRedoing?.Invoke(new RedoEventArgs(Changes[CurrentChangesIndex].RedoText));
            }           
        }

        public void UndoMany(int times)
        {
            for (int i=0; i < times; i++)
            {
                Undo();
            }
        }
        public void RedoMany(int times)
        {
            for (int i = 0; i < times; i++)
            {
                Redo();
            }
        }
        public void AddEntry(ChangeEntry entry)
        {        
            if (CurrentChangesIndex + 1 < Changes.Count)
            {
                Changes.RemoveRange(CurrentChangesIndex + 1, Changes.Count -1-CurrentChangesIndex);
            }
            OnEntryAdded?.Invoke(new EntryAddedEventArgs());
            Changes.Add(entry);
            CurrentChangesIndex = Changes.Count - 1;
          
            //MessageBox.Show(CurrentChangesIndex.ToString());
        }
        public List<ChangeEntry> GetPossibleUndoActions()
        {
          
            return new List<ChangeEntry>(Changes.Take(CurrentChangesIndex + 1));
        }
        public List<ChangeEntry> GetPossibleRedoActions()
        {
           
            return new List<ChangeEntry>(Changes.Skip(CurrentChangesIndex+1));
        }
       
        public event UndoEventHandler OnUndoing;
        public event RedoEventHandler OnRedoing;
        public event EntryAddedEventHandler OnEntryAdded;

    }
}
