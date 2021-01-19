using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TwoPlayerStartScreen : MonoBehaviour
{
    public GameObject timeSlider;
    public TextMeshProUGUI roundTimeText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        roundTimeText.text = timeSlider.GetComponent<Slider>().value + " s";
    }

    public void loadGame(){
        SceneManager.LoadScene("2-PlayerGame");
        StaticData.roundTime = timeSlider.GetComponent<Slider>().value;
    }

    public void back(){
        SceneManager.LoadScene("Menu");
    }
}
