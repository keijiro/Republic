using UnityEngine;

namespace Republic.WallFx
{
    class DiagZoom : MonoBehaviour
    {
        [SerializeField] Color _color = Color.white;
        [SerializeField, Range(1, 64)] float _density = 16;
        [SerializeField, Range(0, 1)] float _thickness = 0.05f;
        [SerializeField, Range(-10, 10)] float _speed = 1;

        public Color color { set { _color = value; } }
        public float density { set { _density = value; } }
        public float thickness { set { _thickness = value; } }
        public float speed { set { _speed = value; } }

        [SerializeField, HideInInspector] Shader _shader;

        Material _material;
        float _time;

        void Start()
        {
            _material = new Material(_shader);
        }

        void OnDestroy()
        {
            if (_material != null) Destroy(_material);
        }

        void Update()
        {
            _time -= _speed * Time.deltaTime;
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            _material.SetColor("_Color", _color);
            _material.SetFloat("_Density", _density);
            _material.SetFloat("_Thickness", _thickness);
            _material.SetFloat("_Progress", _time);
            Graphics.Blit(source, destination, _material, 0);
        }
    }
}
