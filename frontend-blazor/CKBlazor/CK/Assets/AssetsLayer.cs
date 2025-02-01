using CKBlazor.CK.Data;
using System.Net.Http;
using System.Net.Http.Json;

namespace CKBlazor.CK.Assets
{
    public class AssetsLayer
    {
        public Dictionary<string, Vector3>? TerritoryPositions { get; private set; }
        
        HttpClient _httpClient;

        public AssetsLayer(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task InitAssets()
        {
            var response = await _httpClient.GetAsync($"assets/territories/positions.json");
            response.EnsureSuccessStatusCode();

            var parsed = await response.Content.ReadFromJsonAsync<Dictionary<string, Vector3>>();
            if (parsed == null)
                throw new Exception("failed to parse response");

            TerritoryPositions = parsed;
        }
    }
}
