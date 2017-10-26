using System.Data;
using System.Linq;

namespace Lights
{
    public class LightIsRedFitnessEvaluator : IFitnessEvaluator<LightsIndividual>
    {
        public int Evaluate(LightsIndividual individual)
        {
            return individual.Colors.Count(s => s.IsRed());
        }
    }
    
    public class LightIsColorFitnessEvaluator : IFitnessEvaluator<LightsIndividual>
    {
        public Color TargetColor { get; set; }

        public LightIsColorFitnessEvaluator(Color targetColor)
        {
            TargetColor = targetColor;
        }
        
        public int Evaluate(LightsIndividual individual)
        {
            return individual.Colors.Count(s => s.Equals(TargetColor));
        }
    }
}