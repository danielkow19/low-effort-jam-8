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
    private float jumpSpeed;

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

        if (state == MatterState.Gas)
        {
            yVel = Mathf.Clamp(yVel, -downMaxSpeed, upMaxSpeed);
        }
        else
        {
            yVel = Mathf.Max(-downMaxSpeed, yVel);
        }

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

        // Checks the bottom left, bottom middle, and bottom right of the player's hitbox
        BoxCollider2D box = gameObject.GetComponent<BoxCollider2D>();
        Vector2 leftPoint = new Vector2(box.bounds.center.x - box.bounds.extents.x, box.bounds.center.y - box.bounds.extents.y);
        Vector2 midPoint = new Vector2(box.bounds.center.x, box.bounds.center.y - box.bounds.extents.y);
        Vector2 rightPoint = new Vector2(box.bounds.center.x + box.bounds.extents.x, box.bounds.center.y - box.bounds.extents.y);

        // If any of the three raycasts hit and any of the three jump buttons are pressed, jump
        if ((Physics2D.Raycast(leftPoint, Vector2.down, 0.1f)
            || Physics2D.Raycast(midPoint, Vector2.down, 0.1f)
            || Physics2D.Raycast(rightPoint, Vector2.down, 0.1f))
            && (Input.GetKeyDown(KeyCode.UpArrow)
            || Input.GetKeyDown(KeyCode.Space)
            || Input.GetKeyDown(KeyCode.W)))
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpSpeed);
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
