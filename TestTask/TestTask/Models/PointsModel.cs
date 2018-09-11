using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestTask.Models
{
    public class PointsModel
    {
        //массив точек полигона
        public int[] Points { get; set; }
        //координата x контрольной очки
        public int CPX { get; set; }
        //координата y контрольной очки
        public int CPY { get; set; }
    }
}