// Destroys the GameObject after a certain time.
using System.Collections;
using Mirror;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    public bool isNetworkBased = false;
    public float time = 1;

    void Start()
    {
        StartCoroutine(DestroyRoutine());
        //Destroy(gameObject, time);
    }

    private IEnumerator DestroyRoutine()
    {
        yield return new WaitForSeconds(time);

        if (isNetworkBased)
        {
            NetworkServer.Destroy(gameObject);
        }

        Destroy(gameObject);
    }
}
