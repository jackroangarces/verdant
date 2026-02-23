using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    private ExitDoor door;

    void Awake()
    {
        door = GetComponentInParent<ExitDoor>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (door == null) return;
        door.TryEnterDoor(other);
    }
}