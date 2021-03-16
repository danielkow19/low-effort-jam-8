using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockDoors : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> doors;

    [SerializeField]
    private bool triggered = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!triggered)
        {
            triggered = true;

            foreach (GameObject door in doors)
            {
                door.GetComponent<Collider2D>().enabled = false;
                door.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }
}
