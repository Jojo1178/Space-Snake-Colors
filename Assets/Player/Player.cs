﻿using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Rigidbody))]
[RequireComponent(typeof(MeshRenderer))]
public class Player : MonoBehaviour
{
    public Tail TailPrefab;
    public float ForwardSpeed = 4;
    public float LateralSpeed = 3;
    public List<Tail> Tails = new List<Tail>();

    private Rigidbody PlayerRigidbody;
    private MeshRenderer MeshRenderer;

    private Vector3 currentMovement = Vector3.zero;


#if UNITY_EDITOR
    private Vector3 previousMousePosition;
#endif

    private void Awake()
    {
        this.PlayerRigidbody = GetComponent<Rigidbody>();
        this.MeshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Moved)
            {
                this.currentMovement.x = touch.deltaPosition.x * this.LateralSpeed;
            }
            else
            {
                this.currentMovement = Vector3.zero;
            }
        }
#if UNITY_EDITOR
        else if (Input.GetMouseButtonDown(0))
        {
            this.previousMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            this.currentMovement.x = (Input.mousePosition - this.previousMousePosition).x * LateralSpeed;
        }
#endif
        else
        {
            this.currentMovement = Vector3.zero;
        }
        this.currentMovement.z = this.ForwardSpeed;
        this.PlayerRigidbody.velocity = this.currentMovement;
    }

    public void HasBeenHit(string causeOfDeath)
    {
        //Debug.Log(causeOfDeath);
        foreach (Tail tail in this.Tails)
        {
            Destroy(tail.gameObject);
        }
        Destroy(this.gameObject);
        GameController.INSTANCE.StopGame();
    }

    public Color GetColor()
    {
        return this.MeshRenderer.material.color;
    }

    public void SetColor(Color color)
    {
        this.MeshRenderer.material.color = color;
    }

    public void AddTail()
    {
        Transform lastTail;
        if(this.Tails.Count > 0)
        {
            lastTail = this.Tails[this.Tails.Count - 1].transform;
        }
        else
        {
            lastTail = this.transform;
        }
        Tail tail = GameObject.Instantiate(this.TailPrefab, lastTail.position - this.transform.forward, this.TailPrefab.transform.rotation);
        tail.Player = this;
        tail.Target = lastTail;
        this.Tails.Add(tail);
    }
}
