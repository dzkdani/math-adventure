using UnityEditor;

[CustomEditor(typeof(SkillEffectData))]
[CanEditMultipleObjects]
public class SkillEffectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SerializedProperty effectTypeProperty = serializedObject.FindProperty("Type");
        EditorGUILayout.PropertyField(effectTypeProperty);
        if (effectTypeProperty.enumValueIndex == (int)SkillEffectData.SkillType.DAMAGE)
        {
            SerializedProperty damageTypeProperty = serializedObject.FindProperty("DamageType");
            EditorGUILayout.PropertyField(damageTypeProperty);
        }
        else if (effectTypeProperty.enumValueIndex == (int)SkillEffectData.SkillType.DAMAGEOVERTIME)
        {
            SerializedProperty damageOverTimeTypeProperty = serializedObject.FindProperty("DamageOverTimeType");
            EditorGUILayout.PropertyField(damageOverTimeTypeProperty);
        }
        else if (effectTypeProperty.enumValueIndex == (int)SkillEffectData.SkillType.HEAL)
        {
            SerializedProperty healTypeProperty = serializedObject.FindProperty("HealType");
            EditorGUILayout.PropertyField(healTypeProperty);
        }
        else if (effectTypeProperty.enumValueIndex == (int)SkillEffectData.SkillType.BUFF)
        {
            SerializedProperty buffTypeProperty = serializedObject.FindProperty("BuffType");
            EditorGUILayout.PropertyField(buffTypeProperty);
        }
        else if (effectTypeProperty.enumValueIndex == (int)SkillEffectData.SkillType.NERF)
        {
            SerializedProperty nerfTypeProperty = serializedObject.FindProperty("NerfType");
            EditorGUILayout.PropertyField(nerfTypeProperty);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
