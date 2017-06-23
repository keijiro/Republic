using UnityEngine;

namespace Republic
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class PostFx : MonoBehaviour
    {
        [SerializeField] Color _lineColor = Color.black;
        [SerializeField, ColorUsage(false)] Color _fillColor1 = Color.blue;
        [SerializeField, ColorUsage(false)] Color _fillColor2 = Color.red;
        [SerializeField, ColorUsage(false)] Color _fillColor3 = Color.white;
        [SerializeField, Range(0, 1)] float _lowerThreshold = 0;
        [SerializeField, Range(0, 1)] float _higherThreshold = 1;

        [SerializeField, HideInInspector] Shader _shader;

        Material _material;

        void OnValidate()
        {
            _lowerThreshold = Mathf.Min(_lowerThreshold, _higherThreshold);
        }

        void OnDestroy()
        {
            if (Application.isPlaying)
                Destroy(_material);
            else
                DestroyImmediate(_material);
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (_material == null)
            {
                _material = new Material(_shader);
                _material.hideFlags = HideFlags.DontSave;
            }

            _material.SetColor("_LineColor", _lineColor);
            _material.SetColor("_FillColor1", _fillColor1);
            _material.SetColor("_FillColor2", _fillColor2);
            _material.SetColor("_FillColor3", _fillColor3);

            _material.SetVector("_Threshold", new Vector2(
                _lowerThreshold, 1.0f / (_higherThreshold - _lowerThreshold)
            ));

            Graphics.Blit(source, destination, _material, 0);
        }
    }
}
