using System.Net.Http.Json;
using System.Text.Json.Serialization;
using CKBlazor.CK.Data;

namespace CKBlazor.CK
{
    public class GameService
    {
        private readonly HttpClient _httpClient;

        public GameState? LastGameState { get; private set; }
        public DateTime? LastGameStateAt { get; private set; }
        public string? GameId { get; private set; }
        public string? ClientGuid { get; private set; }


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
                
               // var text = await response.Content.ReadAsStringAsync();
                //Console.WriteLine("Response:\n" + System.Text.Json.JsonSerializer.Serialize(LastGameState));


                var parsed = await response.Content.ReadFromJsonAsync<GameState>();
               // var parsed = System.Text.Json.JsonSerializer.Deserialize<GameState>(text);
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
    }
}
