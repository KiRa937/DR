using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    /// <summary>
    /// OnCollisionEnter is called when this collider/rigidbody has begun
    /// touching another rigidbody/collider.
    /// </summary>
    /// <param name="other">The Collision data associated with this collision.</param>
    void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Enemy"))
        {
            var f = Instantiate(GManager.Instance.explosion, other.collider.transform.position, GManager.Instance.explosion.transform.rotation);

            Destroy(f, 0.5f);
			
            GameObject.Destroy(other.collider.gameObject);
        }

        if (!other.collider.CompareTag("Player"))
            GameObject.Destroy(gameObject);
    }
}
