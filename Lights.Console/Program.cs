using System;
using System.Collections.Generic;
using System.Text;

namespace Lights.Console
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            GenerateBinaryLights();
        }

        private static void GenerateBinaryLights()
        {
            var random = new Random();
            var factory = new BinaryLightsIndividualFactory(random);
            var formatter = new BinaryLightsPopulationFormatter();

            var population = Evolver<LightsIndividual>.GenerateInitialPopulation(factory,new SimpleEuclideanLightFitnessEvaluator(Color.NewRed()),  10);
            formatter.Format(population);

            var evolver = BinaryLightsEvololverFactory.Create();

            for (int i = 0; i < 50; i++)
            {
                population = evolver.GenerateNextPopulation(population);
            }

            formatter.Format(population);
        }
        
        private static void GenerateBinaryStrings()
        {
            var random = new Random();
            var binaryStringIndividualFactory = new BinaryStringIndividualFactory(random);
            var formatter = new BinaryStringPopulationFormatter();

            var population = Evolver<BinaryStringIndividual>.GenerateInitialPopulation(binaryStringIndividualFactory, new BinaryStringFitnessEvaluator(),  10);
            formatter.Format(population);

            var fitnessEvaluator = new BinaryStringFitnessEvaluator();
            var breeder = new BinaryStringBreeder();
            var mutator = new BinaryStringMutator(random);
            var crossoverSelector = new EliteBreedingSelector<BinaryStringIndividual>();

            var evolver = new Evolver<BinaryStringIndividual>(fitnessEvaluator, crossoverSelector, breeder, mutator);

            for (int i = 0; i < 50; i++)
            {
                population = evolver.GenerateNextPopulation(population);
            }

            formatter.Format(population);
        }
    }

    public class BinaryStringPopulationFormatter : IPopulationFormatter<BinaryStringIndividual>
    {
        public void Format(Population<BinaryStringIndividual> population)
        {
            foreach (var individual in population.GetIndividuals())
            {
                var binaryStringIndividual = individual as BinaryStringIndividual;
                System.Console.WriteLine(binaryStringIndividual.BinaryString);
            }

            System.Console.WriteLine();
            System.Console.WriteLine();
            System.Console.WriteLine();
            System.Console.WriteLine();
        }
    }
    
        
    public class BinaryLightsPopulationFormatter : IPopulationFormatter<LightsIndividual>
    {
        public void Format(Population<LightsIndividual> population)
        {
            foreach (var individual in population.GetIndividuals())
            {
                var sb = new StringBuilder();
                foreach (var color in individual.Colors)
                {
                    sb.Append(color.IsRed() ? "R" : "B");
                }
                
                System.Console.WriteLine(sb.ToString());
            }

            System.Console.WriteLine();
            System.Console.WriteLine();
            System.Console.WriteLine();
            System.Console.WriteLine();
        }
    }
    
    public interface IPopulationFormatter<T> where T : IIndividual
    {
        void Format(Population<T> population);
    }
}