using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lights
{
    public interface IIndividual : ICloneable
    {
        double Fitness { get; set; }
    }

    public class Evolver<T> where T : class, IIndividual
    {
        public IFitnessEvaluator<T> FitnessEvaluator { get; }
        private readonly IBreedingSelector<T> _breedingSelector;
        private readonly IBreeder<T> _breeder;
        private readonly IMutater<T> _mutater;
        private Random _random;
        
        public int PercentageForBreeding { get; set; } = 50;        
        public int PercentageChildrenToBreed { get; set; } = 50;


        public Evolver(IFitnessEvaluator<T> fitnessEvaluator, IBreedingSelector<T> breedingSelector, IBreeder<T> breeder, IMutater<T> mutater)
        {
            FitnessEvaluator = fitnessEvaluator;
            _breedingSelector = breedingSelector;
            _breeder = breeder;
            _mutater = mutater;
            _random = new Random();
        }

        public Population<T> GenerateNextPopulations(Population<T> population, int numberOfPopulations)
        {
            for (var i = 0; i < numberOfPopulations; i++)
            {
                population = GenerateNextPopulation(population);
            }

            return population;
        }
        
        public Population<T> GenerateNextPopulation(Population<T> population)
        {       
            var individuals = population.GetIndividuals();
            population.SetFitness(FitnessEvaluator);
            
            var breedingPopulation =
                _breedingSelector.ChooseCandidatesForBreeding(population, PercentageForBreeding);

            var populationCountToRemove = PopulationToTake(PercentageChildrenToBreed, individuals.Count);
            
            var children = GenerateChildren(populationCountToRemove, breedingPopulation);
            children.Mutate(_mutater);

            population = SelectFittestFromOriginalAndChildren(population, children);

            return population;
        }

        public Population<T> MultipleMutateAndReplace(Population<T> population, int numberOfPopulations)
        {
            for (int i = 0; i < numberOfPopulations; i++)
            {
                population = MutateAndReplaceIndividualIfFitter(population);
            }

            return population;
        }
        
        //Todo: Move out?
        public Population<T> MutateAndReplaceIndividualIfFitter(Population<T> population)
        {
            var individuals = population.GetIndividuals();
            for (int i = 0; i < individuals.Count; i++)
            {
                var originalInidividual = individuals[i];
                var newIndividual = originalInidividual.Clone() as T;
                _mutater.Mutate(newIndividual);
                newIndividual.Fitness = FitnessEvaluator.Evaluate(newIndividual);
                if (newIndividual.Fitness > originalInidividual.Fitness)
                {
                    individuals[i] = newIndividual;
                }
            }
            return population;
        }
        
        public static Population<T> GenerateInitialPopulation(IIndividualFactory<T> individualFactory, IFitnessEvaluator<T> fitnessEvaluator, int populationCount)
        {
            var individuals = new List<T>();
            while (individuals.Count() < populationCount)
            {
                var newIndividual = individualFactory.GenerateIndividual();
                newIndividual.Fitness = fitnessEvaluator.Evaluate(newIndividual);
                individuals.Add(newIndividual);
            }
            
            return new Population<T>(individuals);
        }

        private Population<T> SelectFittestFromOriginalAndChildren(Population<T> originalPopulation, Population<T> newPopulation)
        {
            var originalCount = originalPopulation.GetIndividuals().Count;
            originalPopulation.Combine(newPopulation);
            originalPopulation.SetFitness(FitnessEvaluator);
            originalPopulation.SelectFittest(originalCount);
            return originalPopulation;
        }

        private Population<T> GenerateChildren(int populationCountToRemove, Population<T> breedingPopulation)
        {
            var newPopulation = new Population<T>();
            for (var i = 0; i < populationCountToRemove; i++)
            {
                var parent1 = GetRandomParent(breedingPopulation);
                var parent2 = GetRandomParent(breedingPopulation);

                var newIndividual = _breeder.Breed(parent1, parent2);

                newPopulation.Add(newIndividual);
            }
            
            return newPopulation;
        }

        private T GetRandomParent(Population<T> breedingPopulation)
        {
            var individuals = breedingPopulation.GetIndividuals();
            var index = _random.Next(0, individuals.Count - 1);
            return individuals[index];
        }

        private static int PopulationToTake(int percentageToTake, int populationCount)
        {
            if (percentageToTake == 0)
            {
                return 0;
            }
            var populationCountToTake = populationCount / (100 / percentageToTake);
            return populationCountToTake;
        }

        public string GetDetails()
        {
            var details = new StringBuilder();           
            details.AppendLine("Population type: " + this.GetType().GetGenericArguments().First().Name);
            details.AppendLine("Fitness evaluator: " + FitnessEvaluator.GetType().Name);
            details.AppendLine("Breeding selector: " + _breedingSelector.GetType().Name);
            details.AppendLine("Breeder: " + _breeder.GetType().Name);
            details.AppendLine("Mutater: " + _mutater.GetType().Name);
            details.AppendLine("Percentage For Breeding: " + PercentageForBreeding);
            details.AppendLine("Percentage Children: " + PercentageChildrenToBreed);

            return details.ToString();
        }
    }

    public class Population<T> where T : IIndividual
    {
        private List<T> _individuals;

        public Population(List<T> individuals)
        {
            _individuals = individuals;
        }

        public Population()
        {
            _individuals = new List<T>();
        }

        public List<T> GetIndividuals()
        {
            return _individuals;
        }

        public void SetFitness(IFitnessEvaluator<T> fitnessEvaluator)
        {
            foreach (var individual in this._individuals)
            {
                individual.Fitness = fitnessEvaluator.Evaluate(individual);
            }
        }

        public void Combine(Population<T> newPopulation)
        {
            this._individuals.AddRange(newPopulation._individuals);
        }

        public void Add(T newIndividual)
        {
            this._individuals.Add(newIndividual);
        }
    
        public void SelectFittest(int originalCount)
        {
            this._individuals = this._individuals.OrderByDescending(f => f.Fitness).Take(originalCount).ToList();
        }

        public void Mutate(IMutater<T> mutater)
        {
            foreach (var individual in _individuals)
            {
                mutater.Mutate(individual);
            }
        }
    }
    
    public class EliteBreedingSelector<T> : IBreedingSelector<T> where T : IIndividual
    {
        public Population<T> ChooseCandidatesForBreeding(Population<T> population, int percentageToTake)
        {
            var orderedPopulation = population.GetIndividuals().OrderByDescending(o => o.Fitness);
            var breedingPopulation = GetBreedingPopulation(percentageToTake, orderedPopulation).ToArray();

            return new Population<T>(breedingPopulation.ToList());
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
        Population<T> ChooseCandidatesForBreeding(Population<T> population, int percentageToTake);
    }

    public interface IIndividualFactory<T> where T : IIndividual
    {
        T GenerateIndividual();
    }

    public interface IFitnessEvaluator<T> where T : IIndividual
    {
        double Evaluate(T individual);
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