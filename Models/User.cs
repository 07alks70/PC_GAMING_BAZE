using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PC_GAMING_BAZE.Models
{
    public class User
    {

        public int id;
        public string username;


        public static async IAsyncEnumerable<User> GetUsers()
        {

            List<User> users = new List<User>();

            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri("http://176.112.164.61/api/users");
            request.Method = HttpMethod.Get;
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Authorization", "Basic " + System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("admin:admin")));

            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                HttpContent responseContent = response.Content;
                var json = await responseContent.ReadAsStringAsync();

                JsonDocument doc = JsonDocument.Parse(json);
                JsonElement root = doc.RootElement;

                for(int i = 0; i < root.GetProperty("result").GetArrayLength(); i++)
                {

                    if(root.GetProperty("result")[i].GetProperty("userGroupId").ToString() == "1")
                    {

                        Debug.WriteLine("UserName: " + root.GetProperty("result")[i].GetProperty("username") + " ID: " + root.GetProperty("result")[i].GetProperty("id"));

                        yield return new User() { id = (int)Int64.Parse(root.GetProperty("result")[i].GetProperty("id").ToString()), username = root.GetProperty("result")[i].GetProperty("username").ToString() };

                    } 

                }

            }

        }

    }
}
