using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Rigidbody))]
[RequireComponent(typeof(MeshRenderer))]
public class Player : MonoBehaviour
{
    public Tail TailPrefab;
    public float ForwardSpeed = 4;
    public float LateralSpeed = 3;
    public float TailGrowSpeedSec = 0.25f;
    public List<Tail> Tails = new List<Tail>();

    private Rigidbody PlayerRigidbody;
    private MeshRenderer MeshRenderer;

    private Vector3 currentMovement = Vector3.zero;
    private float tailGrowStep = 1;

#if UNITY_EDITOR
    private Vector3 previousMousePosition;
#endif

    private void Awake()
    {
        this.PlayerRigidbody = GetComponent<Rigidbody>();
        this.MeshRenderer = GetComponent<MeshRenderer>();
        this.tailGrowStep = 1.0f / GameController.INSTANCE.newTailStep;
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
            lastTail.transform.localScale = this.transform.localScale;
        }
        else
        {
            lastTail = this.transform;
        }
        Tail tail = GameObject.Instantiate(this.TailPrefab, lastTail.position /*- this.transform.forward*/, this.TailPrefab.transform.rotation);
        tail.Player = this;
        tail.Target = lastTail;
        tail.transform.localScale = Vector3.zero;
        this.Tails.Add(tail);
    }

    public void GrowTail()
    {
        Tail tail = this.Tails[this.Tails.Count - 1];
        StartCoroutine(this.GrowTailCoroutine(tail));
    }

    private IEnumerator GrowTailCoroutine(Tail tail)
    {
        Vector3 currentScale = tail.transform.localScale;
        Vector3 target = currentScale + this.transform.localScale * this.tailGrowStep;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / this.TailGrowSpeedSec;
            tail.transform.localScale = Vector3.Lerp(currentScale, target, t);
            yield return null;
        }
    }
}
