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

    //private DatabaseReference reference;
    //private Firebase.Auth.FirebaseAuth auth;
    //private Firebase.Auth.FirebaseUser user;


    // ----------- Elements utiles de la scene -------------
    public GameObject slideMenu;
    public GameObject slideMenuOpenButton;
    public GameObject addStudentMenu;
    public GameObject alertText;
    public GameObject classicText;


    // ------------- Variables C#   -------------------

    public List<string> studentsID; //Liste des élèves 
    public string enterStudentID;


    // ------------- Lancement de la scene -------------------

    void Start()
    {
        // Initialisation de firebase
        //FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://dyscalculie-ensc.firebaseio.com/");
        //reference = FirebaseDatabase.DefaultInstance.RootReference;
        //auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        //user = auth.CurrentUser;

        // Initialisation des variables C#
        studentsID = new List<string> { "Alban", "Lucie", "Manon", "Alex"};
        alertText.GetComponent<Text>().text = "";

    }


    // ---------------- Fonctions de navigation  --------------------
    
    /// <summary>
    /// Ouvre le menu latéral
    /// </summary>
    public void OpenSlideMenu()
    {
        slideMenu.SetActive(true);
        UpdateStudents();
        slideMenuOpenButton.SetActive(false);
    }

    /// <summary>
    /// Ferme le menu latéral
    /// </summary>
    public void CloseSlideMenu()
    {
        slideMenu.SetActive(false);
        slideMenuOpenButton.SetActive(true);
    }

    /// <summary>
    /// Ouvre le menu d'ajout d'élève
    /// </summary>
    public void OpenAddStudentMenu()
    {
        addStudentMenu.SetActive(true);
    }
    
    /// <summary>
    /// Ferme le menu d'ajour d'élève
    /// </summary>
    public void CloseAddStudentMenu()
    {
        addStudentMenu.SetActive(false);
    }


    // ---------------- Fonctions liées au slideMenu  --------------------

    /// <summary>
    /// Met à jour les élèves affichés dans le slideMenu
    /// </summary>
    public void UpdateStudents()
    { // S'assurer que le slideMenu est bien ouvert quand cette fonction est utilisée 
        int i = 0;
        foreach(string student in studentsID)
        {
            i++;
            string objectName = "Text" + i.ToString();
            GameObject studentText = GameObject.Find(objectName);
            studentText.GetComponent<Text>().text = student;
        }
    }

    /// <summary>
    /// Modifie l'élève affiché par celui sur le quel l'enseignant a appuyé
    /// </summary>
    public void UpdateSelectedStudent(int i)
    {
        if (i <= studentsID.Count)
            classicText.GetComponent<Text>().text = "L'élève sélectionné est " + studentsID[i];
        else
            classicText.GetComponent<Text>().text = "Aucun élève sélectionné." ;
    }


    // ------- Fonctions pour le menu d'ajout d'élève ------------


    /// <summary>
    /// Mise à jour du paramètre enterStudentID
    /// </summary>
    public void EnterID(string theID)
    {
        enterStudentID = theID;
    }

    /// <summary>
    /// Ajoute l'ID de l'étudiant entré dans la liste d'élève du prof
    /// </summary>
    public void AddStudent()
    {
        // Récupération des données non finie, penser à ajouter le "async" quand réutilisé
        //bool worked = false;
        //DataSnapshot snapshot;

        //string path = "users/students/" + studentID + "/email";
        //await FirebaseDatabase.DefaultInstance.GetReference(path).GetValueAsync().ContinueWith(task =>
        //{
        //    if (task.IsCompleted)
        //    {
        //        snapshot = task.Result;
        //        if (snapshot.Exists)
        //        {
        //            Debug.Log("d");
        //            worked = true;
        //        }
        //        else
        //        {
        //            Debug.Log("e");
        //        }
        //    }
        //});

        //if (worked)
        //{ alertText.GetComponent<Text>().text = "L'élève a bien été enregistré.";
        //    studentsID.Add(studentID);
        //    studentID = "";
        //}
        //else
        //    alertText.GetComponent<Text>().text = "Cet ID ne correspond à aucun élève.";

        studentsID.Add(enterStudentID);
        UpdateStudents();
        alertText.GetComponent<Text>().text = "L'élève a bien été enregistré.";

    }

}
