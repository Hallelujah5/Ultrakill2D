using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ultrakill2D
{
    public class Shader
    {
        private int _Xcolumn;
        private int _Ycolumn;
        private List<shader_cell> _cells;
        private List<light_cell> _lightcells;
        private Boolean DataLoaded = false;
        private Point2D start_point;
        private Point2D spare_point;


        //private List<shader_cell> _lightcells;
        public void shader_load(Window window, double start_x, double start_y)
        {

            start_point.X = Math.Round(start_x / 10.0);
            start_point.Y = Math.Round(start_y / 10.0);

            spare_point.X = start_x - start_point.X * 10;
            spare_point.Y = start_y - start_point.Y * 10;

            _Xcolumn = window.Width / 10;
            _Ycolumn = window.Height / 10;

            _cells = new List<shader_cell>();
            for (double x = 0; x < _Xcolumn; x++)
            {
                for (double y = 0; y < _Ycolumn; y++)
                {
                    shader_cell newCell = new shader_cell();
                    newCell.store_cell(start_point.X + x, start_point.Y + y, 255);
                    _cells.Add(newCell);
                }
            }
            DataLoaded = true;

        }
        public void draw_loadcell()
        {
            //Console.WriteLine("test");
            foreach (shader_cell cell in _cells)
            {
                cell.Draw(spare_point);
            }
        }
        public void set_lighBud(double x, double y, double radius, Window gameWindow)
        {
            light_bud cell = new light_bud();
            _lightcells = cell.LMain(x, y, radius, gameWindow, start_point);


            foreach (light_cell lightCell in _lightcells)
            {
                double doubleValue = (_Ycolumn) * (lightCell.X - start_point.X) + (lightCell.Y - start_point.Y);
                int i = Convert.ToInt32(doubleValue);
                //Console.WriteLine("the lcation of the cell: " + lightCell.X + "||"+ lightCell.Y + "||"+ i);
                if (lightCell.OnWindow == true)
                {
                    if (_cells[i].Color.A > lightCell.Color.A)
                    {
                        _cells[i].adjust_cell(lightCell.Color);
                    }

                }

            }


        }

    }
    public class shader_cell
    {
        private Point2D _point;
        private int _BaseAlpha;
        private Color _color;

        public void Draw(Point2D spare_point)
        {
            SplashKit.FillRectangle(_color, _point.X * 10 + spare_point.X, _point.Y * 10 + spare_point.Y, 10, 10);
        }

        public void store_cell(double x, double y, int alpha)
        {
            _point.X = x;
            _point.Y = y;
            _BaseAlpha = alpha;
            _color = Color.RGBAColor(0, 0, 0, _BaseAlpha);

        }
        public void adjust_cell(Color color)
        {
            _color = color;

        }
        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        public Point2D cell_location
        {
            get { return _point; }
        }

    }

    public class light_bud
    {
        private Point2D _center;
        private double radius;
        //
        private List<light_cell> check_list;
        private light_cell _1stCheckCell = new light_cell();

        public void SetData(double x, double y, double r)
        {
            _center.X = Math.Round(x / 10.0);
            _center.Y = Math.Round(y / 10.0);
            radius = Math.Round(r / 10.0);
        }
        public void SetCheckList(Window gameWindow, Point2D start_point)
        {
            _1stCheckCell.X = _center.X - radius;
            _1stCheckCell.Y = _center.Y - radius;

            check_list = new List<light_cell>();

            for (int x = 0; x < radius * 2; x++)
            {
                for (int y = 0; y < radius * 2; y++)
                {
                    light_cell Cell = new light_cell();
                    Cell.X = _1stCheckCell.X + x;
                    Cell.Y = _1stCheckCell.Y + y;
                    if (Cell.X < 0 + start_point.X || Cell.X > (gameWindow.Width - 10 + start_point.X * 10) / 10 || Cell.Y < 0 + start_point.Y || Cell.Y > (gameWindow.Height - 10 + start_point.Y * 10) / 10)
                    {
                        Cell.OnWindow = false;
                    }
                    check_list.Add(Cell);

                }
            }
        }
        public void chooseCell(Window gameWindow, Point2D start_point)
        {
            SetCheckList(gameWindow, start_point);

            for (int i = check_list.Count - 1; i >= 0; i--)
            {

                double distance = distanceToCenter(check_list[i]);
                if (distance > radius)  
                {
                    check_list.RemoveAt(i);
                }
                else if (distance >= radius / 100 * 80)
                {
                    check_list[i].Color = Color.RGBAColor(0, 0, 0, 200);
                }
                else if (distance <= radius / 100 * 80 && distance >= radius / 100 * 60)
                {
                    check_list[i].Color = Color.RGBAColor(0, 0, 0, 100);
                }
                else if (distance <= radius / 100 * 60)
                {
                    check_list[i].Color = Color.RGBAColor(0, 0, 0, 0);
                }

            }
        }
        public double distanceToCenter(light_cell cell)
        {
            double distance = Math.Sqrt(Math.Pow(cell.X - _center.X, 2) + Math.Pow(cell.Y - _center.Y, 2));

            return distance;
        }

        public List<light_cell> LMain(double x, double y, double radius, Window gameWindow, Point2D start_point)
        {
            SetData(x, y, radius);
            chooseCell(gameWindow, start_point);
            return check_list;
        }
    }
    public class light_cell
    {
        private double _x;
        private double _y;
        private Color _color;
        private Boolean _onWindow = true;

        public double X
        {
            get { return _x; }
            set { _x = value; }
        }
        public double Y
        {
            get { return _y; }
            set { _y = value; }
        }
        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        public Boolean OnWindow
        {
            get { return _onWindow; }
            set { _onWindow = value; }
        }

    }
}
