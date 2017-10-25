using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lights
{
    public class Color
    {
        public int Red { get; set; }
        
        public int Green { get; set; }
        
        public int Blue { get; set; }

        public Color(int red, int green, int blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        public bool IsRed()
        {
            return this.Red == 255 && this.Green == 0 && this.Blue == 0;
        }

        public static Color NewRed()
        {
            return new Color(255, 0, 0);
        }

        public static Color NewBlue()
        {
            return new Color(0, 0, 255);
        }
    }
    
    public class BinaryLightsIndividual : IIndividual
    {
        public BinaryLightsIndividual(List<Color> colors)
        {
            Colors = colors;
        }
        
        public List<Color> Colors { get; }
        
        public int Fitness { get; set; }
    }

    public class BinaryLightsFitnessEvaluator : IFitnessEvaluator<BinaryLightsIndividual>
    {
        public int Evaluate(BinaryLightsIndividual individual)
        {
            return individual.Colors.Count(s => s.IsRed());
        }
    }
    
    public class BinaryLightsBreeder : IBreeder<BinaryLightsIndividual>
    {
        public BinaryLightsIndividual Breed(BinaryLightsIndividual parent1, BinaryLightsIndividual parent2)
        {
            var firstHalfOfParent1 =
                parent1.Colors.Where((_, i) => i < parent1.Colors.Count / 2);
            var secondHalfOfParent2 =
                parent2.Colors.Where((_, i) => i >= parent2.Colors.Count / 2);

            var newColors = firstHalfOfParent1.Concat(secondHalfOfParent2).ToList();
            
            return new BinaryLightsIndividual(newColors);

        }
    }

    public class BinaryLightsMutator : IMutater<BinaryLightsIndividual>
    {
        private Random _random;

        public BinaryLightsMutator(Random random)
        {
            _random = random;
        }

        public void Mutate(BinaryLightsIndividual individual)
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
    
    public class BinaryLightsIndividualFactory : IIndividualFactory<BinaryLightsIndividual>
    {
        private Random _random;

        public BinaryLightsIndividualFactory(Random random)
        {
            _random = random;
        }

        public BinaryLightsIndividual GenerateIndividual()
        {
            var colorList = new List<Color>();
            for (int i = 0; i < 20; i++)
            {
                colorList.Add(_random.Next(0, 2) == 0 ? Color.NewRed() : Color.NewBlue());
            }
            
            return new BinaryLightsIndividual(colorList);
        }
    }

    public class BinaryLightsEvololverFactory
    {
        public static Evolver<BinaryLightsIndividual> Create()
        {
            var random = new Random();
            var fitnessEvaluator = new BinaryLightsFitnessEvaluator();
            var breeder = new BinaryLightsBreeder();
            var mutator = new BinaryLightsMutator(random);
            var crossoverSelector = new EliteBreedingSelector<BinaryLightsIndividual>();

            return new Evolver<BinaryLightsIndividual>(fitnessEvaluator, crossoverSelector, breeder, mutator);
        }
    }
}