using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LancerJeu : MonoBehaviour
{
    public void LoadLevel()
    {
        //On recupère le numéro du niveau et du jeu
        int gameId = LevelManager.GetParameter("gameId");
        int levelId = LevelManager.GetParameter("levelId");
        if (gameId==1)
        {
            if (levelId==1)
            {
                LevelManager.LoadLevel("Scenes/Niveaux/J1L1");
            }
            else
            {
                if (levelId == 2)
                {
                    LevelManager.LoadLevel("Scenes/Niveaux/J1L2");
                }
                else
                {
                    if (levelId ==3)
                    {
                        LevelManager.LoadLevel("Scenes/Niveaux/J1L3");
                    }
                }
            }
        }else
        {
            if (gameId == 2)
            {
                if (levelId == 1)
                {
                    LevelManager.LoadLevel("Scenes/Niveaux/J2L1"); 
                }
                else
                {
                    if (levelId == 2)
                    {
                        LevelManager.LoadLevel("Scenes/Niveaux/J2L2");
                    }
                    else
                    {
                        if (levelId == 3)
                        {
                            LevelManager.LoadLevel("Scenes/Niveaux/J2L3");
                        }
                    }
                }
            }
            else
            {
                if (gameId==3)
                {
                    if (levelId == 1)
                    {
                        LevelManager.LoadLevel("Scenes/Niveaux/J3L1");
                    }
                    else
                    {
                        if (levelId == 2)
                        {
                            LevelManager.LoadLevel("Scenes/Niveaux/J3L2");
                        }
                        else
                        {
                            if (levelId == 3)
                            {
                                LevelManager.LoadLevel("Scenes/Niveaux/J3L3");
                            }
                            else
                            {
                                if (levelId == 4)
                                {
                                    LevelManager.LoadLevel("Scenes/Niveaux/J3L4");
                                }
                                else
                                {
                                    //Fin
                                    LevelManager.LoadLevel("Scenes/Menu/MainMenu");
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public void Suite(bool depuisMenu)
    {
        //On recupère le numéro du niveau et du jeu
        int gameId = LevelManager.GetParameter("gameId");
        int levelId = LevelManager.GetParameter("levelId");

        //On regarde si on est à la fin du jeu ou non
        //Sachant que les gameId et levelId ont déjà été incrémentés
        if (depuisMenu==false)
        {
            if ((gameId == 2 || gameId == 3) && levelId == 1)
            {
                LevelManager.LoadMessage(false);
            }
            else
            {
                if (gameId == 3 && levelId == 5)
                {
                    LevelManager.LoadMessage(true);
                }
                else
                {
                    LoadLevel();
                }
            }
        }
        else
        {
            LoadLevel();
        }

    }
}
