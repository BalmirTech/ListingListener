using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryTrackingSvc.Model;

namespace InventoryTrackingSvc
{

        

    public class EtsyAPIProxy
    {

        private KeyValuePair<string, string> apikey;
        private string baseURL; 

        public EtsyAPIProxy(string apiBaseURL, string apiKey)
        {
            this.baseURL = apiBaseURL;
            this.apikey = new KeyValuePair<string, string>("api_key", apiKey);
        }

        public EtsyDataContainer<Shop> GetShop(int shopID)
        {
          return GetShops(new int[] { shopID });
        }

        public EtsyDataContainer<Shop> GetShops(IEnumerable<int> shopIDs, int limit = 100)
        {

          EtsyDataContainer<Shop> shopContainer = GetShops(shopIDs, limit, 0);

          if (shopContainer == null)
            return null;

          int iterCnt = (int)Math.Ceiling(shopContainer.Count / 100.0);

          if (iterCnt > 0)
          {

            for (int i = 1; i <= iterCnt; i++)
            {
              EtsyDataContainer<Shop> shopContainerAdded = GetShops(shopIDs, limit, limit * i);
              shopContainer.Results.AddRange(shopContainerAdded.Results);
            }

          }

          return shopContainer;
        }

        
        public EtsyDataContainer<Shop> GetShops(IEnumerable<int> shopIDs, int limit, int offset)
        {
            //Convert shop ids in concat string array
            List<string> mappedShopIds = shopIDs.ToList().ConvertAll<string>((id) => { return id.ToString(); });
            string idsConcatenated = string.Join(",", mappedShopIds);


            var client = new RestClient(this.baseURL);
            var request = new RestRequest($"shops/{idsConcatenated}", Method.GET);
          
            //request.AddUrlSegment("shopids", idsConcatenated);
            request.AddParameter("fields", "shop_id,shop_name");
            request.AddParameter("limit", limit);
            request.AddParameter("offset", offset);

            request.AddParameter(this.apikey.Key, this.apikey.Value);
            

            IRestResponse<EtsyDataContainer<Shop>> response2 = client.Execute<EtsyDataContainer<Shop>>(request);

            return response2.Data;
        }

        public EtsyDataContainer<Listing> GetActiveListingsByShop(int shopID, int limit = 100)
        {
          
          EtsyDataContainer<Listing> lisitingContainer = GetActiveListingsByShop(shopID, limit, 0);
          int iterCnt = (int)Math.Ceiling(lisitingContainer.Count / (double)limit);

          if (iterCnt > 0)
          {

            for (int i = 1; i < iterCnt; i++)
            {
              EtsyDataContainer<Listing> shopContainerAdded = GetActiveListingsByShop(shopID, limit, limit * i);
              lisitingContainer.Results.AddRange(shopContainerAdded.Results);
            }

          }

          return lisitingContainer;
        }


        public EtsyDataContainer<Listing> GetActiveListingsByShop(int shopID, int limit, int offset)
        {

            var client = new RestClient(this.baseURL);
            var request = new RestRequest($"shops/{shopID}/listings/active", Method.GET);

            request.AddParameter("fields", "listing_id,state,title");
            request.AddParameter("limit", limit);
            request.AddParameter("offset", offset);
            request.AddParameter(this.apikey.Key, this.apikey.Value);

            IRestResponse<EtsyDataContainer<Listing>> response2 = client.Execute<EtsyDataContainer<Listing>>(request);

            return response2.Data;
        }

    }

}
