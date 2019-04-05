using System.Collections.Generic;
using UnityEditor;
using System.Reflection;
using System.Linq;
using UnityEngine;
using System.Text.RegularExpressions;

namespace UnityStandardAssets.CinematicEffects
{

    [CustomEditor(typeof(StylisticFog))]
    [CanEditMultipleObjects]
    public class StylisticFogEditor : Editor
    {
        private List<SerializedProperty> m_TopLevelFields = new List<SerializedProperty>();

        class ColorSourceDisplay
        {
            private Dictionary<StylisticFog.ColorSelectionType, List<SerializedProperty>> properties = new Dictionary<StylisticFog.ColorSelectionType, List<SerializedProperty>>();
            private string path;

            public void PopulateMap(SerializedObject so, string path)
            {
                this.path = path;

                properties.Clear();

                var fields = typeof(StylisticFog.FogColorSource).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(x => x.GetCustomAttributes(typeof(StylisticFog.FogColorSource.DisplayOnSelectionType), false).Any());

                foreach (var field in fields)
                {
                    var displayAttributes = field.GetCustomAttributes(typeof(StylisticFog.FogColorSource.DisplayOnSelectionType), false) as StylisticFog.FogColorSource.DisplayOnSelectionType[];
                    StylisticFog.FogColorSource.DisplayOnSelectionType usedAttribute = displayAttributes[0];
                    if (usedAttribute != null)
                    {
                        if (!properties.ContainsKey(usedAttribute.selectionType))
                            properties[usedAttribute.selectionType] = new List<SerializedProperty>();

                        properties[usedAttribute.selectionType].Add(so.FindProperty(path + "." + field.Name));
                    }
                }

            }

            public void OnInspectorGUI(StylisticFog.ColorSelectionType currentSelection, StylisticFog.FogColorSource colorSource, SerializedObject serializedObject)
            {
                EditorGUI.BeginChangeCheck();

                if (properties.ContainsKey(currentSelection))
                {
                    foreach (var prop in properties[currentSelection])
                        EditorGUILayout.PropertyField(prop);
                }

                if(currentSelection == StylisticFog.ColorSelectionType.TextureRamp && colorSource.colorRamp != null)
                {
                    List<string> warnings = new List<string>();
                    if(colorSource.colorRamp.filterMode == UnityEngine.FilterMode.Point)
                    {
                        warnings.Add("Texture filter mode should be either bilinear or trilinear.");
                    }

                    if (colorSource.colorRamp.wrapMode != UnityEngine.TextureWrapMode.Clamp)
                    {
                        warnings.Add("Texture wrap mode should be set to clamp.");
                    }

                    if (colorSource.colorRamp.mipmapCount > 1)
                    {
                        warnings.Add("Texture should not have generated mipmaps.");
                    }

                    if (warnings.Count() > 0)
                    {
                        string warningMSG = "The following settings should be applied to the color ramp texture:";
                        foreach(string warn in warnings)
                        {
                            warningMSG += "\n - " + warn;
                        }

                        EditorGUILayout.HelpBox(warningMSG, MessageType.Warning);
                    }
                }
 
                if (EditorGUI.EndChangeCheck() && currentSelection == StylisticFog.ColorSelectionType.Gradient)
                {
                    SerializedProperty gradientDirtyProp = serializedObject.FindProperty(path + ".m_GradientDirty");
                    if (gradientDirtyProp != null)
                    {
                        gradientDirtyProp.boolValue = true;
                    }
                    else
                        Debug.LogWarning(StylisticFog.m_EffectName + " Editor: Serialized property not found.");
                }
            }
        }

        class InfoMap
        {
            public string name;
            public bool distanceFog;
            public bool heightFog;
            public List<SerializedProperty> properties;
        }
        private List<InfoMap> m_GroupFields = new List<InfoMap>();

        private ColorSourceDisplay distanceFogColorDisplay = new ColorSourceDisplay();
        private ColorSourceDisplay heightFogColorDisplay = new ColorSourceDisplay();

        public void OnEnable()
        {
            distanceFogColorDisplay.PopulateMap(serializedObject, "m_DistanceColorSource");
            heightFogColorDisplay.PopulateMap(serializedObject, "m_HeightColorSource");

            var settingsGroups = typeof(StylisticFog).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(x => x.GetCustomAttributes(typeof(StylisticFog.SettingsGroup), false).Any());

            foreach (var group in settingsGroups)
            {
                var searchPath = group.Name + ".";

                foreach (var setting in group.FieldType.GetFields(BindingFlags.Instance | BindingFlags.Public))
                {
                    var infoGroup = m_GroupFields.FirstOrDefault(x => x.name == group.Name);
                    if (infoGroup == null)
                    {
                        infoGroup = new InfoMap();
                        infoGroup.properties = new List<SerializedProperty>();
                        infoGroup.name = group.Name;
                        infoGroup.distanceFog = group.FieldType == typeof(StylisticFog.DistanceFogSettings);
                        infoGroup.heightFog = group.FieldType == typeof(StylisticFog.HeightFogSettings);
                        m_GroupFields.Add(infoGroup);
                    }


                    var property = serializedObject.FindProperty(searchPath + setting.Name);
                    if (property != null)
                    {
                        infoGroup.properties.Add(property);
                    }
                }
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            StylisticFog targetInstance = (StylisticFog)target;

            foreach (var setting in m_TopLevelFields)
                EditorGUILayout.PropertyField(setting);

            foreach (var group in m_GroupFields)
            {
                EditorGUILayout.Space();

                var groupProperty = serializedObject.FindProperty(group.name);
                var enabledField = group.properties.FirstOrDefault(x => x.propertyPath == group.name + ".enabled");
                bool display = EditorGUIHelper.Header(groupProperty, enabledField);
                if (!display)
                {
                    serializedObject.ApplyModifiedProperties();
                    continue;
                }
                EditorGUI.indentLevel++;

                foreach (var field in group.properties.Where(x => x.propertyPath != group.name + ".enabled"))
                    EditorGUILayout.PropertyField(field);

                if (group.distanceFog)
                {
                    distanceFogColorDisplay.OnInspectorGUI(targetInstance.distanceFogSettings.colorSelectionType, targetInstance.distanceColorSource, serializedObject);
                }

                if (group.heightFog)
                {
                    heightFogColorDisplay.OnInspectorGUI(targetInstance.heightFogSettings.colorSelectionType, targetInstance.heightColorSource, serializedObject);
                }

                EditorGUI.indentLevel--;

                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}



