using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace Lights.Client.Controllers
{
    public class HomeController : Controller
    {
        public static Population<LightsIndividual> Population;
        public IIndividualFactory<LightsIndividual> Factory;
        public Evolver<LightsIndividual> Evolver;

        public HomeController()
        {
            var random = new Random();
            Factory = new RandomLightsIndividualFactory(random);
            Evolver = RandomLightsEvololverFactory.Create();
        }
        
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetInitialPopulation()
        {
            Population = Population<LightsIndividual>.GenerateInitialPopulation(Factory, 10);
            return Json(Population.GetIndividuals(), JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult GetNextPopulation()
        {
            Population = Evolver.GenerateNextPopulations(Population, 10);
            return Json(Population.GetIndividuals(), JsonRequestBehavior.AllowGet);
        }
        
        public string GetEvolutionDetails()
        {
            return Evolver.GetDetails();
        }
    }
}