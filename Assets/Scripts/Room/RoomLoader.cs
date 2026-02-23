using UnityEngine;

public class RoomLoader : MonoBehaviour
{
    GameObject spawned;

    void Start()
    {
        var prefab = GameFlowManager.I.GetCurrentRoomPrefab();
        Debug.Log("RoomLoader Start. Room prefab = " + GameFlowManager.I.GetCurrentRoomPrefab());
        if (prefab == null)
        {
            Debug.LogError("No room prefab found for current room.");
            return;
        }

        spawned = Instantiate(prefab, Vector3.zero, Quaternion.identity);
    }
}