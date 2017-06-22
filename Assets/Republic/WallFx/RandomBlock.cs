using UnityEngine;

namespace Republic.WallFx
{
    class RandomBlock : MonoBehaviour
    {
        [SerializeField] Color _color = Color.white;
        [SerializeField, Range(1, 256)] float _columns = 64;
        [SerializeField, Range(1, 256)] float _rows = 64;
        [SerializeField, Range(0, 1)] float _transition = 0.5f;
        [SerializeField, Range(0, 1)] float _randomSeed = 0;

        public Color color { set { _color = value; } }
        public float columns { set { _columns = value; } }
        public float rows { set { _rows = value; } }
        public float transition { set { _transition = value; } }
        public float randomSeed { set { _randomSeed = value; } }

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
            _material.SetVector("_Dimension", new Vector4(_columns, _rows));
            _material.SetFloat("_Progress", _transition);
            _material.SetInt("_RandomSeed", (int)(_randomSeed * 0x1000000));
            Graphics.Blit(source, destination, _material, 0);
        }
    }
}
