using System;
using ColorMine.ColorSpaces;

namespace Lights
{
    public class SimpleLABLightFitnessEvaluator : IFitnessEvaluator<LightsIndividual>
    {
        public Color TargetColor { get; set; }

        private Lab targetLab;

        public SimpleLABLightFitnessEvaluator(Color targetColor)
        {
            targetLab = ConvertColorToLab(targetColor);
        }

        public int Evaluate(LightsIndividual individual)
        {
            var totalDistance = 0;
            foreach (var color in individual.Colors)
            {
                var lab = ConvertColorToLab(color);
                var lDiff = Math.Pow(targetLab.L - lab.L, 2);
                var aDiff = Math.Pow(targetLab.A - lab.A, 2);
                var bDiff = Math.Pow(targetLab.B - lab.B, 2);

                var colorDiff = Math.Sqrt(lDiff + aDiff + bDiff);
                totalDistance += (int)colorDiff;
            }

            return -totalDistance;
        }

        private Lab ConvertColorToLab(Color color)
        {
            var rgb = new Rgb
            {
                R = color.Red,
                G = color.Green,
                B = color.Blue
            };
            return rgb.To<Lab>();
        }
    }
}