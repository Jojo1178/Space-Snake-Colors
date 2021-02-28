using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Player Player;

    public Vector3 offset;
    public float offetUpdateSec = 1;


    public void TrackPlayer(Player player, Vector3 startPosition = default)
    {
        this.Player = player;
        this.transform.position = startPosition;
        this.offset = this.transform.position - this.Player.transform.position;
    }

    void Update()
    {
        if (this.Player != null)
        {
            this.transform.position = this.Player.transform.position + offset;
        }
    }
    public void UpdateOffset(Vector3 delta)
    {
        StartCoroutine(this.UpdateOffsetCoroutine(this.offset + delta));
    }

    private IEnumerator UpdateOffsetCoroutine(Vector3 newOffset)
    {
        Vector3 start = this.offset;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / this.offetUpdateSec;
            this.offset = Vector3.Lerp(start, newOffset, t);
            yield return null;
        }
    }
}
