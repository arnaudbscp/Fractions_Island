using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMenuScript : MonoBehaviour
{
    /// <summary>
    /// Loads the rules Scene
    /// </summary>
    public void LoadRules(int levelId)
    {
        //On retient le numéro du niveau
        LevelManager.SetParameter("levelId", levelId);
        int gameId= LevelManager.GetParameter("gameId");
        LevelManager.consigne = true;//On lance depuis le menu donc on active la consigne

        // Puis on lance la consigne qui correspond
        if (gameId==1)
        {
            LevelManager.LoadLevel("Scenes/Ecran_affichage/Rules");
        }
        else
        {
            if (gameId == 2 || gameId==3)
            {
                LevelManager.LoadLevel("Scenes/Ecran_affichage/Rules2");
            }
        }
    }


    public void ReturnMenu()
    {
        SceneManager.LoadScene("Scenes/Menu/MainMenu");
    }

    public void ReturnIsland()
    {
        SceneManager.LoadScene(15);
    }

    public void Test()
    {

    }
}
