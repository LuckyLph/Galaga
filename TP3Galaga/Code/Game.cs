using System;
using System.Collections.Generic;
using System.Security.Policy;
using SFML;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Timers;
using System.Linq;

namespace TP3Galaga
{
    public class Game
    {
        #region Variables
        //Nombre de vie retante du joueur
        private int playerLives = 0;
        //Score du joueur en entier
        private int currentScore = 0;
        //Niveau actuel de la partie
        private int currentLevel = 0;
        //Nombre d'étoile approximative
        private int starsAmount = 80;
        //String pour le nom du joueur à ajouter dans le leaderboard
        private string playerName = "Roger";
        //Booléen qui indique si tous les ennemis sont prêt
        private bool enemiesReady = false;
        //Booléen qui détermine si la partie doit terminé
        private bool gameMustEnd = false;
        //Booléen qui détermine si la partie doit recommencer
        private bool gameMustRestart = false;
        //Booléem qui détermine si le nom du joueur est le nom par défaut
        private bool isDefaultName = true;
        //Tableau des niveaux du Jeu
        private string[] levels;
        //Langue actuelle du jeu
        private Language currentLanguage = Constants.DEFAULT_LANGUAGE;
        //WindowState qui représente la fenêtre qui doit être dessinée
        private WindowState currentWindowState = WindowState.SplashScreen;
        //GameState qui représente l'état de la partie actuelle
        private GameState currentGameState;
        //Objet de type RenderWindow pour la fenêtre de rendu
        private RenderWindow window = null;
        //Objet de couleur pour la couleur de fond de la fenêtre de rendu
        private Color backgroundColor = Color.Black;
        //Objet de type Level pour le niveau
        private Level level = null;
        //Objet de type Héro de la partie
        private Hero player = null;
        //Objet de type Inputs pour gèrer les entrées
        private Inputs inputs = null;
        //Liste des projectiles du héro
        private List<Projectile> projectilesHero = null;
        //Liste de projectile des ennemis
        private List<Projectile> projectilesEnemy = null;
        //Liste des projectiles des ennemis à retirer de leur liste
        private List<Projectile> projectilesToRemove = null;
        //Liste d'étoiles pour le fond du jeu Il y a 3 liste pour 3 diférente grosseure d'étoile
        private List<Particule> starsBackground = null;
        //Liste d'étoiles pour le fond du jeu
        private List<Particule> starsMiddleground = null;
        //Liste d'étoiles pour le fond du jeu
        private List<Particule> starsForeground = null;
        //Objet de type LeaderBoard pour le leaderboard du jeu
        private Leaderboard leaderboard = null;
        //Text pour les scores dans le leaderboard
        private Text leaderboardText = null;
        //Text pour l'entête du leaderboard
        private Text leaderboardEntete = null;
        //Text affiché dans le splashscreen
        private Text splashScreenText = null;
        //Text pour les vies
        private Text lifeText = null;
        //Text pour le score
        private Text scoreText = null;
        //Text affiché en dessous du leaderboard
        private Text leaderboardOptions = null;
        //Text pour le nom du joueur
        private Text leaderBoardNameToEnter = null;
        //Font pour le Texte affiché dans le splashscreen
        private Font textFont = null;
        //Texture pour le splash art du Galaga
        private Texture galagaTexture = null;
        //Texture pour les vies
        private Texture livesTexture = null;
        //Sprite pour les vies
        private Sprite livesSprite1 = null;
        //Sprite pour les vies
        private Sprite livesSprite2 = null;
        //Sprite pour les vies
        private Sprite livesSprite3 = null;
        //Sprite pour le splash art du Galaga
        private Sprite galagaSprite = null;
        //Hauteur et largeur du Text LeaderboardEntete
        private Vector2f leaderboardEnteteSize;
        //Hauteur et largeur du Text LeaderboardEntete
        private Vector2f leaderboardOptionsSize;
        //Hauteur et largeur du Text LeaderboardEntete
        private Vector2f leaderboardNameToEnterSize;
        //Hauteur et largeur du Text LeaderboardEntete
        private Vector2f leaderboardTextSize;
        //StringTable pour gérer les langues
        private StringTable stringTable = null;
        //Timer pour déterminer quand un ennemi doit attaquer
        private Timer enemyTimer = null;
        //Random pour déterminer quel enemy doit charger le joueur
        private Random rnd = null;
        #endregion

