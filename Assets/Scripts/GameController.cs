using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LiquidType
{
    Milk,
    OatMilk,
    Honey,
}

public struct Order
{
    public int blackTea;
    public int herbTea;
    public int lightTea;
    public bool hasHoney;
    public bool hasMilk;
    public bool hasOatMilk;
    public int sugarCount;
    public bool hasWater;
    public bool hasSpoon;

    public static Order RandomBasic()
    {
        var difficulty = GameController.SharedInstance.DifficultyLevel;
        var teaType = Utils.Random.Next(3);
        var teaCount = Utils.Random.Next(1, 4);
        var milkType = Utils.Random.Next(3);

        var blackTea = teaType == 0 ? teaCount : 0;
        var herbTea = teaType == 1 ? teaCount : 0;
        var lightTea = teaType == 2 ? teaCount : 0;
        var hasHoney = Utils.Random.Next(2) == 0;
        var hasMilk = milkType == 1;
        var hasOatMilk = milkType == 2;
        var sugarCount = Utils.Random.Next(5);
        var hasSpoon = Utils.Random.Next(2) == 0;

        if (difficulty == 1) {
            return new Order {
                blackTea = teaType == 0 ? 1 : 0,
                herbTea = teaType == 1 ? 1 : 0,
                lightTea = teaType == 2 ? 1 : 0,
                hasHoney = false,
                hasMilk = false,
                hasOatMilk = false,
                sugarCount = 0,
                hasSpoon = false,
                hasWater = true,
            };
        } else if (difficulty == 2) {
            return new Order {
                blackTea = teaType == 0 ? 1 : 0,
                herbTea = teaType == 0 ? 1 : 0,
                lightTea = teaType == 0 ? 1 : 0,
                hasHoney = hasHoney,
                hasMilk = false,
                hasOatMilk = false,
                sugarCount = 0,
                hasSpoon = false,
                hasWater = true,
            };
        } else if (difficulty == 3) {
            return new Order {
                blackTea = teaType == 0 ? 1 : 0,
                herbTea = teaType == 0 ? 1 : 0,
                lightTea = teaType == 0 ? 1 : 0,
                hasHoney = false,
                hasMilk = hasMilk,
                hasOatMilk = hasOatMilk,
                sugarCount = 0,
                hasSpoon = false,
                hasWater = true,
            };
        } else if (difficulty == 4) {
            return new Order {
                blackTea = blackTea,
                herbTea = herbTea,
                lightTea = lightTea,
                hasHoney = hasHoney,
                hasMilk = hasMilk,
                hasOatMilk = hasOatMilk,
                sugarCount = 0,
                hasSpoon = false,
                hasWater = true,
            };
        } else if (difficulty == 5) {
            return new Order {
                blackTea = blackTea,
                herbTea = herbTea,
                lightTea = lightTea,
                hasHoney = hasHoney,
                hasMilk = hasMilk,
                hasOatMilk = hasOatMilk,
                sugarCount = sugarCount,
                hasSpoon = false,
                hasWater = true,
            };
        } else {
            return new Order {
                blackTea = blackTea,
                herbTea = herbTea,
                lightTea = lightTea,
                hasHoney = hasHoney,
                hasMilk = hasMilk,
                hasOatMilk = hasOatMilk,
                sugarCount = sugarCount,
                hasSpoon = hasSpoon,
                hasWater = true,
            };
        }
    }
}

public class GameController : MonoBehaviour
{
    private int difficultyLevel = 1;
    private int level = 0;

    public CustomerController currentCustomer;

    [SerializeField]
    public LayerMask cameraEventMask;

    public static GameController SharedInstance;
    public LevelLoader levelLoader;
    public Camera cam;
    public DialogueManager dialogueManager;
    public LevelUIController levelUIController;
    public float despawnTimeInSecAfterDisdard = 1.5f;

    public CustomerController CurrentCustomer { get => currentCustomer; }
    public int DifficultyLevel { get => difficultyLevel; }
    public int Level { get => level; }

    void Start()
    {
        cam.eventMask = cameraEventMask;
        AdvanceLevel();
    }

    void Awake()
    {
        if (SharedInstance != null && SharedInstance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        SharedInstance = this;
    }

    void Update()
    {
        // if (Input.GetButtonDown("Fire1"))
        // {
        //     Vector3 mousePos = Input.mousePosition;
        //     {
        //         var position = Utils.GetWorldPositionOnPlane(mousePos, 0);
        //         Debug.Log(position.x + ", " + position.y);
        //     }
        // }
    }

    public void AdvanceLevel()
    {
        level++;
        difficultyLevel = 1 + level / 3;
        Debug.Log(string.Format("level: {0}, difficulty: {1}", level, difficultyLevel));
        levelUIController.GenerateCurrentLevel();
    }
}
