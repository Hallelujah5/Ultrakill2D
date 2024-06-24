using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;
using Rectangle = System.Drawing.Rectangle;


namespace Ultrakill2D
{
    public class BlackHole
    {
        private Point2D _point;
        private V1 _v1;
        private Effect _effect;
        private Bitmap _bitmap = new Bitmap("Blackhole", "F:\\Projects\\Ultrakill2D\\Ultrakill2D\\Resources\\images\\black_hole.png");
        public int _livetime = 0;
        private Rectangle _rect;
        public void Load(V1 v1,Effect effect,double x, double y)
        {
            _point.X = x;
            _point.Y = y;
            _v1 = v1;
            _effect = effect;
            _livetime = 60 * 20;
        }
        public void Draw()
        {
            if (_livetime > 0)
            {
                _livetime--;
                //
                SplashKit.DrawBitmap(_bitmap, _point.X, _point.Y, SplashKit.OptionScaleBmp(5, 5));
                //movement
                Vector2D targetV = new Vector2D() { X = _v1.X() - (_point.X + 32), Y = _v1.Y() - (_point.Y + 32) };
                targetV = SplashKit.VectorMultiply(targetV, 1 / SplashKit.VectorMagnitude(targetV));
                _point.X += targetV.X; _point.Y += targetV.Y;
                //
                _rect = new Rectangle(Convert.ToInt32(_point.X) - 64 * 2, Convert.ToInt32(_point.Y) - 64 * 2, 64 * 4, 64 * 4);
                if (_rect.IntersectsWith(_v1.Rect()))
                {
                    if (_v1._hp > 10)
                    {
                        _v1._harddamage = 99;
                        _v1._harddamage_timer = 60 * 2;
                    }
                    else { _v1._hp = 0; }
                    _livetime = 0;
                    //
                    Circle deadeffect = new Circle(_point.X + 64, _point.Y + 64, 10, 32*5, 10, SplashKit.RGBAColor(255, 0, 255, 150));
                    _effect.Add(deadeffect);

                }
            }
        }
    }
}
