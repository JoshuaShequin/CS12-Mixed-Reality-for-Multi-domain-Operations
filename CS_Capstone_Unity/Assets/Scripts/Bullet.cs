using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float speed = 400f;
    public float flyTime = 15;  // How long until we kill the projectile
    public int projectileDamage = 10;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(rb.transform.forward * speed);

    }

    // Update is called once per frame
    void Update()
    {

        // Don't fly forever please
        flyTime -= Time.deltaTime;
        if (flyTime <= 0f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Bullet collision with unit");
        // Layer 8 and 9 should hold the ally/enemy units.
        if (collision.gameObject.layer == 8)
        {
            collision.gameObject.GetComponent<AllyBehavior>().LoseHealth(projectileDamage);
        }
        else if (collision.gameObject.layer == 9)
        {
            collision.gameObject.GetComponent<EnemyBehavior>().LoseHealth(projectileDamage);
        }
        else if (collision.gameObject.tag != "Projectile")
        {
            Destroy(gameObject);
        }
    }

}
