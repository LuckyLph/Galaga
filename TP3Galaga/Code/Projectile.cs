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
    public class Projectile
    {
        private CircleShape projShape = null;
        private CharacterType type;
        private Vector2f position;

        public float Speed
        {
            get;
            set;
        }

        public float XPosition
        {
            get { return position.X; }
        }

        public float YPosition
        {
            get { return position.Y; }
        }


        public Projectile(float xPosition, float yPosition, float speed, CharacterType type)
        {
            projShape = new CircleShape(Constants.PROJECTILE_SIZE);
            position.X = xPosition;
            position.Y = yPosition;
            this.Speed = speed;
            this.type = type;
            projShape.Origin = new Vector2f(Constants.PROJECTILE_SIZE * 0.5f, Constants.PROJECTILE_SIZE * 0.5f);
            if (type == CharacterType.Hero)
                projShape.FillColor = Color.Yellow;
            else
                projShape.FillColor = Color.Red;
        }


        public void Draw(RenderWindow window)
        {
            projShape.Position = new Vector2f(position.X, position.Y);
            window.Draw(projShape);
        }

        public void Update(float deltaTime)
        {
            if (type == CharacterType.Hero)
            {
                position.Y -= Speed * deltaTime;
            }
            else if (type == CharacterType.Enemy)
            {
                position.Y += Speed * deltaTime;
            }
        }
        public RectangleShape GetRect()
        {
            RectangleShape rectShape = new RectangleShape(new Vector2f(Constants.PROJECTILE_SIZE * 2, Constants.PROJECTILE_SIZE * 2));
            rectShape.Origin = new Vector2f(Constants.PROJECTILE_SIZE, Constants.PROJECTILE_SIZE);
            rectShape.Position = projShape.Position;
            return rectShape;
        }
    }
}
