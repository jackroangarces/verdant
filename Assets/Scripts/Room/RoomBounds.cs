using UnityEngine;

public class RoomBounds : MonoBehaviour
{
    public float width = 33.75f;
    public float height = 60f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(
            transform.position,
            new Vector3(width, height, 0)
        );
    }
}
