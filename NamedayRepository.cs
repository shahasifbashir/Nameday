using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace NameDay
{
    public static class NamedayRepository
    {
        private static List<NamedarModel> allNamedaysCache;

        public static async Task<List<NamedarModel>> GetallNamedays()
        {
            if (allNamedaysCache != null)
                return allNamedaysCache;
            var client = new HttpClient();
            var stream = await client.GetStreamAsync("http://www.response.hu/namedays_hu.json");
            var serialize = new DataContractJsonSerializer(typeof(List<NamedarModel>));
            allNamedaysCache = (List<NamedarModel>)serialize.ReadObject(stream);
            return allNamedaysCache;
        }
    }
}
