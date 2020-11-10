using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExerciceScript : MonoBehaviour
{
    public int nbTotalTaches;
    private int[] tableauReponses;
    internal bool exoReussi = false;
    public ItemSlot slot1=null;
    public ItemSlot slot2=null;
    public ItemSlot slot3=null;
    public ItemSlot slot4=null;
    public ItemSlot slot5=null;
    public ItemSlot slot6=null;
    public ItemSlot slot7=null;
    public ItemSlot slot8=null;

    private ItemSlot[] tableauSlots;

    //--------------------------Gestion de l'apparition des explications
    internal int nbTotalErreurs=0;
    public Explications explications;

    public void Awake()
    {
        tableauReponses= new int[nbTotalTaches];
        if (nbTotalTaches==3)
        {
            tableauSlots =new ItemSlot[]{ slot1,slot2,slot3};
        }
        else
        {
            tableauSlots = new ItemSlot[] { slot1, slot2, slot3, slot4, slot5, slot6, slot7, slot8 };
        }

        for (int i=0;i<nbTotalTaches;i++)
        {
            tableauReponses[i] = 0; //On met tout à non reussi
        }
    }

    public void ReussirTache(int nbSlot)
    {
        if (nbSlot>=0 || nbSlot <nbTotalTaches)
        {
            tableauReponses[nbSlot-1] = 1;
            exoReussi=VerifierNiveauReussi();
            explications.affichageReussite(tableauSlots[nbSlot - 1]);
        }
    }

    public void RaterTache(string _nbTache, int numeroSlotDepose) //appelée depuis DragDrop pour gérer les case où le nombre est déposé autre part que sur un slot
    {
        int nbTache = int.Parse(_nbTache);
        if (nbTache >= 0 || nbTache < nbTotalTaches)
        {
            //Si un nombre est déposé au mauvais endroit, il peut venir de 2 endroits
            //-Il vient du bon slot: on cherche le slot en question et on lui enlève la bonne réponse
            //-Il vient du pavé numérique: la réponse associée est déjà à 0 dans le tableau
            foreach (ItemSlot slot in tableauSlots) 
            {
                //Gestion du cas où il vient du bon slot
                if (slot.nombreAttendu==nbTache) 
                {
                    tableauReponses[slot.numeroSlot-1] = 0;
                    if (exoReussi==true)
                    {
                        explications.MessageTest();
                    }
                }
                //Si l'erreur provient du fait que le chiffre ait été posé sur un domino: on compte l'erreur pour le slot 
            }
            if (numeroSlotDepose != -1) //Cas de la démo
            {
                //Gestion des erreurs
                IncrementerErreurs(tableauSlots[numeroSlotDepose - 1]);
            }
            exoReussi = VerifierNiveauReussi();
        }

    }

    public void IncrementerErreurs(ItemSlot slot)
    {
        //On note que pour ce slot, il y a eu une erreur
        slot.nbErreurs++;
        explications.affichageExplication(slot);
        //On incrémente aussi le nombre d'erreurs général
        nbTotalErreurs++;
    }

    public bool VerifierNiveauReussi()
    {
        int compteur = 0;
        bool niveauReussi;
        foreach(int indicateur in tableauReponses)
        {
            if (indicateur==1)
            {
                compteur++;
            }
        }
        if (compteur==nbTotalTaches)
        {
            niveauReussi = true;
        }
        else
        {
            niveauReussi = false;
        }
        return niveauReussi;
    }



}