        /// <summary>
        /// Constructeur de la classe Game
        /// </summary>
        /// <param name="windowTitle">Titre de la fenêtre</param>
        /// <param name="width">Largeur de la fenêtre</param>
        /// <param name="height">Hauteur de la fenêtre</param>
        public Game(RenderWindow window)
        {
            this.window = window;
            this.window.KeyPressed += new EventHandler<KeyEventArgs>(OnKeyPressed);
            this.window.Closed += new EventHandler(OnClose);
            this.window.SetFramerateLimit(Constants.FRAME_LIMIT);
        }

        /// <summary>
        /// Démarre la boucle de jeu
        /// </summary>
        public bool Run()
        {
            //float deltaT = 1.0f / (float)Program.FRAME_LIMIT;
            //Valeur fixe pour corriger le problème d'arondissement d'SFML
            float deltaT = 0.01666f;
            window.SetActive();
            while (window.IsOpen && !gameMustEnd)
            {
                window.Clear(backgroundColor);
                window.DispatchEvents();
                Update(deltaT);
                Draw(currentWindowState, window);
                window.Display();
            }
            return gameMustRestart;
        }
        private void Draw(WindowState currentState, RenderWindow window)
        {
            switch (currentWindowState)
            {
                case WindowState.Leaderboard:
                    DrawLeaderBoard(window);
                    break;
                case WindowState.SplashScreen:
                    DrawSplashScreen(window);
                    break;
                default:
                    DrawNormal(window);
                    break;
            }
        }
        /// <summary>
        /// Dessine le jeu lorsqu'en partie
        /// </summary>
        /// <param name="window">fenêtre de rendu de type RenderWindow</param>
        private void DrawNormal(RenderWindow window)
        {
            DrawStars(window);
            level.Draw(window);
            player.Draw(window);
            DrawScore(window);
        }
        /// <summary>
        /// Dessine le leaderboard dans la fenêtre prise en paramètre
        /// </summary>
        /// <param name="window">fenêtre de rendu de type RenderWindow</param>
        private void DrawLeaderBoard(RenderWindow window)
        {
            DrawStars(window);
            window.Draw(leaderboardEntete);
            window.Draw(leaderboardText);
            window.Draw(leaderboardOptions);
            if (currentGameState == GameState.Starting)
                window.Draw(leaderBoardNameToEnter);
        }
        /// <summary>
        /// Dessine le SplashScreen
        /// </summary>
        /// <param name="window">fenêtre de rendu de type RenderWindow</param>
        private void DrawSplashScreen(RenderWindow window)
        {
            if (currentLanguage == Language.English)
            {
                splashScreenText = new Text(stringTable.GetValue(currentLanguage, "ID_PRESS_ENTER_TO_START"), textFont, 35);
                splashScreenText.Position = new Vector2f(Constants.WINDOW_WIDTH / 2 - 205, (Constants.WINDOW_HEIGHT / 2) + 130);
            }
            else if (currentLanguage == Language.French)
            {
                splashScreenText = new Text(stringTable.GetValue(currentLanguage, "ID_PRESS_ENTER_TO_START"), textFont, 29);
                splashScreenText.Position = new Vector2f(Constants.WINDOW_WIDTH / 2 - 253, (Constants.WINDOW_HEIGHT / 2) + 120);
            }
            splashScreenText.Color = Color.Red;
            DrawStars(window);
            window.Draw(galagaSprite);
            window.Draw(splashScreenText);
        }

        /// <summary>
        /// Dessine les étoiles de la partie
        /// </summary>
        /// <param name="window">fenêtre de rendu de type RenderWindow</param>
        private void DrawStars(RenderWindow window)
        {
            foreach (Projectile i in projectilesEnemy)
            {
                i.Draw(window);
            }
            foreach (Projectile i in projectilesHero)
            {
                i.Draw(window);
            }
            foreach (Particule etoile in starsBackground)
            {
                etoile.Draw(window);
            }
            foreach (Particule etoile in starsForeground)
            {
                etoile.Draw(window);
            }
            foreach (Particule etoile in starsMiddleground)
            {
                etoile.Draw(window);
            }
        }

