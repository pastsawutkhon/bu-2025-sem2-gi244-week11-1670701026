using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public Transform focalPoint;
    public GameObject powerUpIndicator;

    private Rigidbody rb;

    private InputAction moveAction;
    private InputAction smashAction;
    private InputAction breakAction;

    public bool hasPowerUp = false;
    private Coroutine CountDownRoutine;
    public float stunEndTime = 0f;
    public float stunDuration = 5f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        moveAction = InputSystem.actions.FindAction("Move");
        smashAction = InputSystem.actions.FindAction("Smash");
        breakAction = InputSystem.actions.FindAction("Break");
        
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var move = moveAction.ReadValue<Vector2>();
        rb.AddForce(move.y * speed * focalPoint.forward);
        if (breakAction.IsPressed())
        {
            rb.linearVelocity = Vector3.zero;
        }
        powerUpIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);

        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (hasPowerUp)
            {
                var rb = collision.gameObject.GetComponent<Rigidbody>();
                var dir = collision.transform.position - transform.position;

                rb.AddForce(10 * dir.normalized, ForceMode.Impulse);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PowerUp"))
        {
            powerUpIndicator.SetActive(true);
            hasPowerUp = true;
            Destroy(other.gameObject);

            if (CountDownRoutine != null)
            {
                StopCoroutine(CountDownRoutine);
            }
            CountDownRoutine = StartCoroutine(PowerUpCountDown());
        }
        
        if (other.gameObject.CompareTag("StunPowerUp"))
        {
            stunEndTime = Time.time + stunDuration;
            Enemy[] enemies = FindObjectsOfType<Enemy>();
            foreach (Enemy e in enemies)
            {
                e.ApplyGlobalStun(stunEndTime);
            }

            Destroy(other.gameObject);
        }
    }



    IEnumerator PowerUpCountDown()
    {
        yield return new WaitForSeconds(10);
        powerUpIndicator.SetActive(false);
        hasPowerUp = false;
    }

    
}
