using System.Collections;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public MeshRenderer MeshRenderer;
    public float downTimeSec = 0.25f;
    public float upTimeSec = 1.0f;

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

    public IEnumerator GoDown()
    {
        Vector3 currentPos = transform.position;
        Vector3 target = currentPos + Vector3.down * this.transform.localScale.y;
        yield return this.Move(currentPos, target, this.downTimeSec);
        transform.position = target;
    }

    public IEnumerator GoUp()
    {
        Vector3 currentPos = transform.position;
        Vector3 target = currentPos + Vector3.up * this.transform.localScale.y;
        yield return this.Move(currentPos, target, this.upTimeSec);
        transform.position = target;
    }

    private IEnumerator Move(Vector3 start, Vector3 end, float timeSec)
    {
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeSec;
            transform.position = Vector3.Lerp(start, end, t);
            yield return null;
        }
    }
}