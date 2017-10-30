using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace TP3Galaga
{
    public class Hero
    {
        //Vitesse à laquelle le héro se déplace.
        private const float HERO_SPEED = 250.0f;
        //RectangleShape du héro.
        private RectangleShape heroShape = null;

        /// <summary>
        /// Largeur du héro en pixel
        /// </summary>
        public float HeroWidth
        {
            get;
            private set;
        }
        public float HeroHeight
        {
            get;
            private set;
        }
        public float XPosition
        {
            get;
            private set;
        }
        public float YPosition
        {
            get;
            private set;
        }
        public int ShootCounter
        {
            get;
            set;
        }

        /// <summary>
        /// Constructeur du héro
        /// </summary>
        /// <param name="initialXPosition">The initial x position.</param>
        /// <param name="initialYPosition">The initial y position.</param>
        public Hero(float initialXPosition, float initialYPosition)
        {
            XPosition = initialXPosition;
            YPosition = initialYPosition;
            heroShape = new RectangleShape();
            heroShape.Texture = new Texture("Data\\Arts\\New Art\\hero.png");
            heroShape.Size = new Vector2f(heroShape.Texture.Size.X * Constants.GAME_HERO_SCALE, heroShape.Texture.Size.Y * Constants.GAME_HERO_SCALE);
            HeroWidth = heroShape.Texture.Size.X * Constants.GAME_HERO_SCALE;
            heroShape.Origin = new Vector2f(HeroWidth / 2, HeroHeight / 2);
            ShootCounter = Constants.HERO_SHOOT_DELAY;
        }

        /// <summary>
        /// Met à jour le héro
        /// </summary>
        /// <param name="deltaTime">The delta time.</param>
        public void Update(float deltaTime, bool keyLeft, bool keyRight)
        {
            HeroMovement(deltaTime, keyLeft, keyRight);
            heroShape.Position = new Vector2f(XPosition, YPosition);
        }

        /// <summary>
        /// Dessine le héro dans la RenderWindow prise en paramètre
        /// </summary>
        /// <param name="window">The window.</param>
        public void Draw(RenderWindow window)
        {
            window.Draw(heroShape);
        }

        /// <summary>
        /// Retourne le rectangle qui représente le héro
        /// </summary>
        /// <returns></returns>
        public RectangleShape GetRect()
        {
            RectangleShape getRect = new RectangleShape(new Vector2f(heroShape.Size.X, heroShape.Size.Y - 20));
            getRect.Origin = heroShape.Origin;
            getRect.Position = new Vector2f(heroShape.Position.X, heroShape.Position.Y + 30);
            return getRect;
        }

        /// <summary>
        /// Gère le mouvement
        /// </summary>
        /// <param name="deltaT">The delta t.</param>
        private void HeroMovement(float deltaT, bool keyLeft, bool keyRight)
        {
            if (keyLeft)
            {
                if (XPosition >= (HeroWidth / 2) + (HERO_SPEED * deltaT))
                    XPosition -= (HERO_SPEED * deltaT);
                else
                    //Pour ajouter le héro parfaitement avec le bord gauche
                    XPosition = HeroWidth / 2;
            }

            else if (keyRight)
            {
                if (XPosition <= Constants.WINDOW_WIDTH - (HeroWidth / 2) - (HERO_SPEED * deltaT))
                    XPosition += (HERO_SPEED * deltaT);
                else
                    //Pour ajuster le héro parfaitement avec le bord droite
                    XPosition = Constants.WINDOW_WIDTH - (HeroWidth / 2);
            }
        }
    }
}
