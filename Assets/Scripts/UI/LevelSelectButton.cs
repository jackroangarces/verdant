using UnityEngine;

public class LevelSelectButton : MonoBehaviour
{
    public LevelDefinition level;

    public void OnClick()
    {
        GameFlowManager.I.StartLevel(level);
    }
}