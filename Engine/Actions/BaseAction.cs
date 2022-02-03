using System;
using Engine.Models;

namespace Engine.Actions
{
    public abstract class BaseAction
    {
        protected readonly Item _itemInUse;
        public event EventHandler<string> OnActionPerformed;

        protected BaseAction(Item itemInUse)
        {
            _itemInUse = itemInUse;
        }

        protected void ReportResult(string result)
        {
            OnActionPerformed?.Invoke(this, result);
        }
    }
}