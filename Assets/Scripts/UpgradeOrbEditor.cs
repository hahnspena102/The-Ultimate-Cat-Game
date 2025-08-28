using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof(UpgradeOrb))]
public class UpgradeOrbEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        UpgradeOrb orb = (UpgradeOrb)target;

        if (orb == null || orb.gameObject == null) return;

        if (orb.Upgrade != null)
        {
            // Rename GameObject
            if (orb.gameObject.name != orb.Upgrade.name)
            {
                Undo.RecordObject(orb.gameObject, "Rename UpgradeOrb");
                orb.gameObject.name = orb.Upgrade.name;
                EditorUtility.SetDirty(orb.gameObject);
            }

            // Update image sprite
            if (orb.Image != null && orb.Upgrade.Sprite != null && orb.Image.sprite != orb.Upgrade.Sprite)
            {
                Undo.RecordObject(orb.Image, "Assign Upgrade Sprite");
                orb.Image.sprite = orb.Upgrade.Sprite;
                orb.Image.preserveAspect = true; // keep sizing consistent
                EditorUtility.SetDirty(orb.Image);
            }
        }
    }
}
