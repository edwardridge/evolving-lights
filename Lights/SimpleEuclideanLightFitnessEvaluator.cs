using System;

namespace Lights
{
    public class SimpleEuclideanLightFitnessEvaluator : IFitnessEvaluator<LightsIndividual>
    {
        public Color TargetColor { get; set; }

        public SimpleEuclideanLightFitnessEvaluator(Color targetColor)
        {
            TargetColor = targetColor;
        }
        
        public int Evaluate(LightsIndividual individual)
        {
            var totalDistance = 0;
            foreach (var color in individual.Colors)
            {
                var redDiff = Math.Pow(TargetColor.Red - color.Red, 2);
                var greenDiff = Math.Pow(TargetColor.Green - color.Green, 2);
                var blueDiff = Math.Pow(TargetColor.Blue - color.Blue, 2);

                var colorDiff = Math.Sqrt(redDiff + greenDiff + blueDiff);
                totalDistance += (int)colorDiff;
            }

            return -totalDistance;
        }
    }
}