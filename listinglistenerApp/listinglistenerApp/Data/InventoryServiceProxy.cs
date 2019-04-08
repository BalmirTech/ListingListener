using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using listinglistenerApp.Models;
using System.Net;

namespace listinglistenerApp.Data
{

  public class InventoryAnalyisServiceProxy
  {

    private string baseURL;

    public InventoryAnalyisServiceProxy(string apiBaseURL)
    {
      this.baseURL = apiBaseURL;     
    }


    public virtual List<InventoryStatus> GetAnalysis(IEnumerable<int> shopIDs, out HttpStatusCode statusCode)
    {
      var client = new RestClient(this.baseURL);
      var request = new RestRequest($"api/v1/sync/shops", Method.POST);

      request.AddJsonBody(shopIDs);

      IRestResponse<List<InventoryStatus>> response2 = client.Execute<List<InventoryStatus>>(request);
    statusCode = response2.StatusCode;

      return response2.Data;
    }

  }

}
