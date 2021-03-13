using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MatterState { Solid, Liquid, Gas };

public class PlayerController : MonoBehaviour
{
    public MatterState state;

    // Movement fields
    [SerializeField]
    private Rigidbody2D rigidbody;

    private float xVel;
    private float yVel;

    [SerializeField]
    private float gravity = 9.8f;

    [SerializeField]
    private float downMaxSpeed;

    [SerializeField]
    private float upAccel;

    [SerializeField]
    private float upMaxSpeed;

    [SerializeField]
    private float lrAccel;

    [SerializeField]
    private float lrMaxSpeed;

    [SerializeField]
    private float lrDeceleration;

    [SerializeField]
    private float lrIceDeceleration;

    // Temperature fields
    public float temperature;

    [SerializeField]
    private float heatLossRate;

    // Start is called before the first frame update
    void Start()
    {
        xVel = rigidbody.velocity.x;
        yVel = rigidbody.velocity.y;
    }

    // Update is called once per frame
    void Update()
    {
        // Movement
        bool left = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        bool right = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);

        xVel = rigidbody.velocity.x;
        yVel = rigidbody.velocity.y;

        if (left)
        {
            xVel -= lrAccel;
        }
        if (right)
        {
            yVel += lrAccel;
        }

        switch (state)
        {
            case MatterState.Solid:
                SolidUpdate(left, right);
                break;
            case MatterState.Liquid:
                LiquidUpdate(left, right);
                break;
            case MatterState.Gas:
                GasUpdate(left, right);
                break;
        }

        xVel = Mathf.Clamp(xVel, -lrMaxSpeed, lrMaxSpeed);
        yVel = Mathf.Clamp(yVel, -downMaxSpeed, upMaxSpeed);

        rigidbody.velocity = new Vector2(xVel, yVel);

        // Temperature
        temperature -= heatLossRate * Time.deltaTime;
    }

    void SolidUpdate(bool left, bool right)
    {
        if (!(left || right))
        {
            xVel /= lrIceDeceleration;
        }

        yVel -= gravity;
    }

    void LiquidUpdate(bool left, bool right)
    {
        if (!(left || right))
        {
            xVel /= lrDeceleration;
        }

        yVel -= gravity;
    }

    void GasUpdate(bool left, bool right)
    {
        if (!(left || right))
        {
            xVel /= lrDeceleration;
        }

        yVel += upAccel;
    }
}
