using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Calories
{
    public class Class_for_DB
    {
        public static UserContext DB = new UserContext();
        public static async Task<string> GetFood(string linkoffood)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(linkoffood);
            HttpContent content = response.Content;
            string result = await content.ReadAsStringAsync();
            return result;
        }
    }
}
