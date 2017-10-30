using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP3Galaga
{
    public class Constants
    {
        #region Program
        //Largeur de la fenêtre de rendu en pixel
        public const int WINDOW_WIDTH = 540;
        //Hauteur de la fenêtre de rendu en pixel
        public const int WINDOW_HEIGHT = 600;
        //Limite d'images par secondes
        public const int FRAME_LIMIT = 60;
        #endregion

        #region Game
        //Titre de la fenêtre de rendu
        public const string GAME_NAME = "Galaga2016";
        //Vitesse des projectiles du héro
        public const float HERO_PROJECTILE_SPEED = 500;
        //Vitesse des projectile ennemis
        public const float ENEMY_PROJECTILE_SPEED = 150;
        //Échelle de grossisement des ennemis
        public const float GAME_ENEMY_SCALE = 2f;
        //Échelle de grossisement du héro
        public const float GAME_HERO_SCALE = 2.5f;
        //Largeur par défaut de la texture du héro
        public const float GAME_HERO_DEFAULT_WIDTH = 15;
        //Hauteur du héro par rapport au bas de la fenêtre
        public const float DEFAULT_HERO_HEIGHT = 18f;
        //Délai avant que le héro puisse tirer à nouveau
        public const int HERO_SHOOT_DELAY = 30;
        //Nombre de vie du héro
        public const int HERO_LIVES = 3;
        //Nombre de milisecondes entre chaque tick du timer
        public const int TIMER_INTERVAL = 5000;
        //Vitesse des étoiles lentes
        public const float STARS_BACKGROUND_SPEED = 20.0f;
        //Vitesse des étoiles moyennes
        public const float STARS_MIDDLEGROUND_SPEED = 30.0f;
        //Vitesse des étoiles rapides
        public const float STARS_FOREGROUND_SPEED = 40.0f;
        //Grosseur des étoiles lentes
        public const float STARS_BACKGROUND_SIZE = 5.0f;
        //Grosseur des étoiles moyennes
        public const float STARS_MIDDLEGROUND_SIZE = 6.0f;
        //Grosseur des étoiles rapides
        public const float STARS_FOREGROUND_SIZE = 7.0f;
        //Cadence de génération des étoiles
        public const int STARS_SPAWN_RATE = 30;
        #endregion

        #region Enemy
        //Constante pour le changement de texture en mode "Idle"
        public const int TEXTURE_SWITCH_DELAY = 40;
        //Constante pour le vitesse de "spawning" d'un ennemi
        public const int SPAWN_MOVE_SPEED = 125;
        #endregion

        #region Leaderboard
        //Nombre maximum d'entrée possible dans le leaderboard
        public const int MAX_LEADERBOARD_ENTRY = 10;
        //Nombre maximum d'entrée du leaderboard affiché à l'écran
        public const int MAX_LEADERBOARD_ENTRY_SHOWN = 5;
        //Chemin d'accès du leaderboard
        public const string LEADERBOARD_FILE_PATH = "Data\\lb.txt";
        #endregion

        #region Projectile
        //Grosseur du projectile
        public const float PROJECTILE_SIZE = 4.0f;
        #endregion

        #region Particule
        //Grosseur de la particule
        public const float PARTICULE_WIDTH_SIZE = 1;
        #endregion

        #region StringTable
        //Constante qui représente le language par défaut
        public const Language DEFAULT_LANGUAGE = Language.English;
        #endregion
    }
}
