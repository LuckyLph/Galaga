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
    /// Classe qui gère le niveau.
    /// Charge le niveau et les enemies et les places dans une liste.
    /// </summary>
    public class Level
    {
        //Liste contenant les ennemis de la partie.
        private List<Enemy> enemies = null;

        /// <summary>
        /// Valeur de l'énum l'ennemi le plus faible
        /// </summary>
        public int WeakestEnemy
        {
            get;
            private set;
        }
        /// <summary>
        /// Vitesse de l'ennemi le plus faible
        /// </summary>
        public int WeakestEnemySpeed
        {
            get;
            private set;
        }
        /// <summary>
        /// Propriété qui sert à obtenir la liste d'ennemis
        /// </summary>
        public List<Enemy> Enemies
        {
            get
            {
                return enemies;
            }
        }
        //Propriété qui sert à savoir le nombre d'ennemis
        public int EnemyCount
        {
            get
            {
                return enemies.Count;
            }
        }


        /// <summary>
        /// Constructeur de la classe Level
        /// </summary>
        public Level()
        {
            enemies = new List<Enemy>();
        }

        /// <summary>
        /// Appele la fonction Draw() pour chaque ennemis de la liste
        /// </summary>
        /// <param name="window">La window SFML dans laquelle les ennemis doivent être dessiné</param>
        public void Draw(RenderWindow window)
        {
            foreach (Enemy i in enemies)
            {
                i.Draw(window);
            }
        }

        /// <summary>
        /// Charge le niveau prit en paramètre
        /// </summary>
        /// <param name="levelPath">Chemin de lecture du niveau sous forme de string</param>
        public void LoadLevel(string levelPath)
        {
            if (System.IO.File.Exists(levelPath))
            {
                string[] levelContent = System.IO.File.ReadAllLines(levelPath);
                ErrorCode levelCondition = Parse(levelContent);
                if (levelCondition == ErrorCode.BAD_FILE_FORMAT)
                {
                    System.Windows.Forms.MessageBox.Show("Le format du fichier est incorrect !", "Erreur de chargement du niveau !",
                                                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                    Environment.Exit(1);
                }
                else if (levelCondition == ErrorCode.MISSING_FIELD)
                {
                    System.Windows.Forms.MessageBox.Show("Il y a un champ manquant !", "Erreur de chargement du niveau !",
                                                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                    Environment.Exit(1);
                }
                else if (levelCondition == ErrorCode.UNKNOWN_ERROR)
                {
                    System.Windows.Forms.MessageBox.Show("Erreur inconnue !", "Erreur de chargement du niveau !",
                                                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                    Environment.Exit(1);
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Le fichier n'existe pas !", "Erreur de chargement du niveau !",
                                                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                Environment.Exit(1);
            }
        }

        /// <summary>
        /// Charge les ennemis du niveau prit en paramètres en les instanciant et en les stockant dans une liste
        /// </summary>
        /// <param name="fileContent">tableau de string qui représente le niveau à charger</param>
        /// <returns>retourne un code d'erreur pour indique si il y eu un problème avec le chargement du niveau</returns>
        private ErrorCode Parse(string[] fileContent)
        {
            try
            {
                //Sert à traiter un ennemi à la fois puisque une ligne représente un ennemi
                for (int i = 0; i < fileContent.Length; i++)
                {
                    string enemyToLoad = fileContent[i];
                    //Sépare chaque propriété de l'ennemi dans un nouveau tableau de string
                    string[] enemyProperties = enemyToLoad.Split(' ');
                    //Gère les cas ou il y a des cases vides dans les propriétés
                    for (int j = 0; j < enemyProperties.Length; j++)
                    {
                        if (enemyProperties[j] == "")
                        {
                            for (int y = j; y < enemyProperties.Length; y++)
                            {
                                //Décale tous les propriétés vers la gauche en s'assurant de ne pas déborder du tableau
                                if (y < enemyProperties.Length - 1)
                                    enemyProperties[y] = enemyProperties[y + 1];
                            }
                        }
                    }
                    //Vérifie si les propriétés weakeastEnemy et weakestEnemySpeed sont à jour
                    if (Int32.Parse(enemyProperties[0]) > WeakestEnemy)
                    {
                        WeakestEnemy = Int32.Parse(enemyProperties[0]);
                        WeakestEnemySpeed = Int32.Parse(enemyProperties[3]);
                    }
                    //initie un objet de type ennemi avec les propriétés extraites, les 4 premières cases du tableau sont les propriétés voulues
                    Enemy enemy = new Enemy(Int32.Parse(enemyProperties[0]), Int32.Parse(enemyProperties[1]), Int32.Parse(enemyProperties[2]),
                                            Int32.Parse(enemyProperties[3]), Int32.Parse(enemyProperties[3]));
                    enemies.Add(enemy);
                }
                foreach (Enemy i in enemies)
                {
                    //Calcul des chances totales d'un ennemi d'attaquer basé sur le type et la vitesse
                    i.TotalChances = WeakestEnemy * WeakestEnemySpeed;
                }
                return ErrorCode.OK;
            }
            catch (FormatException)
            {
                return ErrorCode.BAD_FILE_FORMAT;
            }
            catch (LoadingFailedException)
            {
                return ErrorCode.BAD_FILE_FORMAT;
            }
            catch (IndexOutOfRangeException)
            {
                return ErrorCode.MISSING_FIELD;
            }
            catch (Exception)
            {
                return ErrorCode.UNKNOWN_ERROR;
            }
        }

        /// <summary>
        /// Retire l'ennemi prit en paramètre de la liste
        /// </summary>
        /// <param name="enemy">objet de type Enemy à retirer de la liste</param>
        public void RemoveEnemy(Enemy enemy)
        {
            if (enemies.Contains(enemy))
            {
                enemies.Remove(enemy);
            }
        }
    }
}
