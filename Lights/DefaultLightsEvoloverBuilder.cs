namespace Lights
{
    public class DefaultLightsEvoloverBuilder : EvolverBuilder<LightsIndividual>
    {
        public DefaultLightsEvoloverBuilder WithDefaults()
        {
            WithBreeder(new AlternatingLightsBreeder())
                .WithMutater(new RandomLightsMutator(random))
                .WithBreedingSelector(new EliteBreedingSelector<LightsIndividual>())
                .WithIndividualFactory(new RandomLightsIndividualFactory(random))
                .WithFitnessEvaluator(new SimpleEuclideanLightFitnessEvaluator(Color.NewRed()));

            return this;
        }

        public DefaultLightsEvoloverBuilder SetTargetColor(Color targetColor)
        {
            WithFitnessEvaluator(new SimpleEuclideanLightFitnessEvaluator(targetColor));
            return this;
        }
    }
}