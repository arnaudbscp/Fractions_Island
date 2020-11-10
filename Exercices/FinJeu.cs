using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinJeu : MonoBehaviour
{
    public GameObject GOtexte;

    public void Start()
    {
        Text texteAffiche = GOtexte.GetComponent<Text>();
        if (LevelManager.finJeux==true)
        {
            texteAffiche.text = "Tu as réussi tous les niveaux disponibles";
        }
        else
        {
            texteAffiche.text = "Tu viens de débloquer le jeu suivant";
        }

    }
}
