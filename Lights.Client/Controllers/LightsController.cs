using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace Lights.Client.Controllers
{
    public class LightsController : Controller
    {
        public static Population<LightsIndividual> Population;        
        public static Random Random = new Random();

        public static Evolver<LightsIndividual> Evolver;
        public static EvolverBuilder<LightsIndividual> EvolverBuilder;

        public LightsController()
        {
            if (EvolverBuilder == null)
            {
                EvolverBuilder = new DefaultLightsEvoloverBuilder()
                    .WithDefaults()
                    .WithMutater(new SlightChangeLightsMutator(Random));
                Evolver = EvolverBuilder.Build();
            }
        }
        
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetInitialPopulation()
        {
            Population = EvolverBuilder.GenerateInitialPopulation(50);
            return Json(Population.GetIndividuals(), JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public JsonResult GetNextPopulation(int populations)
        {
//            Population = Evolver.GenerateNextPopulations(Population, populations);
            Population = Evolver.MultipleMutateAndReplace(Population, populations);
            //MultipleMutateAndReplace
            return Json(Population.GetIndividuals(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public HttpStatusCode SetTargetColor(int red, int green, int blue)
        {
            var fitnesEvaluator = (Evolver.FitnessEvaluator as SimpleEuclideanLightFitnessEvaluator);
            fitnesEvaluator.TargetColor = new Color(red, green, blue);
            Population.SetFitness(fitnesEvaluator);
            return HttpStatusCode.OK;
        }
        
        public JsonResult GetTargetColor()
        {
            var fitnesEvaluator = (Evolver.FitnessEvaluator as SimpleEuclideanLightFitnessEvaluator);
            return Json(fitnesEvaluator.TargetColor, JsonRequestBehavior.AllowGet);
        }
        
        public string GetEvolutionDetails()
        {
            return Evolver.GetDetails();
        }
    }
}