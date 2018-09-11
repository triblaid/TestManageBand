using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestTask.AlgorithmPoligon;
using TestTask.Models;

namespace TestTask.Controllers
{
    public class HomeController : Controller
    {
        //Раздел работы с Ajax
        [HttpPost]
        public ActionResult CreatePoints(int countPoints)
        {
            ViewBag.countPoints = countPoints;
            return PartialView("CreatePoints");
        }

        [HttpPost]
        public ActionResult AnalysisPolygon(PointsModel pointsModel)
        {
            Polygon polygon = new Polygon(pointsModel.Points);

            float x = pointsModel.CPX;
            float y = pointsModel.CPY;

            Point p = new Point((int)x, (int)y);

            if (touch(polygon, p))
            {
                return Json(
                new
                {
                    IsSuccess = true,
                    Message = "Точка находится в полигоне"
                });
            }
            else
                return Json(
                new
                {
                    IsSuccess = true,
                    Message = "Точка находится вне полигона"
                });
        }

        private bool touch(Polygon polygon, Point a) //проверка попадания точки в Polygon
        {
            Polygon.PointInPolygon touch = polygon.pointInPolygon(a);
            return touch == Polygon.PointInPolygon.INSIDE || touch == Polygon.PointInPolygon.BOUNDARY;
        }


        public ActionResult Index()
        {
            return View();
        }
    }
}