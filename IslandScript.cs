
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IslandScript : MonoBehaviour
{
    // ----------- Elements utiles de la scene -------------
    public GameObject moneyText;
    public GameObject house;
    public GameObject bonfire;
    public GameObject bridge;
    public GameObject upgradeMenu;
    public GameObject upgradeMenuBuildingNameText;
    public GameObject upgradeMenuCostText;

    // ----------- Variables C# -------------
    private int money = 150;
    public int actualObjectID = 0;
    public List<IslandObject> islandObjects;

    // --------- Lancement de la scene ---------------

    void Start()
    {
        //Initialisation des objets constructibles en objet IslandObject 
        GameObject bonfireButton = GameObject.Find("BonfireUpgradeButton");
        GameObject houseButton = GameObject.Find("HouseUpgradeButton");
        GameObject bridgeButton = GameObject.Find("BridgeUpgradeButton");
        islandObjects = new List<IslandObject> { new IslandObject(1, bonfire, bonfireButton, "le feu de camp", 10, false),
                                            new IslandObject(2, house, houseButton, "la maison", 20, false),
                                            new IslandObject(3, bridge, bridgeButton, "le pont", 40, false)};

        // Initialisation des autres variables
        updateMoney(0);
    }



    // --------- Gestion des éléments affichés sur la scene ---------------

    /// <summary>
    /// Met à jour la valeur de l'argent par +amount et modifie le texte affiché
    /// </summary>
    /// <param name="amount"></param>
    void updateMoney(int amount)
    {
        money += amount;
        moneyText.GetComponent<Text>().text = money.ToString();
    }

     /// <summary>
    /// Met à jour les objets qui doivent être affichés dans la scene ou non 
    /// </summary>
    public void updateVisibleObjects()
    {
        foreach(IslandObject oneObject in islandObjects)
        {
            oneObject.unityObject.SetActive(oneObject.active);
            oneObject.button.SetActive(!oneObject.active);
        }
    }

    

    // --------- Element pour l'amélioration de l'île  -------------

    /// <summary>
    /// Ouvre le menu d'amélioration avec les informations de l'objet concerné
    /// </summary>
    /// <param name="theID"></param>
    public void openUpgradeMenu(int theID)
    {
        actualObjectID = theID;
        foreach (IslandObject oneObject in islandObjects)
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

    /// <summary>
    /// Ferme le menu d'amélioration et met à jour les éléments visibles de la scene
    /// </summary>
    public void closeUpgradeMenu()
    {
        updateVisibleObjects();
        upgradeMenu.SetActive(false);
    }

    /// <summary>
    /// Verifie si l'utilisateur a assez d'argent pour l'objet, ferme le menu et met à jour les éléments affichés dans la scene
    /// </summary>
    public void validateUpgradeItem()
    {
        foreach (IslandObject oneObject in islandObjects)
        {
            if (oneObject.id == actualObjectID)
                if (money >= oneObject.cost) //Verification de si l'utilisateur a assez d'argent
                {
                    updateMoney(- oneObject.cost);
                    oneObject.active = true;
                }
        }
        closeUpgradeMenu();
    }




    // --------- Navigation vers d'autres scenes  -------------

    /// <summary>
    /// Navigation vers l'écran de sélection des jeux
    /// </summary>
    public void StartTheGame()
    {
        SceneManager.LoadScene(2);
            }

}


public class IslandObject
{
    public int id;
    public GameObject unityObject;
    public GameObject button;
    public string name;
    public int cost;
    public bool active;

    public IslandObject(int newId, GameObject newObject, GameObject itsButton, string newName, int newCost, bool isActive)
      {
        id = newId;
        unityObject = newObject;
        button = itsButton;
        name = newName;
        cost = newCost;
        active = isActive;
        unityObject.SetActive(active);
    }
}