        /// <summary>
        /// Dessine le score et les vies dans la fenêtre de rendu
        /// </summary>
        /// <param name="window"></param>
        private void DrawScore(RenderWindow window)
        {
            if (currentLanguage == Language.English)
            {
                lifeText = new Text(stringTable.GetValue(currentLanguage, "ID_LIFE"), textFont, 20);
                lifeText.Position = new Vector2f(1f, 2f);
                livesSprite1.Position = new Vector2f((livesTexture.Size.X * 5f), 8f);
                livesSprite2.Position = new Vector2f((livesTexture.Size.X * 6f) + 5f, 8f);
                livesSprite3.Position = new Vector2f((livesTexture.Size.X * 7f) + 10f, 8f);
                scoreText = new Text(stringTable.GetValue(currentLanguage, "ID_SCORE") + " " + currentScore, textFont, 20);
                scoreText.Position = new Vector2f((livesTexture.Size.X * 9f) + 10f, 2f);
            }
            else if (currentLanguage == Language.French)
            {
                lifeText = new Text(stringTable.GetValue(currentLanguage, "ID_LIFE"), textFont, 20);
                lifeText.Position = new Vector2f(1f, 2f);
                livesSprite1.Position = new Vector2f((livesTexture.Size.X * 4f), 8f);
                livesSprite2.Position = new Vector2f((livesTexture.Size.X * 5f) + 5f, 8f);
                livesSprite3.Position = new Vector2f((livesTexture.Size.X * 6f) + 10f, 8f);
            }

            lifeText.Color = Color.Green;
            scoreText.Color = Color.Cyan;
            window.Draw(lifeText);
            window.Draw(scoreText);
            if (playerLives >= 3)
                window.Draw(livesSprite3);
            if (playerLives >= 2)
                window.Draw(livesSprite2);
            if (playerLives >= 1)
                window.Draw(livesSprite1);
        }

        /// <summary>
        /// Met à jour le jeu
        /// </summary>
        /// <param name="deltaT">coefficient de temps pour le déplacement des éléments de jeu</param>
        private void Update(float deltaT)
        {
            if (currentGameState == GameState.Playing)
            {
                UpdateEnemies(deltaT);
                UpdateHero(deltaT);
                UpdateProjectiles(deltaT);
            }
            else
            {
                UpdateTexts();
            }
            UpdateStars(deltaT);

            if (level.EnemyCount == 0)
            {
                level.LoadLevel(levels[0]);
            }
            else if (playerLives <= 0 && currentGameState == GameState.Playing)
            {
                EndGame();
            }
        }

        /// <summary>
        /// Met à jour les étoiles de la partie
        /// </summary>
        /// <param name="deltaT"></param>
        public void UpdateStars(float deltaT)
        {
            foreach (Particule particle in starsBackground)
            {
                particle.Update(deltaT);
                if (particle.YPosition >= Constants.WINDOW_HEIGHT)
                {
                    particle.YPosition = 0.0f;
                    particle.XPosition = rnd.Next(-100, Constants.WINDOW_WIDTH);
                }
            }

            foreach (Particule particle in starsMiddleground)
            {
                particle.Update(deltaT);
                if (particle.YPosition >= Constants.WINDOW_HEIGHT)
                {
                    particle.YPosition = 0.0f;
                    particle.XPosition = rnd.Next(-100, Constants.WINDOW_WIDTH);
                }
            }

            foreach (Particule particle in starsForeground)
            {
                particle.Update(deltaT);
                if (particle.YPosition >= Constants.WINDOW_HEIGHT)
                {
                    particle.YPosition = 0.0f;
                    particle.XPosition = rnd.Next(-100, Constants.WINDOW_WIDTH);
                }
            }
        }

