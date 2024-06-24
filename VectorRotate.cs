using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;

namespace Ultrakill2D
{
    public class VectorRotate
    {
        public VectorRotate(Vector2D V, double i) 
        {
            double angle = 180/ i;
            angle = Math.PI / angle;
            V.X = V.X * Math.Cos(angle) - V.Y * Math.Sin(angle);
            V.Y = V.X * Math.Sin(angle) + V.Y * Math.Cos(angle);
            //Console.WriteLine(V.X + " || "+ V.Y);

            returnV(V);
        }
        private Vector2D returnV(Vector2D V) 
        {
            return V;
        }
    }
}
