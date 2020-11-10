using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler,IEndDragHandler, IDragHandler, IDropHandler
{
    //---------------------------Gestion du drag and drop général

    [SerializeField] private Canvas canvas;     //Lien avec l'UI Object Canvas 
    private CanvasGroup canvasGroup; //Pour changer l'alpha du texte pendant le déplacement
    public Vector3 positionParDefaut; //Position de départ du texte
    private RectTransform rectTransform; 
    public bool est_depose; //Texte déposé dans le domino
    public bool bon_endroit;


    //---------------------------Gestion de la demo

    [SerializeField] private GameObject gameObjectDemo;     //Lien avec le game Object qui contient tous les GO de la demo
    internal bool demo = false;

    //---------------------------Gestion des exercices
    public ExerciceScript exercice;
    private Text texteElement;
    internal int numeroSlotDepose=-1;

    public void Start()
    {
        positionParDefaut = GetComponent<RectTransform>().localPosition;
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (gameObjectDemo != null)//On regarde si on est dans la démo ou dans l'exercice
        {
            demo = true;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        est_depose = false;
        canvasGroup.blocksRaycasts = false;
        if (demo==false) canvasGroup.alpha = .6f; //Le texte change d'aspect: il devient plus transparent
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f; //Le texte revient à son aspect de base
        StartCoroutine("GestionPosition");
    }

    public void OnDrag(PointerEventData eventData)
    {
        //On suit les mouvements de la souris tout en effacant l'echelle du canvas qui est différente de 1
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnDrop(PointerEventData eventData)
    {
    }

    IEnumerator GestionPosition()
    {
        yield return new WaitForEndOfFrame();
        if (est_depose == false || bon_endroit==false) //Item pas déposé sur un domino (ItemSlot non activé) ou déposé sur le mauvais domino
        {
            rectTransform.localPosition = positionParDefaut;
            if (demo==false)
            {
                exercice.RaterTache(rectTransform.name, numeroSlotDepose);
            }
            numeroSlotDepose = -1;

        }
        else
        {
            //Gestion de la démo
            if (demo == true)
            {
                gameObjectDemo.SetActive(false); //L'écran de démo disparaît
            }
        }

    }

}

