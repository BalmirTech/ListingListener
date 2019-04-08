using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InventoryTrackingSvc.Model;
using InventoryTrackingSvc.Data;

namespace InventoryTrackingSvc.Controllers
{
  [Route("api/v1/[controller]")]
  [ApiController]
  public class SyncController : ControllerBase
  {

    InventoryAnalytics ia = null;

    public SyncController(InventoryAnalytics ia)
    {
      this.ia = ia;
    }

    [HttpPost]
    [Route("shops")]
    public List<InventoryStatus> Post([FromBody] List<int> shopIDs)
    {
      List<InventoryStatus> stats = ia.GetInventoryStatus(shopIDs);      
      return stats;
    }

  }

}
