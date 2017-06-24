using UnityEngine;
using UnityEngine.Rendering;

namespace Republic
{
    [ExecuteInEditMode]
    public class ScreenBlit : MonoBehaviour
    {
        [SerializeField] RenderTexture _source;

        [SerializeField, Range(0, 1)] float _intensity = 1;
        [SerializeField, Range(0, 1)] float _invert = 0;
        [SerializeField] Color _overlayColor = new Color(0, 0, 0, 0);

        public float intensity { set { _intensity = value; } }
        public float invert { set { _invert = value; } }
        public Color overlayColor { set { _overlayColor = value; } }

        [SerializeField, HideInInspector] Shader _shader;

        Material _material;

        void OnDestroy()
        {
            if (_material != null)
            {
                if (Application.isPlaying)
                    Destroy(_material);
                else
                    DestroyImmediate(_material);
            }
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (_material == null)
            {
                _material = new Material(_shader);
                _material.hideFlags = HideFlags.DontSave;
            }

            _material.SetFloat("_Invert", _invert);
            _material.SetColor("_Color", _overlayColor);
            _material.SetFloat("_Intensity", _intensity);

            Graphics.Blit(_source, destination, _material, 0);
        }
    }
}