        /// <summary>
        /// Gère la mise à jour des ennemis du niveau actif
        /// </summary>
        private void UpdateEnemies(float deltaT)
        {
            for (int i = 0; i < level.EnemyCount; i++)
            {
                level.Enemies[i].Update(deltaT, player.XPosition, player.YPosition);

                if (!enemiesReady)
                {
                    for (int j = 0; j < level.EnemyCount; j++)
                    {
                        if (level.Enemies[j].EnemyCurrentState != EnemyState.Idle)
                            enemiesReady = false;
                        else
                            enemiesReady = true;
                    }
                    if (enemiesReady)
                        enemyTimer.Start();
                }

                if (CheckIntersectionBetweenRectangle(level.Enemies[i].GetRect(), player.GetRect()) && level.Enemies[i].EnemyCurrentState == EnemyState.Attacking)
                {
                    playerLives--;
                    currentScore -= 1000;
                    if (currentScore < 0)
                        currentScore = 0;
                    if (level.Enemies.Contains(level.Enemies[i]))
                        level.RemoveEnemy(level.Enemies[i]);
                }

                if (level.Enemies[i].HaveSightOnHero(player.XPosition) && level.Enemies[i].ShootCounter >= level.Enemies[i].ShootDelay && enemiesReady && level.Enemies[i].EnemyCurrentState == EnemyState.Idle)
                {
                    projectilesEnemy.Add(new Projectile(level.Enemies[i].XPosition - (Constants.PROJECTILE_SIZE / 2),
                                        level.Enemies[i].YPosition, Constants.ENEMY_PROJECTILE_SPEED, CharacterType.Enemy));
                    level.Enemies[i].ShootCounter = 0;
                }

                level.Enemies[i].ShootCounter++;
            }
        }

        /// <summary>
        /// Met à jour le héro
        /// </summary>
        /// <param name="deltaT"></param>
        private void UpdateHero(float deltaT)
        {
            player.Update(deltaT, inputs.LeftArrow, inputs.RightArrow);

            if (inputs.Spacebar && player.ShootCounter >= Constants.HERO_SHOOT_DELAY && enemiesReady)
            {
                projectilesHero.Add(new Projectile(player.XPosition - (Constants.PROJECTILE_SIZE / 2), player.YPosition, Constants.HERO_PROJECTILE_SPEED, CharacterType.Hero));
                player.ShootCounter = 0;
            }
            player.ShootCounter++;
        }

        /// <summary>
        /// Met à jour les projectiles de la partie
        /// </summary>
        /// <param name="deltaT"></param>
        private void UpdateProjectiles(float deltaT)
        {
            foreach (Projectile i in projectilesHero)
            {
                for (int j = 0; j < level.EnemyCount; j++)
                {
                    if (CheckIntersectionBetweenRectangle(i.GetRect(), level.Enemies[j].GetRect()))
                    {
                        currentScore += level.Enemies[j].AttackChance;
                        if (level.Enemies.Contains(level.Enemies[j]))
                            level.RemoveEnemy(level.Enemies[j]);
                        projectilesToRemove.Add(i);
                    }
                }
                if (i.YPosition <= 0)
                {
                    projectilesToRemove.Add(i);
                }
                i.Update(deltaT);
            }
            projectilesHero = projectilesHero.Except(projectilesToRemove).ToList();
            projectilesToRemove.Clear();

            foreach (Projectile i in projectilesEnemy)
            {
                if (i.YPosition >= Constants.WINDOW_HEIGHT || CheckIntersectionBetweenRectangle(player.GetRect(), i.GetRect()))
                {
                    projectilesToRemove.Add(i);
                    if (CheckIntersectionBetweenRectangle(player.GetRect(), i.GetRect()))
                    {
                        playerLives--;
                        currentScore -= 1000;
                        if (currentScore < 0)
                            currentScore = 0;
                    }
                }
                i.Update(deltaT);
            }
            projectilesEnemy = projectilesEnemy.Except(projectilesToRemove).ToList();
            projectilesToRemove.Clear();
        }

