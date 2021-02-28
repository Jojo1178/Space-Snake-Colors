using System.Collections;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public MeshRenderer MeshRenderer;
    public float downTimeSec = 0.25f;

    public void AssignMaterial(Material material)
    {
        this.MeshRenderer.material = material;
    }

    public Color GetColor()
    {
        return this.MeshRenderer.material.GetColor(GameController.CELLCOLORKEY);
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        if (player != null)
        {
            if (player.GetColor() != this.GetColor())
            {
                player.HasBeenHit("obstacle");
            }
            else
            {
                AudioController.INSTANCE.PlayObstacleDeathSound();
                StartCoroutine(this.GoDown());
            }
        }
    }

    private IEnumerator GoDown()
    {
        Vector3 currentPos = transform.position;
        Vector3 target = currentPos + Vector3.down * this.transform.localScale.y;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / this.downTimeSec;
            transform.position = Vector3.Lerp(currentPos, target, t);
            yield return null;
        }
    }
}