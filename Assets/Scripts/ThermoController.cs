using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThermoController : MonoBehaviour
{
    private Transform top;
    private Transform bot;
    private float thermoHeight;
    private float thermoBottom;

    public KettleController kettle;
    public Transform mercury;
    public Transform heatMarker;
    public Transform markers;

    void Start()
    {
        top = markers.Find("Top");
        bot = markers.Find("Bot");
        thermoBottom = bot.position.y;
        thermoHeight = top.position.y - bot.position.y;
        heatMarker.position = new Vector3(heatMarker.position.x,
            thermoBottom + (kettle.unlatchTemperature / 100) * thermoHeight,
            heatMarker.position.z);
    }

    void FixedUpdate()
    {
        mercury.position = new Vector3(mercury.position.x,
                thermoBottom + (kettle.WaterTemperature / 100f) * thermoHeight,
                mercury.position.z);
    }
}
