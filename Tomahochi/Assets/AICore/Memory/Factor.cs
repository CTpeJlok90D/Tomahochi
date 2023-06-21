using System;
using UnityEditor;
using UnityEngine;

namespace AICore
{
    [Serializable]
    public struct Factor
    {
        public string Name;
        public object Source;
        public Factor(string name, object source)
        {
            Name = name;
            Source = source;
		}
        
        public static implicit operator Factor(string name)
        {
            return new Factor() { Name = name };
        }
        public static implicit operator string(Factor f) 
        {
            return f.Name;
        }

        public static bool operator ==(Factor a, Factor b)
        {
            return a.Name == b.Name;
        }
		public static bool operator !=(Factor a, Factor b)
		{
			return a.Name != b.Name;
		}



#if UNITY_EDITOR
		[CustomPropertyDrawer(typeof(FactorDrawer))]
        private class FactorDrawer : PropertyDrawer
        {
			public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
			{
                //base.OnGUI(position, property, label);
                EditorGUI.BeginProperty(position, label, property);
                EditorGUI.PropertyField(position, property.FindPropertyRelative("Name"));
                EditorGUI.EndProperty();
			}
		}
#endif
    }
}
