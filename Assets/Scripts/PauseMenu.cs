using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public TextMeshProUGUI pauseButtonText;
    public GameObject pauseButton;
    public TextMeshProUGUI endText;
    public GameObject endTextObject;
    public GameObject quitButton;
    public GameObject filter;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(StaticData.roundTime >= 0 && StaticData.roundTime < 0.1 && !StaticData.gameOver){
            StaticData.paused = true;
            StaticData.gameOver = true;
            pauseButton.SetActive(false);
            Time.timeScale = 0;
            quitButton.SetActive(true);
            filter.SetActive(true);
            if(StaticData.player1Money > StaticData.player2Money){
                endText.text = "PLAYER 1 WINS!\n<";
            } else if(StaticData.player1Money < StaticData.player2Money){
                endText.text = "PLAYER 2 WINS!\n>";
            } else{
                endText.text = "INCOME EQUALITY ACHIEVED";
            }
            endTextObject.SetActive(true);
        }
    }

    public void Pause(){
        Time.timeScale = 0;
        pauseButtonText.text = ">";
        quitButton.SetActive(true);
        filter.SetActive(true);
    }

    public void Resume(){
        Time.timeScale = 1;
        pauseButtonText.text = "||";
        quitButton.SetActive(false);
        filter.SetActive(false);
    }

    public void onClickPauseButton(){
        if(StaticData.paused){
            StaticData.paused = false;
            Resume();
        } else{
            StaticData.paused = true;
            Pause();
        }
    }

    public void onClickQuitButton(){
        Time.timeScale = 1;
        StaticData.gameOver = false;
        StaticData.paused = false;
        ResetStaticData();
        SceneManager.LoadScene("Menu");
    }

    public void ResetStaticData(){
        StaticData.twoPlayers = false;
        StaticData.paused = false;
        StaticData.player1Money = 100;
        StaticData.player2Money = 100;
        StaticData.player1Traffic = 50; //traffic adds up to 100
        StaticData.player2Traffic = 50;
        StaticData.roundTime = 180; //seconds

        StaticData.placedBusinesses1 = new GameObject[7];
        StaticData.placedBusinesses2 = new GameObject[7];
        StaticData.numEach1 = new int[]{0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
        StaticData.numEach2 = new int[]{0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
        StaticData.timeEach1 = new int[]{0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
        StaticData.timeEach2 = new int[]{0, 0, 0, 0, 0, 0, 0, 0, 0, 0};

        // values for each business
        StaticData.buildCosts = new Dictionary<string, int>(){
            {"LemonadeStand", 100},
            {"GasStation", 10000},
            {"FastFoodRestaurant", 200000},
            {"Park", 1000000},
            {"Farm", 2000000},
            {"Hotel", 5000000},
            {"Bank", 1500000},
            {"Hospital", 10000000},
            {"Theater", 1500000},
            {"Stadium", 100000000}
        };
        StaticData.operatingCosts = new Dictionary<string, int>(){
            {"LemonadeStand", 200},
            {"GasStation", 5000},
            {"FastFoodRestaurant", 10000},
            {"Park", 100000},
            {"Farm", 200000},
            {"Hotel", 1000000},
            {"Bank", 1000000},
            {"Hospital", 1000000},
            {"Theater", 50000},
            {"Stadium", 10000000}
        };
        StaticData.growthSpeed = new Dictionary<string, int>(){
            {"LemonadeStand", 50},
            {"GasStation", 25},
            {"FastFoodRestaurant", 20},
            {"Park", 20},
            {"Farm", 10},
            {"Hotel", 10},
            {"Bank", 1},
            {"Hospital", 5},
            {"Theater", 20},
            {"Stadium", 5}
        };
        StaticData.demand1 = new Dictionary<string, int>(){
            {"LemonadeStand", 100},
            {"GasStation", 100},
            {"FastFoodRestaurant", 100},
            {"Park", 100},
            {"Farm", 100},
            {"Hotel", 100},
            {"Bank", 100},
            {"Hospital", 100},
            {"Theater", 100},
            {"Stadium", 100}
        };
        StaticData.demand2 = new Dictionary<string, int>(){
            {"LemonadeStand", 100},
            {"GasStation", 100},
            {"FastFoodRestaurant", 100},
            {"Park", 100},
            {"Farm", 100},
            {"Hotel", 100},
            {"Bank", 100},
            {"Hospital", 100},
            {"Theater", 100},
            {"Stadium", 100}
        };
        StaticData.baseRevenue1 = new Dictionary<string, int>(){
            {"LemonadeStand", 100},
            {"GasStation", 1000},
            {"FastFoodRestaurant", 2000},
            {"Park", 5000},
            {"Farm", 10000},
            {"Hotel", 100000},
            {"Bank", 50000},
            {"Hospital", 200000},
            {"Theater", 30000},
            {"Stadium", 500000}
        };
        StaticData.baseRevenue2 = new Dictionary<string, int>(){
            {"LemonadeStand", 100},
            {"GasStation", 1000},
            {"FastFoodRestaurant", 2000},
            {"Park", 5000},
            {"Farm", 10000},
            {"Hotel", 100000},
            {"Bank", 50000},
            {"Hospital", 200000},
            {"Theater", 30000},
            {"Stadium", 500000}
        };
    }
}
