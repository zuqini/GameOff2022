using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeaBagController : MonoBehaviour
{
    private Rigidbody2D body;
    private Collider2D col;

    public Rigidbody2D holder;
    public Draggable draggable;

    public Rigidbody2D Body { get => body; }
    public Collider2D Col { get => col; }

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }
}
