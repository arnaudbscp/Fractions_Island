using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void LoadLevelMenu(int gameId)
    {
        //On retient le numéro du jeu choisi
        LevelManager.SetParameter("gameId", gameId);
        // Puis on lance le menu des niveau
        LevelManager.LoadLevel("Scenes/Menu/LevelMenu");
    }

    /// <summary>
    /// Returns to the game play.
    /// </summary>
    public void ReturnGamePlay()
    {
        //Lien avec la partie de Maelle
    }
}
