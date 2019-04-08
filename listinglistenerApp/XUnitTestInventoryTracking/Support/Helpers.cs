using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Diagnostics;

namespace XUnitTestInventoryTracking.Support
{
  public class Helpers
  {

    private static string key = null;
    
    public static string EtsyAPIKey { get {

        string keyfile = Path.Join(Directory.GetCurrentDirectory(), "EtsyAPIKey.txt");

        if(key == null)
        {
          key = File.ReadLines(keyfile).First();
          
          if(String.IsNullOrEmpty(key.Trim()))
          {
            string msg = $"Etsy API Key Required. Enter the API Key into EtsyAPIKey.txt in order to run tests.";
            Trace.WriteLine(msg);
            throw new Exception(msg);
          }

        }
 
        return key;
      }

    }
      
    public static string DBCache => Path.Join(Directory.GetCurrentDirectory(), "InventoryCache");


  }

}
