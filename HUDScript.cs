using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUDScript : MonoBehaviour
{
    public ExerciceScript exercice;
    public Button suivant;
    public LevelLauchScript lls;

    //------Gestion de l'affichage des niveaux
    public GameObject indNiv1=null;
    public GameObject indNiv2=null;
    public GameObject indNiv3=null;
    public GameObject indNiv4=null;

    //------Gestion de l'affichage des score
    public GameObject scoreGO;

    //------Lien Firebase
    public PlayerBehavior pb;

    public void Start()
    {
        if (pb!=null)
        {
            AfficherScore();
        }
    }


    /// <summary>
    /// Redirects to main menu.
    /// </summary>
    public void GoToMenu()
    {
        SceneManager.LoadScene("Scenes/Menu/MainMenu");
    }

    public void GererFirebase()
    {
        AjouterScore();
        int gameIdFirebase = pb.GetGame();
        int levelIdFirebase = pb.GetLevel();
        int gameIdActuel = LevelManager.GetParameter("gameId");
        int levelIdActuel = LevelManager.GetParameter("levelId");
        if ((gameIdFirebase == gameIdActuel && levelIdFirebase < levelIdActuel) ||(gameIdFirebase<gameIdActuel))
        {
            //Cas où le jeu n'avait pas été réussi avant
            EnregistrerProgression();
        }
    }

    public void AjouterScore()
    {
        int score = pb.GetScore();
        pb.SetScore(score + 10);
    }

    public void LauchLevel()
    {
        bool depuisMenu = true;
        //On regarde si le script est appelé depuis la consigne ou depuis le jeu
        if (exercice!=null)//depuis le jeu
        {
            depuisMenu = false;
            //On regarde si le niveau est reussi
            LevelManager.consigne = false;
            if (exercice.exoReussi == true)
            {
                //On regarde si le joueur réussi pour la première fois ou non
                GererFirebase();
                //On a le droit de passer au niveau suivant
                int levelId = LevelManager.RecupererNiveauSuivant();
                int gameId = LevelManager.RecupererJeuSuivant();
                //On met à jour la progression
                LevelManager.SetParameter("gameId", gameId);
                LevelManager.SetParameter("levelId", levelId);
                lls.Suite(depuisMenu);

            }

        }

        if (depuisMenu==true)
        {
            lls.Suite(depuisMenu);
        }
    }


    public void Afficher(GameObject espaceAAfficher)
    {
        espaceAAfficher.SetActive(true);
    }

    public void Fermer(GameObject espaceAFermer)
    {
       espaceAFermer.SetActive(false);
    }

    public void AutoriserSuivant()
    {
        if (suivant!=null)
        {
            suivant.interactable = true;
        }
    }

    public void AfficherProgression(int niveau)
    {
        if (niveau==1)
        {
            indNiv1.SetActive(true);
        }
        else
        {
            if (niveau==2)
            {

                indNiv2.SetActive(true);
            }
            else
            {
                if (niveau==3)
                {
                    indNiv3.SetActive(true);
                }
                else
                {
                    if (niveau == 4)
                    {
                        indNiv4.SetActive(true);
                    }
                }
            }

        }
    }

    public void EnregistrerProgression()
    {
        int gameId = LevelManager.GetParameter("gameId");
        int levelId = LevelManager.GetParameter("levelId");
        pb.Set(gameId, levelId);
    }

    public void AfficherScore()
    {
        int score = pb.GetScore();
        Text texte = scoreGO.GetComponent<Text>();
        texte.text = score.ToString();

    }

}
