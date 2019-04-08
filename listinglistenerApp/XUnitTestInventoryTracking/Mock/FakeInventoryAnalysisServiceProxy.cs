using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using InventoryTrackingSvc.Data;
using listinglistenerApp.Data;
using listinglistenerApp.Models;
using System.IO;

namespace XUnitTestInventoryTracking.Mock
{
  public class FakeInventoryAnalysisServiceProxy : InventoryAnalyisServiceProxy
  {

    List<int> OKshopIDs = new List<int>();

    public FakeInventoryAnalysisServiceProxy(string apiBaseURL) : base(apiBaseURL)
    {     
      OKshopIDs.AddRange(new int[] { 19099937 });
    }

    public override List<InventoryStatus> GetAnalysis(IEnumerable<int> shopIDs, out HttpStatusCode statusCode)
    {

      var inv = new InventoryStatus()
      {
        Added = new List<Listing>(),
        Removed = new List<Listing>(),
        AsOfDate = DateTime.Now,
        IsShopIdValid = true,
        Shop = new Shop() { Shop_id = 19099937, Shop_name = "CozyCowlz" }
      };

      statusCode = HttpStatusCode.OK;

      return new List<InventoryStatus>(new InventoryStatus[] { inv });
    }

  }

}
