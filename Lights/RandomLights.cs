using System;
using System.Collections.Generic;
using System.Text;

namespace Lights
{
    public class RandomLightsGenerator
    {
        public static Color GenerateRandomColor(Random random)
        {
            var redColorRandVal = random.Next(0, 255);
            var greenColorRandVal = random.Next(0, 255);
            var blueColorRandVal = random.Next(0, 255);
            return new Color(redColorRandVal, greenColorRandVal, blueColorRandVal);
        }
    }
   
    public class RandomLightsMutator : IMutater<LightsIndividual>
    {
        private Random _random;

        public RandomLightsMutator(Random random)
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
                    var newColor = RandomLightsGenerator.GenerateRandomColor(_random);
                    individual.Colors[index] = newColor;
                }
            }
        }
    }
    
    public class RandomLightsIndividualFactory : IIndividualFactory<LightsIndividual>
    {
        private Random _random;

        public RandomLightsIndividualFactory(Random random)
        {
            _random = random;
        }

        public LightsIndividual GenerateIndividual()
        {
            var colorList = new List<Color>();
            for (int i = 0; i < 20; i++)
            {
                colorList.Add(RandomLightsGenerator.GenerateRandomColor(_random));
            }
            
            return new LightsIndividual(colorList);
        }
    }

    public class RandomLightsEvololverFactory
    {
        public static Evolver<LightsIndividual> Create()
        {
            var random = new Random();
            var fitnessEvaluator = new SimpleLABLightFitnessEvaluator(Color.NewRed());
            var breeder = new LightsBreeder();
            var mutator = new RandomLightsMutator(random);
            var crossoverSelector = new EliteBreedingSelector<LightsIndividual>();

            return new Evolver<LightsIndividual>(fitnessEvaluator, crossoverSelector, breeder, mutator);
        }
    }
}