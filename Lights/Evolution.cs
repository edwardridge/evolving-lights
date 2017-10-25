using System;
using System.Collections.Generic;
using System.Linq;

namespace Lights
{
    public interface IIndividual
    {
        int Fitness { get; set; }
    }

    public class Evolver<T> where T : IIndividual
    {
        private readonly IFitnessEvaluator<T> _fitnessEvaluator;
        private readonly IBreedingSelector<T> _breedingSelector;
        private readonly IBreeder<T> _breeder;
        private readonly IMutater<T> _mutater;
        private Random _random;
        
        public int PercentageForBreeding { get; set; } = 50;        
        public int PercentageToDie { get; set; } = 50;


        public Evolver(IFitnessEvaluator<T> fitnessEvaluator, IBreedingSelector<T> breedingSelector, IBreeder<T> breeder, IMutater<T> mutater)
        {
            _fitnessEvaluator = fitnessEvaluator;
            _breedingSelector = breedingSelector;
            _breeder = breeder;
            _mutater = mutater;
            _random = new Random();
        }
        
        public Population<T> GenerateNextPopulation(Population<T> population)
        {       
            var individuals = population.GetIndividuals();
            foreach (var individual in individuals)
            {
                individual.Fitness = _fitnessEvaluator.Evaluate(individual);
            }
            
            var breedingPopulation =
                _breedingSelector.ChooseCandidatesForBreeding(individuals, PercentageForBreeding).ToArray();

            var populationCountToRemove = PopulationToTake(PercentageToDie, individuals.Count);
            var newPopulation = individuals.OrderBy(o => o.Fitness).Skip(populationCountToRemove).ToList();

            while (newPopulation.Count < individuals.Count)
            {
                var parent1 = GetRandomParent(breedingPopulation);
                var parent2 = GetRandomParent(breedingPopulation);

                var newIndividual = _breeder.Breed(parent1, parent2);
                _mutater.Mutate(newIndividual);
            
                newPopulation.Add(newIndividual);
            }

            return new Population<T>(newPopulation);
        }

        private T GetRandomParent(T[] breedingPopulation)
        {
            var index = _random.Next(0, breedingPopulation.Length - 1);
            return breedingPopulation[index];
        }

        private static int PopulationToTake(int percentageToTake, int populationCount)
        {
            var populationCountToTake = populationCount / (100 / percentageToTake);
            return populationCountToTake;
        }
    }

    public class Population<T> where T : IIndividual
    {
        private List<T> _individuals;

        public Population(List<T> individuals)
        {
            _individuals = individuals;
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
    
    public class EliteBreedingSelector<T> : IBreedingSelector<T> where T : IIndividual
    {
        public IEnumerable<T> ChooseCandidatesForBreeding(IEnumerable<T> population, int percentageToTake)
        {
            var orderedPopulation = population.OrderByDescending(o => o.Fitness);
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

    public interface IBreedingSelector<T> where T : IIndividual
    {
        IEnumerable<T> ChooseCandidatesForBreeding(IEnumerable<T> population, int percentageToTake);
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