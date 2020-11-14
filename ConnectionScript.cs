using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System.Threading.Tasks;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;


public class ConnectionScript : MonoBehaviour
{
    //  ---------- Variables pour Firebase  --------------
    private DatabaseReference reference;
    private Firebase.Auth.FirebaseAuth auth;
    private Firebase.Auth.FirebaseUser user;

    // ----------- Elements utiles de la scene -------------
    public GameObject studentButton;
    public GameObject teacherButton;
    public GameObject messageErrorText;
    public GameObject registerMenu;
    public GameObject signInMenu;
    public GameObject Developpeur;
    
    // ------------- Variables C#   -------------------
    //Inputs
    public string email;
    public string password;
    public string username;
    public bool isAStudent; // Indique si l'utilisateur qui s'inscrit ou qui se connecte est un élève ou non
    public bool isSigningIn = true; // Indique si le menu de connexion doit être affiché (true) ou si c'est celui d'inscription (false)


    // -------------   Lancement de la scene ----------------

    async void Start()
    {
        // Initialisation de Firebase
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://dyscalculie-ensc.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        // Verification de si un utilisateur est déjà connecté 

        auth.SignOut(); //Pour les tests, à retirer

        user = auth.CurrentUser;

        if (user != null)
        {
            DataSnapshot snapshot;
            string path = "users/students/" + user.UserId;
            isAStudent = false;
            await FirebaseDatabase.DefaultInstance.GetReference(path).GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    snapshot = task.Result;
                    if (snapshot.Exists)
                        isAStudent = true;
                }
            });
            AccessToNextScene();
        }

        // Si pas d'utilisateur connecté : initialisation des éléments de la scene
        isAStudent = true;
        changeColorTypeButton();
        messageErrorText.GetComponent<Text>().text = "";
    }


    // Gestion des inputs de la scène 

    public void enterEmail(string theEmail)
    {
        email = theEmail;
    }

    public void enterPassword(string thePassword)
    {
        password = thePassword;
    }

    public void enterName(string theName)
    {
        name = theName;
    }


    /// Navigation entre les deux menus de connexion et d'inscription
    public void switchMenu()
    {
        isSigningIn = !isSigningIn;
        signInMenu.SetActive(false);
        registerMenu.SetActive(false);
        if (isSigningIn)
            signInMenu.SetActive(true);
        else
            registerMenu.SetActive(true);
    }


    //-------------------------------------- PARTIE CONNEXION --------------------------------------

    /// Si connecté, charge la scene suivante, l'île pour un élève sinon l'interface des profs
    private void AccessToNextScene()
    {
        if (isAStudent)
            SceneManager.LoadScene("Island");
        else
            SceneManager.LoadScene("TeachersScreen");
    }


    /// Gère la connexion au projet Firebase, la récupération du profil et le passage à la scene suivante
    public async void SignIn()
    {
        await auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
        });
    
        user = auth.CurrentUser;
        if (user == null)
            messageErrorText.GetComponent<Text>().text = "La combinaison Email/mot de passe n'est pas valide";
        else
        {
            isAStudent = false;
            DataSnapshot snapshot;
            string path = "users/students/" + user.UserId;

            await FirebaseDatabase.DefaultInstance.GetReference(path).GetValueAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("The research encountered an error: " + task.Exception);
                }
                else if (task.IsCompleted)
                {
                    snapshot = task.Result;
                    if (snapshot.Exists)
                        isAStudent = true;
                }
            });
            AccessToNextScene();
        }
    }

    //-------------------------------------- PARTIE INSCRIPTION --------------------------------------
    // reste à faire : vérification du code d'inscription pour les profs
    // sélection des classes pour les profs
    // sélection de la classe pr les élèves

    ///  Gère le paramètre élève/prof pendant l'inscription
    /// <param name="number"> 0 pour élève, 1 pour prof</param>
    public void changeType(int number)
    {
        if (number == 0)
            isAStudent = true;
        else
            isAStudent = false;
        changeColorTypeButton();
    }

    ///  Modification des couleurs des boutons élève/prof lors de l'inscription
    public void changeColorTypeButton()
    {
        studentButton.GetComponent<Image>().color = Color.white;
        teacherButton.GetComponent<Image>().color = Color.white;

        if (isAStudent)
            studentButton.GetComponent<Image>().color = Color.blue;
        else
            teacherButton.GetComponent<Image>().color = Color.blue;
    }
    
    /// Gère la création de compte sur le projet firebase, l'ajout à la database et l'acces à la scene suivante
    public async void Register()
    {
        // Création du compte sur le projet firebase
        await auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }
            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
        });

        // Mise à jour de l'user
        user = auth.CurrentUser;

        if (user == null) //Si compte non crée
            messageErrorText.GetComponent<Text>().text = "Erreur de saisie des données : adresse mail ou mot de passe non conforme";
        else //Si compte crée
        {
            writeNewUser();
            AccessToNextScene();
        }
    }

    // Fonctions d'inscriptions pour User et Teacher
    public void writeNewUser()
    {
        if (isAStudent)
        {
            Student newUser = new Student(email, name, password);
            string json = JsonUtility.ToJson(newUser);
            reference.Child("users/students/").Child(user.UserId).SetRawJsonValueAsync(json);
            Debug.Log("Student has been added to firebase");
        }
        else
        {
            Teacher newUser = new Teacher(email, name, password);
            string json = JsonUtility.ToJson(newUser);
            reference.Child("users/teachers/").Child(user.UserId).SetRawJsonValueAsync(json);
            Debug.Log("Teacher has been added to firebase");
        }
    }

}