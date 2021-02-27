using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class FloorTile : MonoBehaviour
{
    private MeshRenderer MeshRenderer;
    private BoxCollider BoxCollider;

    private void Awake()
    {
        this.MeshRenderer = this.GetComponent<MeshRenderer>();
        this.BoxCollider = this.GetComponent<BoxCollider>();
    }

    public void SetAsHole(bool value)
    {
        this.MeshRenderer.enabled = !value;
        this.BoxCollider.enabled = value;
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        if (player != null)
        {
            player.HasBeenHit("hole");
        }
    }
}
