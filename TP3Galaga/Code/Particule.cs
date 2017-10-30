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
    public class Particule
    {
        private Vector2f position = new Vector2f(-1.0f, -1.0f);
        private float speed = 0.0f;
        private float sizeHeight = 0.0f;

        public float XPosition
        {
            get { return position.X; }
            set { position.X = value; }
        }

        public float YPosition
        {
            get { return position.Y; }
            set { position.Y = value; }
        }

        public Particule(float xPosition, float yPosition, float speed, float sizeHeight)
        {
            position.X = xPosition;
            position.Y = yPosition;
            this.speed = speed;
            this.sizeHeight = sizeHeight;
        }

        public void Draw(RenderWindow window)
        {
            RectangleShape rectShape = new RectangleShape(new Vector2f(Constants.PARTICULE_WIDTH_SIZE, sizeHeight));
            rectShape.Position = new Vector2f(position.X, position.Y);
            rectShape.Origin = new Vector2f(5.0f / 2.0f, 5.0f / 2.0f);
            rectShape.FillColor = Color.White;
            window.Draw(rectShape);
        }


        public void Update(float deltaTime)
        {
            position.Y += speed * deltaTime;
        }
    }
}
