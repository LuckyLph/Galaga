using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML;
using SFML.Window;
using SFML.System;
using SFML.Graphics;
using SFML.Audio;

namespace TP3Galaga
{
    class Program
    {
        static void Main(string[] args)
        {
            RenderWindow window = new RenderWindow(new SFML.Window.VideoMode(Constants.WINDOW_WIDTH, Constants.WINDOW_HEIGHT), Constants.GAME_NAME, Styles.Titlebar);
            Game app = new Game(window);
            //try { app.InitializeGame(); }
            //catch
            //{
            //    System.Windows.Forms.MessageBox.Show("Fichier corrompu ou manquant !", "Erreur de chargement des ressources !",
            //                                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
            //    Environment.Exit(1);
            //}
            app.InitializeGame();
            do
            {
                app.ResetGame();
            }
            while (app.Run());
        }
    }
}
