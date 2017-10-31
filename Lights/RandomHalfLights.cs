using System;
using System.Collections.Generic;
using System.Text;

namespace Lights
{
    public class RandomHalfLightsGenerator
    {
        public static Color GenerateRandomColor(Random random)
        {
            var redColorRandVal = GenerateRandomValue(random);
            var greenColorRandVal = GenerateRandomValue(random);
            var blueColorRandVal = GenerateRandomValue(random);
            return new Color(redColorRandVal, greenColorRandVal, blueColorRandVal);
        }

        private static int GenerateRandomValue(Random random)
        {
            return (random.Next(0, 124) * 2) - 1;
        }
    }
   
    public class RandomHalfLightsMutator : IMutater<LightsIndividual>
    {
        private Random _random;

        public RandomHalfLightsMutator(Random random)
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
                    var newColor = RandomHalfLightsGenerator.GenerateRandomColor(_random);
                    individual.Colors[index] = newColor;
                }
            }
        }
    }

    public class RandomHalfLightsIndividualFactory : IIndividualFactory<LightsIndividual>
    {
        private Random _random;

        public RandomHalfLightsIndividualFactory(Random random)
        {
            _random = random;
        }

        public LightsIndividual GenerateIndividual()
        {
            var colorList = new List<Color>();
            for (int i = 0; i < 20; i++)
            {
                colorList.Add(RandomHalfLightsGenerator.GenerateRandomColor(_random));
            }
            
            return new LightsIndividual(colorList);
        }
    }
}