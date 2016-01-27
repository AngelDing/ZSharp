using System;
using System.Collections.Generic;

namespace ZSharp.Domain.Demo
{
    public static class MemoryDatabase
    {
        public static Dictionary<Guid, InventoryItemDetailsDto> details = new Dictionary<Guid, InventoryItemDetailsDto>();

        public static List<InventoryItemListDto> list = new List<InventoryItemListDto>();
    }
}
