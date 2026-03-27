using UnityEngine;

public class StunPowerUp : MonoBehaviour
{
    public float stunEndTime = 0f;
    public float stunDuration = 5f;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            float endTime = Time.time + stunDuration;

            Enemy[] enemies = FindObjectsOfType<Enemy>();
            foreach (Enemy e in enemies)
            {
                e.ApplyGlobalStun(endTime);
            }

            Destroy(gameObject);
            Debug.Log("😵‍💫 Enemies stunned for 5 seconds");
        }
    }
}
