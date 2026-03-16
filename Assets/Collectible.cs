using UnityEngine;

public class Collectible : MonoBehaviour
{
    public int pointValue = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Coin touched by: " + other.name);

        PlayerController2D player = other.GetComponent<PlayerController2D>();

        if (player == null)
            player = other.GetComponentInParent<PlayerController2D>();

        if (player != null)
        {
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddPoint(pointValue);
            }

            Destroy(gameObject);
        }
    }
}