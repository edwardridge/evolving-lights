using System;
using System.Linq;
using System.Text;

namespace Lights
{
    public class BinaryStringIndividual : IIndividual
    {
        public BinaryStringIndividual(string binaryString)
        {
            BinaryString = binaryString;
        }

        public string BinaryString { get; set; }
        
        public int Fitness { get; set; }
    }

    public class BinaryStringFitnessEvaluator : IFitnessEvaluator<BinaryStringIndividual>
    {
        public int Evaluate(BinaryStringIndividual individual)
        {
            return individual.BinaryString.Count(s => s == '1');
        }
    }
    
    public class BinaryStringBreeder : IBreeder<BinaryStringIndividual>
    {
        public BinaryStringIndividual Breed(BinaryStringIndividual parent1, BinaryStringIndividual parent2)
        {
            var firstHalfOfParent1 =
                parent1.BinaryString.Substring(0, (parent1.BinaryString.Length / 2));
            var secondHalfOfParent2 =
                parent2.BinaryString.Substring(parent2.BinaryString.Length / 2);

            var newString = firstHalfOfParent1 + secondHalfOfParent2;
            
            return new BinaryStringIndividual(newString);

        }
    }

    public class BinaryStringMutator : IMutater<BinaryStringIndividual>
    {
        private Random _random;

        public BinaryStringMutator(Random random)
        {
            _random = random;
        }

        public void Mutate(BinaryStringIndividual individual)
        {
            for (var index = 0; index < individual.BinaryString.Length; index++)
            {
                var letter = individual.BinaryString[index];
                var randVal = _random.Next(0, 100);
                if (randVal < 5)
                {
                    var newLetter = letter == '1' ? '0' : '1';
                    var sb = new StringBuilder(individual.BinaryString);
                    sb[index] = newLetter;
                    individual.BinaryString = sb.ToString();
                }
            }
        }
    }
    
    public class BinaryStringIndividualFactory : IIndividualFactory<BinaryStringIndividual>
    {
        private Random _random;

        public BinaryStringIndividualFactory(Random random)
        {
            _random = random;
        }

        public BinaryStringIndividual GenerateIndividual()
        {
            var stringBuilder = new StringBuilder();
            for (int i = 0; i < 20; i++)
            {
                var newChar = _random.Next(0, 2).ToString();
                stringBuilder.Append(newChar);
            }
            
            return new BinaryStringIndividual(stringBuilder.ToString());
        }
    }
}