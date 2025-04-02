using static System.Net.WebRequestMethods;

namespace lol_rotation_tracker
{   
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.Write("Enter version: ");
            string? version = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(version))
            {
                Console.WriteLine("Version cannot be empty.");
                return;
            }

            using var db = new LeagueChampions();
            await db.UpdateChampions(version);
            while(true)
            {
                Console.Write("Enter champion name to search: ");
                string? championName = Console.ReadLine()?.Trim();
                Console.Clear();
                if (!string.IsNullOrEmpty(championName))
                {
                    await db.LoadChampion(championName, version);
                    db.DisplayChampion(championName);
                }
            }   
        }
    }
}
