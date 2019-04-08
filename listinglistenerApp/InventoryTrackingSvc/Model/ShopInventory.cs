using System.Collections.Generic;

namespace InventoryTrackingSvc.Model
{
  public class ShopInventory
    {

        public Shop Shop { get; set; }

        public List<Listing> Listings { get; set; }

    }
    

}
