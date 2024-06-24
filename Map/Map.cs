using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultrakill2D;
using Rectangle = System.Drawing.Rectangle;


namespace PNGtest2.Map
{
    public abstract class Obj
    {
        public abstract void Draw();
        public abstract Rectangle Rect();


    }
    public class FleshBlock : Obj
    {
        private Bitmap _block;
        private Point2D _point;
        private Rectangle _rect;
        public FleshBlock(double x, double y)  
        {
            _block = new Bitmap("FleshBlock", "F:\\Projects\\Ultrakill2D\\Ultrakill2D\\Resources\\images\\FleshBlock.png");
            _point.X = x;
            _point.Y = y;
            _rect = new Rectangle(Convert.ToInt32(_point.X), Convert.ToInt32(_point.Y), _block.Height, _block.Height);

        }
        public override void Draw()
        {
            _block.Draw(_point.X, _point.Y);
            //Console.WriteLine("the block size is " + _block.Height);
        }
        public override Rectangle Rect()
        {
           return _rect;
        }
    }
    public class DarkBlock : Obj
    {
        private Point2D _point;
        private Rectangle _rect;
        private Point2D _size;
        public DarkBlock(double x, double y, double sizeX, double sizeY)
        {
            _point.X = x;
            _point.Y = y;
            _size.X = sizeX;
            _size.Y = sizeY;
            _rect = new Rectangle(Convert.ToInt32(_point.X), Convert.ToInt32(_point.Y), Convert.ToInt32(_size.X), Convert.ToInt32(_size.Y));

        }
        public override void Draw()
        {
            SplashKit.FillRectangle(Color.Black, _point.X,_point.Y, _size.X,_size.Y);
            //Console.WriteLine("the block size is " + _block.Height);
        }
        public override Rectangle Rect()
        {
            return _rect;
        }
    }
    public class Terminal : Obj
    {
        private Bitmap _block;
        private Point2D _point;
        private Rectangle _rect;
        public Terminal(double x, double y)
        {
            _block = new Bitmap("Terminal", "F:\\Projects\\Ultrakill2D\\Ultrakill2D\\Resources\\images\\terminal.png");
            _point.X = x;
            _point.Y = y;
            _rect = new Rectangle(0, 0, 0, 0);

        }
        public override void Draw()
        {
            _block.Draw(_point.X, _point.Y);
            //Console.WriteLine("the block size is " + _block.Height);
        }
        public override Rectangle Rect()
        {
            return _rect;
        }
    }


}
