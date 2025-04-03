using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace lol_rotation_tracker
{
    class LeagueChampions : DbContext
    {
        public DbSet<ChampionDto> Champions { get; set; }
        public DbSet<Skills> ChampionSkills { get; set; }
        public LeagueChampions()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite(@"Data Source=Lol.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChampionDto>().HasKey(c => c.Id);
            modelBuilder.Entity<Skills>().HasKey(s => s.Id);
            modelBuilder.Entity<Skills>().HasOne<ChampionDto>().WithOne().HasForeignKey<Skills>(s => s.Id);
            base.OnModelCreating(modelBuilder);
        }

        public async Task UpdateChampions(string version)
        {
            var existingChampion = Champions.FirstOrDefault();
            if (existingChampion != null && existingChampion.Version == version)
            {
                Console.WriteLine("The requested version is already in the database. No update needed.");
                return;
            }

            RiotAPI api = new RiotAPI();
            await api.LoadChampionsAsync(version);
            var data = api.Data?.Data;

            if (data == null || data.Count == 0)
            {
                Console.WriteLine("No champions to update in the database.");
                return;
            }

            Champions.RemoveRange(Champions);
            Champions.AddRange(data.Values);

            await SaveChangesAsync();
            Console.WriteLine("Champion list successfully updated to the latest version.");
        }

        public async Task LoadChampion(string id, string version)
        {
            if (ChampionSkills.Any(s => s.Id == id && s.Version == version))
            {
                Console.WriteLine($"Skills for {id} already exist in the database.");
                return;
            }
            else if (ChampionSkills.Any(s => s.Id == id && s.Version != version))
            {
                ChampionSkills.RemoveRange(ChampionSkills.Where(s => s.Id == id && s.Version != version));
            }
            RiotAPI api = new RiotAPI();
            await api.LoadChampionSkillsAsync(version, id);
            var data = api.SkillData;
            if(data == null)
            {
                Console.WriteLine($"No skills to update for {id} in the database.");
                return;
            }
            ChampionSkills.Add(data);
            await SaveChangesAsync();
            Console.WriteLine($"{id} skills updated successfully.");
            
        }

        public void DisplayChampion(string id)
        {
            var champion = Champions.AsEnumerable().FirstOrDefault(c => c.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
            if (champion != null)
            {
                Console.WriteLine(champion.ToString());
                DisplayChampionSkills(id);
            }
            else
            {
                Console.WriteLine("Champion not found in the latest version.");
            }

        }

        public void DisplayChampionSkills(string id)
        {
            var skills = ChampionSkills.FirstOrDefault(s => s.Id == id);
            if (skills == null) Console.WriteLine($"{id} skills not found in the database.");
            else Console.WriteLine(skills.ToString());
        }
    }
}


