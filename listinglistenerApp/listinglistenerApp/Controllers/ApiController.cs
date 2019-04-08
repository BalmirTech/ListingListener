using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using listinglistenerApp.Models;
using listinglistenerApp.Data;

namespace listinglistenerApp.Controllers
{

  [Route("/api/v1")]
  public class ApiController : ControllerBase
  {

    InventoryAnalyisServiceProxy iaSvcProxy = null;

    public ApiController(InventoryAnalyisServiceProxy iaSvcProxy)
    {
      this.iaSvcProxy = iaSvcProxy;
    }

    [HttpPost]
    [Route("sync/shops")]
    public List<InventoryStatus> SyncShop ([FromBody] int[] shopIDs)
    {

        System.Net.HttpStatusCode code;
        List<InventoryStatus> obj = this.iaSvcProxy.GetAnalysis(shopIDs, out code);

        if(this.HttpContext != null)
          this.HttpContext.Response.StatusCode = (int)code;

        return obj;     
    }

  }

}
