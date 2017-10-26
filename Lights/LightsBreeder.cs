using System.Linq;

namespace Lights
{
    public class LightsBreeder : IBreeder<LightsIndividual>
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
}