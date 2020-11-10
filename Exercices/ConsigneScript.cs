using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsigneScript : MonoBehaviour
{
    public GameObject ecranConsigne;
    // Start is called before the first frame update
    public void Start()
    {
        if (LevelManager.consigne==true)
        {
            ecranConsigne.SetActive(true);
        }
        else
        {
            ecranConsigne.SetActive(false);
        }
    }
}
