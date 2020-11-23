
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;

public class IslandScript : MonoBehaviour
{

    //  ---------- Variables pour Firebase  --------------
    private Firebase.Auth.FirebaseAuth auth;
    private Firebase.Auth.FirebaseUser user;
    private DatabaseReference reference;

    // ----------- Elements utiles de la scene -------------
    public GameObject moneyText;
    public GameObject house;
    public GameObject bonfire;
    public GameObject bridge;
    public GameObject upgradeMenu;
    public GameObject upgradeMenuBuildingNameText;
    public GameObject upgradeMenuCostText;
    public GameObject bonfireButton;
    public GameObject houseButton;
    public GameObject bridgeButton;
    public GameObject profileButton;
    public GameObject profile1;
    public GameObject profile2;
    public GameObject profile3;
    public GameObject profile0;
    public GameObject upgradesAvatar;

    // ----------- Variables C# -------------
    private int money = 0;
    public int actualObjectID = 0;
    public List<Island> Islands;
    public Student current_student;

    // --------- Lancement de la scene ---------------

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

        //Initialisation des objets constructibles en objet Island
        bonfireButton = GameObject.Find("BonfireUpgradeButton");
        houseButton = GameObject.Find("HouseUpgradeButton");
        bridgeButton = GameObject.Find("BridgeUpgradeButton");
        Islands = new List<Island> { new Island(1, bonfire, bonfireButton, "le feu de camp", 15, false),
                                            new Island(2, house, houseButton, "la maison", 30, false),
                                            new Island(3, bridge, bridgeButton, "le pont", 45, false)};

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
        updateMoney(current_student.money);
        // Selectionne l'avatar et mets à jour les objets visibles en meme temps
        selectAvatar(current_student.avatar);
    }

    /// Met à jour la valeur de l'argent par amount et modifie le texte affiché
    void updateMoney(int amount)
    {
        money = amount;
        moneyText.GetComponent<Text>().text = money.ToString();
    }

    /// Met à jour les objets qui doivent être affichés dans la scene selon progression 
    public void updateVisibleObjects()
    {
        bonfireButton.SetActive(true);
        houseButton.SetActive(true);
        bridgeButton.SetActive(true);
        foreach (string i in current_student.avancement_carte)
        {
            if (i.Equals("le feu de camp"))
            {
                bonfire.SetActive(true);
                bonfireButton.SetActive(false);
            }
                

            if(i.Equals("le pont"))
            {
                bridge.SetActive(true);
                bridgeButton.SetActive(false);
            }

            if(i.Equals("la maison"))
            {
                house.SetActive(true);
                houseButton.SetActive(false);
            }
        }
    }

    /// Ouvre le menu d'amélioration avec les informations de l'objet concerné
    public void openUpgradeMenu(int theID)
    {
        actualObjectID = theID;
        foreach (Island oneObject in Islands)
        {
            oneObject.button.SetActive(false); //Désactivation de tous les boutons d'amélioration pour ne pas qu'ils ne supposent au menu d'amélioration
            if (oneObject.id == actualObjectID)  // Récupération des informations de l'objet concerné
            {
                upgradeMenuBuildingNameText.GetComponent<Text>().text = oneObject.name;
                upgradeMenuCostText.GetComponent<Text>().text = oneObject.cost.ToString();
                upgradeMenu.SetActive(true);
            }
        }
    }

    /// Ouvre le menu des avatars avec les informations de l'objet concerné
    public void openUpgradeAvatar()
    {
        foreach (Island oneObject in Islands)
        {
            oneObject.button.SetActive(false); //Désactivation de tous les boutons d'amélioration pour ne pas qu'ils ne supposent au menu d'amélioration

        }
        upgradesAvatar.SetActive(true);
    }

    /// Selectionne l'avatar et ferme le menu
    public void selectAvatar(int id)
    {
        upgradesAvatar.SetActive(false);
        updateVisibleObjects();
        current_student.avatar = id;
        profile0.SetActive(false);
        if (id == 1)
           profile1.SetActive(true);
        else if(id == 2)
            profile2.SetActive(true);
        else if(id == 3)
            profile3.SetActive(true);
        else
            profile0.SetActive(true);
        updateStudent();
    }

    /// Ferme le menu d'amélioration et met à jour les éléments visibles de la scene
    public void closeUpgradeMenu()
    {
        updateVisibleObjects();
        upgradeMenu.SetActive(false);
    }

    /// Verifie si l'utilisateur a assez d'argent pour l'objet, ferme le menu et met à jour les éléments affichés dans la scene
    public void validateUpgradeItem()
    {
        foreach (Island oneObject in Islands)
        {
            if (oneObject.id == actualObjectID)
                if (money >= oneObject.cost)
                {
                    current_student.money = current_student.money - oneObject.cost;
                    updateMoney(current_student.money);
                    string[] tmp = new string[current_student.avancement_carte.Length + 1];
                    if (current_student.avancement_carte.Length == 0)
                        tmp[0] = oneObject.name;
                    else
                    {
                        for (int i = 0; i < tmp.Length - 1; i++)
                            tmp[i] = current_student.avancement_carte[i];
                        tmp[tmp.Length - 1] = oneObject.name;
                    }
                    current_student.avancement_carte = tmp;
                    updateStudent();
                    oneObject.active = true;
                }
        }
        closeUpgradeMenu();
    }

    /// Mettre à jour l'utilisateur sur la base de données
    public void updateStudent()
    {
        string json = JsonUtility.ToJson(current_student);
        reference.Child("users/students/").Child(user.UserId).SetRawJsonValueAsync(json);
    }

    /// Navigation vers l'écran de sélection des jeux
    public void StartTheGame()
    {
        SceneManager.LoadScene(2);
    }

    /// Se déconnecter
    public void QuitTheGame()
    {
        auth.SignOut();
        SceneManager.LoadScene("Connection");
    }

}