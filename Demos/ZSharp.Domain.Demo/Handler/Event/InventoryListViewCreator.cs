using ZSharp.Framework.Domain;

namespace ZSharp.Domain.Demo
{
    public class InventoryListViewCreator :
        IEventHandler<InventoryItemCreated>,
        IEventHandler<InventoryItemRenamed>,
        IEventHandler<InventoryItemDeactivated>
    {
        public void Handle(InventoryItemCreated message)
        {
            MemoryDatabase.list.Add(new InventoryItemListDto(message.SourceId, message.Name));
        }

        public void Handle(InventoryItemRenamed message)
        {
            var item = MemoryDatabase.list.Find(x => x.Id == message.SourceId);
            item.Name = message.NewName;
        }

        public void Handle(InventoryItemDeactivated message)
        {
            MemoryDatabase.list.RemoveAll(x => x.Id == message.SourceId);
        }
    }
}
