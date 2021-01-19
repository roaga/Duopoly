using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller1 : MonoBehaviour
{
    public GameObject cursor1;
    public int cursor1Pos = 0;
    public GameObject[] buttonList;
    public GameObject[] spawnPts1;
    public GameObject[] possibleBusinesses;

    public AudioClip investSound;
    public AudioClip placeSound;
    public AudioClip deleteSound;
    public AudioClip invalidSound;
    public AudioSource sfxSource;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update(){
        if(StaticData.paused == false){
        if(Input.GetKeyDown(KeyCode.A)){
            //move left
            if(cursor1Pos > 0){ 
                cursor1Pos--;
                cursor1.transform.position = buttonList[cursor1Pos].transform.position + new Vector3(0, -0.5f, 0);
            }
        }
        if(Input.GetKeyDown(KeyCode.D)){
            //move right
            if(cursor1Pos < 9){ 
                cursor1Pos++;
                cursor1.transform.position = buttonList[cursor1Pos].transform.position + new Vector3(0, -0.5f, 0);
            }
        }
        if(Input.GetKeyDown(KeyCode.Q)){
            //build
            int spawnNum = -1;
            for(int i = 0; i < StaticData.placedBusinesses1.Length; i++){
                if(StaticData.placedBusinesses1[i] == null){
                    spawnNum = i;
                    break;
                }
            }
            if(spawnNum != -1){
                Vector3 spawnPos = spawnPts1[spawnNum].transform.position;
                spawnPos += new Vector3(0, 0, spawnNum * -1f);
                GameObject toInstantiate = possibleBusinesses[cursor1Pos];
                string name = toInstantiate.name;
                int buildCost = StaticData.buildCosts[name];
                if(StaticData.player1Money >= buildCost){
                    GameObject thisBusiness = (GameObject)Instantiate(toInstantiate,spawnPos, Quaternion.identity);
                    StaticData.placedBusinesses1[spawnNum] = thisBusiness;
                    StaticData.player1Money -= buildCost;
                    StaticData.numEach1[cursor1Pos] += 1;
                    sfxSource.PlayOneShot(placeSound, 1f);
                }else{
                    sfxSource.PlayOneShot(invalidSound, 1f);
                }
            }else{
                sfxSource.PlayOneShot(invalidSound, 1f);
            }
        }
        if(Input.GetKeyDown(KeyCode.W)){
            //invest
            string name = possibleBusinesses[cursor1Pos].name;
            int buildCost = StaticData.buildCosts[name];
            if(StaticData.player1Money >= 0.75*buildCost){
                StaticData.demand1[name] += 10;
                StaticData.player1Money -= (int)(0.1*buildCost);
                sfxSource.PlayOneShot(investSound, 0.5f);
            }else{
                sfxSource.PlayOneShot(invalidSound, 1f);
            }
        }
        if(Input.GetKeyDown(KeyCode.E)){
            //sell
            Sprite spriteToCompare = possibleBusinesses[cursor1Pos].GetComponent<SpriteRenderer>().sprite;
            for(int i = 0; i < StaticData.placedBusinesses1.Length; i++){
                if(StaticData.placedBusinesses1[i] != null && spriteToCompare == StaticData.placedBusinesses1[i].GetComponent<SpriteRenderer>().sprite){
                    Destroy(StaticData.placedBusinesses1[i]);
                    StaticData.placedBusinesses1[i] = null;
                    StaticData.numEach1[cursor1Pos] -= 1;
                    string name = possibleBusinesses[cursor1Pos].name;
                    StaticData.demand1[name] -= 10;
                    StaticData.player1Money += (int)(StaticData.buildCosts[name] * 0.1);
                    sfxSource.PlayOneShot(deleteSound, 1f);
                    break;
                }
            }
        }
    }
    }

}
