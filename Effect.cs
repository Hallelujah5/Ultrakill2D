using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;

namespace Ultrakill2D
{
    public class Effect
    {
        private List<EffectType> _effects; //(tip) List = array but with flex length

        public Effect()
        {
            _effects = new List<EffectType>();// creating array
        }

        public void Add(EffectType item)
        {
            _effects.Add(item); // add folder or file
        }

        public void Draw() //print out all what it have inside the array
        {
            for (int i = _effects.Count - 1; i >= 0; i--)
            {
                EffectType item = _effects[i];
                if (item.Remove())
                {
                    _effects.RemoveAt(i);
                }
                else
                {
                    item.Draw();
                }
            }
        }
    }
    public abstract class EffectType()
    {
        public abstract void Draw();
        public abstract Boolean Remove();


    }
    public class Circle : EffectType
    {
        private Point2D _point;
        private int _timer;
        private double _r; 
        private double _e;
        private Color _color;
        private int _index =0;
        private Boolean _remove = false;
        public Circle(double x,double y,int time,double r, double expand,Color color)
        {
            _point.X = x;
            _point.Y = y;
            _r = r;
            _e = expand;
            _timer = time;
            _color = color;
        }

        public override void Draw()
        {
            if (_timer <= 0) { _remove = true; }
            else
            {
                SplashKit.FillCircle(_color, _point.X, _point.Y, _r + _index * _e);
                _index++;
                _timer--;
            }
        }
        public override Boolean Remove() { return _remove; }
    }
    public class Pillar : EffectType
    {
        private Point2D _point;
        private int _timer;
        private double _w;
        private double _r;
        private double _e;
        private Color _color;
        private int _index = 0;
        private int _delay = 0;
        private Boolean _remove = false;
        public Pillar(double x, double y, int time,int delay, double w, double r, double expand, Color color)
        {
            _point.X = x;
            _point.Y = y;
            _w = w;
            _r = r;
            _e = expand;
            _timer = time;
            _delay = delay;
            _color = color;
        }

        public override void Draw()
        {
            if (_timer <= 0) { _remove = true; }
            else
            {
                SplashKit.FillRectangle(_color, _point.X, _point.Y, _r + _index * _e, _w);
                SplashKit.FillRectangle(_color, _point.X, _point.Y, -(_r + _index * _e), _w);
                if (_delay > 0)
                {
                    _delay--;
                }
                else
                {
                    _index++;
                    _timer--;
                }
                
            }
        }
        public override Boolean Remove() { return _remove; }
    }

}
