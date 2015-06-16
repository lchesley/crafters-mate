using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EL
{
    public class Entity
    {
        public string Name { get; set; }
        public string ToolkitLink { get; set; }
        public EntityType Type { get; set; }
    }

    public enum EntityType
    {
        Recipe,
        Item,
        GatheringLog
    };
}
