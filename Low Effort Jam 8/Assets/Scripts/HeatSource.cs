using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatSource : MonoBehaviour
{
    // Total heat possible to transmit
    public float heatAmt = 200;
    
    // Heat transmitted every second
    public int rate = 15;

    public CircleCollider2D collider;

    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.Equals(player))
        {
            heatAmt -= Time.deltaTime * rate;
            player.GetComponent<PlayerController>().temperature += Time.deltaTime * rate;
        }
        
    }


}
