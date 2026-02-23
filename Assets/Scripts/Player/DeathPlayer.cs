using UnityEngine;

public class DeathPlayer : MonoBehaviour
{
    public bool isDead { get; private set; }

    public void Die()
    {
        if (isDead) return;

        isDead = true;

        Debug.Log("Player died");

        GameFlowManager.I.OnPlayerDied();
    }
}
