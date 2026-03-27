using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public static float globalStunEndTime = 0f;
    public float speed = 3f;
    public bool isStunned = false;
    private Rigidbody rb;
    private GameObject player;
    private Coroutine stunCountdownRoutine;
    private float localStunEndTime = 0f;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        
    }
    void Start()
    {
        PlayerController p = FindObjectOfType<PlayerController>();
        ApplyGlobalStun(globalStunEndTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (isStunned) return;
        Vector3 dir = player.transform.position - transform.position;
        dir.Normalize();
        rb.linearVelocity = dir * speed;
    }

    public void ApplyGlobalStun(float globalEndTime)
    {
        globalStunEndTime = globalEndTime;
        float remaining = globalEndTime - Time.time;
        if (remaining <= 0) return;

        if (stunCountdownRoutine != null)
        {
            StopCoroutine(stunCountdownRoutine);
        }
        stunCountdownRoutine = StartCoroutine(StunCountdown(remaining));
    }

    IEnumerator StunCountdown(float duration)
    {
        isStunned = true;
        rb.linearVelocity = Vector3.zero;
        yield return new WaitForSeconds(duration);
        isStunned = false;
    }

        
}
