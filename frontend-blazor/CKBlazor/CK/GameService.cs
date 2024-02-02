using System.Net.Http.Json;
using System.Text.Json.Serialization;
using CKBlazor.CK.Data;

namespace CKBlazor.CK
{
    public class GameService
    {
        // Assets
        public Dictionary<string, Vector3>? TerritoryPositions { get; private set; }

        // Session State
        public string? GameId { get; private set; }
        public string? ClientGuid { get; private set; }

        // Game State
        public GameState? LastGameState { get; private set; }
        public DateTime? LastGameStateAt { get; private set; }

        private readonly HttpClient _httpClient;

        public GameService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<GameState> JoinGameAsync(string gameId, string clientGuid)
        {
            try
            {
                var response = await _httpClient.GetAsync($"sample-data/game-state.json");
                //var response = await _httpClient.PostAsJsonAsync($"games/{gameId}/players?p={ClientGuid}", clientGuid);
                response.EnsureSuccessStatusCode();

                var parsed = await response.Content.ReadFromJsonAsync<GameState>();
                if (parsed == null)
                    throw new Exception("failed to parse response");

                GameId = gameId;
                ClientGuid = clientGuid;
                LastGameState = parsed;
                LastGameStateAt = DateTime.Now;

                Console.WriteLine("LastGameState:\n" + System.Text.Json.JsonSerializer.Serialize(LastGameState));

                return LastGameState;
            }
            catch(Exception ex)
            {
                Console.WriteLine("JoinGameAsync error, " + ex);
                return LastGameState;
            }
        }

        public async Task<GameState> GetGameState()
        {
            try
            {
                var response = await _httpClient.GetAsync($"sample-data/game-state.json");
                response.EnsureSuccessStatusCode();
                var parsed = await response.Content.ReadFromJsonAsync<GameState>();
                if (parsed == null)
                    throw new Exception("failed to parse response");

                LastGameState = parsed;
                LastGameStateAt = DateTime.Now;

                Console.WriteLine("LastGameState: " + System.Text.Json.JsonSerializer.Serialize(LastGameState));

                return LastGameState;
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetGameState error, " + ex);
                return LastGameState;
            }
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
