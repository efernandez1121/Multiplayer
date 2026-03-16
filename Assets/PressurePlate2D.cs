using UnityEngine;

public class PressurePlate2D : MonoBehaviour
{
    public MovingBlocker2D blocker;
    private int playersOnPlate = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController2D>() != null)
        {
            playersOnPlate++;
            blocker.SetOpen(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController2D>() != null)
        {
            playersOnPlate--;
            if (playersOnPlate < 0) playersOnPlate = 0;

            if (playersOnPlate == 0)
            {
                blocker.SetOpen(false);
            }
        }
    }
}