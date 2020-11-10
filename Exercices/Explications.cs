using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Explications : MonoBehaviour
{
    public ExerciceScript exo;
    public GameObject espaceTexteExplication;
    public GameObject espaceTexteFeedback;
    public SonScript son;
    public GameObject GOexpl1;
    public GameObject GOexpl2;
    public bool dominoEntier = false;
    public bool dominoChiffres = false;
    public bool haut = false;

    private bool slotComplet = false;
    private HUDScript hud;
    private Text expl1;
    private Text expl2;



    // Start is called before the first frame update
    void Awake()
    {
        hud = GetComponent<HUDScript>();
        expl1 = GOexpl1.GetComponent<Text>();
        expl2 = GOexpl2.GetComponent<Text>();

    }


    public void affichageExplication(ItemSlot slot)
    {
        Text texteFeedback = espaceTexteFeedback.GetComponent<Text>();
        Text explication = espaceTexteExplication.GetComponent<Text>();
        int nbErreursSlot = slot.nbErreurs;
        string t1 = "Oups, essaie encore! ";
        string t2 = "Mauvaise réponse! ";
        string t3 = "C'est faux! ";
        string textEpl= "";

        if (slotComplet==false) //Le joueur n'a jamais fait 3 erreurs sur un même slot: on lui met toujours l'info des explications
        {
            if (nbErreursSlot!=1) //1ère erreur: on ne propose pas d'explication
            {
                son.choix = 1;
                int nbExpl = nbErreursSlot - 1;
                textEpl = "Clique sur l'icône ? pour obtenir l'explication " + nbExpl;
                if (nbErreursSlot >= 3)
                {
                    slotComplet = true;
                    explication.text = expl2.text;
                    son.choix = 3;
                }
                else
                {
                    if (nbErreursSlot==2)
                    {
                        explication.text = expl1.text;
                        son.choix = 2;
                    }
                }
            }
        }
        else
        {
            textEpl = "Clique sur l'icône ? pour obtenir de l'aide";
        }

        if (nbErreursSlot==1)
        {
            texteFeedback.text = t1 + textEpl;
        }
        else
        {
            if (nbErreursSlot==2)
            {
                texteFeedback.text = t2 + textEpl;
            }
            else
            {
                if (nbErreursSlot==3)
                {
                    texteFeedback.text = t3 + textEpl;
                }
                else
                {
                    if (dominoEntier==true)
                    {
                        if (dominoChiffres==false)
                        {
                            if (haut == true)
                            {
                                texteFeedback.text = "La bonne réponse est le domino qui a " + slot.nombreAttendu + " points sur la case du haut.";
                            }
                            else
                            {
                                texteFeedback.text = "La bonne réponse est le domino qui a " + slot.nombreAttendu + " points sur la case du bas.";
                            }
                        }
                        else
                        {
                            if (haut==true)
                            {
                                texteFeedback.text = "La bonne réponse est le domino qui a le chiffre " + slot.nombreAttendu + " en haut.";
                            }
                            else
                            {
                                texteFeedback.text = "La bonne réponse est le domino qui a le chiffre " + slot.nombreAttendu + " en bas.";
                            }
                        }
                    }
                    else
                    {
                        texteFeedback.text = "La bonne réponse est : " + slot.nombreAttendu;
                    }
                }
            }
        }

    }

    public void affichageReussite(ItemSlot slot)
    {
        int nbErreursSlot = slot.nbErreurs;
        Text texteFeedback = espaceTexteFeedback.GetComponent<Text>();
        if (nbErreursSlot==0)//Reussite du premier coup
        {
            texteFeedback.text = "Très bon travail!";
        }
        else 
        { 
            if (nbErreursSlot==1)
            {
                texteFeedback.text = "Bien joué!";
            }
            else
            {
                if (nbErreursSlot==2)
                {
                    texteFeedback.text = "Bien!";
                }
                else
                {
                    if (nbErreursSlot==3)
                    {
                        texteFeedback.text = "C'est juste.";
                    }
                }
            }
        }

        if (exo.exoReussi==true)
        {
            texteFeedback.text = "Bravo, tu as réussi l'exercice! Clique sur la flèche pour passer au niveau suivant.";
            hud.AutoriserSuivant();
            //On récupère le niveau
            int niveau = LevelManager.GetParameter("levelId");
            hud.AfficherProgression(niveau);
        }
    }

    public void MessageTest()
    {
        Text texteFeedback = espaceTexteFeedback.GetComponent<Text>();
        texteFeedback.text = "Tu avais tout juste! Rectifies ta réponse et tu pourras passer au niveau suivant.";
    }



}
