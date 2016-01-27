using ZSharp.Framework.Domain;
using System;

namespace ZSharp.Domain.Demo
{
    public class InvenotryItemDetailViewCreator :
        IEventHandler<InventoryItemCreated>,
        IEventHandler<InventoryItemDeactivated>,
        IEventHandler<InventoryItemRenamed>,
        IEventHandler<ItemsRemovedFromInventory>,
        IEventHandler<ItemsCheckedInToInventory>
    {
        public void Handle(InventoryItemCreated message)
        {
            MemoryDatabase.details.Add(message.SourceId, new InventoryItemDetailsDto(message.SourceId, message.Name, 0, 0));
        }

        public void Handle(InventoryItemRenamed message)
        {
            InventoryItemDetailsDto d = GetDetailsItem(message.SourceId);
            d.Name = message.NewName;
            d.Version = message.Version;
        }

        private InventoryItemDetailsDto GetDetailsItem(Guid id)
        {
            InventoryItemDetailsDto d;

            if (!MemoryDatabase.details.TryGetValue(id, out d))
            {
                throw new InvalidOperationException("did not find the original inventory this shouldnt happen");
            }

            return d;
        }

        public void Handle(ItemsRemovedFromInventory message)
        {
            InventoryItemDetailsDto d = GetDetailsItem(message.SourceId);
            d.CurrentCount -= message.Count;
            d.Version = message.Version;
        }

        public void Handle(ItemsCheckedInToInventory message)
        {
            InventoryItemDetailsDto d = GetDetailsItem(message.SourceId);
            d.CurrentCount += message.Count;
            d.Version = message.Version;
        }

        public void Handle(InventoryItemDeactivated message)
        {
            MemoryDatabase.details.Remove(message.SourceId);
        }
    }
}
