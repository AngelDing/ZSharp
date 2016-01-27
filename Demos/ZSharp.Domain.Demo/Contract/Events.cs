using ZSharp.Framework.Domain;
using System;

namespace ZSharp.Domain.Demo
{
    public class InventoryItemDeactivated : DomainEvent
    {
        public InventoryItemDeactivated(Guid id) 
        {
        }
    }

    public class InventoryItemCreated : DomainEvent
    {
        public readonly string Name;
        public InventoryItemCreated(Guid id, string name) 
        {
            Name = name;
        }
    }

    public class InventoryItemRenamed : DomainEvent
    {
        public readonly string NewName;
        public InventoryItemRenamed(Guid id, string newName) 
        {
            NewName = newName;
        }
    }

    public class ItemsCheckedInToInventory : DomainEvent
    {
        public readonly int Count;
        public ItemsCheckedInToInventory(Guid id, int count) 
        {
            Count = count;
        }
    }

    public class ItemsRemovedFromInventory : DomainEvent
    {
        public readonly int Count;
        public ItemsRemovedFromInventory(Guid id, int count)
        {
            Count = count;
        }
    }
}
