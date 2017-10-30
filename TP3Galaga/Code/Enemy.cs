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
    /// <summary>
    /// Classe qui gère un ennemi du de la partie de Galaga
    /// </summary>
    public class Enemy
    {
        #region Variables
        //Position initiale de l'ennemi à utiler comme référence
        private Vector2f spawnPosition = new Vector2f();
        //Position que l'ennemi devrait occuper s'il est en mode "Idle"
        private Vector2f idlePosition = new Vector2f();
        //Position que l'ennemi doit atteindre lorsqu'il est en mode Capturing
        private Vector2f capturePosition = new Vector2f();
        //Position ou l'art du capture doit être dessiné
        private Vector2f captureArtPosition = new Vector2f();
        //Entier qui représente la vitesse de déplacement de l'ennemi
        private float moveSpeed = 0;
        //Variable de type EnemyType qui représente le type de l'ennemi
        private EnemyType enemyType = EnemyType.BlueGalaxian;
        //RectangleShape pour dessiner l'ennemi
        private RectangleShape enemyShape = null;
        //RectangleShape pour dessiner le rayon de capture
        private RectangleShape captureShape = null;
        //Texture de l'ennemi
        private Texture texture = null;
        //Texture secondaire de l'ennemi(pour les commanders et les bleus)
        private Texture secondaryTexture = null;
        //Texture que l'ennemi doit avoir lorsqu'il est "Idle"
        private Texture idleTexture = null;
        //Texture pour le rayon de capture du héro
        private Texture captureTexture1 = null;
        //Texture pour le rayon de capture du héro
        private Texture captureTexture2 = null;
        //Texture pour le rayon de capture du héro
        private Texture captureTexture3 = null;
        //Entier qui détermine la direction de l'ennemi (1 étant vers la droite et -1 étant vers la gauche)
        private float direction = 1;
        //Distance en X entre l'ennemi et sa cible
        private float xDistance = 0;
        //Distance en Y entre l'ennemi et sa cible
        private float yDistance = 0;
        //Rapport de distance en X entre l'ennemi et sa cible
        private float rapportDistanceX = 0;
        //Compteur d'update en mode "idle" pour le changement de texture
        private int updateCounter = 0;
        //Compteur d'update en mode Capturing
        private int capturingUpdateCounter = 0;
        //Booléen qui représente quelle texture est sélectionnée pour le mode "Idle"
        private bool idleTextureSelected = true;
        //Booléen qui représente si l'ennemi viens juste de passer en mode Capturing
        private bool hasAcquiredTarget = false;
        //Offset pour le déplacement horizontal en mode "Idle"
        private const float horizontalIdleOffset = 50;
        //Random qui détermine si l'ennemi peut attaquer
        private static Random rnd = new Random();

        /// <summary>
        /// Propriété qui gère la variable XPosition
        /// </summary>
        public float XPosition
        {
            get;
            private set;
        }
        /// <summary>
        /// Propriété qui gère la variable YPosition
        /// </summary>
        public float YPosition
        {
            get;
            private set;
        }
        /// <summary>
        /// Propriété qui gère la variable EnemyHeight
        /// </summary>
        public float EnemyHeight
        {
            get;
            private set;
        }
        /// <summary>
        /// Propriété qui gère la variable EnemyWidth
        /// </summary>
        public float EnemyWidth
        {
            get;
            private set;
        }
        /// <summary>
        /// Propriété qui gère sert de référence pour le délai permit entre chaque tir d'ennemi
        /// </summary>
        public int ShootDelay
        {
            get;
            private set;
        }
        /// <summary>
        /// Compteur pour le nombre d'update écoulée depuis que l'ennemi a tiré
        /// </summary>
        public int ShootCounter
        {
            get;
            set;
        }
        /// <summary>
        /// Statut de l'ennemi qui détermine son comportement
        /// </summary>
        public EnemyState EnemyCurrentState
        {
            get;
            set;
        }
        /// <summary>
        /// Chances d'attaque de l'ennemi
        /// </summary>
        public int AttackChance
        {
            get;
            private set;
        }
        /// <summary>
        /// ChancesTotales d'attaquer pour un ennemi, calculé a partir de l'ennemi le plus faible
        /// </summary>
        public int TotalChances
        {
            get;
            set;
        }
        #endregion

        /// <summary>
        /// Constructeur de la classe Enemy
        /// </summary>
        /// <param name="enemyType">variable de type EnemyType qui représente le type de l'ennemi</param>
        /// <param name="positionX">Position de l'ennemi sous sur l'axe des X sous forme d'entier</param>
        /// <param name="positionY">Position de l'ennemi sous sur l'axe des Y sous forme d'entier</param>
        /// <param name="attackSpeed">entier qui représente la vitesse d'attaque de l'ennemi</param>
        public Enemy(int enemyType, float positionX, float positionY, int moveSpeed, int attackChance)
        {
            this.enemyType = (EnemyType)enemyType;
            this.AttackChance = (attackChance * enemyType);
            this.moveSpeed = moveSpeed / 2;
            this.spawnPosition = new Vector2f(positionX, positionY);
            this.XPosition = (Constants.WINDOW_WIDTH / 2);
            this.YPosition = -50;
            this.idlePosition = new Vector2f(positionX, positionY);
            this.enemyShape = new RectangleShape();
            this.captureShape = new RectangleShape();
            this.texture = new Texture("Data\\Arts\\New Art\\" + this.enemyType.ToString() + ".png");
            //Initialise la texture secondaire pour les types qui l'utilise (BlueGalaxian et GalaganCommander)
            if (this.enemyType == EnemyType.GalaganCommander || this.enemyType == EnemyType.BlueGalaxian)
            {
                //Ajoute "Secondary" au chemin pour charger la bonne texture
                this.secondaryTexture = new Texture("Data\\Arts\\New Art\\" + this.enemyType.ToString() + "Secondary" + ".png");
            }
            //Sinon Initialiser la texture secondaire à la texture principale pour les autres types qui ne l'utilise pas
            else
                this.secondaryTexture = texture;
            this.idleTexture = texture;
            this.enemyShape.Texture = texture;
            this.captureTexture1 = new Texture("Data\\Arts\\New Art\\Capture1.png");
            this.captureTexture2 = new Texture("Data\\Arts\\New Art\\Capture2.png");
            this.captureTexture3 = new Texture("Data\\Arts\\New Art\\Capture3.png");
            this.captureShape.Texture = captureTexture1;
            this.captureShape.Size = new Vector2f(captureTexture1.Size.X, captureTexture1.Size.Y);
            this.captureShape.Origin = new Vector2f(captureShape.Size.X / 2, captureShape.Size.Y / 2);
            //Multiplie la grosseur da la texture par l'échelle pour donner la grosseur voulue
            this.EnemyWidth = enemyShape.Texture.Size.X * Constants.GAME_ENEMY_SCALE;
            this.EnemyHeight = enemyShape.Texture.Size.Y * Constants.GAME_ENEMY_SCALE;
            this.enemyShape.Size = new Vector2f(EnemyWidth, EnemyHeight);
            this.enemyShape.Origin = new Vector2f(EnemyWidth / 2, EnemyHeight / 2);
            this.EnemyCurrentState = EnemyState.Spawning;
            //Calcul du délai entre chaque tir à partir du type et la vitesse
            this.ShootDelay = enemyType * attackChance;
            this.ShootCounter = 0;
        }
        /// <summary>
        /// Dessine l'ennemi à la position du sprite et appele le Draw de Projectile
        /// </summary>
        /// <param name="window">la fenêtre de rendu de type RenderWindow</param>
        public void Draw(RenderWindow window)
        {
            enemyShape.Position = new Vector2f(XPosition, YPosition);
            captureShape.Position = captureArtPosition;
            window.Draw(enemyShape);
            if (EnemyCurrentState == EnemyState.Capturing && YPosition >= capturePosition.Y)
                window.Draw(captureShape);
        }
        /// <summary>
        /// Met à jour l'ennemi
        /// </summary>
        /// <param name="deltaT">Entier qui représente le nombre de pixel que l'ennemi doit parcourir en un appel de Update</param>
        /// <param name="heroPosX">réel qui représente la position du héro en X</param>
        /// <param name="heroPosY">réel qui représente la position du héro en Y</param>
        /// <returns></returns>
        public void Update(float deltaT, float heroPosX, float heroPosY)
        {
            //Calculer la position en mode "Idle" de l'ennemi, peut importe son état actuel
            if (idlePosition.X <= spawnPosition.X - horizontalIdleOffset || idlePosition.X >= spawnPosition.X + horizontalIdleOffset)
            {
                //Inverser la direction si l'ennemi a atteint le offset horizontal pré-déterminé
                direction = -direction;
            }
            idlePosition.X += direction * (moveSpeed * deltaT);
            //S'assurer que l'ennemi ne dépasse pas l'offset horizontal
            idlePosition.X = Math.Min(spawnPosition.X + horizontalIdleOffset, Math.Max(spawnPosition.X - horizontalIdleOffset, idlePosition.X));
            //Calcule la texture que l'ennemi doit avoir en mode "Idle" entre sa texture principale ou sa texture secondaire(Pour les ennemis qui changent de texture)
            if (idleTextureSelected && updateCounter == Constants.TEXTURE_SWITCH_DELAY)
            {
                idleTexture = secondaryTexture;
                idleTextureSelected = false;
                updateCounter = 0;
            }
            else if (!idleTextureSelected && updateCounter == Constants.TEXTURE_SWITCH_DELAY)
            {
                idleTexture = texture;
                idleTextureSelected = true;
                updateCounter = 0;
            }
            updateCounter++;

            //Si l'ennemi est en mode "Idle"
            if (EnemyCurrentState == EnemyState.Idle)
            {
                //Insert la texture et la position que l'ennemi doit avoir en mode "Idle" et règle la rotation à 0
                enemyShape.Texture = idleTexture;
                XPosition = idlePosition.X;
                YPosition = idlePosition.Y;
                enemyShape.Rotation = 0;
            }
            //Si l'ennemi est en mode "Spawning"
            if (EnemyCurrentState == EnemyState.Spawning)
            {
                enemyShape.Texture = texture;
                xDistance = spawnPosition.X - XPosition;
                yDistance = spawnPosition.Y - YPosition;
                //Calcule le rapport des distances en X et en Y pour savoir le coefficient de déplacement
                //Calcule avec la valeur absolue de la distance en Y pour que la direction ne s'inverse pas lorsque l'ennemi a passé le héro
                rapportDistanceX = xDistance / Math.Abs(yDistance);
                //Applique le coefficient de déplacement au déplacement en X pour que l'ennemi se déplace dans le bon angle
                XPosition += (Constants.SPAWN_MOVE_SPEED * deltaT * rapportDistanceX);
                YPosition += (Constants.SPAWN_MOVE_SPEED * deltaT);
                //Calcule l'angle grace à la tangente de la distance en Y sur la distance en X, puis est converti en degré pour avoir l'angle exact vers le héro
                enemyShape.Rotation = (float)Math.Atan2(Math.Abs(yDistance), xDistance) * (float)(180 / Math.PI) - 90;
                if (YPosition >= spawnPosition.Y)
                    EnemyCurrentState = EnemyState.Idle;
            }
            //Si l'ennemi est en mode "AboutToAttack"
            //Sert à donner des probabilités d'attaque différentes pour chaque ennemi
            if (EnemyCurrentState == EnemyState.AboutToAttack)
            {
                //Si le Random.Next donne un nombre dans la plage de chances de l'ennemi, il passe en mode "Attacking" sinon il retourne "Idle"
                if (rnd.Next(0, TotalChances) > AttackChance)
                    EnemyCurrentState = EnemyState.Idle;
                else
                {
                    enemyShape.Texture = texture;
                    EnemyCurrentState = EnemyState.Attacking;
                }
            }
            //Si l'ennemi est mode "Attacking"
            if (EnemyCurrentState == EnemyState.Attacking)
            {
                xDistance = heroPosX - XPosition;
                yDistance = heroPosY - YPosition;
                //Calcule le rapport des distances en X et en Y pour savoir le coefficient de déplacement
                //Calcule avec la valeur absolue de la distance en Y pour que la direction ne s'inverse pas lorsque l'ennemi a passé le héro
                //S'assure que le coefficient ne dépasse pas 1 ou -1 pour que l'angle maximum soit de 45 degré
                rapportDistanceX = Math.Max(Math.Min(xDistance / Math.Abs(yDistance), 1), -1);
                //Applique le coefficient de déplacement au déplacement en X pour que l'ennemi se déplace dans le bon angle
                XPosition += (moveSpeed * deltaT * rapportDistanceX);
                YPosition += (deltaT * moveSpeed);

                //Calcule l'angle grace à la tangente de la distance en Y sur la distance en X, puis est converti en degré pour avoir l'angle exact vers le héro
                //S'assure que l'angle de rotation ne dépasse pas 45 degré

                enemyShape.Rotation = Math.Max(Math.Min((float)Math.Atan2(Math.Abs(yDistance), xDistance) * (float)(180 / Math.PI) - 90, 45), -45);
                //Gère si l'ennemi manque le héro et se retrouve en dehors de la fenêtre
                if (YPosition > Constants.WINDOW_HEIGHT + EnemyHeight)
                {
                    //Repositionne le héro en haut de la fenêtre et le met en mode "Spawning"
                    XPosition = Constants.WINDOW_WIDTH / 2;
                    YPosition = 0;
                    EnemyCurrentState = EnemyState.Spawning;
                }
            }
            if (EnemyCurrentState == EnemyState.Capturing)
            {
                enemyShape.Texture = texture;
                if (!hasAcquiredTarget)
                {
                    capturePosition = new Vector2f(heroPosX, Constants.WINDOW_HEIGHT - captureShape.Size.Y - (enemyShape.Size.Y / 2));
                    xDistance = capturePosition.X - XPosition;
                    yDistance = capturePosition.Y - YPosition;
                    //Calcule le rapport des distances en X et en Y pour savoir le coefficient de déplacement
                    //Calcule avec la valeur absolue de la distance en Y pour que la direction ne s'inverse pas lorsque l'ennemi a passé le héro
                    rapportDistanceX = xDistance / Math.Abs(yDistance);
                    hasAcquiredTarget = true;
                }
                if (YPosition < capturePosition.Y)
                {
                    //Applique le coefficient de déplacement au déplacement en X pour que l'ennemi se déplace dans le bon angle
                    XPosition += (Constants.SPAWN_MOVE_SPEED * deltaT * rapportDistanceX);
                    YPosition += (Constants.SPAWN_MOVE_SPEED * deltaT);
                    //Calcule l'angle grace à la tangente de la distance en Y sur la distance en X, puis est converti en degré pour avoir l'angle exact vers le héro
                    enemyShape.Rotation = (float)Math.Atan2(Math.Abs(yDistance), xDistance) * (float)(180 / Math.PI) - 90;
                }
                else if (YPosition >= capturePosition.Y)
                {
                    enemyShape.Rotation = 0;
                    //captureShape.TextureRect = new IntRect(0, 0, 46, 1 + capturingUpdateCounter);
                    captureArtPosition = new Vector2f(capturePosition.X, capturePosition.Y + enemyShape.Size.Y * 1.5f);
                    capturingUpdateCounter++;
                    if (capturingUpdateCounter == 180)
                    {
                        capturingUpdateCounter = 0;
                        hasAcquiredTarget = false;
                        EnemyCurrentState = EnemyState.Attacking;
                    }
                }
            }
        }
        /// <summary>
        /// Méthode qui vérifie si l'ennemi a une vue directe sur le héro
        /// </summary>
        /// <param name="heroPosX">position du héro en X</param>
        /// <returns>booléen qui représente si l'ennemi a une vue directe sur l'ennemi</returns>
        public bool HaveSightOnHero(float heroPosX)
        {
            if (heroPosX > (XPosition - EnemyWidth / 2) && heroPosX < (XPosition + EnemyWidth / 2))
                return true;
            else
                return false;
        }
        /// <summary>
        /// Méthode qui retourne le Rectangle correspondant à l'ennemi
        /// </summary>
        /// <returns>RectangleShape qui représente l'ennemi</returns>
        public RectangleShape GetRect()
        {
            return enemyShape;
        }

        /// <summary>
        /// Retourne le Rectangle correspondant au rayon de capture de l'ennemi
        /// </summary>
        /// <returns></returns>
        public RectangleShape GetCaptureRect()
        {
          return captureShape;
        }
    }
}
