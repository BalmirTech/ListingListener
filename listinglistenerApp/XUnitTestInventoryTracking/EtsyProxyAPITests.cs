using System;
using Xunit;
using InventoryTrackingSvc;
using InventoryTrackingSvc.Model;
using System.Collections.Generic;
using InventoryTrackingSvc.Data;
using XUnitTestInventoryTracking.Support;

namespace XUnitTestInventoryTracking
{
  public class EtsyProxyAPITests
  {

    string baseURL = "https://openapi.etsy.com/v2";
    string etsyapikey = "";
    EtsyAPIProxy etsyAPI = null;


    Dictionary<string, int> testShops = null;
    
    public EtsyProxyAPITests()
    {
      this.etsyapikey = Helpers.EtsyAPIKey;

      this.etsyAPI = new EtsyAPIProxy(this.baseURL, this.etsyapikey);

      fillTestShops();
    }
    
    private void fillTestShops()
    {
      testShops = new Dictionary<string, int>();
      testShops.Add("A", 19099945);
      testShops.Add("B", 19099937);
      testShops.Add("C", 19120225);      
    }


    [Fact]
    public void GetEtsyShops()
    {
     
      int[] shopIds = new int[] { testShops["A"] , testShops["B"] };

      EtsyAPIProxy ec = this.etsyAPI;
      List<Shop> requestedShop = ec.GetShops(shopIds, 100, 0).Results;

      Assert.Equal(2, requestedShop.Count);
    }

    [Fact]
    public void GetEtsyShopListing()
    {
      int shopId = testShops["A"];

      EtsyAPIProxy ec = this.etsyAPI;
      List<Listing> requestedShop = ec.GetActiveListingsByShop(shopId, 100, 0).Results;

      Assert.True(requestedShop.Count > 0);
    }
    
    [Fact]
    public void GetEtsyShopListingAll()
    {      
      int shopId = testShops["C"];

      EtsyAPIProxy ec = this.etsyAPI;
      EtsyDataContainer<Listing> requestedShop = ec.GetActiveListingsByShop(shopId, limit:6);

      Assert.Equal<int>(requestedShop.Count, requestedShop.Results.Count);
    }

    [Fact]
    public void GetInvalidShopID()
    {
      int invalidShopID = int.MaxValue;

      EtsyAPIProxy ec = this.etsyAPI;

      EtsyDataContainer<Listing> requestedShop = ec.GetActiveListingsByShop(invalidShopID, 100, 0);

      Assert.Null(requestedShop);
    }

  }

}
