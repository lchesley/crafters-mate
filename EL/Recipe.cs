using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EL
{
    public class Recipe : Entity
    {        
        public string DiscipleOfTheHand { get; set; }
        public int RecipeLevel { get; set; }
        public int ItemLevel { get; set; }
        public int TotalCrafted { get; set; }
        public int Difficulty { get; set; }
        public int Durability { get; set; }
        public int MaximumQuality { get; set; }        
        public List<Entity> Crystals { get; set; }
        public List<Entity> Materials { get; set; }
        public List<string> Characteristics { get; set; }
    }
}
