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
        var teaType = Utils.Random.Next(3);
        var teaCount = Utils.Random.Next(1, 4);
        var milkType = Utils.Random.Next(3);
        return new Order {
            blackTea = teaType == 0 ? teaCount : 0,
            herbTea = teaType == 1 ? teaCount : 0,
            lightTea = teaType == 2 ? teaCount : 0,
            hasHoney = Utils.Random.Next(2) == 0,
            hasMilk = milkType == 1,
            hasOatMilk = milkType == 2,
            sugarCount = Utils.Random.Next(5),
            hasSpoon = Utils.Random.Next(2) == 0,
            hasWater = true,
        };
    }
}

public class GameController : MonoBehaviour
{
    public CustomerController currentCustomer;

    [SerializeField]
    public LayerMask cameraEventMask;

    public static GameController SharedInstance;
    public LevelLoader levelLoader;
    public Camera cam;
    public DialogueManager dialogueManager;
    public DialogueTrigger dialogueTrigger;
    public float despawnTimeInSecAfterDisdard = 1.5f;

    public CustomerController CurrentCustomer { get => currentCustomer; }

    void Start()
    {
        cam.eventMask = cameraEventMask;
        // dialogueTrigger.TriggerDialogue(0);
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
}
