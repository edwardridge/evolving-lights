using System;
using System.Collections.Generic;
using System.Text;

namespace Lights
{
    public class BinaryLightsMutator : IMutater<LightsIndividual>
    {
        private Random _random;

        public BinaryLightsMutator(Random random)
        {
            _random = random;
        }

        public void Mutate(LightsIndividual individual)
        {
            for (var index = 0; index < individual.Colors.Count; index++)
            {
                var color = individual.Colors[index];
                var randVal = _random.Next(0, 100);
                if (randVal < 5)
                {
                    var newColor = color.IsRed() ? Color.NewBlue() : Color.NewRed();
                    individual.Colors[index] = newColor;
                }
            }
        }
    }
    
    public class BinaryLightsIndividualFactory : IIndividualFactory<LightsIndividual>
    {
        private Random _random;

        public BinaryLightsIndividualFactory(Random random)
        {
            _random = random;
        }

        public LightsIndividual GenerateIndividual()
        {
            var colorList = new List<Color>();
            for (int i = 0; i < 20; i++)
            {
                colorList.Add(_random.Next(0, 2) == 0 ? Color.NewRed() : Color.NewBlue());
            }
            
            return new LightsIndividual(colorList);
        }
    }

    public class BinaryLightsEvololverFactory
    {
        public static Evolver<LightsIndividual> Create()
        {
            var random = new Random();
            var fitnessEvaluator = new LightIsRedFitnessEvaluator();
            var breeder = new HalfwayPointLightsBreeder();
            var mutator = new BinaryLightsMutator(random);
            var crossoverSelector = new EliteBreedingSelector<LightsIndividual>();

            return new Evolver<LightsIndividual>(fitnessEvaluator, crossoverSelector, breeder, mutator);
        }
    }
}