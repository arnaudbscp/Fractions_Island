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
    public GameObject MenuJeux;
    // Stats
    public GameObject StatsTemps;
    public GameObject StatsErreurs;
    public GameObject StatsAides;
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

    public void Retour()
    {
        MenuJeux.SetActive(false);
        MenuEnseignant.SetActive(true);
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
        }
            
    }

    // ------- Fonctions pour les stats ------------

    public void afficherStatsTemps()
    {
        // Vérifier que les autres stats sont bien cachées

        // Afficher le graphe
        Graphe.SetActive(true);
        // Mettre à jour l'unité
        unite.GetComponent<Text>().text = "(s)";
        // Mettre à jour l'échelle et positionner les images
        double secondesMax = 0;
        for(int i = 1; i < current_student_stats.temps.Length; i++)
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
            if (secondes > secondesMax)
                secondesMax = secondes;
        }
        echelle.GetComponent<Text>().text = (secondesMax / 2).ToString().Substring(0, 4) + "s";

        // Modifier la hauteur des éléments
        //Vector2 size = StatsAides.transform.lossyScale;
        //size.x = 30;
        //size.x = (size.x * StatsAides.transform.localScale.x) / StatsAides.transform.lossyScale.x;
        //size.y = StatsAides.transform.localScale.y;
        //StatsAides.transform.localScale = size;
    }
    public void afficherStatsErreurs()
    {
        // Vérifier que les autres stats sont bien cachées

    }
    public void afficherStatsAides()
    {
        // Vérifier que les autres stats sont bien cachées

    }

    // ------- Fonctions pour le menu d'ajout d'élève ------------

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
