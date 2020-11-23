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
    public GameObject registerMenuEleve;
    public GameObject signInMenu;
    public GameObject Developpeur;
    public GameObject registerMenuEns;
    public GameObject PreInscription;

    // ------------- Variables C#   -------------------
    public string email;
    public string password;
    public string username;
    public string teacherPassword;
    const string TEACHER_VERIFICATION = "18765";
    public string classe; // variable de l'élève
    public string classes; // variable du prof
    public bool isAStudent; // Indique si l'utilisateur qui s'inscrit ou qui se connecte est un élève ou non
    public bool isSigningIn = false; // Indique si le menu de connexion doit être affiché (true) ou si c'est celui d'inscription (false)


    // -------------   Lancement de la scene ----------------

    async void Start()
    {
        // Initialisation de Firebase
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://dyscalculie-ensc.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        // Verification de si un utilisateur est déjà connecté 

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
        isSigningIn = false;
        changeColorTypeButton();
        messageErrorText.GetComponent<Text>().text = "";
        switchMenu();
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
        username = theName;
    }

    public void enterTeacherPassword(string teachPassword)
    {
        teacherPassword = teachPassword;
    }

    // Modifier ici les fonctions et la scène sur Unity pour éditer les groupes
    public void enterClasseCP()
    {
        classe = "CP";
        classes += "CP;";
    }
    public void enterClasseCE1()
    {
        classe = "CE1";
        classes += "CE1;";
    }
    public void enterClasseCE2()
    {
        classe = "CE2";
        classes += "CE2;";
    }
    public void enterClasseCM1()
    {
        classe = "CM1";
        classes += "CM1;";
    }
    public void enterClasseCM2()
    {
        classe = "CM2";
        classes += "CM2;";
    }


    /// Navigation entre les deux menus de connexion et d'inscription
    public void switchMenu()
    {
        isSigningIn = !isSigningIn;
        signInMenu.SetActive(false);
        PreInscription.SetActive(false);
        registerMenuEns.SetActive(false);
        registerMenuEleve.SetActive(false);
        if (isSigningIn)
            signInMenu.SetActive(true);
        else
            PreInscription.SetActive(true);
    }
    public void GotoEl()
    {
        PreInscription.SetActive(false);
        registerMenuEleve.SetActive(true);
        isAStudent = true;
    }
    public void GotoEns()
    {
        PreInscription.SetActive(false);
        registerMenuEns.SetActive(true);
        isAStudent = false;
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
        if (string.Equals(TEACHER_VERIFICATION, teacherPassword) || isAStudent == true)
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

        else
        {
            Debug.Log("Wrong password teacher");
        }
    }

    // Fonctions d'inscriptions pour User et Teacher
    public void writeNewUser()
    {
        if (isAStudent)
        {
            Student newUser = new Student(email, username, password, classe);
            string json = JsonUtility.ToJson(newUser);
            reference.Child("users/students/").Child(user.UserId).SetRawJsonValueAsync(json);
            Debug.Log("Student has been added to firebase");
        }
        else
        {
            string[] classesTabTmp = classes.Split(';');
            string res = "";
            int occ = 0;
            for (int i = 0; i < classesTabTmp.Length; i++)
            {
                for (int j = 0; j < classesTabTmp.Length; j++)
                {
                    if (string.Equals(classesTabTmp[i], classesTabTmp[j]))
                        occ += 1;
                }
                if (occ % 2 != 0)
                {
                    bool uniq = true;
                    for (int j = 0; j < res.Split(';').Length; j++)
                    {
                        if (res.Split(';')[j] == classesTabTmp[i])
                            uniq = false;
                    }
                    if (uniq)
                        res += classesTabTmp[i] + ";";
                }
                occ = 0;
            }
            Teacher newUser = new Teacher(email, username, password, res.Split(';'));
            string json = JsonUtility.ToJson(newUser);
            reference.Child("users/teachers/").Child(user.UserId).SetRawJsonValueAsync(json);
            Debug.Log("Teacher has been added to firebase");
        }
    }

}