using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitLevelMenu : MonoBehaviour
{

    public Button niv1;
    public Button niv2;
    public Button niv3;
    public Button niv4;

    public PlayerBehavior pb;

    public void Start()
    {
        MAJ();

    }

    // Start is called before the first frame update
    public void MAJ()
    {
        Button[] listeBoutons = { niv1, niv2, niv3,niv4};
        int levelId = pb.GetLevel();
        int gameId = pb.GetGame();
        listeBoutons[0].interactable = true;
        if (gameId == LevelManager.GetParameter("gameId"))
        {
            for (int i = 0; i < levelId; i++)
            {
                listeBoutons[i].interactable = true;
            }
            if (levelId <= 2 || (levelId == 3 && gameId == 3))
            {
                listeBoutons[levelId].interactable = true;
            }
        }

    }
}
