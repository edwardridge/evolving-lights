using System;
using System.Runtime.Remoting.Messaging;

namespace Lights
{
    public class SlightChangeLightsMutator : IMutater<LightsIndividual>
    {
        private Random _random;

        public SlightChangeLightsMutator(Random random)
        {
            _random = random;
        }

        public void Mutate(LightsIndividual individual)
        {
            for (var index = 0; index < individual.Colors.Count; index++)
            {
                var randVal = _random.Next(0, 100);
                if (randVal < 10)
                {
                    var colorToChangeIndex = _random.Next(0, 3);
                    var color = individual.Colors[index];
                    var upOrDown = _random.Next(0, 2) == 0 ? Add : Subtract;
                    if (colorToChangeIndex == 0)
                    {
                        color.Red = upOrDown(color.Red, 5);
                    }
                    else if (colorToChangeIndex == 1)
                    {
                        color.Green = upOrDown(color.Green, 5);
                    }
                    else if (colorToChangeIndex == 2)
                    {
                        color.Blue = upOrDown(color.Blue, 5);
                    }

                    color.EnforceMinAndMax();
                    
                    individual.Colors[index] = color;
                }
            }
        }

        private Func<int, int, int> Add = (original, addition) => original + addition;
        private Func<int, int, int> Subtract = (original, subtraction) => original - subtraction;
        private enum UpOrDown
        {
            Up = 1,
            Down = 2
        }
    }
}