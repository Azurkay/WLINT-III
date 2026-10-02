using UnityEngine;

public class JokeWeapon : MonoBehaviour
{
    void Update()
    {
        transform.position += Vector3.right * 5;
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
        Destroy(this.gameObject, 0.1f);
    }

    private void Start()
    {
        Destroy(this.gameObject, 15f);
    }
}
