using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InventoryTrackingSvc.Model;

namespace InventoryTrackingSvc.Data
{
 
  public class InventoryAnalytics
  {
    internal class Deltas<T>
    {
      public List<T> Adds { get; set; }
      public List<T> Deletes { get; set; }
    }


    ISyncDatabase syncDatabase = null;
    EtsyAPIProxy estyAPI = null;

    public InventoryAnalytics(ISyncDatabase syncDatabase, EtsyAPIProxy etsyAPI)
    {
      this.syncDatabase = syncDatabase;
      this.estyAPI = etsyAPI;
    }

    /// <summary>
    /// Determine the difference in listing from now to the last cached listing.
    /// </summary>
    /// <param name="shopID"></param>
    /// <returns></returns>
    public InventoryStatus GetInventoryStatus(int shopID)
    {
      
      Shop shop = getShopSmart(shopID);

      //Indicated that ShopId Invalud 
      if (shop is null)
      {
        return new InventoryStatus() {
          Shop = new Shop()
          {
            Shop_id = shopID,
            Shop_name = null
          },
          IsShopIdValid = false
        };
      }

   
      ShopInventory previousInventoryState = syncDatabase.ReadShopInventory(shopID);
      EtsyDataContainer<Listing> listingsFromEtsy = estyAPI.GetActiveListingsByShop(shopID);

      syncDatabase.PersistShopInventory(shop, listingsFromEtsy.Results);

      //Indicates first time request for synchronization.
      if (previousInventoryState == null)
      {
        return new InventoryStatus()
        {
          Shop = shop,
          AsOfDate = DateTime.Today,
          Added = listingsFromEtsy.Results,
          Removed = new List<Listing>()
        };
      }
      
      //Determine the changes between the two lists.
      Deltas<Listing> deltas = determineListingsDeltas(listingsFromEtsy, previousInventoryState.Listings);

      InventoryStatus invStat = new InventoryStatus()
      {
        Shop = shop,
        AsOfDate = DateTime.Today,
        Added = deltas.Adds,
        Removed = deltas.Deletes
      };

      return invStat;
    }


    /// <summary>
    /// Determine the difference in listing from now to the last cached listing.
    /// Runs analysis on each shop on seperate thread.
    /// </summary>
    /// <param name="shopIDs"></param>
    /// <returns></returns>
    public List<InventoryStatus> GetInventoryStatus(List<int> shopIDs)
    {

      List<Task<InventoryStatus>> tasks = new List<Task<InventoryStatus>>();

      shopIDs.ForEach(shopid => {

        Task<InventoryStatus> t = Task.Factory.StartNew<InventoryStatus>(function: (id) => {

          int aShopId = (int)id;
          return GetInventoryStatus(aShopId);

        }, state: (object)shopid);

        tasks.Add(t);

      });


      Task.WaitAll(tasks.ToArray());

      List<InventoryStatus> status = new List<InventoryStatus>();
      tasks.ForEach(t => { status.Add(t.Result); });

      return status;
    }



    private Shop getShopSmart(int shopId)
    {
      ShopInventory previousInventoryState = syncDatabase.ReadShopInventory(shopId);

      if(previousInventoryState != null)
        return previousInventoryState.Shop;

      EtsyDataContainer<Shop> testShop = estyAPI.GetShop(shopId);

      if (testShop != null && testShop.Count == 1)
        return testShop.Results[0];
      else
        return null;
      
    }


    private Deltas<Listing> determineListingsDeltas(EtsyDataContainer<Listing> listingsFromEtsy, List<Listing> previousList)
    {

      Dictionary<int, Listing> prevListingAsHashTable = previousList.ToDictionary((l) => l.Listing_id);
      Dictionary<int, Listing> currListingAsHashTable = listingsFromEtsy.Results.ToDictionary((l) => l.Listing_id);

      listingsFromEtsy.Results.ForEach((l) => {

        if (prevListingAsHashTable.ContainsKey(l.Listing_id))
        {
            prevListingAsHashTable.Remove(l.Listing_id);
            currListingAsHashTable.Remove(l.Listing_id);
        }

      });

      Deltas<Listing> changes = new Deltas<Listing>()
      {
        Adds = currListingAsHashTable.Values.ToList(),
        Deletes = prevListingAsHashTable.Values.ToList()
      };

      return changes;
    }
    
  }

}
