using UnityEngine;
using UnityEditor;

namespace WraithavenGames.FirstSight
{
    [CustomEditor(typeof(PlayerController))]
    public class PlayerControllerEditor : Editor
    {
        override public void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            PlayerController controller = target as PlayerController;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Utilities", EditorStyles.boldLabel);

            EditorGUI.indentLevel++;

            // Jump Tuning
            {
                EditorGUILayout.LabelField("Jump Tuning", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;

                controller.TEMP_JumpHeight = EditorGUILayout.FloatField("Jump Height",
                    controller.TEMP_JumpHeight);
                controller.TEMP_JumpTime = EditorGUILayout.FloatField("Time To Peak",
                    controller.TEMP_JumpTime);

                Rect buttonRect = EditorGUILayout.GetControlRect();
                buttonRect = EditorGUI.IndentedRect(buttonRect);
                buttonRect.width = Mathf.Min(buttonRect.width, 200);

                if (GUI.Button(buttonRect, "Calculate Gravity and Velocity"))
                {
                    float h = controller.TEMP_JumpHeight;
                    float t = controller.TEMP_JumpTime;

                    float grav = (2f * h) / (t * t);
                    float vel = (2f * h) / t;

                    controller.SetGravityAndJumpSpeed(grav, vel);

                    EditorUtility.SetDirty(target);
                }

                EditorGUI.indentLevel--;
            }

            EditorGUI.indentLevel--;
        }
    }
}