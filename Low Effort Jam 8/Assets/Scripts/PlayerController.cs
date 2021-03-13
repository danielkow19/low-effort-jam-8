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

    // Temperature fields
    public float temperature;

    [SerializeField]
    private float minTemp;

    [SerializeField]
    private float maxTemp;

    [SerializeField]
    private float freezingTemp;

    [SerializeField]
    private float boilingTemp;

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

        if (left)
        {
            rigidbody.AddForce(new Vector2(-lrAccel, 0));
        }
        if (right)
        {
            rigidbody.AddForce(new Vector2(lrAccel, 0));
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

        xVel = rigidbody.velocity.x;
        yVel = rigidbody.velocity.y;

        xVel = Mathf.Clamp(xVel, -lrMaxSpeed, lrMaxSpeed);
        yVel = Mathf.Clamp(yVel, -downMaxSpeed, upMaxSpeed);

        rigidbody.velocity = new Vector2(xVel, yVel);

        // Temperature
        temperature -= heatLossRate * Time.deltaTime;

        temperature = Mathf.Clamp(temperature, minTemp, maxTemp);

        if (temperature < freezingTemp)
        {
            state = MatterState.Solid;
            gameObject.layer = 8;
        }
        else if (temperature < boilingTemp)
        {
            state = MatterState.Liquid;
            gameObject.layer = 9;
        }
        else
        {
            state = MatterState.Gas;
            gameObject.layer = 10;
        }
    }

    void SolidUpdate(bool left, bool right)
    {
        if (!(left || right))
        {
            rigidbody.AddForce(new Vector2(-xVel * lrDeceleration, 0));
        }

        rigidbody.AddForce(new Vector2(0, -gravity / rigidbody.mass));
    }

    void LiquidUpdate(bool left, bool right)
    {
        if (!(left || right))
        {
            rigidbody.AddForce(new Vector2(-xVel * lrDeceleration, 0));
        }

        rigidbody.AddForce(new Vector2(0, -gravity / rigidbody.mass));
    }

    void GasUpdate(bool left, bool right)
    {
        if (!(left || right))
        {
            rigidbody.AddForce(new Vector2(-xVel * lrDeceleration, 0));
        }

        rigidbody.AddForce(new Vector2(0, upAccel / rigidbody.mass));
    }
}
