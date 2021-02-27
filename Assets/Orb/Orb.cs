using UnityEngine;

public class Orb : MonoBehaviour
{
    public int ScoreValue = 1;
    public MeshRenderer MeshRenderer;
    private Color Color;

    public void SetColor(Color color)
    {
        this.MeshRenderer.material.color = color;
    }

    public Color GetColor()
    {
        return this.MeshRenderer.material.color;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("test");
        Player player = other.gameObject.GetComponent<Player>();
        if (player != null)
        {
            GameController.INSTANCE.AddScore(this.ScoreValue);
            this.gameObject.SetActive(false);
        }
    }
}
