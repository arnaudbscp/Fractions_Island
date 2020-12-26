using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


using System.Threading.Tasks;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;


public class TeachersScript : MonoBehaviour
{


    //  ---------- Variables pour Firebase  --------------

    private DatabaseReference reference;
    private Firebase.Auth.FirebaseAuth auth;
    private Firebase.Auth.FirebaseUser user;


    // ----------- Elements utiles de la scene -------------
    public GameObject slideMenu;
    public GameObject slideMenuOpenButton;
    public GameObject addStudentMenu;
    public GameObject alertText;
    public GameObject classicText;
    public GameObject statistiquesText;
    public GameObject MenuEnseignant;
    public GameObject helpStat;
    public GameObject MenuJeux;
    public GameObject resume1;
    public GameObject resume2;
    public GameObject resume3;
    // Stats
    public GameObject StatsTemps;
    public GameObject StatsErreurs;
    public GameObject StatsAides;
    public GameObject StatsHelp;
    public GameObject Graphe;
    public GameObject petit1;
    public GameObject petit2;
    public GameObject petit3;
    public GameObject petit11;
    public GameObject petit22;
    public GameObject petit33;
    public GameObject petit111;
    public GameObject petit222;
    public GameObject petit333;
    public GameObject petit444;
    public GameObject unite;
    public GameObject echelle;
    public GameObject echelle1;
    public GameObject echelle2;
    public GameObject echelle3;

    // ------------- Variables C#   -------------------

    public List<string> studentsID; //Liste des élèves
    public Teacher current_teacher; //Liste des élèves
    public Student current_student_stats; // élève en cours d'analyse
    public List<Student> eleves; //Liste des élèves du professeur
    public SortedList<string, string> studentStat;//chaines de caracteres contenant toutes les statistiques correspondant a un eleve, pseudo de l'eleve est la clef
    public string enterStudentID;


    // ------------- Lancement de la scene -------------------

    void Start()
    {
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
            .GetReference("users/teachers/" + user.UserId)
            .ValueChanged += HandleValueChanged;

            // On récupère les élèves
            FirebaseDatabase.DefaultInstance
            .GetReference("users/students/").GetValueAsync().ContinueWith(task => {
                if (task.IsFaulted)
                {
                    Debug.Log(task.Exception.Message);
                }
                else if (!task.Result.HasChildren)
                {
                    Debug.Log("no children!");
                }
                else
                {
                    eleves = new List<Student>();
                    foreach (var child in task.Result.Children)
                    {
                        eleves.Add(JsonUtility.FromJson<Student>(child.GetRawJsonValue()));
                    }
                    // Initialisation des variables C#
                    studentsID = new List<string>();
                    foreach (Student e in eleves)
                    {
                        foreach(string c in current_teacher.classes)
                            if(e.classe.Equals(c))
                                studentsID.Add(e.username);
                    }
                }
            });

        }
        
