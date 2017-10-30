using System;
using System.Collections.Generic;
using System.Text;

namespace Lights
{
    public class FiveLightsColorGenerator
    {
        public static Color GetColor(int colorNum)
        {
            if (colorNum == 0) return Color.NewBlack();
            if (colorNum == 1) return Color.NewBlue();
            if (colorNum == 2) return Color.NewGreen();
            if (colorNum == 3) return Color.NewRed();
            if (colorNum == 4) return Color.NewWhite();
            throw new Exception();
        }
    } 
    
    public class FiveLightsMutator : IMutater<LightsIndividual>
    {
        private Random _random;

        public FiveLightsMutator(Random random)
        {
            _random = random;
        }

        public void Mutate(LightsIndividual individual)
        {
            for (var index = 0; index < individual.Colors.Count; index++)
            {
                var randVal = _random.Next(0, 100);
                if (randVal < 5)
                {
                    var colorRandVal = _random.Next(0, 5);
                    var newColor = FiveLightsColorGenerator.GetColor(colorRandVal);
                    individual.Colors[index] = newColor;
                }
            }
        }
    }
    
    public class FiveLightsIndividualFactory : IIndividualFactory<LightsIndividual>
    {
        private Random _random;

        public FiveLightsIndividualFactory(Random random)
        {
            _random = random;
        }

        public LightsIndividual GenerateIndividual()
        {
            var colorList = new List<Color>();
            for (int i = 0; i < 20; i++)
            {
                colorList.Add(FiveLightsColorGenerator.GetColor(_random.Next(0, 5)));
            }
            
            return new LightsIndividual(colorList);
        }
    }

    public class FiveLightsEvololverFactory
    {
        public static Evolver<LightsIndividual> Create()
        {
            var random = new Random();
            var fitnessEvaluator = new SimpleEuclideanLightFitnessEvaluator(Color.NewRed());
            var breeder = new HalfwayPointLightsBreeder();
            var mutator = new FiveLightsMutator(random);
            var crossoverSelector = new EliteBreedingSelector<LightsIndividual>();

            return new Evolver<LightsIndividual>(fitnessEvaluator, crossoverSelector, breeder, mutator);
        }
    }
}