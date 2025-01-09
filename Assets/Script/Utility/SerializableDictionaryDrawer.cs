using UnityEditor;
using UnityEngine;

/// <summary>
/// �ν����Ϳ��� dictionary�� ����ϰ� �� �� �ֵ��� �ϴ� ��ũ��Ʈ
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

        // �� ���
        EditorGUI.LabelField(rect, label.text);
        rect.y += EditorGUIUtility.singleLineHeight + 4; // �� ������ ���� �߰� (4px ����)

        for (int i = 0; i < keys.arraySize; i++)
        {
            SerializedProperty keyProp = keys.GetArrayElementAtIndex(i);
            SerializedProperty valueProp = values.GetArrayElementAtIndex(i);

            // Ű�� ���� UI ����
            Rect keyRect = new Rect(rect.x, rect.y, rect.width / 2 - 5, rect.height);
            Rect valueRect = new Rect(rect.x + rect.width / 2 + 5, rect.y, rect.width / 2 - 5, rect.height);

            EditorGUI.PropertyField(keyRect, keyProp, GUIContent.none);
            EditorGUI.PropertyField(valueRect, valueProp, GUIContent.none);

            // ���� �׸����� �̵� (y ��ġ ����)
            rect.y += EditorGUIUtility.singleLineHeight + 2; // ���� �߰� (2px)
        }

        // Add Item ��ư�� ��ġ ����
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
        // ��ü ���� ���: Ű-�� �׸� ���� ���� ���̸� ���� (�� �׸��� ���� + �� ���� + ��ư ����)
        float totalHeight = EditorGUIUtility.singleLineHeight * (keys.arraySize + 2) + (2 * keys.arraySize);
        return totalHeight;
    }
}
#else
public class SerializableDictionaryDrawer
{
}
#endif