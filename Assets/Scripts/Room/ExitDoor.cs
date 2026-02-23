using UnityEngine;
using System;

public class ExitDoor : MonoBehaviour
{
    public enum DoorState { Locked, Opening, Open }

    [Header("State")]
    public DoorState state = DoorState.Locked;

    [Header("References")]
    [SerializeField] private Collider2D solidCollider;     // blocks like wall
    [SerializeField] private Collider2D enterTrigger;      // triggers exit

    [Header("Tuning")]
    public float openAnimDuration = 0.4f; // placeholder for later

    public event Action OnDoorEntered;

    void Awake()
    {
        ApplyState(state);
    }
    public void UnlockAndOpen()
    {
        if (state != DoorState.Locked) return;

        state = DoorState.Opening;
        
        ApplyState(DoorState.Opening);

        // Placeholder for animation; later drive this via Animator event
        Invoke(nameof(FinishOpen), openAnimDuration);
    }

    void FinishOpen()
    {
        state = DoorState.Open;
        ApplyState(DoorState.Open);
    }

    void ApplyState(DoorState s)
    {
        if (solidCollider == null || enterTrigger == null)
        {
            Debug.LogError("ExitDoor: Missing collider references.");
            return;
        }

        switch (s)
        {
            case DoorState.Locked:
            case DoorState.Opening:
                solidCollider.enabled = true;
                enterTrigger.enabled = false;
                break;

            case DoorState.Open:
                solidCollider.enabled = false;
                enterTrigger.enabled = true;
                break;
        }
    }
    public void TryEnterDoor(Collider2D other)
    {
        Debug.Log("Door trigger hit by: " + other.name);
        if (state != DoorState.Open) return;
        if (!other.CompareTag("Player")) return;

        GameFlowManager.I.NextRoom();
        OnDoorEntered?.Invoke();
    }
}