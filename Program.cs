using SplashKitSDK;
using System.Security.Cryptography.X509Certificates;
using Game_Menu;


namespace Ultrakill2D
{
    public class Program
    {

        public static void Main(string[] args)
        {
            Window gameWindow = new Window("Ultrakill2D", 1280, 700);

            int Page = 1;
            Menu menu = new Menu();
            Game game = new Game();

            DrawingOptions opts = new DrawingOptions(); 
            opts.ScaleX = 3;
            opts.ScaleY = 3;

           do
           {    
                SplashKit.ProcessEvents();
                
                switch (Page)
                {
                    case 1:
                        menu.Menu_main(gameWindow, opts);
                        if (SplashKit.KeyDown(KeyCode.RKey))
                        {
                            Page = 2;
                        }
                        break;
                    case 2:
                        game.Game_main(gameWindow);
                        

                        break;
                    default:
                        Console.WriteLine("Blank Window");
                        break;
                }
                gameWindow.Refresh(1);

            } while (!gameWindow.CloseRequested);
            
            
           SplashKit.CloseWindow(gameWindow);
         
            
            

        }
    }
}
