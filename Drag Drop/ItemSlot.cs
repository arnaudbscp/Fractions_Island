using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    public int nombreAttendu;
    public int numeroSlot;
    public ExerciceScript exo=null;

    //------------------Gestion de l'apparition des explications
    internal int nbErreurs = 0;


    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            //Gestion du drop sur un coin du domino: le texte prend les caractéristiques d'anchor du slot
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;

            //Gestion de l'exactitude du résultats
            //Text texteElement = eventData.pointerDrag.GetComponent<Text>();
            RectTransform test = eventData.pointerDrag.GetComponent<RectTransform>();
            eventData.pointerDrag.GetComponent<DragDrop>().est_depose = true; //On envoie l'information du dépôt au script DragDrop
            //if (int.Parse(texteElement.text)==nombreAttendu)

            if (int.Parse(test.name)==nombreAttendu)
            {
                eventData.pointerDrag.GetComponent<DragDrop>().bon_endroit = true;
                if (exo!=null)
                {
                    exo.ReussirTache(numeroSlot);
                }
            }
            else
            {
                eventData.pointerDrag.GetComponent<DragDrop>().bon_endroit = false;
                eventData.pointerDrag.GetComponent<DragDrop>().numeroSlotDepose = numeroSlot;
            }

        }
    }

}
