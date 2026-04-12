// PlayerToolManager.cs — attach to player
using UnityEngine;

public class PlayerToolManager : MonoBehaviour
{
    public string currentTool = "";

    public void EquipTool(string toolName)
    {
        currentTool = toolName;
    }

    public void UnequipTool()
    {
        currentTool = "";
    }

    public string GetCurrentTool()
    {
        return currentTool;
    }
}