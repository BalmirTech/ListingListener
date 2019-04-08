using System;
using System.Collections.Generic;

namespace InventoryTrackingSvc.Model
{
  public class InventoryStatus
  {   
      public bool IsShopIdValid = true;    

      public DateTime AsOfDate { get; set; }

      public Shop Shop { get; set; }

      public List<Listing> Added { get; set; }

      public List<Listing> Removed { get; set; }
  }
    
}
