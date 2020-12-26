using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    public GameObject TextIntro;
    public GameObject TextIntro2;
    public GameObject TextIntro3;
    public GameObject TextIntro4;
    public GameObject TextIntro5;
    public GameObject ButtonNext;
    public Text ButtonNextText;
    public Animator anim;
    
    public GameObject[] texts;
    int compteurTxt;

    // Start is called before the first frame update
    public void Start()
    {
        compteurTxt = 0;
        texts = new GameObject[5] { TextIntro, TextIntro2, TextIntro3, TextIntro4, TextIntro5 };
        TextIntro.SetActive(true);//on active le premier texte
    }

    // Update is called once per frame
    public void UpdateText()
    {
        if (compteurTxt < 4)
        {
            texts[compteurTxt].SetActive(false);//on desactive le texte precedent
            compteurTxt++;
            texts[compteurTxt].SetActive(true);//on active le suivant
        }else
        {
            compteurTxt++;
        }
        if (compteurTxt == 1)
        {
            ButtonNextText.text = "OK!";
            anim.SetTrigger("isStretching");
        }
        else if (compteurTxt == 2 || compteurTxt == 3)
        {
            anim.SetTrigger("isJumping");
        }
        if (compteurTxt == 4)
        {
            ButtonNextText.text = "OUI!";
            anim.SetTrigger("isDancing");
        }
        if (compteurTxt == 5)
        {
            SceneManager.LoadScene("Island");
        }


    }
}
