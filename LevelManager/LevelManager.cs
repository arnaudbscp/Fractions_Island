using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LevelManager
{
    private static Dictionary<string, int> _parameters = new Dictionary<string, int>();
    internal static bool finJeux = false;
    internal static bool consigne = true;


    public static int GetParameter(string key)
    {
        if (_parameters.ContainsKey(key))
            return _parameters[key];
        return 0;
    }

    public static void SetParameter(string key,int value)
    {
        if (_parameters.ContainsKey(key))
            _parameters[key] = value;
        else
            _parameters.Add(key, value);
    }

    public static void ClearParameters()
    {
        _parameters.Clear();
    }

    public static void LoadLevel(string level)
    {
        LoadLevel(level, null);
    }

    public static void LoadLevel(string level, Dictionary<string, int> dictionnaire)
    {
        if (dictionnaire != null)
            _parameters = dictionnaire;
        SceneManager.LoadScene(level);
    }

    public static void LoadMessage(bool fin)
    {
        finJeux = fin;
        SceneManager.LoadScene("Scenes/Ecran_affichage/JeuReussi");
    }

    public static int RecupererNiveauSuivant()
    {
        int niveauActuel = GetParameter("levelId");
        int jeuActuel = GetParameter("gameId");
        if (niveauActuel == 3 && jeuActuel != 3)//Fin des jeu 1 et 2
        {
            niveauActuel = 1;
        }
        else
        {
            //On garde le même gameId;
            niveauActuel = niveauActuel + 1;
        }

        return niveauActuel;
    }

    public static int RecupererJeuSuivant()
    {
        int niveauActuel = GetParameter("levelId");
        int jeuActuel = GetParameter("gameId");
        if (niveauActuel == 3 && jeuActuel != 3)//Fin des jeu 1 et 2
        {
            jeuActuel++;
        }
        //Sinon on garde le même gameId;

        return jeuActuel;
    }

}