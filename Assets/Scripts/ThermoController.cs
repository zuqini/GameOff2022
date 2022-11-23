using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThermoController : MonoBehaviour
{
    private Transform top;
    private Transform bot;

    public KettleController kettle;
    public Transform mercury;
    public Transform markers;

    void Start()
    {
        top = markers.Find("Top");
        bot = markers.Find("Bot");
    }

    void FixedUpdate()
    {
        mercury.position = new Vector3(mercury.position.x,
                bot.position.y + (kettle.WaterTemperature / 100f) * (top.position.y - bot.position.y),
                mercury.position.z);
    }
}
