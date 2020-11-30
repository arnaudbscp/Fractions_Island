using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;

// Gestion des jeux : connexion base de données, menu principal, avancement, argent
// En relation avec le levelmanager, le dossier exercices et lancerjeu
public class HUDScript : MonoBehaviour
{
    public ExerciceScript exercice;
    public Button suivant;

    //------Gestion de l'affichage des niveaux
    public GameObject indNiv1 = null;
    public GameObject indNiv2 = null;
    public GameObject indNiv3 = null;
    public GameObject indNiv4 = null;

    public Button jeu1;
    public Button jeu2;
    public Button jeu3;

    public GameObject img1;
    public GameObject img2;
    public GameObject img3;

    //------Gestion de l'affichage de l'argent
    public GameObject scoreGO;
    public GameObject textConsigne1;
    public GameObject textConsigne2;
    public GameObject textConsigne3;

    //  ---------- Variables pour Firebase  --------------
    private Firebase.Auth.FirebaseAuth auth;
    private Firebase.Auth.FirebaseUser user;
    private DatabaseReference reference;

    public Student current_student;
    public LancerJeu lj = new LancerJeu();

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
        MAJ();
    }

    /// Redirige à l'île
    public void GoToMenu()
    {
        SceneManager.LoadScene("Island");
    }

    // Affiche des GameObject
    public void Afficher(GameObject espaceAAfficher)
    {
        espaceAAfficher.SetActive(true);
    }

    // Affiche l'argent
    public void AfficherScore()
    {
        int score = current_student.money;
        Text texte = scoreGO.GetComponent<Text>();
        texte.text = score.ToString();
    }

    // Ferme des GameObjects
    public void Fermer(GameObject espaceAFermer)
    {
       espaceAFermer.SetActive(false);
    }

    // Autoriser à passer au niveau suivant
    public void AutoriserSuivant()
    {
        if (suivant!=null)
        {
            suivant.interactable = true;
        }
    }

    // Enregistrer la progression à chaque fin de niveau
    public void EnregistrerProgression()
    {
        int gameId = LevelManager.GetParameter("gameId");
        int levelId = LevelManager.GetParameter("levelId");
        if(gameId == 2 && levelId == 1)
        {
            current_student.money += 15;
        }else if(gameId == 3 && levelId == 1)
        {
            current_student.money += 30;
        }
        else if(gameId == 3 && levelId == 5)
        {
            current_student.money += 55;
        }
        current_student.progression[0] = gameId;
        current_student.progression[1] = levelId;
        string json = JsonUtility.ToJson(current_student);
        reference.Child("users/students/").Child(user.UserId).SetRawJsonValueAsync(json);
    }

    // Fonction de mise à jour du menu principal pour démarrer à l'exercice en cours
    public void MAJ()
    {
        AfficherScore();
        // Améliorer visuel de l'avancement
        Button[] listeBoutons = { jeu1, jeu2, jeu3 };
        GameObject[] listeImages = { img1, img2, img3 };
        GameObject[] listeConsignes = { textConsigne1, textConsigne2, textConsigne3 };
        int gameId = current_student.progression[0];
        int levelId = current_student.progression[1]-1;
        listeBoutons[gameId - 1].interactable = true;
        listeImages[gameId - 1].SetActive(true);
        Text t = listeConsignes[gameId - 1].GetComponent<Text>();
        if (levelId == 0)
        {
            t.text = "Début : 0/3";
        }
        if(levelId == 1)
        {
            t.text = "Début : 1/3";
        }
        if(levelId == 2)
        {
            t.text = "Milieu : 2/3";
                
        }
        if(levelId > 2)
        {
            t.text = "Fin : 3/3";
        }
        LevelManager.SetParameter("gameId", gameId);
        LevelManager.SetParameter("levelId", levelId+1);
    }

    // Permet de démmarrer le niveau suivant depuis l'exercice
    public void LauchLevel()
    {
        bool depuisMenu = true;
        //On regarde si le script est appelé depuis la consigne ou depuis le jeu
        if (exercice != null)//depuis le jeu
        {
            depuisMenu = false;
            //On regarde si le niveau est reussi
            LevelManager.consigne = false;
            if (exercice.exoReussi == true)
            {
                //On a le droit de passer au niveau suivant
                int levelId = LevelManager.RecupererNiveauSuivant();
                int gameId = LevelManager.RecupererJeuSuivant();
                //On met à jour la progression
                LevelManager.SetParameter("gameId", gameId);
                LevelManager.SetParameter("levelId", levelId);
                EnregistrerProgression();
                lj.Suite(depuisMenu);

            }
        }
        if (depuisMenu == true)
        {
            lj.Suite(depuisMenu);
        }
    }

    // Affiche la progression dans le jeu
    public void AfficherProgression(int niveau)
    {
        if (niveau == 1)
        {
            indNiv1.SetActive(true);
        }
        else
        {
            if (niveau == 2)
            {

                indNiv2.SetActive(true);
            }
            else
            {
                if (niveau == 3)
                {
                    indNiv3.SetActive(true);
                }
                else
                {
                    if (niveau == 4)
                    {
                        indNiv4.SetActive(true);
                    }
                }
            }

        }
    }

}
