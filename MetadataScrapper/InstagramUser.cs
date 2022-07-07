using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetadataScrapper
{
    public class InstagramUser
    {
        public string FullName { get; set; }
        public int FollowerCount { get; set; }
        public int FolloweingCount { get; set; }
        public void Display()
        {
            Console.WriteLine($"Full Name{FullName}");
            Console.WriteLine($"Follower{FollowerCount}");
            Console.WriteLine($"Following {FolloweingCount}");
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }


    }
}
