using System;
using Xunit;
using InventoryTrackingSvc;
using InventoryTrackingSvc.Model;
using System.Collections.Generic;
using InventoryTrackingSvc.Data;

namespace XUnitTestInventoryTracking
{
  public class FSDatabaseTest
  {

    //Supports Etsy API configuration
    string baseURL = "https://openapi.etsy.com/v2";
    string estyapikey = null;
    EtsyAPIProxy etsyAPI;

    //Supports DB configuration
    ISyncDatabase syncDatabase;
    string rootDir = null;

    public FSDatabaseTest()
    {
      this.rootDir = Support.Helpers.DBCache;

      //Esty Proxy
      this.etsyAPI = new EtsyAPIProxy(this.baseURL, Support.Helpers.EtsyAPIKey);

      //Database Proxy
      this.syncDatabase = new SyncDatabaseFS(rootDir);
    }
        
    /// <summary>
    /// Test persisting inventory and retrieving persisted inventory for specified shop.
    /// </summary>
    [Fact]
    public void PersistInventory()
    {
      int shopId = 19099945;

      EtsyAPIProxy ec = this.etsyAPI;

      List<Shop> requestedShops = ec.GetShops(new int[] { shopId }, 100, 0).Results;
      Shop shop = requestedShops[0];


      List<Listing> reqListing = ec.GetActiveListingsByShop(shopId, 100, 0).Results;
      int listingCount = reqListing.Count;
      this.syncDatabase.PersistShopInventory(shop, reqListing);


      List<ShopInventory> shopInventorys = this.syncDatabase.ReadShopInventory(new int[] { shopId });
      int persistedListingCount = shopInventorys[0].Listings.Count;      
      

      Assert.Equal(listingCount, persistedListingCount);
    }

  }

}
