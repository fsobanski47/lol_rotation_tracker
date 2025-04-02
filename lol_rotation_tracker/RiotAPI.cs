using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace lol_rotation_tracker
{
    class RiotAPI
    {
        public ChampionDataRoot? Data { get; private set; }
        public Skills? SkillData { get; private set; }
        public async Task LoadChampionsAsync(string version)
        {
            var url = $"https://ddragon.leagueoflegends.com/cdn/{version}/data/en_US/champion.json";

            try
            {
                using var client = new HttpClient();
                var json = await client.GetStringAsync(url);

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                Data = JsonSerializer.Deserialize<ChampionDataRoot>(json, options);

                if (Data?.Data == null || Data.Data.Count == 0)
                {
                    Console.WriteLine("No champion data found for this version of the game.");
                    return;
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Error fetching champion data: " + e.Message);
            }
            catch (JsonException e)
            {
                Console.WriteLine("Error parsing champion data: " + e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected error: " + e.Message);
            }
        }

        public ChampionDto? GetChampionDto(string key)
        {
            return Data?.Data[key];
        }

        public async Task<Skills?> LoadChampionSkillsAsync(string version, string id)
        {
            var url = $"https://ddragon.leagueoflegends.com/cdn/{version}/data/en_US/champion/{id}.json";
            try
            {
                using var client = new HttpClient();
                var json = await client.GetStringAsync(url);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var root = JsonSerializer.Deserialize<JsonElement>(json, options);
                var data = root.GetProperty("data").GetProperty(id).GetProperty("spells");
                var skills = new Skills
                {
                    Id = id,
                    Version = version,
                    Q = data[0].GetProperty("name").GetString() ?? "Unknown",
                    W = data[1].GetProperty("name").GetString() ?? "Unknown",
                    E = data[2].GetProperty("name").GetString() ?? "Unknown",
                    R = data[3].GetProperty("name").GetString() ?? "Unknown"
                };
                SkillData = skills;
                return skills;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Error fetching champion skills: " + e.Message);
            }
            catch (JsonException e)
            {
                Console.WriteLine("Error parsing champion skills: " + e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected error: " + e.Message);
            }
            return null;
        }
    }
}
