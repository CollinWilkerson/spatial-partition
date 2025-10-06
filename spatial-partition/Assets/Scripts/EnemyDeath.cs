using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision");
        if (collision.gameObject.CompareTag("Friendly"))
        {
            GameController.RemoveEnemy(transform);
        }
    }
}
