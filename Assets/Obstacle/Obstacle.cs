using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public MeshRenderer MeshRenderer;
    private Color Color;

    public void AssignMaterial(Material material)
    {
        this.MeshRenderer.material = material;
        this.Color = this.GetColor();
    }

    public Color GetColor()
    {
        return this.MeshRenderer.material.color;
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
                this.MeshRenderer.material.color = Color.yellow;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Player>() != null)
        {
            this.MeshRenderer.material.color = this.Color;
        }
    }
}
