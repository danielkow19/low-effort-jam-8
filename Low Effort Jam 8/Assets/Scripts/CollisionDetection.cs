using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    [SerializeField]
    BoxCollider2D playerBox;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        List<Collider2D> others = new List<Collider2D>();
        playerBox.OverlapCollider(new ContactFilter2D(), others);

        foreach (Collider2D other in others)
        {
            
        }
    }
}
