using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Auth0.AuthenticationApi;
using MentalVerse.Web.Api.Helper;
using MentalVerse.Web.Api.Interfaces;
using MentalVerse.Web.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MentalVerse.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ICompanyRepository<Company> _companyRepository;

        public AuthController(ICompanyRepository<Company> companyRepository)
        {
            _companyRepository = companyRepository;
        }

        [HttpPost]
        [Route("GetAuthOAccessToken")]
        public async Task<IActionResult> GetAuthOUserToken(int companyId, string username = "testuser@mentalfish.com", string password = "PasswOrd@123")
        {

            //string url = $"https://{Startup.AuthoDomain}/oauth/token";
            //string audience = Startup.Auth0ApiIdentifier;
            //string client_id = Startup.Client_id;
            //string client_secret = Startup.Client_secret;

            var companyInfo = _companyRepository.FindByCompanyID(companyId);

            if(companyInfo == null)
                return NotFound();

            string url = $"https://{companyInfo.Auth0Domain}/oauth/token";
            string audience = companyInfo.Auth0ApiIdentifier;
            string client_id = companyInfo.Auth0ClientId;
            string client_secret = companyInfo.Auth0ClientSecret;    

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var pairs = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>( "grant_type", "password" ),
                        new KeyValuePair<string, string>( "username", username),
                        new KeyValuePair<string, string> ( "password", password ),
                        new KeyValuePair<string, string> ( "scope", "openid email" ),
                        new KeyValuePair<string, string> ( "client_id", client_id ),
                        new KeyValuePair<string, string> ( "client_secret", client_secret ),
                        new KeyValuePair<string, string> ( "audience", audience ),
                    };

                FormUrlEncodedContent requestBody = new FormUrlEncodedContent(pairs);
                //Request Token
                var request = await client.PostAsync(url, requestBody);
                //var response = await request.Content.ReadAsStringAsync();
              
                var Obj = JsonConvert.DeserializeObject<AccessToken>(request.Content.ReadAsStringAsync().Result);
                return new OkObjectResult(Obj);
            }


        }
        
        [HttpGet]
        [Route("UserInfo")]
        public async Task<object> UserInformation()
        {
            // Retrieve the access_token claim which we saved in the OnTokenValidated event
            string accessToken = User.Claims.FirstOrDefault(c => c.Type == "access_token").Value;

            // If we have an access_token, then retrieve the user's information
            if (!string.IsNullOrEmpty(accessToken))
            {
                var apiClient = new AuthenticationApiClient(Startup.AuthoDomain);
                var userInfo = await apiClient.GetUserInfoAsync(accessToken);

                return userInfo;
            }
            return null;
        }
    }
    public class AccessToken
    {
        public string access_token { get; set; }
    }
}