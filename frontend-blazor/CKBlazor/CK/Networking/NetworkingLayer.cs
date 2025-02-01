using CKBlazor.CK.Data;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace CKBlazor.CK.Networking
{
    public class NetworkingLayer
    {
        private HttpClient _httpClient;

        public NetworkingLayer(HttpClient httpClient) 
        {
            _httpClient = httpClient;
        }

        public async Task<(string gameId, string clientGuid, GameState? state, DateTime stateAt, Exception? error)> JoinGameAsync(string gameId, string clientGuid)
        {
            try
            {
                var response = await _httpClient.GetAsync($"sample-data/game-state.json");
                //var response = await _httpClient.PostAsJsonAsync($"games/{gameId}/players?p={ClientGuid}", clientGuid);
                response.EnsureSuccessStatusCode();

                var parsed = await response.Content.ReadFromJsonAsync<GameState>();
                if (parsed == null)
                    throw new Exception("failed to parse response");

                return new (gameId, clientGuid, parsed, DateTime.UtcNow, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine("JoinGameAsync error, " + ex);
                return new (gameId, clientGuid, null, DateTime.UtcNow, ex);
            }
        }

        public async Task<(GameState? state, DateTime stateAt, Exception? error)> GetGameState()
        {
            try
            {
                var response = await _httpClient.GetAsync($"sample-data/game-state.json");
                response.EnsureSuccessStatusCode();
                var parsed = await response.Content.ReadFromJsonAsync<GameState>();
                if (parsed == null)
                    throw new Exception("failed to parse response");

                return new (parsed, DateTime.Now, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetGameState error, " + ex);
                return new(null, DateTime.Now, ex);
            }
        }
    }
}
