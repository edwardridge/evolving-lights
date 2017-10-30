using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Lights
{
    public class HalfwayPointLightsBreeder : IBreeder<LightsIndividual>
    {
        public LightsIndividual Breed(LightsIndividual parent1, LightsIndividual parent2)
        {
            var firstHalfOfParent1 =
                parent1.Colors.Where((_, i) => i < parent1.Colors.Count / 2);
            var secondHalfOfParent2 =
                parent2.Colors.Where((_, i) => i >= parent2.Colors.Count / 2);

            var newColors = firstHalfOfParent1.Concat(secondHalfOfParent2).ToList();
            
            return new LightsIndividual(newColors);

        }
    }
    
    public class AlternatingLightsBreeder : IBreeder<LightsIndividual>
    {
        public LightsIndividual Breed(LightsIndividual parent1, LightsIndividual parent2)
        {
            var colors = new List<Color>();
            for (int i = 0; i < parent1.Colors.Count; i++)
            {
                if (i % 2 == 0)
                {
                    colors.Add(parent1.Colors[i]);
                }
                else
                {
                    colors.Add(parent2.Colors[i]);
                }
            }
            
            return new LightsIndividual(colors);

        }
    }
}