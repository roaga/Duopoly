using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller2 : MonoBehaviour
{
    public GameObject cursor2;
    public int cursor2Pos = 0;
    public GameObject[] buttonList;
    public GameObject[] spawnPts2;
    public GameObject[] possibleBusinesses;
    public string[] possibleBusinessNames = new string[]{"LemonadeStand","GasStation","FastFoodRestaurant","Park","Farm","Hotel","Bank","Hospital","Theater","Stadium"};
    public AudioClip investSound;
    public AudioClip placeSound;
    public AudioClip deleteSound;
    public AudioClip invalidSound;
    public AudioSource sfxSource;

    public string aiNextMoveName = null;
    public int aiCost = 0;
    public float trackedTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        if(!StaticData.twoPlayers){
            //hide cursor2
            cursor2.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update(){
        if(StaticData.paused == false){
        if(StaticData.twoPlayers){
            if(Input.GetKeyDown(KeyCode.LeftArrow)){
                //move left
                if(cursor2Pos > 0){ 
                    cursor2Pos--;
                    cursor2.transform.position = buttonList[cursor2Pos].transform.position + new Vector3(0.2f, -0.5f, 0);
                }
            }
            if(Input.GetKeyDown(KeyCode.RightArrow)){
                //move right
                if(cursor2Pos < 9){ 
                    cursor2Pos++;
                    cursor2.transform.position = buttonList[cursor2Pos].transform.position + new Vector3(0.2f, -0.5f, 0);
                }
            }
            if(Input.GetKeyDown(KeyCode.Period)){
                //build
                int spawnNum = -1;
                for(int i = 0; i < StaticData.placedBusinesses2.Length; i++){
                    if(StaticData.placedBusinesses2[i] == null){
                        spawnNum = i;
                        break;
                    }
                }
                if(spawnNum != -1){
                    Vector3 spawnPos = spawnPts2[spawnNum].transform.position;
                    spawnPos += new Vector3(0, 0, spawnNum * -1f);
                    GameObject toInstantiate = possibleBusinesses[cursor2Pos];
                    string name = toInstantiate.name;
                    int buildCost = StaticData.buildCosts[name];
                    if(StaticData.player2Money >= buildCost){
                        GameObject thisBusiness = (GameObject)Instantiate(toInstantiate,spawnPos, Quaternion.identity);
                        StaticData.placedBusinesses2[spawnNum] = thisBusiness;
                        StaticData.player2Money -= buildCost;
                        StaticData.numEach2[cursor2Pos] += 1;
                        sfxSource.PlayOneShot(placeSound, 1f);
                    } else{
                        sfxSource.PlayOneShot(invalidSound, 1f);
                    }
                }else{
                    sfxSource.PlayOneShot(invalidSound, 1f);
                }
            }
            if(Input.GetKeyDown(KeyCode.Slash)){
                //invest
                string name = possibleBusinesses[cursor2Pos].name;
                int buildCost = StaticData.buildCosts[name];
                if(StaticData.player2Money >= 0.75*buildCost){
                    StaticData.demand2[name] += 10;
                    StaticData.player2Money -= (int)(0.1*buildCost);
                    sfxSource.PlayOneShot(investSound, 0.5f);
                } else{
                        sfxSource.PlayOneShot(invalidSound, 1f);
                }
            }
            if(Input.GetKeyDown(KeyCode.RightShift)){
                //sell
                Sprite spriteToCompare = possibleBusinesses[cursor2Pos].GetComponent<SpriteRenderer>().sprite;
                for(int i = 0; i < StaticData.placedBusinesses2.Length; i++){
                    if(StaticData.placedBusinesses2[i] != null && spriteToCompare == StaticData.placedBusinesses2[i].GetComponent<SpriteRenderer>().sprite){
                        Destroy(StaticData.placedBusinesses2[i]);
                        StaticData.placedBusinesses2[i] = null;
                        StaticData.numEach2[cursor2Pos] -= 1;
                        string name = possibleBusinesses[cursor2Pos].name;
                        StaticData.demand2[name] -= 10;
                        StaticData.player2Money += (int)(StaticData.buildCosts[name] * 0.1);
                        sfxSource.PlayOneShot(deleteSound, 1f);
                        break;
                    }
                }
            }
        } else{ //AI Player
            trackedTime += Time.deltaTime;
            int[] business1Over2 = new int[]{0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
            for(int i = 0; i < business1Over2.Length; i++){
                business1Over2[i] = StaticData.numEach1[i] - StaticData.numEach2[i];
            }
            if(trackedTime > 1f){ //aiNextMoveName == null && 
                trackedTime = 0f;
                //calculate business with highest profit w/ build cost less than double of current money
                int posMaxProfit = 0;
                int maxProfit = 0;
                for(int i = 0; i < possibleBusinesses.Length; i++){
                    string name = StaticData.possibleBusinessNames[i];
                    int numMore = (business1Over2[Array.IndexOf(possibleBusinessNames, name)] - 1)* -1;
                    if(StaticData.numEach1[Array.IndexOf(possibleBusinessNames, name)] == 0){
                        numMore = 20;
                    }
                    int profit = StaticData.baseRevenue2[name];
                    profit += (int)((profit * 2.4 * numMore) + (profit * 1.2 * StaticData.player2Traffic));
                    profit *= (int)(0.1 * StaticData.demand2[name]);
                    profit -= StaticData.operatingCosts[name];
                    profit /= 50000;
                    profit += (int)(profit + 1);
                    
                    if(profit >= maxProfit && StaticData.buildCosts[possibleBusinesses[i].name] <= 2.0 * StaticData.player2Money){
                        maxProfit = profit;
                        posMaxProfit = i;
                        aiCost = StaticData.buildCosts[possibleBusinesses[i].name];
                    }
                }

                //calculate profit of any current business invested 5 times
                int currentPosMaxProfit = 0;
                int currentMaxProfit = 0;
                int investCost = 0;
                for(int i = 0; i < StaticData.placedBusinesses2.Length; i++){
                    if(StaticData.placedBusinesses2[i] != null){
                        string name = StaticData.placedBusinesses2[i].name;
                        name = name.Substring(0, name.Length-7);
                        int numMore = (business1Over2[Array.IndexOf(possibleBusinessNames, name)])* -1;
                        if(StaticData.numEach1[Array.IndexOf(possibleBusinessNames, name)] == 0 && StaticData.numEach2[Array.IndexOf(possibleBusinessNames, name)] != 0){
                            numMore = 20;
                        }
                        int profit = StaticData.baseRevenue2[name];
                        profit += (int)((profit * 2.4 * numMore) + (profit * 1.2 * StaticData.player2Traffic));
                        profit *= (int)(0.1 * StaticData.demand2[name] * 1.61);
                        profit -= StaticData.operatingCosts[name];
                        profit /= 50000;
                        profit += (int)(profit + 1);
                        
                        if(profit >= maxProfit && StaticData.buildCosts[name] * 0.5 <= 2.0 * StaticData.player2Money && StaticData.demand2[name] < 150){
                            currentMaxProfit = profit;
                            currentPosMaxProfit = Array.IndexOf(possibleBusinessNames, name);
                            investCost = (int)(StaticData.buildCosts[name] * 0.1);
                        }
                    }
                }
                //set next move
                if(currentMaxProfit > maxProfit){
                    aiNextMoveName = "invest";
                    cursor2Pos = currentPosMaxProfit;
                    aiCost = investCost;
                } else{
                    aiNextMoveName = "build";
                    cursor2Pos = posMaxProfit;
                }
            } else{
                if(aiNextMoveName == "build" && StaticData.player2Money >= aiCost){
                    if(nullCount(StaticData.placedBusinesses2) == 0){
                        // calculate least profitable business and delete it if not the same as replacement
                        int worstPos = 0;
                        int worstProfit = Int32.MaxValue;
                        for(int i = 0; i < StaticData.placedBusinesses2.Length; i++){
                            if(StaticData.placedBusinesses2[i] != null){
                                string str = StaticData.placedBusinesses2[i].name;
                                str = str.Substring(0, str.Length-7);
                                int numMore = (business1Over2[Array.IndexOf(possibleBusinessNames, str)])* -1;
                                if(StaticData.numEach1[Array.IndexOf(possibleBusinessNames, str)] == 0 && StaticData.numEach2[Array.IndexOf(possibleBusinessNames, str)] != 0){
                                    numMore = 20;
                                }
                                int profit = StaticData.baseRevenue2[str];
                                profit += (int)((profit * 2.4 * numMore) + (profit * 1.2 * StaticData.player2Traffic));
                                profit *= (int)(0.1 * StaticData.demand2[str]);
                                profit -= StaticData.operatingCosts[str];
                                profit /= 50000;
                                profit += (int)(profit + 1);
                                
                                if(profit < worstProfit){
                                    worstProfit = profit;
                                    worstPos = i;
                                }
                            }
                        }
                        string name = StaticData.placedBusinesses2[worstPos].name;
                        name = name.Substring(0, name.Length-7);
                        if(possibleBusinesses[cursor2Pos].name != name){
                            Destroy(StaticData.placedBusinesses2[worstPos]);
                            StaticData.placedBusinesses2[worstPos] = null;
                            StaticData.numEach2[Array.IndexOf(possibleBusinessNames, name)] -= 1;
                            StaticData.demand2[name] -= 10;
                            StaticData.player2Money += (int)(StaticData.buildCosts[name] * 0.1);
                            sfxSource.PlayOneShot(deleteSound, 1f);
                        }
                    }

                    //build business
                    int spawnNum = -1;
                    for(int i = 0; i < StaticData.placedBusinesses2.Length; i++){
                        if(StaticData.placedBusinesses2[i] == null){
                            spawnNum = i;
                            break;
                        }
                    }
                    if(spawnNum != -1){
                        Vector3 spawnPos = spawnPts2[spawnNum].transform.position;
                        spawnPos += new Vector3(0, 0, spawnNum * -1f);
                        GameObject toInstantiate = possibleBusinesses[cursor2Pos];
                        string name = toInstantiate.name;
                        int buildCost = StaticData.buildCosts[name];
                        if(StaticData.player2Money >= buildCost){
                            GameObject thisBusiness = (GameObject)Instantiate(toInstantiate,spawnPos, Quaternion.identity);
                            StaticData.placedBusinesses2[spawnNum] = thisBusiness;
                            StaticData.player2Money -= buildCost;
                            StaticData.numEach2[cursor2Pos] += 1;
                            sfxSource.PlayOneShot(placeSound, 1f);
                        } 
                    }

                } else if (aiNextMoveName == "invest" && StaticData.player2Money >= aiCost){
                    string name = possibleBusinesses[cursor2Pos].name;
                    int buildCost = StaticData.buildCosts[name];
                    StaticData.demand2[name] += 10;
                    StaticData.player2Money -= (int)(0.1*buildCost);
                    sfxSource.PlayOneShot(investSound, 0.5f);
                }
                aiNextMoveName = null;
                aiCost = 0;
            }

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

}
