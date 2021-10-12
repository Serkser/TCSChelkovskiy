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
    /// Класс предназначен для объединения различных условий в одно
    /// К примеру, происходит комплексное действие и его нельзя описать одним объектом
    /// </summary>
    public class ComplexChangeEntry : ChangeEntry
    {
        public ComplexChangeEntry(List<ChangeEntry> changeEntries)
        {
            Conditions = changeEntries;
        }
        public ComplexChangeEntry() { }
        public List<ChangeEntry> Conditions { get; set; }
        public override void Undo()
        {
            foreach (var cond in Conditions)
            {
                cond.Undo();
            }
        }
        public override void Redo()
        {
            foreach (var cond in Conditions)
            {
                cond.Redo();
            }
        }
    }
}
