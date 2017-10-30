using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML;
using SFML.Window;

namespace TP3Galaga
{
    /// <summary>
    /// Gère les entrées du jeu
    /// </summary>
    public class Inputs
    {
        public bool LeftArrow
        {
            get { return Keyboard.IsKeyPressed(Keyboard.Key.Left); }
        }
        public bool RightArrow
        {
            get { return Keyboard.IsKeyPressed(Keyboard.Key.Right); }
        }
        public bool Spacebar
        {
            get { return Keyboard.IsKeyPressed(Keyboard.Key.Space); }
        }
    }
}
