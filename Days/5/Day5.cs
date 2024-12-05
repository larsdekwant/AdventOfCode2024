using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode
{
    static class Day5
    {
        public static int RunPart1()
        {
            var lines = File.ReadLines("../../../Days/5/InputPart1.txt").ToArray();

            Dictionary<string, List<string>> order = new Dictionary<string, List<string>>();

            // Construct dictionary denoting the dependencies between pages.
            int index = 0;
            foreach (string line in lines) 
            {
                if (line == "") break;

                string[] vals = line.Split('|', StringSplitOptions.RemoveEmptyEntries);
                if(!order.TryAdd(vals[0], new List<string> { vals[1] }))
                {
                    order[vals[0]].Add(vals[1]);
                }
                index++;
            }

            // Check the pages of the updates.
            int total = 0;
            for (int i = index + 1; i < lines.Length; i++)
            {
                string[] update = lines[i].Split(",", StringSplitOptions.RemoveEmptyEntries);
                total += CheckPageOrder(order, update);
            }
            
            return total;
        }

        public static int CheckPageOrder(Dictionary<string, List<string>> orderMap, string[] update)
        {
            for (int i = 0; i < update.Length; i++)
            {
                List<string>? precedeList;
                if (!orderMap.TryGetValue(update[i], out precedeList))
                {
                    precedeList = new List<string>();
                }

                // Check whether the order isn't violated
                foreach (string page in precedeList)
                {
                    int pageIndex = Array.IndexOf(update, page);
                    if (pageIndex != -1 && pageIndex < i) return 0;
                }               
            } 
            
            return int.Parse(update[update.Length / 2]);
        }
        
        public static int RunPart2()
        {
            var lines = File.ReadLines("../../../Days/5/InputPart1.txt").ToArray();

            Dictionary<string, List<string>> order = new Dictionary<string, List<string>>();

            // Construct dictionary denoting the dependencies between pages.
            int index = 0;
            foreach (string line in lines)
            {
                if (line == "") break;

                string[] vals = line.Split('|', StringSplitOptions.RemoveEmptyEntries);
                if (!order.TryAdd(vals[0], new List<string> { vals[1] }))
                {
                    order[vals[0]].Add(vals[1]);
                }
                index++;
            }

            // Check the pages of the updates.
            int total = 0;
            for (int i = index + 1; i < lines.Length; i++)
            {
                string[] update = lines[i].Split(",", StringSplitOptions.RemoveEmptyEntries);
                total += CheckAndFixPageOrder(order, update);
            }

            return total;
        }

        public static int CheckAndFixPageOrder(Dictionary<string, List<string>> orderMap, string[] update, bool swappedPages = false)
        {
            for (int i = 0; i < update.Length; i++)
            {
                List<string>? precedeList;
                if (!orderMap.TryGetValue(update[i], out precedeList))
                {
                    precedeList = new List<string>();
                }

                // Check whether the order isn't violated, fix if it is.
                foreach (string page in precedeList)
                {
                    int pageIndex = Array.IndexOf(update, page);
                    if (pageIndex != -1 && pageIndex < i)
                    {
                        // Swap the incorrect pages and try again.
                        string temp = update[pageIndex];
                        update[pageIndex] = update[i];
                        update[i] = temp;
                        return CheckAndFixPageOrder(orderMap, update, true);
                    }
                }
            }

            // Return middle page number of the ones that were fixed.
            return swappedPages ? int.Parse(update[update.Length / 2]) : 0;
        }
    }
}
