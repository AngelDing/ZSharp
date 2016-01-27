using ZSharp.Framework.Domain;

namespace ZSharp.Domain.Demo
{
    public class InventoryCommandHandler:
        ICommandHandler<CreateInventoryItem>,
        ICommandHandler<DeactivateInventoryItem>,
        ICommandHandler<RemoveItemsFromInventory>,
        ICommandHandler<CheckInItemsToInventory>,
        ICommandHandler<RenameInventoryItem>
    {
        private readonly IEventStore<InventoryItem> repository;

        public InventoryCommandHandler(IEventStore<InventoryItem> repository)
        {
            this.repository = repository;
        }

        public void Handle(CreateInventoryItem message)
        {
            var item = new InventoryItem(message.InventoryItemId, message.Name);
            item.CorrelationId = message.Id;
            item.Topic = Constants.ApplicationRuntime.DefaultEventTopic;
            repository.Save(item);
        }

        public void Handle(DeactivateInventoryItem message)
        {
            var item = repository.Get(message.InventoryItemId);
            item.Deactivate();
            item.CorrelationId = message.Id;
            repository.Save(item);
        }

        public void Handle(RemoveItemsFromInventory message)
        {
            var item = repository.Get(message.InventoryItemId);
            item.Remove(message.Count);
            item.CorrelationId = message.Id;
            repository.Save(item);
        }

        public void Handle(CheckInItemsToInventory message)
        {
            var item = repository.Get(message.InventoryItemId);
            item.CheckIn(message.Count);
            item.CorrelationId = message.Id;
            repository.Save(item);
        }

        public void Handle(RenameInventoryItem message)
        {
            var item = repository.Get(message.InventoryItemId);
            item.ChangeName(message.NewName);
            item.CorrelationId = message.Id;
            repository.Save(item);
        }
    }
}
