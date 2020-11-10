using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitMainMenu : MonoBehaviour
{

    public Button jeu1;
    public Button jeu2;
    public Button jeu3;

    public PlayerBehavior pb;

    public void Start()
    {
        MAJ();

    }

    // Start is called before the first frame update
    public void MAJ()
    {
        Button[] listeBoutons = { jeu1, jeu2, jeu3};
        int gameId = pb.GetGame();
        int levelId = pb.GetLevel();
        listeBoutons[0].interactable = true;
        for (int i = 0; i < gameId; i++)
        {
            listeBoutons[i].interactable = true;
        }
        if (levelId == 3 && gameId <= 2)
        {
            listeBoutons[gameId].interactable = true;
        }
    }
}
