using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public float interval = 1.0f;
    public float trackedTime = 0.0f;
    public AudioClip beep;
    public AudioClip longBeep;
    public AudioSource sfxSource;
    public TextMeshProUGUI player1MoneyText;
    public TextMeshProUGUI player2MoneyText;
    public TextMeshProUGUI timeText;
    public ParticleSystem player1Particles;
    public ParticleSystem player2Particles;
    public ParticleSystem trafficParticles;
    public GameObject forceField1;
    public GameObject forceField2;
    public string[] possibleBusinessNames = new string[]{"LemonadeStand","GasStation","FastFoodRestaurant","Park","Farm","Hotel","Bank","Hospital","Theater","Stadium"};

    // Start is called before the first frame update
    void Start()
    {
        player1Particles = player1Particles.GetComponent<ParticleSystem>();
        player2Particles = player2Particles.GetComponent<ParticleSystem>();
        trafficParticles = trafficParticles.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if(StaticData.paused == false){
        //update player money
        player1MoneyText.text = "$" + StaticData.player1Money.ToString();
        player2MoneyText.text = "$" + StaticData.player2Money.ToString();

        //update traffic
        int numBusinessDiff = nullCount(StaticData.placedBusinesses1) - nullCount(StaticData.placedBusinesses2);
        StaticData.player1Traffic = 50 - 3 * numBusinessDiff;
        StaticData.player2Traffic = 50 + 3 * numBusinessDiff;
        int typeBusinessDiff = businessTypesCount(StaticData.placedBusinesses1) - businessTypesCount(StaticData.placedBusinesses2);
        StaticData.player1Traffic = 50 + 3 * typeBusinessDiff;
        StaticData.player2Traffic = 50 - 3 * typeBusinessDiff;
        if(StaticData.player1Traffic < 0){
            StaticData.player1Traffic = -1;
        }  
        if(StaticData.player2Traffic < 0){
            StaticData.player2Traffic = -1;
        }

        //calculate profit for each business and add to player money
        int player1Profit = 0;
        int player2Profit = 0;
        
        int[] business1Over2 = new int[]{0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
        for(int i = 0; i < business1Over2.Length; i++){
            business1Over2[i] = StaticData.numEach1[i] - StaticData.numEach2[i];
        }

        for(int i = 0; i < StaticData.placedBusinesses1.Length; i++){
            if(StaticData.placedBusinesses1[i] != null){
                string name = StaticData.placedBusinesses1[i].name;
                name = name.Substring(0, name.Length-7);
                int numMore = business1Over2[Array.IndexOf(possibleBusinessNames, name)];
                if(StaticData.numEach2[Array.IndexOf(possibleBusinessNames, name)] == 0 && StaticData.numEach1[Array.IndexOf(possibleBusinessNames, name)] != 0){
                    numMore = 20;
                }
                int profit = StaticData.baseRevenue1[name];
                profit += (int)((profit * 2.4 * numMore) + (profit * 1.2 * StaticData.player1Traffic));
                profit *= (int)(0.1 * StaticData.demand1[name]);
                profit -= StaticData.operatingCosts[name];
                profit /= 50000;
                profit *= timeEach1[i] / 1000;
                if (timeEach1[i] < 10000) {
                    timeEach1[i] += StaticData.growthSpeed[name];
                }
                player1Profit += (int)(profit  + 1);
            }
        }
        for(int i = 0; i < StaticData.placedBusinesses2.Length; i++){
            if(StaticData.placedBusinesses2[i] != null){
                string name = StaticData.placedBusinesses2[i].name;
                name = name.Substring(0, name.Length-7);
                int numMore = business1Over2[Array.IndexOf(possibleBusinessNames, name)] * -1;
                if(StaticData.numEach1[Array.IndexOf(possibleBusinessNames, name)] == 0 && StaticData.numEach2[Array.IndexOf(possibleBusinessNames, name)] != 0){
                    numMore = 20;
                }
                int profit = StaticData.baseRevenue2[name];
                profit += (int)((profit * 2.4 * numMore) + (profit * 1.2 * StaticData.player2Traffic));
                profit *= (int)(0.1 * StaticData.demand2[name]);
                profit -= StaticData.operatingCosts[name];
                profit /= 50000;
                profit *= timeEach2[i] / 1000;
                if (timeEach2[i] < 10000) {
                    timeEach2[i] += StaticData.growthSpeed[name];
                }
                player2Profit += (int)(profit + 1);
            }
        }

        if(StaticData.player1Money < Int32.MaxValue - 10000000){
            StaticData.player1Money += player1Profit;
        }
        if(StaticData.player2Money < Int32.MaxValue - 10000000){
            StaticData.player2Money += player2Profit;
        }

        //update animations
        var emission1 = player1Particles.emission;
        var emission2 = player2Particles.emission;
        var emissionTraffic = trafficParticles.emission;
        emission1.rateOverTime = (float)Math.Ceiling((double)player1Profit / 200);
        emission2.rateOverTime = (float)Math.Ceiling((double)player2Profit / 200);
        emissionTraffic.rateOverTime = 14 - nullCount(StaticData.placedBusinesses1) - nullCount(StaticData.placedBusinesses2);

        forceField1.transform.localScale = new Vector3((7f-nullCount(StaticData.placedBusinesses1)) * 0.005f * StaticData.player1Traffic, 0.638f, 0.638f);
        forceField2.transform.localScale = new Vector3((7f-nullCount(StaticData.placedBusinesses2)) * 0.005f * StaticData.player2Traffic, 0.638f, 0.638f);
        // forceField1.transform.localScale = new Vector3((float)Math.Abs(player1Profit/(player2Profit + 1f)) * 0.5f, 0.638f, 0.638f);
        // forceField2.transform.localScale = new Vector3((float)Math.Abs(player2Profit/(player1Profit + 1f)) * 0.5f, 0.638f, 0.638f);

        //update time, end game when time up
        if(StaticData.roundTime > 0){
            StaticData.roundTime -= Time.deltaTime;
        }
        timeText.text = Mathf.Round(StaticData.roundTime).ToString();
        trackedTime += Time.deltaTime;
        if(StaticData.roundTime <= 31 && StaticData.roundTime > 0 && trackedTime >= interval){
            trackedTime = 0.0f;
            sfxSource.PlayOneShot(beep, 1f);
        } else if(StaticData.roundTime < 0.1 && StaticData.roundTime > 0 && !StaticData.gameOver){
            sfxSource.PlayOneShot(longBeep, 1f);
            this.GetComponent<AudioSource>().volume = 0f;
        }        

    }
    }

    public int nullCount(GameObject[] list){
        int count = 0;
        for(int i = 0; i < list.Length; i++){
            if(list[i] == null){
                count++;
            }
        }
        return count;
    }
    public int businessTypesCount(GameObject[] list){
        int count = 0;
        List<Sprite> sprites = new List<Sprite>();
        for(int i = 0; i < list.Length; i++){
            if (list[i] != null && !sprites.Contains(list[i].GetComponent<SpriteRenderer>().sprite)){
                count++;
            }
        }
        return count;
    }
    
}
