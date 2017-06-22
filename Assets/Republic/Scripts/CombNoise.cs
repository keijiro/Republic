using UnityEngine;

namespace Republic.WallFx
{
    class CombNoise : MonoBehaviour
    {
        [SerializeField, ColorUsage(false)] Color _color = Color.white;
        [SerializeField, Range(1, 50)] float _density = 8;
        [SerializeField, Range(0, 10)] float _speed = 1;
        [SerializeField, Range(0, 1)] float _thickness = 0.05f;
        [SerializeField, Range(1, 20)] float _rowRepeat = 1;

        public float density { set { _density = value; } }
        public float speed { set { _speed = value; } }
        public float thickness { set { _thickness = value; } }
        public float rowRepeat { set { _rowRepeat = value; } }

        [SerializeField, HideInInspector] Shader _shader;

        Material _material;

        void Start()
        {
            _material = new Material(_shader);
        }

        void OnDestroy()
        {
            if (_material != null) Destroy(_material);
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            _material.SetColor("_Color", _color);
            _material.SetFloat("_Density", _density);
            _material.SetFloat("_Speed", _speed);
            _material.SetFloat("_Thickness", _thickness);
            _material.SetFloat("_RowRepeat", _rowRepeat);
            Graphics.Blit(source, destination, _material, 0);
        }
    }
}
