using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;
using System;

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
    public DateTime debut;
    public DateTime fin;
    public Text ConsigneDemo=null;
    public GameObject boutonSon1;
    public GameObject boutonSon2;
    public GameObject boutonSon3;
    public GameObject boutonSon4;
    public AudioClip unSurDeux;
    public AudioClip deuxSurTrois;
    public AudioClip troisSurDeux;
    public AudioClip quatreSurCinq;
    public AudioClip cinqSurSix;
    public AudioClip cinqSurSept;
    public AudioClip sixSurCinq;
    public AudioClip septSurCinq;
    public AudioClip septSurNeuf;
    public AudioClip huitSurTrois;
    public AudioClip[] lesSons=null;

    // Exercice en cours
    public Exercice e;
    public ExerciceDom exerciceDomino;

    private ItemSlot[] tableauSlots;

    //--------------------------Gestion de l'apparition des explications
    internal int nbTotalErreurs=0;
    public Explications explications;

    //  ---------- Variables pour Firebase  --------------
    private Firebase.Auth.FirebaseAuth auth;
    private Firebase.Auth.FirebaseUser user;
    private DatabaseReference reference;

    public Student current_student;

    void Start()
    {
        // Initialisation de Firebase
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://dyscalculie-ensc.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        // On récupère l'utilisateur connecté
        user = auth.CurrentUser;
        // Sinon, on va se connecter
        if (user == null)
            SceneManager.LoadScene("Connection");
        else
        {
            // On s'abonne à la base de données
            FirebaseDatabase.DefaultInstance
              .GetReference("users/students/" + user.UserId)
              .ValueChanged += HandleValueChanged;
        }
    
    }

    // Récupération de l'utilisateur actuel, mise à jour des infos
    void HandleValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        DataSnapshot snapshot = args.Snapshot;
        current_student = JsonUtility.FromJson<Student>(snapshot.GetRawJsonValue());
        if (current_student.progression[0] == 1 && current_student.progression[1] == 1)
            FirebaseDatabase.DefaultInstance.GetReference("exercices/11/").ValueChanged += HandleValueChanged2;
        else if (current_student.progression[0] == 1 && current_student.progression[1] == 2)
            FirebaseDatabase.DefaultInstance.GetReference("exercices/12/").ValueChanged += HandleValueChanged2;
        else if (current_student.progression[0] == 1 && current_student.progression[1] == 3)
            FirebaseDatabase.DefaultInstance.GetReference("exercices/13/").ValueChanged += HandleValueChanged3;
        else if (current_student.progression[0] == 2 && current_student.progression[1] == 1)
            FirebaseDatabase.DefaultInstance.GetReference("exercices/21/").ValueChanged += HandleValueChanged3;
        else if (current_student.progression[0] == 2 && current_student.progression[1] == 2)
            FirebaseDatabase.DefaultInstance.GetReference("exercices/22/").ValueChanged += HandleValueChanged3;
        else if (current_student.progression[0] == 2 && current_student.progression[1] == 3)
            FirebaseDatabase.DefaultInstance.GetReference("exercices/23/").ValueChanged += HandleValueChanged3;
        else if (current_student.progression[0] == 3 && current_student.progression[1] == 1)
            FirebaseDatabase.DefaultInstance.GetReference("exercices/31/").ValueChanged += HandleValueChanged3;
        else if (current_student.progression[0] == 3 && current_student.progression[1] == 2)
            FirebaseDatabase.DefaultInstance.GetReference("exercices/32/").ValueChanged += HandleValueChanged3;
        else if (current_student.progression[0] == 3 && current_student.progression[1] == 3)
            FirebaseDatabase.DefaultInstance.GetReference("exercices/33/").ValueChanged += HandleValueChanged3;
        else if (current_student.progression[0] == 3 && current_student.progression[1] == 4)
            FirebaseDatabase.DefaultInstance.GetReference("exercices/34/").ValueChanged += HandleValueChanged3;
    }

    // Récupération de l'utilisateur actuel, mise à jour des infos
    void HandleValueChanged2(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        DataSnapshot snapshot = args.Snapshot;
        e = JsonUtility.FromJson<Exercice>(snapshot.GetRawJsonValue());
        // Mise à jour de l'exercice :
        // Mise à jour de la consigne :
        ConsigneDemo.text = e.consigneDemo;
        // Mise à jour de ce que l'on veut dans les slots
        for (int i = 0; i < tableauSlots.Length; i++)
        {
            int nb = 0;
            if (i == 0)
                nb = e.Haut1[0];
            else if (i == 1)
                nb = e.Bas1[0];
            else if (i == 2)
                nb = e.Haut2[0];
            else if (i == 3)
                nb = e.Bas2[0];
            else if (i == 4)
                nb = e.Haut3[0];
            else if (i == 5)
                nb = e.Bas3[0];
            else if (i == 6)
                nb = e.Haut4[0];
            else if (i == 7)
                nb = e.Bas4[0];
            tableauSlots[i].nombreAttendu = nb;
        }
        // Mise à jour des vocaux
        string[] nom_audio = { "1_2", "2_3", "3_2", "4_5", "5_6", "5_7", "6_5", "7_5", "7_9", "8_3" };
        lesSons = new AudioClip[] { unSurDeux, deuxSurTrois, troisSurDeux, quatreSurCinq, cinqSurSix, cinqSurSept, sixSurCinq, septSurCinq, septSurNeuf, huitSurTrois };
        for (int i = 0; i < nom_audio.Length; i++)
        {
            if (nom_audio[i].Equals(e.boutonSon1))
                boutonSon1.GetComponent<AudioSource>().clip = lesSons[i];
            if (nom_audio[i].Equals(e.boutonSon2))
                boutonSon2.GetComponent<AudioSource>().clip = lesSons[i];
            if (nom_audio[i].Equals(e.boutonSon3))
                boutonSon3.GetComponent<AudioSource>().clip = lesSons[i];
            if (nom_audio[i].Equals(e.boutonSon4))
                boutonSon4.GetComponent<AudioSource>().clip = lesSons[i];
        }
    }

    // Récupération de l'utilisateur actuel, mise à jour des infos
    void HandleValueChanged3(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        DataSnapshot snapshot = args.Snapshot;
        exerciceDomino = JsonUtility.FromJson<ExerciceDom>(snapshot.GetRawJsonValue());
        // Mise à jour de la consigne :
        ConsigneDemo.text = exerciceDomino.consigneDemo;
        // Mise à jour de ce que l'on veut dans les slots. 
        for (int i = 0; i < tableauSlots.Length; i++)
        {
            int nb = 0;
            if (i == 0)
                nb = exerciceDomino.Dom1[0];
            if (i == 1)
                nb = exerciceDomino.Dom2[0];
            if (i == 2)
                nb = exerciceDomino.Dom3[0];
            tableauSlots[i].nombreAttendu = nb;
        }

        // Mise à jour des vocaux
        string[] nom_audio = { "1_2", "2_3", "3_2", "4_5", "5_6", "5_7", "6_5", "7_5", "7_9", "8_3" };
        lesSons = new AudioClip[] { unSurDeux, deuxSurTrois, troisSurDeux, quatreSurCinq, cinqSurSix, cinqSurSept, sixSurCinq, septSurCinq, septSurNeuf, huitSurTrois };
        for (int i = 0; i < nom_audio.Length; i++)
        {
            if (nom_audio[i].Equals(exerciceDomino.boutonSon1))
                boutonSon1.GetComponent<AudioSource>().clip = lesSons[i];
            if (nom_audio[i].Equals(exerciceDomino.boutonSon2))
                boutonSon2.GetComponent<AudioSource>().clip = lesSons[i];
            if (nom_audio[i].Equals(exerciceDomino.boutonSon3))
                boutonSon3.GetComponent<AudioSource>().clip = lesSons[i];
        }
    }

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
        debut = DateTime.Now;
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
        string[] tmp = new string[current_student.nb_erreurs.Length+1];
        for (int i = 0; i < tmp.Length - 1; i++)
            tmp[i] = current_student.nb_erreurs[i];
        // On note que pour ce slot, il y a eu une erreur
        slot.nbErreurs++;
        explications.affichageExplication(slot);
        // On incrémente aussi le nombre d'erreurs général
        nbTotalErreurs++;
        tmp[tmp.Length - 1] = current_student.progression[0] + ":" + current_student.progression[1] + ":" + slot.name + ":" + nbTotalErreurs; // Partie:Niveau:Nom:ErreurTotalDuNiveau
        current_student.nb_erreurs = tmp;
        string json = JsonUtility.ToJson(current_student);
        reference.Child("users/students/").Child(user.UserId).SetRawJsonValueAsync(json);
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
            fin = DateTime.Now;
            string[] tmp = new string[current_student.temps.Length + 1];
            for (int i = 0; i < tmp.Length - 1; i++)
                tmp[i] = current_student.temps[i];
            tmp[tmp.Length - 1] = current_student.progression[0] + ";" + current_student.progression[1] + ";" + (fin - debut); // Partie;Niveau;Temps
            current_student.temps = tmp;
            string json = JsonUtility.ToJson(current_student);
            reference.Child("users/students/").Child(user.UserId).SetRawJsonValueAsync(json);
        }
        else
        {
            niveauReussi = false;
        }
        return niveauReussi;
    }



}
