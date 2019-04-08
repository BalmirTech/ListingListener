namespace InventoryTrackingSvc.Model
{
  public class Listing
  {
    public int Listing_id { get; set; }

    //active
    //(removed|edit|unavailable)
    //https://www.etsy.com/developers/documentation/reference/listing#section_listing_states
    public string State { get; set; }

    public string Title { get; set; }
  }
    
}
