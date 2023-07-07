using UnityEngine;

namespace Fight
{
public class MoveScript : MonoBehaviour
{
    public Vector3 direction;
    public float speed;
    
    void Start()
    {
        if(direction == null)
            direction = transform.forward;
    }

    void FixedUpdate()
    {
        transform.position+=direction* speed * Time.fixedDeltaTime;
    }
}
}