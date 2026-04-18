using UnityEngine;
using UnityEditor;

public class FixInputSettings
{
    public static void Execute()
    {
        // Enable both old and new input systems
        PlayerSettings.SetPropertyString("activeInputHandler", "2", BuildTargetGroup.Standalone);
        Debug.Log("Input settings changed to 'Both' (legacy + new Input System)");
        AssetDatabase.SaveAssets();
    }
}
