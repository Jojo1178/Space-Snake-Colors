using UnityEngine;

public class Tail : MonoBehaviour
{
    public Player Player;
    public Transform Target;

    public float MinDistance = 0.1f;

    private void FixedUpdate()
    {
        if(this.Target != null)
        {

            this.transform.LookAt(this.Target.position);
            if (Vector3.Distance(this.transform.position, this.Target.position) > this.MinDistance)
            {
                this.transform.position = Vector3.Lerp(this.transform.position, this.Target.position, this.Player.ForwardSpeed * Time.deltaTime);
            }
        }
    }
}
