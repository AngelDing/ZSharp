using System;
using System.Collections.Generic;

namespace ZSharp.Domain.Demo
{
    public interface IReadModelFacade
    {
        IEnumerable<InventoryItemListDto> GetInventoryItems();

        InventoryItemDetailsDto GetInventoryItemDetails(Guid id);
    }

    public class ReadModelFacade : IReadModelFacade
    {
        public IEnumerable<InventoryItemListDto> GetInventoryItems()
        {
            return MemoryDatabase.list;
        }

        public InventoryItemDetailsDto GetInventoryItemDetails(Guid id)
        {
            return MemoryDatabase.details[id];
        }
    }
}
