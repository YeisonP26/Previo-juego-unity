using UnityEngine;
using UnityEditor;

public class FixBowVisual
{
    public static void Execute()
    {
        // Find the BowLimb and resize it to be more bow-shaped
        GameObject archer = GameObject.Find("Archer");
        if (archer == null) { Debug.LogError("No Archer found!"); return; }

        // Fix BowLimb - thinner, taller, more bow-like
        Transform bowLimb = archer.transform.Find("BowLimb");
        if (bowLimb != null)
        {
            bowLimb.localScale = new Vector3(0.12f, 0.9f, 1f); // much thinner
            bowLimb.localPosition = new Vector3(0.6f, 0.4f, 0);
        }

        // Fix BowString - adjust to match
        Transform bowString = archer.transform.Find("BowString");
        if (bowString != null)
        {
            bowString.localScale = new Vector3(0.015f, 0.85f, 1f);
            bowString.localPosition = new Vector3(0.55f, 0.4f, 0);
        }

        // Fix arrow resting - shorter, positioned better
        Transform arrowRest = archer.transform.Find("ArrowResting");
        if (arrowRest != null)
        {
            arrowRest.localScale = new Vector3(0.55f, 0.035f, 1f);
            arrowRest.localPosition = new Vector3(0.8f, 0.4f, 0);
        }

        Transform arrowTip = archer.transform.Find("ArrowRestTip");
        if (arrowTip != null)
        {
            arrowTip.localScale = new Vector3(0.06f, 0.06f, 1f);
            arrowTip.localPosition = new Vector3(1.08f, 0.4f, 0);
        }

        Transform fletch = archer.transform.Find("ArrowRestFletch");
        if (fletch != null)
        {
            fletch.localScale = new Vector3(0.05f, 0.1f, 1f);
            fletch.localPosition = new Vector3(0.54f, 0.4f, 0);
        }

        EditorUtility.SetDirty(archer);
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
        UnityEditor.SceneManagement.EditorSceneManager.SaveOpenScenes();
        Debug.Log("Bow visual refined!");
    }
}
