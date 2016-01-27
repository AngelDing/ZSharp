using ZSharp.Framework.Domain;
using System;

namespace ZSharp.Domain.Demo
{
    public class InventoryItem : EventSourced
    {
        private bool activated;
        private Guid id;

        public InventoryItem(Guid id) : base(id)
        {
            base.Handles<InventoryItemCreated>(this.OnItemCreated);
            base.Handles<InventoryItemDeactivated>(this.OnItemDeactivated);
        }

        private void OnItemDeactivated(InventoryItemDeactivated obj)
        {
            activated = false;
        }

        private void OnItemCreated(InventoryItemCreated e)
        {
            id = e.SourceId;
            activated = true;
        }

        public InventoryItem(Guid id, string name) : this(id)
        {
            Update(new InventoryItemCreated(id, name));
        }

        public void ChangeName(string newName)
        {
            if (string.IsNullOrEmpty(newName))
                throw new ArgumentException("newName");
            Update(new InventoryItemRenamed(id, newName));
        }

        public void Remove(int count)
        {
            if (count <= 0)
                throw new InvalidOperationException("cant remove negative count from inventory");
            Update(new ItemsRemovedFromInventory(id, count));
        }

        public void CheckIn(int count)
        {
            if (count <= 0)
            {
                throw new InvalidOperationException("must have a count greater than 0 to add to inventory");
            }
            Update(new ItemsCheckedInToInventory(id, count));
        }

        public void Deactivate()
        {
            if (!activated)
            {
                throw new InvalidOperationException("already deactivated");
            }
            Update(new InventoryItemDeactivated(id));
        }

        protected override void DoLoadFromSnapshot(ISnapshot snapshot)
        {
            throw new NotImplementedException();
        }

        protected override ISnapshot DoCreateSnapshot()
        {
            throw new NotImplementedException();
        }       
    }
}
