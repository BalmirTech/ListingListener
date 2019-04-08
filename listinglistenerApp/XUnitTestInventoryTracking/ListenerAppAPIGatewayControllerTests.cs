using System;
using System.Linq;
using Xunit;

using System.Collections.Generic;
using InventoryTrackingSvc.Data;
using System.IO;
using listinglistenerApp.Models;
using InventoryTrackingSvc.Controllers;
using listinglistenerApp.Controllers;

using XUnitTestInventoryTracking.Mock;

namespace XUnitTestInventoryTracking
{
  public class ListenerAppAPIGatewayControllerTests
  {
    ApiController APIGatewayController;
         

    public ListenerAppAPIGatewayControllerTests()
    {

      FakeInventoryAnalysisServiceProxy fia = new FakeInventoryAnalysisServiceProxy("http://notused.com");
           
      this.APIGatewayController = new ApiController(fia);

    }

    [Fact]
    public void GetAnalysisStatus()
    {
      List<int> ids = fillTestShops().Values.ToList();

      List<InventoryStatus> invStatus = this.APIGatewayController.SyncShop(ids.ToArray());
      invStatus = this.APIGatewayController.SyncShop(ids.ToArray());

      bool res = invStatus.All((status) =>
      {
        return (status.IsShopIdValid == true);
      });

      Assert.True(res);

    }


    private Dictionary<string, int> fillTestShops()
    {
      Dictionary<string, int> testShops = new Dictionary<string, int>();
      testShops.Add("A", 19099937);

      return testShops;
    }


  }

}
