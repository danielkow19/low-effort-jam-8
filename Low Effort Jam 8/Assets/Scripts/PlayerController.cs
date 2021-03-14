using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MatterState { Solid, Liquid, Gas };

public class PlayerController : MonoBehaviour
{
    public MatterState state;

    public Animator animator;

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

    // Input
    private bool left;
    private bool right;
    private bool jump;

    [SerializeField]
    private GameObject thermo;

    // Start is called before the first frame update
    void Start()
    {
        xVel = rigidbody.velocity.x;
        yVel = rigidbody.velocity.y;
    }

    // Update is called once per frame
    void Update()
    {
        left = Input.GetKey(KeyCode.A)
            || Input.GetKey(KeyCode.LeftArrow);

        right = Input.GetKey(KeyCode.D)
            || Input.GetKey(KeyCode.RightArrow);

        jump = Input.GetKeyDown(KeyCode.UpArrow)
            || Input.GetKeyDown(KeyCode.Space)
            || Input.GetKeyDown(KeyCode.W);

        if (state == MatterState.Solid)
        {
            // Checks the bottom left, bottom middle, and bottom right of the player's hitbox
            BoxCollider2D box = gameObject.GetComponent<BoxCollider2D>();
            Vector2 leftPoint = new Vector2(box.bounds.center.x - box.bounds.extents.x, box.bounds.center.y - box.bounds.extents.y);
            Vector2 midPoint = new Vector2(box.bounds.center.x, box.bounds.center.y - box.bounds.extents.y);
            Vector2 rightPoint = new Vector2(box.bounds.center.x + box.bounds.extents.x, box.bounds.center.y - box.bounds.extents.y);

            LayerMask mask = ~LayerMask.GetMask(new string[] { "Solid", "Liquid", "Gas" });

            // If any of the three raycasts hit and any of the three jump buttons are pressed, jump
            if ((Physics2D.Raycast(leftPoint, Vector2.down, 0.1f, mask)
                || Physics2D.Raycast(midPoint, Vector2.down, 0.1f, mask)
                || Physics2D.Raycast(rightPoint, Vector2.down, 0.1f, mask))
                && jump)
            {
                rigidbody.AddForce(Vector2.up * jumpSpeed);
            }
        }
    }

    void FixedUpdate()
    {
        if (left)
        {
            rigidbody.AddForce(new Vector2(-lrAccel, 0));
        }
        if (right)
        {
            rigidbody.AddForce(new Vector2(lrAccel, 0));
        }
        if (!(left || right))
        {
            rigidbody.AddForce(new Vector2(-xVel * lrDeceleration, 0));
        }

        switch (state)
        {
            case MatterState.Solid:
                SolidUpdate();
                break;
            case MatterState.Liquid:
                LiquidUpdate();
                break;
            case MatterState.Gas:
                GasUpdate();
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

        animator.SetBool("IsFalling", yVel < 0);

        rigidbody.velocity = new Vector2(xVel, yVel);

        // Temperature
        temperature -= heatLossRate * Time.deltaTime;

        temperature = Mathf.Clamp(temperature, minTemp, maxTemp);

        animator.SetFloat("Temperature", temperature);

        thermo.GetComponent<Thermometer>().SetTemperature(temperature);

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

    void SolidUpdate()
    {
        rigidbody.AddForce(new Vector2(0, -gravity / rigidbody.mass));
    }

    void LiquidUpdate()
    {
        rigidbody.AddForce(new Vector2(0, -gravity / rigidbody.mass));
    }

    void GasUpdate()
    {
        rigidbody.AddForce(new Vector2(0, upAccel / rigidbody.mass));
    }
}
