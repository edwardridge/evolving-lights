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
        public static Population<BinaryLightsIndividual> Population;
        
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetInitialPopulation()
        {
            var random = new Random();
            var factory = new BinaryLightsIndividualFactory(random);
            
            Population = Population<BinaryLightsIndividual>.GenerateInitialPopulation(factory, 10);
            return Json(Population.GetIndividuals(), JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult GetNextPopulation()
        {
            var evolver = BinaryLightsEvololverFactory.Create();

            Population = evolver.GenerateNextPopulation(Population);
            return Json(Population.GetIndividuals(), JsonRequestBehavior.AllowGet);
        }
    }
}