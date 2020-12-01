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


    // ------------- Variables C#   -------------------

    public List<string> studentsID; //Liste des élèves
    public Teacher current_teacher; //Liste des élèves 
    public List<Student> eleves; //Liste des élèves 
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
        if (i <= studentsID.Count)
        {
            classicText.GetComponent<Text>().text = "L'élève sélectionné est " + studentsID[i];
            statistiquesText.GetComponent<Text>().text = "Statistiques: " + studentStat[studentsID[i]];//on affiche les stat de l'eleve
        }
        else
            classicText.GetComponent<Text>().text = "Aucun élève sélectionné.";
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
