using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Supervisors.Interfaces;
using Supervisors.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Supervisors.Helpers
{
    public class Managers : IManager
    {
        private readonly IConfiguration _configuration;
        public Managers(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async void PostManager(NotificationRegistration registration)
        {
            // Requirement: The submitted data above should be printed to the console upon receiving the post request
            await Task.Run(() => Console.WriteLine($"First Name: {registration.FirstName} \r\nLast Name: {registration.LastName} \r\nEmail: {registration.Email}"));
        }

        public async Task<List<Manager>> GetManagers()
        {
            // gets base url from config file
            string AWS_BaseURL = _configuration.GetValue<string>("AWS_BaseURL");
            List<Manager> managers = new();
            
            // call to AWS endpoint
            using (var http = new HttpClient())
            {
                http.DefaultRequestHeaders.Accept.Clear();
                http.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage amazonResponse = await http.GetAsync(string.Format("{0}{1}", AWS_BaseURL, "managers")).ConfigureAwait(false);

                // populate managers list if repsonse is not null
                if (amazonResponse != null)
                {
                    var responseContent = amazonResponse.Content.ReadAsStringAsync().Result;
                    managers = JsonConvert.DeserializeObject<List<Manager>>(responseContent);
                }
            }

            return managers;
        }
    }
}