        /// <summary>
        /// Met à jour les objets Text pour le leaderboard
        /// </summary>
        private void UpdateTexts()
        {
            if (currentWindowState == WindowState.Leaderboard)
            {
                if (currentGameState == GameState.Starting)
                {
                    leaderboardOptions.DisplayedString = stringTable.GetValue(currentLanguage, "ID_ENTER_YOUR_NAME");
                    leaderBoardNameToEnter.DisplayedString = " " + playerName;
                    leaderboardNameToEnterSize = new Vector2f(leaderBoardNameToEnter.GetGlobalBounds().Width, leaderBoardNameToEnter.GetGlobalBounds().Height);
                    leaderboardOptionsSize = new Vector2f(leaderboardOptions.GetGlobalBounds().Width, leaderboardOptions.GetGlobalBounds().Height);
                    leaderboardOptions.Position = new Vector2f((Constants.WINDOW_WIDTH / 2) - (leaderboardOptionsSize.X + leaderboardNameToEnterSize.X) / 2,
                                                                (Constants.WINDOW_HEIGHT / 2) + (Constants.WINDOW_HEIGHT / 3.5f));
                    leaderBoardNameToEnter.Position = new Vector2f(leaderboardOptions.Position.X + leaderboardOptionsSize.X,
                                                                   Constants.WINDOW_HEIGHT / 2 + Constants.WINDOW_HEIGHT / 3.5f);
                }
                else
                {
                    leaderboardOptions.DisplayedString = stringTable.GetValue(currentLanguage, "ID_REPLAY");
                    leaderboardOptionsSize = new Vector2f(leaderboardOptions.GetGlobalBounds().Width, leaderboardOptions.GetGlobalBounds().Height);
                    leaderboardOptions.Position = new Vector2f((Constants.WINDOW_WIDTH / 2) - (leaderboardOptionsSize.X / 2),
                                                                (Constants.WINDOW_HEIGHT / 2) + (Constants.WINDOW_HEIGHT / 3.5f));
                }
                leaderboardEntete.DisplayedString = stringTable.GetValue(currentLanguage, "ID_LEADERBOARD");
                leaderboardEnteteSize = new Vector2f(leaderboardEntete.GetGlobalBounds().Width, leaderboardEntete.GetGlobalBounds().Height);
                leaderboardTextSize = new Vector2f(leaderboardText.GetGlobalBounds().Width, leaderboardText.GetGlobalBounds().Height);
                leaderboardEntete.Position = new Vector2f((Constants.WINDOW_WIDTH / 2) - leaderboardEnteteSize.X / 2, (Constants.WINDOW_HEIGHT / 2) - (Constants.WINDOW_HEIGHT / 4));
                leaderboardText.Position = new Vector2f(leaderboardEntete.Position.X, (Constants.WINDOW_HEIGHT / 2) - (Constants.WINDOW_HEIGHT / 6));
            }
            else if (currentWindowState == WindowState.SplashScreen)
            {

            }
        }

        /// <summary>
        /// Initialise la partie
        /// </summary>
        public void ResetGame()
        {
            gameMustEnd = false;
            gameMustRestart = false;
            enemiesReady = false;
            playerLives = Constants.HERO_LIVES;
            currentScore = 0;
            currentLanguage = Constants.DEFAULT_LANGUAGE;
            currentGameState = GameState.Starting;
            currentWindowState = WindowState.SplashScreen;
            level.LoadLevel(levels[currentLevel]);
            leaderboardText.DisplayedString = leaderboard.GetLeaderboard();
        }

        /// <summary>
        /// Termine la partie
        /// </summary>
        private void EndGame()
        {
            projectilesEnemy.Clear();
            projectilesHero.Clear();
            projectilesToRemove.Clear();
            level.Enemies.Clear();
            leaderboard.AddEntryToLeaderboard(currentScore, playerName);
            leaderboardText.DisplayedString = leaderboard.GetLeaderboard();
            currentWindowState = WindowState.Leaderboard;
            currentGameState = GameState.Finished;
        }

