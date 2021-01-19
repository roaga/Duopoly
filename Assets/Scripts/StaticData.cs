using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public static class StaticData
{
    public static bool twoPlayers = false;
    public static bool paused = false;
    public static bool gameOver = false;
    public static int player1Money = 100;
    public static int player2Money = 100;
    public static int player1Traffic = 50; //traffic adds up to 100
    public static int player2Traffic = 50;
    public static float roundTime = 10; //seconds

    public static GameObject[] placedBusinesses1 = new GameObject[7];
    public static GameObject[] placedBusinesses2 = new GameObject[7];
    public static int[] numEach1 = new int[]{0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
    public static int[] numEach2 = new int[]{0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
    public static int[] timeEach1 = new int[]{0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
    public static int[] timeEach2 = new int[]{0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
    public static string[] possibleBusinessNames = new string[]{"LemonadeStand","GasStation","FastFoodRestaurant","Park","Farm","Hotel","Bank","Hospital","Theater","Stadium"};

    // values for each business
    public static Dictionary<string, int> buildCosts = new Dictionary<string, int>(){
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
    public static Dictionary<string, int> operatingCosts = new Dictionary<string, int>(){
        {"LemonadeStand", 200},
        {"GasStation", 5000},
        {"FastFoodRestaurant", 10000},
        {"Park", 100000},
        {"Farm", 200000},
        {"Hotel", 1000000},
        {"Bank", 500000},
        {"Hospital", 1000000},
        {"Theater", 50000},
        {"Stadium", 10000000}
    };
    public static Dictionary<string, int> growthSpeed = new Dictionary<string, int>(){
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
    public static Dictionary<string, int> demand1 = new Dictionary<string, int>(){
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
    public static Dictionary<string, int> demand2 = new Dictionary<string, int>(){
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
    public static Dictionary<string, int> baseRevenue1 = new Dictionary<string, int>(){
        {"LemonadeStand", 100},
        {"GasStation", 1000},
        {"FastFoodRestaurant", 2000},
        {"Park", 5000},
        {"Farm", 10000},
        {"Hotel", 100000},
        {"Bank", 50000},
        {"Hospital", 200000},
        {"Theater", 20000},
        {"Stadium", 500000}
    };
    public static Dictionary<string, int> baseRevenue2 = new Dictionary<string, int>(){
        {"LemonadeStand", 100},
        {"GasStation", 1000},
        {"FastFoodRestaurant", 2000},
        {"Park", 5000},
        {"Farm", 10000},
        {"Hotel", 100000},
        {"Bank", 50000},
        {"Hospital", 200000},
        {"Theater", 20000},
        {"Stadium", 500000}
    };


}