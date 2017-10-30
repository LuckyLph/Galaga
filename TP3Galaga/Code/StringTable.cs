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
    public class StringTable
    {
        //Dictionnaire qui contient les mots clés pour l'affichage
        private Dictionary<string, string> items;

        /// <summary>
        /// Propriété qui gère le nombre d'items dans le dictionnaire
        /// </summary>
        public int NumItems
        {
            get
            {
                return items.Count;
            }
        }


        /// <summary>
        /// Constructeur de la classe StringTable
        /// </summary>
        public StringTable()
        {
            this.items = new Dictionary<string, string>();
        }

        /// <summary>
        /// Retourne la valeur associé à la langue et l'id entré en paramètre
        /// </summary>
        /// <param name="language">language de la string voulue</param>
        /// <param name="id">id qui représente quelle string est demandée</param>
        /// <returns>string dans la labgue voulue</returns>
        public string GetValue(Language language, string id)
        {
            string valueToReturn;
            try
            {
                if (language == Language.French)
                {
                    //Va chercher la valeur voulue dans le dictionnaire
                    items.TryGetValue(id, out valueToReturn);
                    //Sépare les 2 langues et retourne celle qui équivaut au français
                    string[] languageProperties = valueToReturn.Split('-');
                    valueToReturn = languageProperties[0];
                    return valueToReturn;
                }
                else
                {
                    //Va chercher la valeur voulue dans le dictionnaire
                    items.TryGetValue(id, out valueToReturn);
                    //Sépare les 2 langues et retourne celle qui équivaut à l'anglais
                    string[] languageProperties = valueToReturn.Split('-');
                    valueToReturn = languageProperties[3];
                    return valueToReturn;
                }
            }
            catch (IndexOutOfRangeException)
            {
                System.Windows.Forms.MessageBox.Show("Il y a un champ manquant !", "Erreur de chargement du fichier de langues !",
                                                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                Environment.Exit(1);
                return "Échec";
            }
        }

        /// <summary>
        /// Charge les langues à partir du fichier texte prit en paramètre
        /// </summary>
        /// <param name="languagePath">string qui équivaut au chemin du fichier texte à charger</param>
        public void LoadLanguage(string languagePath)
        {
            if (System.IO.File.Exists(languagePath))
            {
                string[] languageContent = System.IO.File.ReadAllLines(languagePath);
                ErrorCode languageCondition = Parse(languageContent);
                if (languageCondition == ErrorCode.BAD_FILE_FORMAT)
                {
                    System.Windows.Forms.MessageBox.Show("Le format du fichier est incorrect !", "Erreur de chargement du fichier de langues !",
                                                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                    Environment.Exit(1);
                }
                else if (languageCondition == ErrorCode.MISSING_FIELD)
                {
                    System.Windows.Forms.MessageBox.Show("Il y a un champ manquant !", "Erreur de chargement du fichier de langues !",
                                                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                    Environment.Exit(1);
                }
                else if (languageCondition == ErrorCode.UNKNOWN_ERROR)
                {
                    System.Windows.Forms.MessageBox.Show("Erreur inconnue !", "Erreur de chargement du fichier de langues !",
                                                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                    Environment.Exit(1);
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Le fichier n'existe pas !", "Erreur de chargement du fichier de langues !",
                                                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                Environment.Exit(1);
            }
        }

        /// <summary>
        /// Charge les string et les id en extraiyant du tableau et en les placant dans un dictionnaire
        /// </summary>
        /// <param name="fileContent">tableau de string qu contient le fichier à charger</param>
        /// <returns>ErrorCode qui indique si le chargement a réussi</returns>
        private ErrorCode Parse(string[] fileContent)
        {
            try
            {
                //Sert à traiter un id à la fois puisque une ligne représente un id
                for (int i = 0; i < fileContent.Length; i++)
                {
                    string lineToLoad = fileContent[i];
                    //Sépare l'id du reste de la ligne
                    string[] getId = lineToLoad.Split('=');
                    string id = getId[0];
                    //Sépare la string du reste de la ligne et ajoute un nouvel item dans le dictionnaire avec l'id et la string extraite
                    string[] languageProperties = lineToLoad.Split('>');
                    items.Add(id, languageProperties[1]);
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
    }
}
