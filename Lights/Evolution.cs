using System;
using System.Collections.Generic;
using System.Linq;

namespace Lights
{
    public interface IIndividual
    {
    }

    public class Evolver<T> where T : IIndividual
    {
        private readonly IFitnessEvaluator<T> _fitnessEvaluator;
        private readonly ICrossoverSelector<T> _crossoverSelector;
        private readonly IBreeder<T> _breeder;
        private readonly IMutater<T> _mutater;
        private Random _random;

        public Evolver(IFitnessEvaluator<T> fitnessEvaluator, ICrossoverSelector<T> crossoverSelector, IBreeder<T> breeder, IMutater<T> mutater)
        {
            _fitnessEvaluator = fitnessEvaluator;
            _crossoverSelector = crossoverSelector;
            _breeder = breeder;
            _mutater = mutater;
            _random = new Random();
        }
        
        public Population<T> GenerateNextPopulation(Population<T> population)
        {
            var percentageToTakeForBreeding = 50;

            var individuals = population.GetIndividuals();
            var orderedPopulation = individuals.OrderByDescending(_fitnessEvaluator.Evaluate);
            
            var breedingPopulation =
                _crossoverSelector.ChooseCandidatesForCrossover(individuals, _fitnessEvaluator,
                    percentageToTakeForBreeding).ToArray();

            var percentageToRemove = 50;
            var populationCountToRemove = PopulationToTake(percentageToRemove, individuals.Count);
            var newPopulation = orderedPopulation.Reverse().Skip(populationCountToRemove).ToList();

            while (newPopulation.Count() < individuals.Count)
            {
                var parent1 = GetRandomParent(breedingPopulation);
                var parent2 = GetRandomParent(breedingPopulation);

                var newInidivual = _breeder.Breed(parent1, parent2);
                _mutater.Mutate(newInidivual);
            
                newPopulation.Add(newInidivual);
            }

            return new Population<T>(newPopulation);
        }

        private T GetRandomParent(T[] breedingPopulation)
        {
            var index1 = _random.Next(0, breedingPopulation.Count() - 1);
            var parent1 = breedingPopulation[index1];
            return parent1;
        }

        private static int PopulationToTake(int percentageToTake, int populationCount)
        {
            var populationToTakeDenominator = (100 / percentageToTake);
            var populationCountToTake = populationCount / populationToTakeDenominator;
            return populationCountToTake;
        }
    }

    public class Population<T> where T : IIndividual
    {
        private List<T> _individuals;
        private readonly Random _random;

        public Population(List<T> individuals)
        {
            _individuals = individuals;
            _random = new Random();
        }

        public List<T> GetIndividuals()
        {
            return _individuals;
        }
        
        public static Population<T> GenerateInitialPopulation(IIndividualFactory<T> individualFactory, int populationCount)
        {
            var individuals = new List<T>();
            while (individuals.Count() < populationCount)
            {
                var newIndividual = individualFactory.GenerateIndividual();
                individuals.Add(newIndividual);
            }
            
            return new Population<T>(individuals);
        }
    }
    
    public class TruncationCrossoverSelector<T> : ICrossoverSelector<T> where T : IIndividual
    {
        public IEnumerable<T> ChooseCandidatesForCrossover(IEnumerable<T> population, IFitnessEvaluator<T> fitnessEvaluator,
            int percentageToTake)
        {
            var orderedPopulation = population.OrderByDescending(fitnessEvaluator.Evaluate);
            var breedingPopulation = GetBreedingPopulation(percentageToTake, orderedPopulation).ToArray();

            return breedingPopulation;
        }
        
        private static IEnumerable<T> GetBreedingPopulation(int percentageToTake, IEnumerable<T> population)
        {
            var populationCountToTake = PopulationToTake(percentageToTake, population.Count());
            var breedingPopulation = population.Take(populationCountToTake);
            return breedingPopulation;
        }

        private static int PopulationToTake(int percentageToTake, int populationCount)
        {
            var populationToTakeDenominator = 100 / percentageToTake;
            var populationCountToTake = populationCount / populationToTakeDenominator;
            return populationCountToTake;
        }
    }

    public interface ICrossoverSelector<T> where T : IIndividual
    {
        IEnumerable<T> ChooseCandidatesForCrossover(IEnumerable<T> population,
            IFitnessEvaluator<T> fitnessEvaluator, int percentageToTake);
    }

    public interface IIndividualFactory<T> where T : IIndividual
    {
        T GenerateIndividual();
    }

    public interface IFitnessEvaluator<T> where T : IIndividual
    {
        int Evaluate(T individual);
    }

    public interface IBreeder<T> where T : IIndividual
    {
        T Breed(T parent1, T parent2);
    }

    public interface IMutater<T> where T : IIndividual
    {
        void Mutate(T individual);
    }
}