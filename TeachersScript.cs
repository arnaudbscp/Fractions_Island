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
    public GameObject verifEdit;
    public GameObject alertText;
    public GameObject classicText;
    public GameObject statistiquesText;
    public GameObject MenuEnseignant;
    public GameObject helpStat;
    public GameObject MenuJeux;
    public GameObject MenuSettings;
    public GameObject resume1;
    public GameObject resume2;
    public GameObject resume3;
    public GameObject jeu11Edit;
    public GameObject jeu12Edit;
    public GameObject consignesEdit;
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
    // Modifier jeux
    public Dropdown drop111;
    public Dropdown drop112;
    public Dropdown drop113;
    public Dropdown drop114;
    public Dropdown drop121;
    public Dropdown drop122;
    public Dropdown drop123;
    public Dropdown drop124;
    public InputField consigne;
    // Récupérer exercice
    public Exercice exo;
    public ExerciceDom exo2;

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

    public void OpenSettings()
    {
        MenuEnseignant.SetActive(false);
        MenuJeux.SetActive(false);
        MenuSettings.SetActive(true);
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

    public void Retour2()
    {
        MenuSettings.SetActive(false);
        MenuEnseignant.SetActive(true);
    }

    public void ouvrirJeu1()
    {
        jeu12Edit.SetActive(false);
        consignesEdit.SetActive(false);
        jeu11Edit.SetActive(true);

        // Récupérer le jeu 1
        FirebaseDatabase.DefaultInstance.GetReference("exercices/11/").ValueChanged += HandleValueChanged2;
    }

    public void ouvrirVerif()
    {
        verifEdit.SetActive(true);
    }

    public void fermerVerif()
    {
        verifEdit.SetActive(false);
    }

    public void ouvrirJeu2()
    {
        jeu11Edit.SetActive(false);
        consignesEdit.SetActive(false);
        jeu12Edit.SetActive(true);

        // Récupérer le jeu 2
        FirebaseDatabase.DefaultInstance.GetReference("exercices/12/").ValueChanged += HandleValueChanged3;
    }

    public void ouvrirConsigne()
    {
        jeu11Edit.SetActive(false);
        jeu12Edit.SetActive(false);
        consignesEdit.SetActive(true);
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
    // ------- Fonctions éditions de jeux ----------
    // ---------------------------------------------

    // Mise à jour de la consigne
    public void editerConsigne(Dropdown dd)
    {
        // Récupérer l'exercice
        string refeData = "";
        if (dd.value == 0)
        {
            FirebaseDatabase.DefaultInstance.GetReference("exercices/11/").ValueChanged += HandleValueChanged2;
            refeData = "11";
        }
        else if (dd.value == 1)
        {
            FirebaseDatabase.DefaultInstance.GetReference("exercices/12/").ValueChanged += HandleValueChanged3;
            refeData = "12";
        }
        else if (dd.value > 1)
        {
            if (dd.value == 2)
                refeData = "13";
            if (dd.value == 3)
                refeData = "21";
            if (dd.value == 4)
                refeData = "22";
            if (dd.value == 5)
                refeData = "23";
            if (dd.value == 6)
                refeData = "31";
            if (dd.value == 7)
                refeData = "32";
            if (dd.value == 8)
                refeData = "33";
            if (dd.value == 9)
                refeData = "34";
            FirebaseDatabase.DefaultInstance.GetReference("exercices/" + refeData + "/").ValueChanged += HandleValueChanged4;
        }
        //Mise à jour de la consigne
        if (int.Parse(refeData) < 13)
        {
            exo.consigneDemo = consigne.text;
            reference.Child("exercices/" + refeData + "/consigneDemo/").SetValueAsync(consigne.text);
        }
        else
        {
            exo2.consigneDemo = consigne.text;
            reference.Child("exercices/" + refeData + "/consigneDemo/").SetValueAsync(consigne.text);
        }

    }

    // Mise à jour de l'exercice
    public void editerExercice(Dropdown dd)
    {
        string[] nom_audio = { "1_2", "2_3", "3_2", "4_5", "5_6", "5_7", "6_5", "7_5", "7_9", "8_3" };
        string json = "";
        if (dd.name.Equals("drop111") || dd.name.Equals("drop121"))
        {
            exo.boutonSon1 = nom_audio[dd.value];
            exo.Haut1[0] = int.Parse(nom_audio[dd.value].Substring(0, 1));
            exo.Bas1[0] = int.Parse(nom_audio[dd.value].Substring(2, 1));
            json = JsonUtility.ToJson(exo);
        }
        if (dd.name.Equals("drop112") || dd.name.Equals("drop122"))
        {
            exo.boutonSon2 = nom_audio[dd.value];
            exo.Haut2[0] = int.Parse(nom_audio[dd.value].Substring(0, 1));
            exo.Bas2[0] = int.Parse(nom_audio[dd.value].Substring(2, 1));
            json = JsonUtility.ToJson(exo);
        }
        if (dd.name.Equals("drop113") || dd.name.Equals("drop123"))
        {
            exo.boutonSon3 = nom_audio[dd.value];
            exo.Haut3[0] = int.Parse(nom_audio[dd.value].Substring(0, 1));
            exo.Bas3[0] = int.Parse(nom_audio[dd.value].Substring(2, 1));
            json = JsonUtility.ToJson(exo);
        }
        if (dd.name.Equals("drop114") || dd.name.Equals("drop124"))
        {
            exo.boutonSon4 = nom_audio[dd.value];
            exo.Haut4[0] = int.Parse(nom_audio[dd.value].Substring(0, 1));
            exo.Bas4[0] = int.Parse(nom_audio[dd.value].Substring(2, 1));
            json = JsonUtility.ToJson(exo);
        }
        if (dd.name.Equals("drop114") || dd.name.Equals("drop112") || dd.name.Equals("drop113") || dd.name.Equals("drop111"))
            reference.Child("exercices/11/").SetRawJsonValueAsync(json);
        else
            reference.Child("exercices/12/").SetRawJsonValueAsync(json);
    }

    // Récupération de l'exercice 1.1
    void HandleValueChanged2(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        DataSnapshot snapshot = args.Snapshot;
        exo = JsonUtility.FromJson<Exercice>(snapshot.GetRawJsonValue());
        // Mise à jour de la valeur des selects :
        string[] nom_audio = { "1_2", "2_3", "3_2", "4_5", "5_6", "5_7", "6_5", "7_5", "7_9", "8_3" };

        for (int i = 0; i < nom_audio.Length; i++)
        {
            if (nom_audio[i].Equals(exo.boutonSon1))
                drop111.value = i;
            if (nom_audio[i].Equals(exo.boutonSon2))
                drop112.value = i;
            if (nom_audio[i].Equals(exo.boutonSon3))
                drop113.value = i;
            if (nom_audio[i].Equals(exo.boutonSon4))
                drop114.value = i;
        }
    }

    // Récupération de l'exercice 1.2
    void HandleValueChanged3(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        DataSnapshot snapshot = args.Snapshot;
        exo = JsonUtility.FromJson<Exercice>(snapshot.GetRawJsonValue());
        // Mise à jour de la valeur des selects :
        string[] nom_audio = { "1_2", "2_3", "3_2", "4_5", "5_6", "5_7", "6_5", "7_5", "7_9", "8_3" };

        for (int i = 0; i < nom_audio.Length; i++)
        {
            if (nom_audio[i].Equals(exo.boutonSon1))
                drop121.value = i;
            if (nom_audio[i].Equals(exo.boutonSon2))
                drop122.value = i;
            if (nom_audio[i].Equals(exo.boutonSon3))
                drop123.value = i;
            if (nom_audio[i].Equals(exo.boutonSon4))
                drop124.value = i;
        }
    }

    // Récupération des autres exercices
    void HandleValueChanged4(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        DataSnapshot snapshot = args.Snapshot;
        exo2 = JsonUtility.FromJson<ExerciceDom>(snapshot.GetRawJsonValue());
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
            if (posY > 550)
            {
                posY = 550;
            }
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
