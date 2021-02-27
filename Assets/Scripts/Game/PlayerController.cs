using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController mainPlayer;

    public int health = 100;
    public int maxHealth = 100;

    public GameObject damageParticle;

    public float moveSpeed = 80.0f;
    public float sensitivity = 5.0f;
    public float verticalSensitivity = 5.0f;
    public float jumpHeight = 100.0f;

    public GameObject test;

    private float rotY = 0f;

    private bool grounded = false;
    
    public Vector3 jump;

    private RaycastHit trace;

    private Transform eyes;

    private CharacterController controller = null;

    // Start is called before the first frame update
    void Start()
    {
        mainPlayer = this;

        controller = GetComponent<CharacterController>();

        eyes = Camera.main.transform;

        Cursor.lockState = CursorLockMode.Locked;
    }

    private float PlayerForward()
    {
        float a = 0;

        if (Input.GetKey(KeyCode.W)) a += 1; if (Input.GetKey(KeyCode.S)) a -= 1;

        return a;
    }

    private float PlayerRight()
    {
        float a = 0;

        if (Input.GetKey(KeyCode.D)) a += 1; if (Input.GetKey(KeyCode.A)) a -= 1;

        return a;
    }

    private void Move()
    {
        grounded = controller.isGrounded;
        if (grounded && Input.GetKeyDown(KeyCode.Space))
        {
            jump.y = jumpHeight;
        }

        // Mouse
        transform.Rotate(transform.up, Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime);

        rotY += -Input.GetAxis("Mouse Y") * verticalSensitivity * Time.fixedDeltaTime;
        rotY = Mathf.Clamp(rotY, -90f, 90f);

        eyes.rotation = Quaternion.Euler(rotY, eyes.eulerAngles.y, eyes.eulerAngles.z);

        // Movement
        if (PlayerForward() != 0)
            controller.Move(transform.forward * PlayerForward() * moveSpeed * Time.fixedDeltaTime);

        if (PlayerRight() != 0)
            controller.Move(transform.right * PlayerRight() * moveSpeed * Time.fixedDeltaTime);

        if (!grounded)
            jump += Physics.gravity * Time.deltaTime;

        controller.Move(jump * Time.deltaTime);
    }

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (trace.collider != null)
            {
                EnemyController e;
                if (trace.transform.root.TryGetComponent(out e))
                {
                    if (e.TakeDamage(10, trace.transform.name) 
                        || trace.transform.name == "LeftArm"
                        || trace.transform.name == "RightArm")
                    {
                        CharacterJoint c;
                        if (trace.transform.TryGetComponent(out c))
                        {
                            trace.transform.parent = null;
                            Destroy(c);
                        }
                    }
                }

                if (trace.collider != null && trace.collider.attachedRigidbody != null)
                {
                    GameObject dP = Instantiate(damageParticle, trace.point, new Quaternion(), null);
                    Destroy(dP, 5);

                    trace.collider.attachedRigidbody.AddForceAtPosition(trace.normal * -1000, trace.point);
                }
            }

        }
    }

    void Update()
    {
        Physics.Raycast(eyes.position, eyes.forward, out trace);

        Move();
        Attack();
    }

    //void OnControllerColliderHit(ControllerColliderHit hit)
    //{
    //    Rigidbody body = hit.collider.attachedRigidbody;
    //    if (body == null || body.isKinematic)
    //        return;

    //    if (hit.moveDirection.y < -0.3f)
    //        return;

    //    Vector3 pushDirection = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
    //    body.velocity = pushDirection * pushPower;
    //}
}
