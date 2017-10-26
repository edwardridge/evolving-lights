using System.Collections.Generic;

namespace Lights
{
    public class LightsIndividual : IIndividual
    {
        public LightsIndividual(List<Color> colors)
        {
            Colors = colors;
        }
        
        public List<Color> Colors { get; }
        
        public int Fitness { get; set; }
    }
}