using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using InventoryTrackingSvc.Model;

namespace InventoryTrackingSvc.Data
{

  //TODO: Need a locking mechanism at the shop level files.
  //Need to recognize when the file becomes dirty between the 
  //time file is read and updated from.
  /// <summary>
  /// ! Currently Not Thread Safe
  /// This database stores each shop in a text file. 
  /// The first line contains shop information {shopid}:{shopname}
  /// All following lines contain the listings {listingid}:{listingtitle}
  /// </summary>
  public class SyncDatabaseFS : ISyncDatabase
    {
      private const char delimeter = ':';
      private string rootPath = null;

      public SyncDatabaseFS(string root)
      {
          this.rootPath = root;
          
          try
          {
              if (Directory.Exists(root) == false)
                  Directory.CreateDirectory(root);

          }
          catch(Exception e)
          {
              throw new Exception("Accessing or creating root directory threw exception", e);
          }
                        
      }

    public List<ShopInventory> ReadShopInventory(IEnumerable<int> shopIds)
    {
      List<ShopInventory> shopsWithInv = new List<ShopInventory>();
          
      shopIds.ToList().ForEach(id => {

        ShopInventory si = ReadShopInventory(id);
        shopsWithInv.Add(si);                        
      });

      return shopsWithInv;
    }


    public ShopInventory ReadShopInventory(int shopId)
    {
        string fileName = $"{shopId}.txt";
        string shopFile = Path.Combine(this.rootPath, fileName);

        if (File.Exists(shopFile))
        {
          string[] persistedShopData = File.ReadAllLines(shopFile);

          ShopInventory si = new ShopInventory()
          {
            Shop = readShopInfo(persistedShopData),
            Listings = readListings(persistedShopData)
          };

          return si;
        }
        
      return null;
    }



    public void PersistShopInventory(Shop shop, List<Listing> listings)
    {
      string shopCachePath = Path.Combine(this.rootPath, $"{shop.Shop_id}.txt");

      using (StreamWriter sw = new StreamWriter(shopCachePath, false))
      {
        sw.WriteLine(generateShopEntry(shop));

        listings.ForEach(l =>
        {
          sw.WriteLine(generateListingEntry(l));
        });

        sw.WriteLine();
      }

    }
    
    private Shop readShopInfo(string[] data)
    {
      if(data.Count() >= 1)
      {
        Tuple<string, string> parsed = extractTuple(data[0]);

        if(parsed != null)
        {
          Shop shop = new Shop()
          {
            Shop_id = int.Parse(parsed.Item1),
            Shop_name = parsed.Item2
          };

          return shop;
        }

      }

      return null;
    }

    private string generateShopEntry(Shop shop) => $"{shop.Shop_id}:{shop.Shop_name.Trim()}";

    private string generateListingEntry(Listing listing) => $"{listing.Listing_id}:{listing.Title.Trim()}";

    private List<Listing> readListings(string[] data)
    {
      List<Listing> listings = new List<Listing>();

      if (data.Count() >= 2)
      {

        for(int i = 1; i < data.Length; i++)
        {
          Tuple<string, string> parsed = extractTuple(data[i]);

          if(parsed != null)
          {
            Listing listing = new Listing()
            {
              Listing_id = int.Parse(parsed.Item1),
              Title = parsed.Item2
            };

            listings.Add(listing);
          }

        }

      }

      return listings;
    }


    //ID and Description
    private Tuple<string,string> extractTuple(string data)
    {
      int loc = data.IndexOf(':');

      if(loc != -1)
      {
        string idStr = data.Substring(0, loc);
        string name = data.Substring(loc + 1, data.Length - loc - 1);
        return new Tuple<string, string>(idStr, name);
      }

      return null;
    }

  }

}
