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
                    .WithMutater(new RandomHalfLightsMutator(Random));
                Evolver = EvolverBuilder.Build();
            }
        }
        
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetInitialPopulation()
        {
            Population = EvolverBuilder.GenerateInitialPopulation(20);
            return Json(Population.GetIndividuals(), JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public JsonResult GetNextPopulation()
        {
            Population = Evolver.GenerateNextPopulations(Population, 20);
            return Json(Population.GetIndividuals(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public HttpStatusCode SetTargetColor(int red, int green, int blue)
        {
            var defaultLightsEvoloverBuilder = (EvolverBuilder as DefaultLightsEvoloverBuilder);
            defaultLightsEvoloverBuilder.SetTargetColor(new Color(red, green, blue));
            Evolver = EvolverBuilder.Build();
            return HttpStatusCode.OK;
        }
        
        public string GetEvolutionDetails()
        {
            return Evolver.GetDetails();
        }
    }
}