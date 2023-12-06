using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BallControler : NetworkBehaviour
{

    public float speed;
    public Rigidbody2D rb;
    public Vector3 lastVelocity;
    Vector3 direction;
    public override void OnStartServer()
    {
        
        base.OnStartServer();
        int random = Random.Range(-1, 2);
        rb.AddForce(Vector2.up * -1 * 500);
     
       
    }
    private void Update()
    {
        lastVelocity = rb.velocity;
    }
    //float HitFactor(Vector2 ballPos, Vector2 racketPos, float racketHeight)
    //{
    //    // ascii art:
    //    // ||  1 <- at the top of the racket
    //    // ||
    //    // ||  0 <- at the middle of the racket
    //    // ||
    //    // || -1 <- at the bottom of the racket
    //    return (ballPos.y - racketPos.y) / racketHeight;
    //}

    // only call this on server
    [ServerCallback]
    void OnCollisionEnter2D(Collision2D col)
    {

        
        if (col.gameObject.tag =="Player")
        {

            direction = Vector2.Reflect(lastVelocity.normalized, col.contacts[0].normal);
            speed += lastVelocity.magnitude + (lastVelocity.magnitude/3);

            if(speed > 20)
            {
                speed = 20;
                rb.velocity = direction * speed;
            }else
                rb.velocity = direction * speed;

            // Set Velocity with dir * speed

        }
        if (col.gameObject.tag == "Wall")
        {


            direction = Vector2.Reflect(lastVelocity.normalized, col.contacts[0].normal);
            speed = lastVelocity.magnitude;
            speed -= .3f;
            rb.velocity = direction * speed;


        }
      
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IScore score = collision.gameObject.GetComponent<IScore>();

        if(score != null)
        {
            score.Score();
        }
    }
}
