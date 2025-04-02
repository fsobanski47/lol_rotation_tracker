using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lol_rotation_tracker
{
    public class ChampionDataRoot
    {
        public required Dictionary<string, ChampionDto> Data { get; set; }
    }

    public class ChampionDto
    {
        public required string Id { get; set; }
        public required string Key { get; set; }
        public required string Name { get; set; }
        public required string Title { get; set; }
        public required List<string> Tags { get; set; }
        public required string Version { get; set; }

        public override string ToString()
        {
            string champString = "Version: " + Version + "\nId: " + Id + "\nKey: " + Key + "\nTitle: " + Title;
            string tagString = string.Join(" ", Tags);
            return champString + "\nTags: " + tagString;
        }
    }

    public class Skills
    {
        public required string Id { get; set; }
        public required string Version { get; set; }
        public required string Q { get; set; }
        public required string W { get; set; }
        public required string E { get; set; }
        public required string R { get; set; }

        public override string ToString()
        {
            return  "Q: " + Q + "\nW: " + W + "\nE: " + E + "\nR: " + R;
        }
    }
}
