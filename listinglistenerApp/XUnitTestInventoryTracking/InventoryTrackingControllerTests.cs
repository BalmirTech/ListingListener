using System;
using System.Linq;
using Xunit;
using InventoryTrackingSvc;
using InventoryTrackingSvc.Model;
using System.Collections.Generic;
using InventoryTrackingSvc.Data;
using System.IO;

using InventoryTrackingSvc.Controllers;

namespace XUnitTestInventoryTracking
{
  public class InventoryTrackingControllerTests
  {

    //Supports Etsy API configuration
    string baseURL = "https://openapi.etsy.com/v2";
    string estyapikey = null;
    EtsyAPIProxy etsyAPI;

    //Supports DB configuration
    ISyncDatabase syncDatabase;
    string rootDir;

    SyncController syncController;
    

    public InventoryTrackingControllerTests()
    {
      this.estyapikey = Support.Helpers.EtsyAPIKey;

      this.rootDir = Support.Helpers.DBCache;

      //Esty Proxy
      this.etsyAPI = new EtsyAPIProxy(this.baseURL, this.estyapikey);
      //Database Proxy
      this.syncDatabase = new SyncDatabaseFS(rootDir);
      
      //Analytic Generator
      InventoryAnalytics ia = new InventoryAnalytics(this.syncDatabase, this.etsyAPI);
      this.syncController = new SyncController(ia);
    }



    [Fact]
    public void GetAnalysisStatus()
    {
      clearCache();

      List<int> ids = fillTestShops().Values.ToList();

      List<InventoryStatus> invStatus = this.syncController.Post(ids);

      bool res = invStatus.All((status) =>
      {
        return (status.IsShopIdValid == false || status.Added.Count > 0);
      });

      Assert.True(res);
    }

    [Fact]
    public void MakeEmptyAnalysisRequest()
    {
      clearCache();

      List<int> emptyIds = new List<int>();
      List<InventoryStatus> invStatus = this.syncController.Post(emptyIds);

      bool res = invStatus.All((status) =>
      {
        return (status.Added.Count > 0);
      });

      Assert.True(res);
    }




    private void clearCache()
    {
      if (Directory.Exists(this.rootDir))
      {
        Directory.Delete(this.rootDir, true);
        Directory.CreateDirectory(this.rootDir);
      }

    }

    private Dictionary<string, int> fillTestShops()
    {
      Dictionary<string, int> testShops = new Dictionary<string, int>();
      testShops.Add("A", 19099945);
      testShops.Add("B", 19099937);
      testShops.Add("C", 19120225);
      testShops.Add("D", int.MaxValue);

      return testShops;
    }


  }

}
