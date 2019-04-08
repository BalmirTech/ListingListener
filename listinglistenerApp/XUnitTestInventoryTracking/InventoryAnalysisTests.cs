using System;
using System.Linq;
using Xunit;
using InventoryTrackingSvc;
using InventoryTrackingSvc.Model;
using System.Collections.Generic;
using InventoryTrackingSvc.Data;
using System.IO;

namespace XUnitTestInventoryTracking
{
  public class InventoryAnalysisTests
  {

    //Supports Etsy API configuration
    string baseURL = "https://openapi.etsy.com/v2";
    string etsyapikey = null;
    EtsyAPIProxy etsyAPI;

    //Supports DB configuration
    ISyncDatabase syncDatabase;
    string rootDir = null;

    private Dictionary<string, int>fillTestShops()
    {
      Dictionary<string, int> testShops = new Dictionary<string, int>();
      testShops.Add("A", 19099945);
      testShops.Add("B", 19099937);
      testShops.Add("C", 19120225);

      return testShops;
    }


    public InventoryAnalysisTests()
    {
      this.rootDir = Support.Helpers.DBCache;
      this.etsyapikey = Support.Helpers.EtsyAPIKey;

      //Esty Proxy
      this.etsyAPI = new EtsyAPIProxy(this.baseURL, this.etsyapikey);

      //Database Proxy
      this.syncDatabase = new SyncDatabaseFS(rootDir);
    }
        
    [Fact]
    public void GetAnalysis()
    {
      int shopId = 19120225;
      InventoryAnalytics ia = new InventoryAnalytics(this.syncDatabase, this.etsyAPI);

      InventoryStatus invStatus = ia.GetInventoryStatus(shopId);

      Assert.True(invStatus.Added.Count > 0);
    }

    [Fact]
    public void GetAnalysisForMultiple()
    {
      if (Directory.Exists(this.rootDir))
      {
        Directory.Delete(this.rootDir, true);
        Directory.CreateDirectory(this.rootDir);
      }

      List<int> shopIds = fillTestShops().Values.ToList();

      InventoryAnalytics ia = new InventoryAnalytics(this.syncDatabase, this.etsyAPI);
      List<InventoryStatus> invStatus = ia.GetInventoryStatus(shopIds);

      bool res = invStatus.All((status) =>
      {
        return (status.Added.Count > 0);
      });

      Assert.True(res);
    }

  }

}
