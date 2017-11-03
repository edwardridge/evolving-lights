using System.Collections.Generic;
using System.Linq;

namespace Lights
{
    public class LightsIndividual : IIndividual
    {
        public LightsIndividual(List<Color> colors)
        {
            Colors = colors;
        }
        
        public List<Color> Colors { get; }
        
        public double Fitness { get; set; }
        
        public object Clone()
        {
            return new LightsIndividual(this.Colors.Select(s => s.Clone() as Color).ToList());
        }
    }
}