        /// <summary>
        /// Initialize tous les objets de la Game à risque de causer une exception
        /// </summary>
        public void InitializeGame()
        {
            textFont = new Font("Data\\Arts\\Fonts\\8bitOperatorPlus8-Regular.ttf");
            livesTexture = new Texture("Data\\Arts\\New Art\\Hero.png");
            galagaTexture = new Texture("Data\\Arts\\New Art\\Galaga.png");
            int levelAmount = System.IO.Directory.GetFiles("Data\\Levels").Length;
            levels = new string[levelAmount];
            for (int i = 0; i < levelAmount; i++)
            {
                levels[i] = "Data\\Levels\\Level" + i + ".txt";
            }
            level = new Level();
            for (int i = 0; i < levelAmount; i++)
            {
                level.LoadLevel(levels[i]);
            }
            player = new Hero((Constants.WINDOW_WIDTH - ((Constants.GAME_HERO_DEFAULT_WIDTH * Constants.GAME_HERO_SCALE) / 2)) / 2, Constants.WINDOW_HEIGHT - (Constants.DEFAULT_HERO_HEIGHT * Constants.GAME_HERO_SCALE));
            stringTable = new StringTable();
            inputs = new Inputs();
            projectilesHero = new List<Projectile>();
            projectilesEnemy = new List<Projectile>();
            projectilesToRemove = new List<Projectile>();
            starsBackground = new List<Particule>();
            starsMiddleground = new List<Particule>();
            starsForeground = new List<Particule>();
            leaderboardEntete = new Text("", textFont, (Constants.WINDOW_WIDTH / Constants.WINDOW_HEIGHT) * 40);
            leaderboardText = new Text("", textFont, (Constants.WINDOW_WIDTH / Constants.WINDOW_HEIGHT) * 40);
            leaderBoardNameToEnter = new Text("", textFont, (Constants.WINDOW_WIDTH / Constants.WINDOW_HEIGHT) * 40);
            leaderboardOptions = new Text("", textFont, (Constants.WINDOW_WIDTH / Constants.WINDOW_HEIGHT) * 40);
            leaderboardOptions.Color = Color.Red;
            leaderboardEntete.Color = Color.Magenta;
            enemyTimer = new Timer(Constants.TIMER_INTERVAL);
            rnd = new Random();
            galagaSprite = new Sprite(galagaTexture);
            livesSprite1 = new Sprite(livesTexture);
            livesSprite2 = new Sprite(livesTexture);
            livesSprite3 = new Sprite(livesTexture);
            galagaSprite.Origin = new Vector2f(galagaTexture.Size.X / 2, galagaTexture.Size.Y / 2);
            galagaSprite.Position = new Vector2f((Constants.WINDOW_WIDTH / 2) + 5, Constants.WINDOW_HEIGHT / 3);
            galagaSprite.Scale = new Vector2f(0.8f, 0.8f);
            leaderboard = new Leaderboard();
            stringTable.LoadLanguage("Data\\st.txt");
            enemyTimer.Elapsed += OnTimerEnemyEvent;
            enemyTimer.AutoReset = true;
            for (int i = 0; i < starsAmount; i++)
            {
                if (rnd.Next(0, 100) < Constants.STARS_SPAWN_RATE)
                {
                    int starToSpawn = rnd.Next(0, 2);
                    if (starToSpawn == 0)
                        starsBackground.Add(new Particule(rnd.Next(-100, Constants.WINDOW_WIDTH), Constants.WINDOW_WIDTH / starsAmount * i, Constants.STARS_BACKGROUND_SPEED, Constants.STARS_BACKGROUND_SIZE));
                    else if (starToSpawn == 1)
                        starsMiddleground.Add(new Particule(rnd.Next(-100, Constants.WINDOW_WIDTH), Constants.WINDOW_WIDTH / starsAmount * i, Constants.STARS_MIDDLEGROUND_SPEED, Constants.STARS_MIDDLEGROUND_SIZE));
                    else
                        starsForeground.Add(new Particule(rnd.Next(-100, Constants.WINDOW_WIDTH), Constants.WINDOW_WIDTH / starsAmount * i, Constants.STARS_FOREGROUND_SPEED, Constants.STARS_FOREGROUND_SIZE));
                }
            }
        }

        /// <summary>
        /// Gère l'évènement lorsqu'une touche du clavier est appuyée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnKeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.C && currentGameState == GameState.Playing)
            {
                level.Enemies[0].EnemyCurrentState = EnemyState.Capturing;
            }
            if (e.Code == Keyboard.Key.Escape && currentWindowState != WindowState.SplashScreen)
            {
                EndGame();
                gameMustEnd = true;
                gameMustRestart = true;
            }
            else if (e.Code == Keyboard.Key.Escape)
            {
                Environment.Exit(0);
            }

            if (e.Code == Keyboard.Key.Return && currentWindowState == WindowState.SplashScreen)
            {
                currentWindowState = WindowState.Leaderboard;
            }
            else if (e.Code == Keyboard.Key.Return)
            {
                for (int i = 0; i < playerName.Length; i++)
                {
                    if (playerName[i] != ' ')
                    {
                        currentGameState = GameState.Playing;
                        currentWindowState = WindowState.Normal;
                    }
                }
            }

