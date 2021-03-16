using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatSource : MonoBehaviour
{
    // Total heat possible to transmit
    public float heatAmt = 200;
    public float maxHeat = 200;
    
    // Heat transmitted every second
    public int rate = 15;

    public CircleCollider2D collider;

    public GameObject player;

    public Animator animator;

    public int animationState = 0;

    void Update()
    {
        animator.SetInteger("State", animationState);
        gameObject.transform.GetChild(0).GetComponent<ParticleSystem>().emissionRate = heatAmt / maxHeat * 10;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.Equals(player) && heatAmt > 0)
        {
            player.GetComponent<PlayerController>().heatLossRate = 0;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.Equals(player) && heatAmt > 0)
        {
            heatAmt -= Time.deltaTime * rate;
            if(heatAmt < 0)
            {
                heatAmt = 0;
            }
            player.GetComponent<PlayerController>().temperature += Time.deltaTime * rate;
        }
        if(heatAmt <= 0 && player.GetComponent<PlayerController>().heatLossRate == 0)
        {
            player.GetComponent<PlayerController>().heatLossRate = 5;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.Equals(player))
        {
            player.GetComponent<PlayerController>().heatLossRate = 5;
        }
    }
}
