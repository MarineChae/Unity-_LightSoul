using UnityEditor;
using UnityEngine;

/// <summary>
/// 인스펙터에서 dictionary를 사용하게 할 수 있도록 하는 스크립트
/// </summary>
#if DEBUG

[CustomPropertyDrawer(typeof(SerializedDictianary<,>), true)]
public class SerializableDictionaryDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty keys = property.FindPropertyRelative("keys");
        SerializedProperty values = property.FindPropertyRelative("values");

        Rect rect = position;
        rect.height = EditorGUIUtility.singleLineHeight;

        // 라벨 출력
        EditorGUI.LabelField(rect, label.text);
        rect.y += EditorGUIUtility.singleLineHeight + 4; // 라벨 다음에 간격 추가 (4px 여백)

        for (int i = 0; i < keys.arraySize; i++)
        {
            SerializedProperty keyProp = keys.GetArrayElementAtIndex(i);
            SerializedProperty valueProp = values.GetArrayElementAtIndex(i);

            // 키와 값의 UI 분할
            Rect keyRect = new Rect(rect.x, rect.y, rect.width / 2 - 5, rect.height);
            Rect valueRect = new Rect(rect.x + rect.width / 2 + 5, rect.y, rect.width / 2 - 5, rect.height);

            EditorGUI.PropertyField(keyRect, keyProp, GUIContent.none);
            EditorGUI.PropertyField(valueRect, valueProp, GUIContent.none);

            // 다음 항목으로 이동 (y 위치 증가)
            rect.y += EditorGUIUtility.singleLineHeight + 2; // 여백 추가 (2px)
        }

        // Add Item 버튼의 위치 설정
        Rect buttonRect = new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight);

        if (GUI.Button(buttonRect, "Add Item"))
        {
            keys.arraySize++;
            values.arraySize++;
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty keys = property.FindPropertyRelative("keys");
        // 전체 높이 계산: 키-값 항목 수에 따라 높이를 조절 (각 항목의 높이 + 라벨 높이 + 버튼 높이)
        float totalHeight = EditorGUIUtility.singleLineHeight * (keys.arraySize + 2) + (2 * keys.arraySize);
        return totalHeight;
    }
}
#else
public class SerializableDictionaryDrawer
{
}
#endif