            if (e.Code == Keyboard.Key.F4)
            {
                currentLanguage = Language.French;
            }
            if (e.Code == Keyboard.Key.F5)
            {
                currentLanguage = Language.English;
            }

            if (((e.Code >= Keyboard.Key.A && e.Code <= Keyboard.Key.Num9) || (e.Code == Keyboard.Key.Space)) && currentWindowState == WindowState.Leaderboard && playerName.Length < 10)
            {
                if (isDefaultName)
                {
                    playerName = "";
                    isDefaultName = false;
                }
                if (e.Code >= Keyboard.Key.Num0 && e.Code <= Keyboard.Key.Num9)
                {
                    string[] numbers = e.Code.ToString().Split('m');
                    playerName += numbers[1];
                }
                else if (e.Code >= Keyboard.Key.A && e.Code <= Keyboard.Key.Z)
                {
                    if (Keyboard.IsKeyPressed(Keyboard.Key.LShift) || Keyboard.IsKeyPressed(Keyboard.Key.RShift))
                        playerName += e.Code.ToString().ToUpper();
                    else
                        playerName += e.Code.ToString().ToLower();
                }
                else
                {
                    playerName += " ";
                }
            }
            if (e.Code == Keyboard.Key.BackSpace && currentWindowState == WindowState.Leaderboard)
            {
                if (playerName.Length > 0)
                    playerName = playerName.Substring(0, playerName.Length - 1);
            }

            if ((e.Code == Keyboard.Key.Y || e.Code == Keyboard.Key.O) && currentWindowState == WindowState.Leaderboard && currentGameState == GameState.Finished)
            {
                gameMustEnd = true;
                gameMustRestart = true;
            }
            else if (e.Code == Keyboard.Key.N && currentWindowState == WindowState.Leaderboard && currentGameState == GameState.Finished)
            {
                gameMustEnd = true;
                gameMustRestart = false;
            }
        }

        /// <summary>
        /// Gère la fermeture de la fenêtre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClose(object sender, EventArgs e)
        {
            //RenderWindow window = (RenderWindow)sender;
            //window.Close();
        }

        /// <summary>
        /// Évènement qui gère le tick du timer attaché à la méthode
        /// </summary>
        /// <param name="source">Object source qui est relié à la méthode</param>
        /// <param name="e">Évènement de type ElapsedEventArgs</param>
        private void OnTimerEnemyEvent(Object source, ElapsedEventArgs e)
        {
            int enemyToAttack = rnd.Next(0, level.EnemyCount);
            while (!level.Enemies.Contains(level.Enemies[enemyToAttack]) && level.Enemies[enemyToAttack].EnemyCurrentState != EnemyState.Idle)
            {
                enemyToAttack = rnd.Next(0, level.EnemyCount);
            }
            level.Enemies[enemyToAttack].EnemyCurrentState = EnemyState.AboutToAttack;
        }

        /// <summary>
        /// Vérifie si les 2 rectangles prit en paramètre sont en collision
        /// </summary>
        /// <param name="r1">premier rectangle à comparer de type RectangleShape</param>
        /// <param name="r2">deuxième rectangle à comparer de type RectangleShape</param>
        /// <returns>Retourne un booléen qui indique si les rectangles se touchent</returns>
        private static bool CheckIntersectionBetweenRectangle(RectangleShape r1, RectangleShape r2)
        {
            float xInter1 = ((r1.Position.X - r1.Size.X / 2) + r1.Size.X - r2.Position.X);
            float xInter2 = (((r2.Position.X - r2.Size.X / 2) + r2.Size.X) - r1.Position.X);
            bool xCollide = (xInter1 >= -r2.Size.X / 2) && (xInter2 >= -r1.Size.X / 2);

            float yInter1 = ((r1.Position.Y - r1.Size.Y / 2) + r1.Size.Y - r2.Position.Y);
            float yInter2 = (((r2.Position.Y - r2.Size.Y / 2) + r2.Size.Y) - r1.Position.Y);
            bool yCollide = (yInter1 >= -r2.Size.X / 2) && (yInter2 >= -r1.Size.Y / 2);

            return (xCollide && yCollide);
        }
    }
}