using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController mainPlayer;

    public int health = 100;
    public int maxHealth = 100;

    public float defaultMoveSpeed = 1f;
    public float sensitivity = 5.0f;
    public float verticalSensitivity = 5.0f;
    public float jumpHeight = 100.0f;
    public float reloadSpeed = 100.0f;

    public Vector3 scopedSpread;
    public Vector3 defaultSpread;

    public GameObject damageParticle;
    public GameObject projectile;
    public GameObject hitMarker;
    public HUD pHud;
    public Transform gun;
    public Transform scopeDefault;
    public Transform gunDefault;

    private float rotY = 0f;

    private Vector3 spread;

    private bool grounded = false;

    private float moveSpeed;
    public Vector3 jump;
    
    private RaycastHit eyeTrace;
    private RaycastHit weaponTrace;

    private Transform eyes;
    private CharacterController controller = null;

    private float primaryAmmo = 10;
    private float primaryClip = 30;
    private float blades = 3;

    private bool reloading = false;
    private float reloadRot = 0;

    // Start is called before the first frame update
    void Start()
    {
        mainPlayer = this;

        controller = GetComponent<CharacterController>();

        eyes = Camera.main.transform;

        Cursor.lockState = CursorLockMode.Locked;

        pHud.maxHealth = maxHealth;
        pHud.Assign(health, 10, 30, 3);

        spread = defaultSpread;
        moveSpeed = defaultMoveSpeed;
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

        float running = (Input.GetKey(KeyCode.LeftShift) ? 2f : 1f);

        // Movement
        if (PlayerForward() != 0)
            controller.Move(transform.forward * PlayerForward() * moveSpeed * running * Time.fixedDeltaTime);

        if (PlayerRight() != 0)
            controller.Move(transform.right * PlayerRight() * moveSpeed * running * Time.fixedDeltaTime);

        if (!grounded)
            jump += Physics.gravity * Time.deltaTime;

        controller.Move(jump * Time.deltaTime);
    }

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !(primaryAmmo <= 0 && primaryClip <= 0) && !reloading)
        {
            primaryAmmo -= 1;

            pHud.ammoTo = primaryAmmo;
            pHud.clipTo = primaryClip;

            int spreadMult = Input.GetKey(KeyCode.LeftShift) ? 2 : 1;
            
            Physics.Raycast(eyes.position, (eyes.forward + (new Vector3(Random.Range(-spread.x * spreadMult, spread.x * spreadMult), Random.Range(-spread.y * spreadMult, spread.y * spreadMult), Random.Range(-spread.z * spreadMult, spread.z * spreadMult)))).normalized, out weaponTrace);
            GameObject dP = Instantiate(damageParticle, weaponTrace.point, new Quaternion(), null);
            Destroy(dP, 5);

            if (weaponTrace.collider != null)
            {
                EnemyController e;
                if (weaponTrace.transform.root.TryGetComponent(out e))
                {
                    // There are many ways to do this not like this but im a lazy fucker.

                    int rand = Random.Range(10, 30);

                    string n = weaponTrace.transform.name;
                    if (e.TakeDamage(rand, n, weaponTrace)
                        || n == "LeftArm"
                        || n == "RightArm"
                        || n == "LeftLeg"
                        || n == "LeftUpLeg"
                        || n == "RightLeg"
                        || n == "RightUpLeg")
                    {
                        CharacterJoint c;
                        if (weaponTrace.transform.TryGetComponent(out c))
                        {
                            weaponTrace.transform.parent = null;
                            Destroy(c);
                        }
                    }
                }

                if (weaponTrace.collider != null && weaponTrace.collider.attachedRigidbody != null)
                {
                    weaponTrace.collider.attachedRigidbody.AddForceAtPosition(weaponTrace.normal * -1000, weaponTrace.point);
                }
            }

            if (primaryAmmo <= 0)
            {
                reloading = true;
            }
        }
        
        if (!reloading)
        {
            if (Input.GetKey(KeyCode.Mouse1))
            {
                gun.position = Vector3.Lerp(gun.position, scopeDefault.position, 5 * Time.deltaTime);
                gun.rotation = Quaternion.Lerp(gun.rotation, scopeDefault.rotation, 5 * Time.deltaTime);
                spread = scopedSpread;
                moveSpeed = defaultMoveSpeed * 0.1f;


            }
            else
            {
                spread = defaultSpread;
                gun.position = Vector3.Lerp(gun.position, gunDefault.position, 5 * Time.deltaTime);
                gun.rotation = Quaternion.Lerp(gun.rotation, gunDefault.rotation, 5 * Time.deltaTime);
                moveSpeed = defaultMoveSpeed;
            }
        }

        if (blades > 0 && Input.GetKeyDown(KeyCode.Q))
        {
            blades -= 1;
            pHud.secondaryTo = blades;

            GameObject obj = Instantiate(projectile, null);
            obj.transform.position = eyes.transform.position + eyes.transform.forward;
            obj.GetComponent<Rigidbody>().AddForce(eyes.transform.forward * 50f, ForceMode.Impulse);
            obj.transform.rotation = eyes.transform.rotation;

            Destroy(obj, 10);
        }
    }

    public void Reload()
    {
        gun.Rotate(reloadSpeed * Time.deltaTime, 0, 0);
        reloadRot += reloadSpeed * Time.deltaTime;

        if (reloadRot >= 360)
        {
            reloadRot = 0;
            reloading = false;

            primaryAmmo = Mathf.Clamp(primaryClip, 0, 10);
            primaryClip = primaryClip - primaryAmmo;
        }
    }

    void Update()
    {
        Physics.Raycast(eyes.position, eyes.forward, out eyeTrace);

        if (reloading) Reload();
        Move();
        Attack();
    }

    public bool TakeDamage(int damage)
    {
        health -= damage;

        pHud.healthTo = health;

        if (health <= 0)
        {

            return true;
        }

        return false;
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
