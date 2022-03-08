using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Enemy))]
public class EnemyEditor : Editor {

    SerializedProperty lookAtPoint;

    void OnEnable()
    {
        lookAtPoint = serializedObject.FindProperty("lookAtPoint");
    }

    public override void OnInspectorGUI() {
        //base.OnInspectorGUI();

        //MaxHealth
        //Current Health
        //Start Position
        //Distance
        //StatusBar
        //EnergySpeed
        //BasicAttack
        //MainSkill
        //SecondSkill
        Enemy myTarget = (Enemy) target;

        myTarget.maxHealth = EditorGUILayout.IntField("Vida maxima", myTarget.maxHealth);
        myTarget.currentHealth = EditorGUILayout.IntField("Vida atual", myTarget.currentHealth);
        EditorGUILayout.LabelField("Ações da IA");
        GUILayout.Button("+");

        //EditorGUILayout.LabelField("Level", myTarget.Level.ToString());
    }
}