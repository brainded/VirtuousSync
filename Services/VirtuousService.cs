using DAL;
using RestSharp;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    public interface IVirtuousService
    {
        Task<PagedResult<Contact>> GetContactsAsync(int skip, int take, string state);
    }
    /// <summary>
    /// API Docs found at https://docs.virtuoussoftware.com/
    /// </summary>
    public class VirtuousService : IVirtuousService
    {
        private readonly RestClient _restClient;

        public VirtuousService(IConfiguration configuration) 
        {
            var apiBaseUrl = configuration.GetValue("VirtuousApiBaseUrl");
            var apiKey = configuration.GetValue("VirtuousApiKey");
            
            var options = new RestClientOptions(apiBaseUrl)
            {
                Authenticator = new RestSharp.Authenticators.OAuth2.OAuth2AuthorizationRequestHeaderAuthenticator(apiKey)
            };

            _restClient = new RestClient(options);
        }

        public async Task<PagedResult<Contact>> GetContactsAsync(int skip, int take, string state)
        {
            var request = new RestRequest("/api/Contact/Query", Method.Post);
            request.AddQueryParameter("Skip", skip);
            request.AddQueryParameter("Take", take);

            var body = new ContactQueryRequest();
            request.AddJsonBody(body);

            var response = await _restClient.PostAsync<PagedResult<Contact>>(request);
            //Expression<Func<AbbreviatedContact, bool>> predicate = x => string.IsNullOrWhiteSpace(state) ? true : x.Address.Contains(state);

            response.List = response.List.Where(x => string.IsNullOrWhiteSpace(state) ? true : x.Address.Contains(state)).ToList();

            return response;
        }

    }
}
