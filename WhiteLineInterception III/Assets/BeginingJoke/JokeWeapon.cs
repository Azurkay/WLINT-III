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
        Debug.Log("zzeoijzejgzejgzoeij");
        Destroy(this.gameObject, 0.5f);
    }

    private void Start()
    {
        Destroy(this.gameObject, 15f);
    }
}
