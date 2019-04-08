using System.Collections.Generic;
using InventoryTrackingSvc.Model;

namespace InventoryTrackingSvc.Data
{
  public interface ISyncDatabase
    {
      List<ShopInventory> ReadShopInventory(IEnumerable<int> shopIds);

      ShopInventory ReadShopInventory(int shopIds);

      void PersistShopInventory(Shop shop, List<Listing> listings);
    }

}
