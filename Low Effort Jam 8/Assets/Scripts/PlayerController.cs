using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MatterState { Solid, Liquid, Gas };

public class PlayerController : MonoBehaviour
{
    public MatterState state;

    [SerializeField]
    private Rigidbody2D rigidbody;

    [SerializeField]
    private float gravity = 9.8f;

    [SerializeField]
    private float lrAccel;

    [SerializeField]
    private float lrMaxSpeed;

    public float temperature;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool left = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        bool right = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
        bool up = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow);

        switch (state)
        {
            case MatterState.Solid:
                SolidUpdate(left, right, up);
                break;
            case MatterState.Liquid:
                LiquidUpdate(left, right);
                break;
            case MatterState.Gas:
                GasUpdate(left, right);
                break;
        }

        Mathf.Clamp(rigidbody.velocity.x, -lrMaxSpeed, lrMaxSpeed);
    }

    void SolidUpdate(bool left, bool right, bool up)
    {

    }

    void LiquidUpdate(bool left, bool right)
    {

    }

    void GasUpdate(bool left, bool right)
    {

    }
}
