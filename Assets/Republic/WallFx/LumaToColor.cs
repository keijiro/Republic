using UnityEngine;

namespace Republic.WallFx
{
    class LumaToColor : MonoBehaviour
    {
        [SerializeField] Color _color1 = Color.black;
        [SerializeField] Color _color2 = Color.white;
        [SerializeField, Range(0, 1)] float _intensity = 1;

        public Color color1 { set { _color1 = value; } }
        public Color color2 { set { _color2 = value; } }
        public float intensity { set { _intensity = value; } }

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
            _material.SetColor("_Color1", _color1);
            _material.SetColor("_Color2", _color2);
            _material.SetFloat("_Blend", _intensity);
            Graphics.Blit(source, destination, _material, 0);
        }
    }
}
