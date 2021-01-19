using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;

public class EventManager : MonoBehaviour
{
    public TextMeshProUGUI newsText;
    public GameObject newsTextObject;
    public AudioClip notification;
    public AudioSource sfxSource;
    public float interval = 1.0f;
    public float trackedTime = 0.0f;

    public static Dictionary<string, string> newsEvents = new Dictionary<string, string>(){
        {"LemonadeStand", "Lemonade suspected sour! Do not drink!"},
        {"GasStation", "New theory that gas contributes to climate change! Save the planet!"},
        {"FastFoodRestaurant", "Big burgers lead to obesity! Avoid fast food!"},
        {"Park", "Leading influencer says 'Parks are Boring'!"},
        {"Farm", "Kids hate vegetables! Farm demand drops!"},
        {"Hotel", "Virus pandemic restricts travel! Hotels crash!"},
        {"Bank", "Study shows banks caused financial collapse! Boycott!"},
        {"Hospital", "Healthcare is a rip off!"},
        {"Theater", "Movie streaming takes over the world!"},
        {"Stadium", "E-sports finally socially acceptable and popular!?"}
    };

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        trackedTime += Time.deltaTime;

        if(newsTextObject.activeSelf == true && trackedTime > 6){
            trackedTime = 0;
            newsTextObject.SetActive(false);
        }

        //call random2PlayerEvent
        if((int)StaticData.roundTime % 30 == 0 && StaticData.roundTime > 5){
            random2PlayerEvent();
        }
        
    }

    public void random2PlayerEvent(){
        //find most popular business of leading player
        string name = "";
        int topPos = 0;
        if(StaticData.player1Money >= StaticData.player2Money){
            topPos = Array.IndexOf(StaticData.numEach1, StaticData.numEach1.Max());
        } else {
            topPos = Array.IndexOf(StaticData.numEach2, StaticData.numEach2.Max());
        }
        name = StaticData.possibleBusinessNames[topPos];

        //lower demand of that business type for all players
        StaticData.demand1[name] = 1;
        StaticData.demand2[name] = 1;

        //display text for that event
        newsText.text = "Breaking News:" + Environment.NewLine + newsEvents[name];

        newsTextObject.SetActive(true);
        sfxSource.PlayOneShot(notification, 0.5f);
    }

}
