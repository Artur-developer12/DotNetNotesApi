using System;
namespace MyNotes.Services
{
	public class CatApiService
	{
        private readonly IHttpClientFactory _httpClientFactory;

        public CatApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

       public async Task<string> GetFact()
        {
            HttpClient client = _httpClientFactory.CreateClient();
            var responce = await client.GetFromJsonAsync<Fact>("https://catfact.ninja/fact?max_length=140");
            return responce == null ? throw new Exception() : (responce.fact);
        }
    }
    record class Fact(string fact, int lenght);
}


