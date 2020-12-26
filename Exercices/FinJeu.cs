using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinJeu : MonoBehaviour
{
    public GameObject GOtexte;
    public float fallSpeed = 3000.0f;
    public float spinSpeed = 1f;
    // Chaque pièce est animée pour tomber
    public GameObject piece; public GameObject piece1; public GameObject piece2; public GameObject piece3; public GameObject piece4;
    public GameObject piece5; public GameObject piece6; public GameObject piece7; public GameObject piece8; public GameObject piece9;
    public GameObject piece10; public GameObject piece11; public GameObject piece12; public GameObject piece13; public GameObject piece14;
    public GameObject piece15; public GameObject piece16; public GameObject piece17; public GameObject piece18; public GameObject piece19;
    public GameObject piece20; 

    public void Start()
    {
        Text texteAffiche = GOtexte.GetComponent<Text>();
        if (LevelManager.finJeux==true)
        {
            texteAffiche.text = "Tu as réussi tous les niveaux disponibles";
        }
        else
        {
            texteAffiche.text = "Tu viens de débloquer la partie suivante, et tu as gagné de l'argent !";
        }
    }

    // Chaque pièce est animée pour tomber
    void Update()
    {
        fallSpeed = 100.0f;
        spinSpeed = 100f;
        piece.transform.Translate(Vector3.down * fallSpeed, Space.World);
        piece.transform.Rotate(Vector3.forward, spinSpeed);
        piece1.transform.Translate(Vector3.down * fallSpeed, Space.World);
        piece1.transform.Rotate(Vector3.forward, spinSpeed);
        piece2.transform.Translate(Vector3.down * fallSpeed, Space.World);
        piece2.transform.Rotate(Vector3.forward, spinSpeed);
        fallSpeed = 80.0f;
        spinSpeed = 1f;
        piece3.transform.Translate(Vector3.down * fallSpeed, Space.World);
        piece3.transform.Rotate(Vector3.forward, spinSpeed);
        piece4.transform.Translate(Vector3.down * fallSpeed, Space.World);
        piece4.transform.Rotate(Vector3.forward, spinSpeed);
        piece5.transform.Translate(Vector3.down * fallSpeed, Space.World);
        piece5.transform.Rotate(Vector3.forward, spinSpeed);
        piece6.transform.Translate(Vector3.down * fallSpeed, Space.World);
        piece6.transform.Rotate(Vector3.forward, spinSpeed);
        fallSpeed = 32.0f;
        spinSpeed = 20f;
        piece7.transform.Translate(Vector3.down * fallSpeed, Space.World);
        piece7.transform.Rotate(Vector3.forward, spinSpeed);
        piece8.transform.Translate(Vector3.down * fallSpeed, Space.World);
        piece8.transform.Rotate(Vector3.forward, spinSpeed);
        piece9.transform.Translate(Vector3.down * fallSpeed, Space.World);
        piece9.transform.Rotate(Vector3.forward, spinSpeed);
        piece10.transform.Translate(Vector3.down * fallSpeed, Space.World);
        piece10.transform.Rotate(Vector3.forward, spinSpeed);
        piece11.transform.Translate(Vector3.down * fallSpeed, Space.World);
        piece11.transform.Rotate(Vector3.forward, spinSpeed);
        fallSpeed = 41.0f;
        spinSpeed = 1f;
        piece12.transform.Translate(Vector3.down * fallSpeed, Space.World);
        piece12.transform.Rotate(Vector3.forward, spinSpeed);
        piece13.transform.Translate(Vector3.down * fallSpeed, Space.World);
        piece13.transform.Rotate(Vector3.forward, spinSpeed);
        piece14.transform.Translate(Vector3.down * fallSpeed, Space.World);
        piece14.transform.Rotate(Vector3.forward, spinSpeed);
        piece15.transform.Translate(Vector3.down * fallSpeed, Space.World);
        piece15.transform.Rotate(Vector3.forward, spinSpeed);
        piece16.transform.Translate(Vector3.down * fallSpeed, Space.World);
        piece16.transform.Rotate(Vector3.forward, spinSpeed);
        fallSpeed = 87.0f;
        spinSpeed = 1f;
        piece17.transform.Translate(Vector3.down * fallSpeed, Space.World);
        piece17.transform.Rotate(Vector3.forward, spinSpeed);
        piece18.transform.Translate(Vector3.down * fallSpeed, Space.World);
        piece18.transform.Rotate(Vector3.forward, spinSpeed);
        fallSpeed = 13.0f;
        spinSpeed = 1f;
        piece19.transform.Translate(Vector3.down * fallSpeed, Space.World);
        piece19.transform.Rotate(Vector3.forward, spinSpeed);
        fallSpeed = 23.0f;
        spinSpeed = 1f;
        piece20.transform.Translate(Vector3.down * fallSpeed, Space.World);
        piece20.transform.Rotate(Vector3.forward, spinSpeed);
    }
}
