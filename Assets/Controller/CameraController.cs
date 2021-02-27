using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Player Player;

    public Vector3 offset;


    public void TrackPlayer(Player player, Vector3 startPosition = default)
    {
        this.Player = player;
        this.transform.position = startPosition;
        this.offset = this.transform.position - this.Player.transform.position;
    }

    void LateUpdate()
    {
        if (this.Player != null)
        {
            this.transform.position = this.Player.transform.position + offset;
        }
    }
}
