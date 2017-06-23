using UnityEngine;

namespace Republic.WallFx
{
    class TiledCircle : MonoBehaviour
    {
        [SerializeField] Color _color = Color.white;
        [SerializeField, Range(1, 64)] float _columns = 16;
        [SerializeField, Range(1, 64)] float _rows = 9;
        [SerializeField, Range(1, 16)] float _repeat = 6;
        [SerializeField, Range(0, 1)] float _thickness = 0.5f;
        [SerializeField, Range(0, 1)] float _displace = 0;

        public Color color { set { _color = value; } }
        public float columns { set { _columns = value; } }
        public float rows { set { _rows = value; } }
        public float repeat { set { _repeat = value; } }
        public float thickness { set { _thickness = value; } }
        public float displace { set { _displace = value; } }

        public void Rehash()
        {
            _randomSeed = (int)(Random.value * 0x1000000);
        }

        [SerializeField, HideInInspector] Shader _shader;

        Material _material;
        int _randomSeed;

        void Start()
        {
            _material = new Material(_shader);
            Rehash();
        }

        void OnDestroy()
        {
            if (_material != null) Destroy(_material);
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            var aaf = source.height / (_repeat * 2 * _rows);
            _material.SetColor("_Color", _color);
            _material.SetVector("_Density", new Vector4(_columns, _rows));
            _material.SetFloat("_Repeat", _repeat);
            _material.SetFloat("_Thickness", _thickness);
            _material.SetFloat("_Displace", _displace);
            _material.SetFloat("_AAFactor", aaf);
            _material.SetInt("_RandomSeed", _randomSeed);
            Graphics.Blit(source, destination, _material, 0);
        }
    }
}