        // Mise à jour des stats à faire
        studentStat = new SortedList<string, string>();//associer chaque pseudo a statistiques ex: moyenne =14 etc. en fonction des stats qu'on aura
        alertText.GetComponent<Text>().text = "";

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
        current_teacher = JsonUtility.FromJson<Teacher>(snapshot.GetRawJsonValue());
    }

    // Déconnexion
    public void deconnexion()
    {
        auth.SignOut();
        SceneManager.LoadScene("Connection");
    }

    // ---------------- Fonctions de navigation  --------------------

    /// Ouvre le menu latéral
    public void OpenSlideMenu()
    {
        slideMenu.SetActive(true);
        UpdateStudents();
        slideMenuOpenButton.SetActive(false);
    }

    /// Ferme le menu latéral
    public void CloseSlideMenu()
    {
        slideMenu.SetActive(false);
        slideMenuOpenButton.SetActive(true);
    }

    /// Ouvre le menu d'ajout d'élève
    public void OpenAddStudentMenu()
    {
        addStudentMenu.SetActive(true);
    }

    /// Ferme le menu d'ajour d'élève
    public void CloseAddStudentMenu()
    {
        addStudentMenu.SetActive(false);
    }

    public void OpenJeux()
    {
        MenuEnseignant.SetActive(false);
        MenuJeux.SetActive(true);
    }

    public void OpenHelp()
    {
        helpStat.SetActive(true);
    }

    public void CloseHelp()
    {
        helpStat.SetActive(false);
    }

    public void Retour()
    {
        MenuJeux.SetActive(false);
        MenuEnseignant.SetActive(true);
    }
    public void ouvrirResume1()
    {
        resume1.SetActive(true);
        resume2.SetActive(false);
        resume3.SetActive(false);
    }
    public void ouvrirResume2()
    {
        resume1.SetActive(false);
        resume2.SetActive(true);
        resume3.SetActive(false);
    }
    public void ouvrirResume3()
    {
        resume1.SetActive(false);
        resume2.SetActive(false);
        resume3.SetActive(true);
    }

    // ---------------- Fonctions liées au slideMenu  --------------------

    /// Met à jour les élèves affichés dans le slideMenu
    public void UpdateStudents()//cette fonction ajoute un student
    {
        int i = 0;
        foreach (string student in studentsID)
        {
            i++;
            string objectName = "Text" + i.ToString();
            GameObject studentText = GameObject.Find(objectName);
            studentText.GetComponent<Text>().text = student;

        }
    }

    /// Modifie l'élève affiché par celui sur le quel l'enseignant a appuyé
    /// Stats à afficher
    public void UpdateSelectedStudent(int i)
    {
        CloseSlideMenu();
        if (i <= studentsID.Count)
        {
            classicText.GetComponent<Text>().text = "L'élève sélectionné est " + studentsID[i];
            StatsAides.SetActive(true);
            StatsErreurs.SetActive(true);
            StatsTemps.SetActive(true);
            StatsHelp.SetActive(true);
            Graphe.SetActive(false);
            reinitialiserPoints();
            foreach (Student e in eleves)
            {
                if (e.username.Equals(studentsID[i]))
                    current_student_stats = e;
            }
            
        }
        else
        {
            classicText.GetComponent<Text>().text = "Aucun élève sélectionné.";
            StatsAides.SetActive(false);
            StatsErreurs.SetActive(false);
            StatsTemps.SetActive(false);
            Graphe.SetActive(false);
            StatsHelp.SetActive(false);
            Graphe.SetActive(false);
            reinitialiserPoints();
        }
            
    }

    // ---------------------------------------------
    // ---------------------------------------------
    // ------- Fonctions pour les stats ------------
    // ---------------------------------------------

    public void deplacerPoints(GameObject g, double s, double max)
    {
        float posX = g.transform.position.x;
        float posY;
        // -570px : 0
        // -320px : 50% on fait +250
        // -80px : 100% on fait +240 ou +490
        if (s == 0)
        {
            posY = g.transform.position.y;
        }
        else if (s == max)
        {
            posY = g.transform.position.y + 490; // le plus grand à 100%
        }
        else
        {
            double proportionValeur = (s * 100) / max;
            double proportionPixels = (proportionValeur * 490) / 100;
            posY = g.transform.position.y + (float)proportionPixels;

        }
        Vector2 newPos = new Vector2(posX, posY);
        g.transform.position = newPos;
    }

    public void deplacerPointsEntiers(GameObject g, int s, int max)
    {
        float posX = g.transform.position.x;
        float posY;
        // -570px : 0
        // -320px : 50% on fait +250
        // -80px : 100% on fait +240 ou +490
        if (s == 0)
        {
            posY = g.transform.position.y;
        }
        else if (s == max)
        {
            posY = g.transform.position.y + 490; // le plus grand à 100%
        }
        else
        {
            double proportionValeur = (s * 100) / max;
            double proportionPixels = (proportionValeur * 490) / 100;
            posY = g.transform.position.y + (float)proportionPixels;
        }
        Vector2 newPos = new Vector2(posX, posY);
        g.transform.position = newPos;
    }

    public void reinitialiserPoints() // Remets tous les points à 0 sur le graphe
    {
        float posX = petit1.transform.position.x;
        Vector2 newPos = new Vector2(posX, (float)221.3992);
        petit1.transform.position = newPos;

        posX = petit2.transform.position.x;
        newPos = new Vector2(posX, (float) 221.3992);
        petit2.transform.position = newPos;

        posX = petit3.transform.position.x;
        newPos = new Vector2(posX, (float)221.3992);
        petit3.transform.position = newPos;

        posX = petit11.transform.position.x;
        newPos = new Vector2(posX, (float)221.3992);
        petit11.transform.position = newPos;

        posX = petit22.transform.position.x;
        newPos = new Vector2(posX, (float)221.3992);
        petit22.transform.position = newPos;

        posX = petit33.transform.position.x;
        newPos = new Vector2(posX, (float)221.3992);
        petit33.transform.position = newPos;

        posX = petit111.transform.position.x;
        newPos = new Vector2(posX, (float)221.3992);
        petit111.transform.position = newPos;

        posX = petit222.transform.position.x;
        newPos = new Vector2(posX, (float)221.3992);
        petit222.transform.position = newPos;

        posX = petit333.transform.position.x;
        newPos = new Vector2(posX, (float)221.3992);
        petit333.transform.position = newPos;

        posX = petit444.transform.position.x;
        newPos = new Vector2(posX, (float)221.3992);
        petit444.transform.position = newPos;
    }

    public double majEchelleTemps()
    {
        double secondesMax = 0;
        for (int i = 1; i < current_student_stats.temps.Length; i++)
        {
            // On récupère le niveau et la partie
            string[] partieNiveauTemps = current_student_stats.temps[i].Split(';');
            // On récupère le temps associé
            string[] tempsTmp = partieNiveauTemps[2].Split(':');
            double secondes = 0;
            for (int j = 0; j < tempsTmp.Length; j++)
            {
                if (j == 0)
                    secondes += double.Parse(tempsTmp[j]) * 3600;
                if (j == 1)
                    secondes += double.Parse(tempsTmp[j]) * 60;
                if (j == 2)
                    secondes += double.Parse(tempsTmp[j]);
            }
            // On met à jour l'échelle
            if (secondes > secondesMax)
                secondesMax = secondes;
        }

        return secondesMax;
    }

    public int majEchelleEntier(int type)
    {
        int nbMax= 0;
        if (type == 0)
        {
            for (int i = 1; i < current_student_stats.nb_erreurs.Length; i++)
            {
                string[] erreursTmp = current_student_stats.nb_erreurs[i].Split(':');
                string erreurMax = erreursTmp[erreursTmp.Length - 1];
                if (nbMax < int.Parse(erreurMax))
                    nbMax = int.Parse(erreurMax);
            }
        }
        else if (type == 1)
        {
            int[] clicsaides = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            for (int i = 1; i < current_student_stats.clics_sur_aide.Length; i++)
            {
                string[] aidesTmp = current_student_stats.clics_sur_aide[i].Split(':');
                if (aidesTmp[0].Equals("1") && aidesTmp[1].Equals("1"))
                    clicsaides[0] += 1;
                if (aidesTmp[0].Equals("1") && aidesTmp[1].Equals("2"))
                    clicsaides[1] += 1;
                if (aidesTmp[0].Equals("1") && aidesTmp[1].Equals("3"))
                    clicsaides[2] += 1;
                if (aidesTmp[0].Equals("2") && aidesTmp[1].Equals("1"))
                    clicsaides[3] += 1;
                if (aidesTmp[0].Equals("2") && aidesTmp[1].Equals("2"))
                    clicsaides[4] += 1;
                if (aidesTmp[0].Equals("2") && aidesTmp[1].Equals("3"))
                    clicsaides[5] += 1;
                if (aidesTmp[0].Equals("3") && aidesTmp[1].Equals("1"))
                    clicsaides[6] += 1;
                if (aidesTmp[0].Equals("3") && aidesTmp[1].Equals("2"))
                    clicsaides[7] += 1;
                if (aidesTmp[0].Equals("3") && aidesTmp[1].Equals("3"))
                    clicsaides[8] += 1;
                if (aidesTmp[0].Equals("3") && aidesTmp[1].Equals("4"))
                    clicsaides[9] += 1;

            }
            foreach(int i in clicsaides)
            {
                if (nbMax < i)
                    nbMax = i;
            }
            
        }
        return nbMax;
    }

    public void majGrapheEntier(int erreursMax, int type)
    {
        int[] erreursNiveaux = new int[] {0,0,0,0,0,0,0,0,0,0}; // 1.1, 1.2, 1.3, 2.1,... 3.4
        string[] tab;
        if(type == 0)
            tab = current_student_stats.nb_erreurs;
        else
            tab = current_student_stats.clics_sur_aide;
        for (int i = 1; i < tab.Length; i++)
        {
            // On récupère le niveau et la partie
            string[] erreursTmp = tab[i].Split(':');
            int partieEnCours = int.Parse(erreursTmp[0]);
            int niveauEnCours = int.Parse(erreursTmp[1]);
            if (partieEnCours == 1 && niveauEnCours == 1)
                erreursNiveaux[0] += 1; 
            else if (partieEnCours == 1 && niveauEnCours == 2)
                erreursNiveaux[1] += 1;
            else if (partieEnCours == 1 && niveauEnCours == 3)
                erreursNiveaux[2] += 1;
            else if (partieEnCours == 2 && niveauEnCours == 1)
                erreursNiveaux[3] += 1;
            else if (partieEnCours == 2 && niveauEnCours == 2)
                erreursNiveaux[4] += 1;
            else if (partieEnCours == 2 && niveauEnCours == 3)
                erreursNiveaux[5] += 1;
            else if (partieEnCours == 3 && niveauEnCours == 1)
                erreursNiveaux[6] += 1;
            else if (partieEnCours == 3 && niveauEnCours == 2)
                erreursNiveaux[7] += 1;
            else if (partieEnCours == 3 && niveauEnCours == 3)
                erreursNiveaux[8] += 1;
            else if (partieEnCours == 3 && niveauEnCours == 4)
                erreursNiveaux[9] += 1;
        }

        for(int i = 0; i < erreursNiveaux.Length; i++)
        {
            if (i == 0)
                deplacerPointsEntiers(petit1, erreursNiveaux[i], erreursMax);
            else if (i == 1)
                deplacerPointsEntiers(petit2, erreursNiveaux[i], erreursMax);
            else if (i == 2)
                deplacerPointsEntiers(petit3, erreursNiveaux[i], erreursMax);
            else if (i == 3)
                deplacerPointsEntiers(petit11, erreursNiveaux[i], erreursMax);
            else if (i == 4)
                deplacerPointsEntiers(petit22, erreursNiveaux[i], erreursMax);
            else if (i == 5)
                deplacerPointsEntiers(petit33, erreursNiveaux[i], erreursMax);
            else if (i == 6)
                deplacerPointsEntiers(petit111, erreursNiveaux[i], erreursMax);
            else if (i == 7)
                deplacerPointsEntiers(petit222, erreursNiveaux[i], erreursMax);
            else if (i == 8)
                deplacerPointsEntiers(petit333, erreursNiveaux[i], erreursMax);
            else if (i == 9)
                deplacerPointsEntiers(petit444, erreursNiveaux[i], erreursMax);
        }
    }

    public void majGraphe(double secondesMax) {
        for (int i = 1; i < current_student_stats.temps.Length; i++)
        {
            // On récupère le niveau et la partie
            string[] partieNiveauTemps = current_student_stats.temps[i].Split(';');
            int partieEnCours = int.Parse(partieNiveauTemps[0]);
            int niveauEnCours = int.Parse(partieNiveauTemps[1]);
            // On récupère le temps associé
            string[] tempsTmp = partieNiveauTemps[2].Split(':');
            double secondes = 0;
            for (int j = 0; j < tempsTmp.Length; j++)
            {
                if (j == 0)
                    secondes += double.Parse(tempsTmp[j]) * 3600;
                if (j == 1)
                    secondes += double.Parse(tempsTmp[j]) * 60;
                if (j == 2)
                    secondes += double.Parse(tempsTmp[j]);
            }
            // On met à jour les images selon le niveau
            if (partieEnCours == 1 && niveauEnCours == 1)
                deplacerPoints(petit1, secondes, secondesMax);
            else if (partieEnCours == 1 && niveauEnCours == 2)
                deplacerPoints(petit2, secondes, secondesMax);
            else if (partieEnCours == 1 && niveauEnCours == 3)
                deplacerPoints(petit3, secondes, secondesMax);
            else if (partieEnCours == 2 && niveauEnCours == 1)
                deplacerPoints(petit11, secondes, secondesMax);
            else if (partieEnCours == 2 && niveauEnCours == 2)
                deplacerPoints(petit22, secondes, secondesMax);
            else if (partieEnCours == 2 && niveauEnCours == 3)
                deplacerPoints(petit33, secondes, secondesMax);
            else if (partieEnCours == 3 && niveauEnCours == 1)
                deplacerPoints(petit111, secondes, secondesMax);
            else if (partieEnCours == 3 && niveauEnCours == 2)
                deplacerPoints(petit222, secondes, secondesMax);
            else if (partieEnCours == 3 && niveauEnCours == 3)
                deplacerPoints(petit333, secondes, secondesMax);
            else if (partieEnCours == 3 && niveauEnCours == 4)
                deplacerPoints(petit444, secondes, secondesMax);
        }
    }

    public void afficherStatsTemps()
    {
        // Vérifier que les autres stats sont bien cachées
        Graphe.SetActive(false);
        // Afficher le graphe
        Graphe.SetActive(true);
        // Remet les points à 0
        reinitialiserPoints();
        // Mettre à jour l'unité
        unite.GetComponent<Text>().text = "(s)";
        // Mettre à jour l'échelle et positionner les images
        double secondesMax = majEchelleTemps();
        echelle.GetComponent<Text>().text = (secondesMax / 2).ToString().Substring(0, 4) + "s";
        echelle1.GetComponent<Text>().text = ((secondesMax / 2) / 2).ToString().Substring(0, 4) + "s";
        echelle2.GetComponent<Text>().text = ((secondesMax / 2) + (secondesMax / 4)).ToString().Substring(0, 4) + "s";
        echelle3.GetComponent<Text>().text = (secondesMax).ToString().Substring(0, 4) + "s";
        // On déplace les points
        majGraphe(secondesMax);
    }

    public void afficherStatsErreurs() // à faire
    {
        // Vérifier que les autres stats sont bien cachées
        Graphe.SetActive(false);
        // Afficher le graphe
        Graphe.SetActive(true);
        // Remet les points à 0
        reinitialiserPoints();
        // Mettre à jour l'unité
        unite.GetComponent<Text>().text = "(nb)";
        // Mettre à jour l'échelle et positionner les images
        int nbMax = majEchelleEntier(0);
        echelle.GetComponent<Text>().text = ((float)nbMax / 2).ToString();
        echelle1.GetComponent<Text>().text = (((float)nbMax / 2) / 2).ToString();
        echelle2.GetComponent<Text>().text = (((float)nbMax / 2) + ((float)nbMax / 4)).ToString();
        echelle3.GetComponent<Text>().text = ((float)nbMax).ToString();
        // On déplace les points
        majGrapheEntier(nbMax, 0);
    }

    public void afficherStatsAides() // à faire
    {
        // Vérifier que les autres stats sont bien cachées
        Graphe.SetActive(false);
        // Afficher le graphe
        Graphe.SetActive(true);
        // Remet les points à 0
        reinitialiserPoints();
        // Mettre à jour l'unité
        unite.GetComponent<Text>().text = "(nb)";
        // Mettre à jour l'échelle et positionner les images
        int nbMax = majEchelleEntier(1);
        echelle.GetComponent<Text>().text = ((float)nbMax / 2).ToString();
        echelle1.GetComponent<Text>().text = (((float)nbMax / 2) / 2).ToString();
        echelle2.GetComponent<Text>().text = (((double)nbMax / 2) + ((double)nbMax / 4)).ToString();
        echelle3.GetComponent<Text>().text = ((float)nbMax).ToString();
        // On déplace les points
        majGrapheEntier(nbMax, 1);
    }


    // -----------------------------------------------------
    // ------- Fonctions pour le menu d'ajout d'élève ------
    // -----------------------------------------------------
    // -----------------------------------------------------

    /// Mise à jour du paramètre enterStudentID
    public void EnterID(string theID)
    {
        enterStudentID = theID;
    }

    /// Ajoute l'ID de l'étudiant entré dans la liste d'élève du prof
    public void AddStudent()
    {
        // On récupère les élèves
        FirebaseDatabase.DefaultInstance
        .GetReference("users/students/").GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                Debug.Log(task.Exception.Message);
            }
            else if (!task.Result.HasChildren)
            {
                Debug.Log("no children!");
            }
            else
            {
                foreach (var child in task.Result.Children)
                {
                    eleves.Add(JsonUtility.FromJson<Student>(child.GetRawJsonValue()));
                }
                foreach (Student e in eleves)
                {
                    if (e.username.Equals(enterStudentID))
                    {
                        if(!studentsID.Contains(e.username))
                        {
                            studentsID.Add(e.username);
                        }
                    }    
                }
            }
        });
        CloseSlideMenu();
        CloseAddStudentMenu();
    }

}
