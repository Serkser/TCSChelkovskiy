using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeCenterAdmin.ChangesPool.Abstractions
{
    public abstract class ChangeEntry
    {
        public abstract void Undo();
        public abstract void Redo();

        public string UndoText { get; set; }
        public string RedoText { get; set; }

    }
}
