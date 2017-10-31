using System;

namespace Lights
{
    public abstract class EvolverBuilder<T> where T : IIndividual
    {
        protected Random random;
        private IFitnessEvaluator<T> fitnessEvaluator;
        private IBreeder<T> breeder;
        private IMutater<T> mutator;
        private IBreedingSelector<T> breedingSelector;
        private IIndividualFactory<T> individualFactory;
        
        public EvolverBuilder()
        {
            random = new Random();
        }

        public EvolverBuilder<T> WithFitnessEvaluator(IFitnessEvaluator<T> fitnessEvaluator)
        {
            this.fitnessEvaluator = fitnessEvaluator;
            return this;
        }
        
        public EvolverBuilder<T> WithBreeder(IBreeder<T> breeder)
        {
            this.breeder = breeder;
            return this;
        }
        
        public EvolverBuilder<T> WithMutater(IMutater<T> mutator)
        {
            this.mutator = mutator;
            return this;
        }
        
        public EvolverBuilder<T> WithBreedingSelector(IBreedingSelector<T> breedingSelector)
        {
            this.breedingSelector = breedingSelector;
            return this;
        }
        
        public EvolverBuilder<T> WithIndividualFactory(IIndividualFactory<T> individualFactory)
        {
            this.individualFactory = individualFactory;
            return this;
        }
        
        public Population<T> GenerateInitialPopulation(int populationCount)
        {
            return Population<T>.GenerateInitialPopulation(individualFactory, populationCount);
        }

        public Evolver<T> Build()
        {
            return new Evolver<T>(fitnessEvaluator, breedingSelector, breeder, mutator);
        }
    }
}