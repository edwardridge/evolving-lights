using System;
using System.Collections.Generic;

namespace Lights.Console
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var random = new Random();
            var binaryStringIndividualFactory = new BinaryStringIndividualFactory(random);
            var formatter = new BinaryStringPopulationFormatter();
            
            var population = Population<BinaryStringIndividual>.GenerateInitialPopulation(binaryStringIndividualFactory , 10);
            formatter.Format(population);
            
            var fitnessEvaluator = new BinaryStringFitnessEvaluator();
            var breeder = new BinaryStringBreeder();
            var mutator = new BinaryStringMutator(random);
            var crossoverSelector = new TruncationCrossoverSelector<BinaryStringIndividual>();
            
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

    public interface IPopulationFormatter<T> where T : IIndividual
    {
        void Format(Population<T> population);
    }
}