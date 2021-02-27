using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Rigidbody))]
[RequireComponent(typeof(MeshRenderer))]
public class Player : MonoBehaviour
{
    public float ForwardSpeed = 4;
    public float LateralSpeed = 3;

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
        // Debug.Log(causeOfDeath);
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
}
