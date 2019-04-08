using System.Collections.Generic;

namespace InventoryTrackingSvc.Model
{
  public class EtsyDataContainer<T>
    {
        public int Count { get; set; }

        public List<T> Results { get; set; }
    }
   
}
