using System;
using System.Collections.Generic;

namespace listinglistenerApp.Models
{

  //TODO: Refactor into separate classes
  public class Listing
  {
    public int Listing_id { get; set; }
    //active
    //(removed|edit|unavailable)
    //https://www.etsy.com/developers/documentation/reference/listing#section_listing_states
    public string State { get; set; }

    public string Title { get; set; }
  }

  public class Shop
  {
    public int Shop_id { get; set; }

    public string Shop_name { get; set; }
  }

  public class InventoryStatus
  {
    public bool IsShopIdValid { get; set; }

    public DateTime AsOfDate { get; set; }

    public Shop Shop { get; set; }

    public List<Listing> Added { get; set; }

    public List<Listing> Removed { get; set; }
  }

}
