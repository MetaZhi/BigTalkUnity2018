using System;
using UnityEngine;
using System.IO;

namespace UnityStandardAssets.CinematicEffects
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("Image Effects/Cinematic/Stylistic Fog")]
#if UNITY_5_4_OR_NEWER
    [ImageEffectAllowedInSceneView]
#endif
    public class StylisticFog : MonoBehaviour
    {

        public delegate string WarningDelegate();

        [AttributeUsage(AttributeTargets.Field)]
        public class SettingsGroup : Attribute
        { }

        [Serializable]
        public enum ColorSelectionType
        {
            Gradient = 1,
            TextureRamp = 2,
            ShareOther = 3,
        }

        #region settings
        [Serializable]
        public struct FogColorSource
        {
            [AttributeUsage(AttributeTargets.Field)]
            public class DisplayOnSelectionType : Attribute
            {
                public readonly ColorSelectionType selectionType;
                public DisplayOnSelectionType(ColorSelectionType _selectionType)
                {
                    selectionType = _selectionType;
                }
            }

            [SerializeField, HideInInspector]
            internal bool m_GradientDirty;

            [Tooltip("Color gradient.")]
            [DisplayOnSelectionType(ColorSelectionType.Gradient)]
            [SerializeField]
            private Gradient m_Gradient;
            public Gradient gradient
            {
                get 
                {
                    if (m_Gradient == null)
                    {
                        m_Gradient = defaultFogGradient;
                        m_GradientDirty = true;
                    }
                    return m_Gradient;
                }
                set 
                {
                    m_GradientDirty = true;
                    m_Gradient = value;
                }
            }

            [Tooltip("Custom fog color ramp.")]
            [DisplayOnSelectionType(ColorSelectionType.TextureRamp)]
            [SerializeField]
            private Texture2D m_ColorRamp;
            public Texture2D colorRamp
            {
                get { return m_ColorRamp; }
                set { m_ColorRamp = value; }
            }

            public static Gradient defaultFogGradient
            {
                get
                {
                    GradientAlphaKey firstAlpha = new GradientAlphaKey(0f, 0f);
                    GradientAlphaKey lastAlpha = new GradientAlphaKey(1f, 1f);
                    GradientAlphaKey[] initialAlphaKeys = { firstAlpha, lastAlpha };
                    Gradient g = new Gradient();
                    g.alphaKeys = initialAlphaKeys;
                    return g;
                }
            }

            public static FogColorSource defaultSettings
            {
                get
                {
                    FogColorSource source = new FogColorSource()
                    {
                        gradient = defaultFogGradient,
                        m_GradientDirty = true,
                        colorRamp = null,
                    };
                    return source;
                }
            }
        }

        [Serializable]
        public struct DistanceFogSettings
        {
            [Tooltip("Wheter or not to apply distance based fog.")]
            public bool enabled;

            [Tooltip("Wheter or not to apply distance based fog to the skybox.")]
            public bool fogSkybox;

            [Tooltip("Fog is fully saturated beyond this distance.")]
            public float endDistance;

            [Tooltip("Color selection for distance fog")]
            public ColorSelectionType colorSelectionType;

            public static DistanceFogSettings defaultSettings
            {
                get
                {
                    return new DistanceFogSettings()
                    {
                        enabled = true,
                        fogSkybox = false,
                        endDistance = 100f,
                        colorSelectionType = ColorSelectionType.Gradient,
                    };
                }
            }
        }

        [Serializable]
        public struct HeightFogSettings
        {
            [Tooltip("Wheter or not to apply height based fog.")]
            public bool enabled;

            [Tooltip("Wheter or not to apply height based fog to the skybox.")]
            public bool fogSkybox;

            [Tooltip("Height where the fog starts.")]
            public float baseHeight;

            [Tooltip("Fog density at fog altitude given by height.")]
            public float baseDensity;

            [Tooltip("The rate at which the thickness of the fog decays with altitude.")]
            [Range(0.001f, 1f)]
            public float densityFalloff;

            [Tooltip("Color selection for height fog.")]
            public ColorSelectionType colorSelectionType;

            public static HeightFogSettings defaultSettings
            {
                get
                {
                    return new HeightFogSettings()
                    {
                        enabled = false,
                        fogSkybox = true,
                        baseHeight = 0f,
                        baseDensity = 0.1f,
                        densityFalloff = 0.5f,
                        colorSelectionType = ColorSelectionType.ShareOther,
                    };
                }
            }
        }
        #endregion

        #region settingFields
        [SettingsGroup, SerializeField]
        private DistanceFogSettings m_DistanceFogSettings = DistanceFogSettings.defaultSettings;
        public DistanceFogSettings distanceFogSettings
        {
            get { return m_DistanceFogSettings; }
            set { m_DistanceFogSettings = value; }
        }

        [SettingsGroup, SerializeField]
        private HeightFogSettings m_HeightFogSettings = HeightFogSettings.defaultSettings;
        public HeightFogSettings heightFogSettings
        {
            get { return m_HeightFogSettings; }
            set { m_HeightFogSettings = value; }
        }

        [SerializeField]
        private FogColorSource m_DistanceColorSource = FogColorSource.defaultSettings;
        public FogColorSource distanceColorSource
        {
            get { return m_DistanceColorSource; }
            set { m_DistanceColorSource = value; }
        }

        [SerializeField]
        private FogColorSource m_HeightColorSource = FogColorSource.defaultSettings;
        public FogColorSource heightColorSource
        {
            get { return m_HeightColorSource; }
            set { m_HeightColorSource = value; }
        }
        #endregion

        #region fields
        public const string m_EffectName = "Stylistic Fog";

        private Camera m_Camera;
        private Camera camera_
        {
            get
            {
                if (m_Camera == null)
                    m_Camera = GetComponent<Camera>();

                return m_Camera;
            }
        }

        [SerializeField]
        private Texture2D m_DistanceColorTexture;
        private Texture2D distanceColorTexture
        {
            get
            {
                if (m_DistanceColorTexture == null)
                {
                    m_DistanceColorTexture = new Texture2D(1024, 1, TextureFormat.ARGB32, false, false)
                    {
                        name = m_EffectName + ": Distance fog color texture.",
                        wrapMode = TextureWrapMode.Clamp,
                        filterMode = FilterMode.Bilinear,
                        anisoLevel = 0,
                    };
                }
                return m_DistanceColorTexture;
            }
        }

        [SerializeField]
        private Texture2D m_HeightColorTexture;
        private Texture2D heightColorTexture
        {
            get
            {
                if (m_HeightColorTexture == null)
                {
                    m_HeightColorTexture = new Texture2D(256, 1, TextureFormat.ARGB32, false, false)
                    {
                        name = m_EffectName + ": Distance fog color texture.",
                        wrapMode = TextureWrapMode.Clamp,
                        filterMode = FilterMode.Bilinear,
                        anisoLevel = 0,
                    };
                }
                return m_HeightColorTexture;
            }
        }

        [SerializeField, HideInInspector]
        private Shader m_Shader;
        private Shader shader
        {
            get
            {
                if (m_Shader == null)
                {
                    const string shaderName = "Hidden/Image Effects/StylisticFog";
                    m_Shader = Shader.Find(shaderName);
                }

                return m_Shader;
            }
        }

        private Material m_Material;
        private Material material
        {
            get
            {
                if (m_Material == null)
                    m_Material = ImageEffectHelper.CheckShaderAndCreateMaterial(shader);

                return m_Material;
            }
        }
        #endregion

        #region Private Members
        private void OnEnable()
        {
            if (!ImageEffectHelper.IsSupported(shader, true, false, this))
                enabled = false;

            camera_.depthTextureMode |= DepthTextureMode.Depth;
            m_DistanceColorSource.m_GradientDirty = true;
            m_HeightColorSource.m_GradientDirty = true;
        }

        private void OnDisable()
        {
            if (m_Material != null)
                DestroyImmediate(m_Material);

            if (m_DistanceColorTexture != null)
                DestroyImmediate(m_DistanceColorTexture);

            if (m_HeightColorTexture != null)
                DestroyImmediate(m_HeightColorTexture);

            m_Material = null;
            m_DistanceColorTexture = null;
            m_HeightColorTexture = null;
        }

        public void Update()
        {
            if (distanceFogSettings.colorSelectionType == heightFogSettings.colorSelectionType && heightFogSettings.colorSelectionType == ColorSelectionType.ShareOther)
            {
                m_DistanceFogSettings.colorSelectionType = ColorSelectionType.Gradient;
                m_DistanceColorSource.m_GradientDirty = true;
            }
            UpdateDistanceFogTextures();
            UpdateHeightFogTextures();
        }
        

        private bool SetMaterialUniforms()
        {
            if (! (heightFogSettings.enabled || distanceFogSettings.enabled))
                return false;

            bool seperateColorSettings = (distanceFogSettings.colorSelectionType != ColorSelectionType.ShareOther
                                          && heightFogSettings.colorSelectionType != ColorSelectionType.ShareOther)
                                          && (heightFogSettings.enabled && distanceFogSettings.enabled);

            if (seperateColorSettings)
            {
                material.SetInt("_SharedColor", 0);
                material.SetTexture("_FogColorTexture0", distanceFogSettings.colorSelectionType == ColorSelectionType.Gradient ? distanceColorTexture : distanceColorSource.colorRamp);
                material.SetTexture("_FogColorTexture1", heightFogSettings.colorSelectionType == ColorSelectionType.Gradient ? heightColorTexture : heightColorSource.colorRamp);
            }
            else
            {
                material.SetInt("_SharedColor", 1);
                bool selectingFromDistanceFog = distanceFogSettings.colorSelectionType != ColorSelectionType.ShareOther;
                ColorSelectionType activeSelectionType = selectingFromDistanceFog ? distanceFogSettings.colorSelectionType : heightFogSettings.colorSelectionType;
                if (activeSelectionType == ColorSelectionType.Gradient)
                    material.SetTexture("_FogColorTexture0", selectingFromDistanceFog ? distanceColorTexture : heightColorTexture);
                else
                    material.SetTexture("_FogColorTexture0", selectingFromDistanceFog ? distanceColorSource.colorRamp : heightColorSource.colorRamp);
            }

            Vector4 fogModeSettings = new Vector4();
            fogModeSettings.x = distanceFogSettings.enabled ? 1f : 0f;
            fogModeSettings.y = heightFogSettings.enabled ? 1f : 0f;
            fogModeSettings.z = distanceFogSettings.fogSkybox ? 1f : 0f;
            fogModeSettings.w = heightFogSettings.fogSkybox ? 1f : 0f;

            Vector4 fogParameters = new Vector4();
            fogParameters.x = distanceFogSettings.endDistance;
            fogParameters.y = heightFogSettings.baseHeight;
            fogParameters.z = heightFogSettings.baseDensity;
            fogParameters.w = heightFogSettings.densityFalloff;
            
            material.SetMatrix("_InverseViewMatrix", camera_.cameraToWorldMatrix);
            material.SetVector("_FogModeSettings", fogModeSettings);
            material.SetVector("_FogParameters", fogParameters);

            return true;
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (!SetMaterialUniforms())
                Graphics.Blit(source, destination);
            else
                Graphics.Blit(source, destination, material, 0);
        }

        private void UpdateDistanceFogTextures()
        {
            ColorSelectionType selectionType = distanceFogSettings.colorSelectionType;

            // If the gradient texture is not used, delete it.
            if (selectionType != ColorSelectionType.Gradient)
            {
                if (m_DistanceColorTexture != null)
                    DestroyImmediate(m_DistanceColorTexture);
                m_DistanceColorTexture = null;
            }

            if (distanceColorSource.m_GradientDirty && distanceFogSettings.colorSelectionType == ColorSelectionType.Gradient)
            {
                BakeFogColor(distanceColorTexture, distanceColorSource.gradient);
                m_DistanceColorSource.m_GradientDirty = false;
            }
        }

        private void UpdateHeightFogTextures()
        {
            ColorSelectionType selectionType = heightFogSettings.colorSelectionType;

            // If the gradient texture is not used, delete it.
            if (selectionType != ColorSelectionType.Gradient)
            {
                if (m_HeightColorTexture != null)
                    DestroyImmediate(m_HeightColorTexture);
                m_HeightColorTexture = null;
            }

            if (heightColorSource.m_GradientDirty && heightFogSettings.colorSelectionType == ColorSelectionType.Gradient)
            {
                BakeFogColor(heightColorTexture, heightColorSource.gradient);
                m_HeightColorSource.m_GradientDirty = false;
            }
        }

        private void BakeFogColor(Texture2D target, Gradient gradient)
        {
            if (target == null)
            {
                return;
            }

            if (target.height > 1)
            {
                Debug.LogWarning(m_EffectName + ": Baking color ramp of height more than 1.");
            }

            float fWidth = target.width;
            Color[] pixels = new Color[target.width];

            for (float i = 0f; i <= 1f; i += 1f / fWidth)
            {
                Color color = gradient.Evaluate(i);
                pixels[(int)Mathf.Floor(i * (fWidth - 1f))] = color;
            }

            target.SetPixels(pixels);
            target.Apply();
        }
        #endregion

    }
}
