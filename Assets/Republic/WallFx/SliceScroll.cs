using UnityEngine;

namespace Republic.WallFx
{
    class SliceScroll : MonoBehaviour
    {
        [SerializeField] Color _color = Color.white;
        [SerializeField, Range(1, 16)] float _density = 1;
        [SerializeField, Range(1, 512)] float _rows = 64;
        [SerializeField, Range(0, 16)] float _speed = 1;
        [SerializeField, Range(0, 1)] float _sliceLength = 0.5f;

        public Color color { set { _color = value; } }
        public float density { set { _density = value; } }
        public float rows { set { _rows = value; } }
        public float speed { set { _speed = value; } }
        public float sliceLength { set { _sliceLength = value; } }

        [SerializeField, HideInInspector] Shader _effectShader;
        [SerializeField, HideInInspector] Shader _blendShader;

        Material _effectMaterial;
        Material _blendMaterial;

        int _randomSeed;
        float _time;

        void Start()
        {
            _effectMaterial = new Material(_effectShader);
            _blendMaterial = new Material(_blendShader);
            _randomSeed = Random.Range(0, 0x1000000);
        }

        void OnDestroy()
        {
            if (_effectMaterial != null) Destroy(_effectMaterial);
            if (_blendMaterial != null) Destroy(_blendMaterial);
        }

        void Update()
        {
            _time += _speed * Time.deltaTime;
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            var rtTemp = RenderTexture.GetTemporary(
                source.width, (int)_rows, 0, RenderTextureFormat.R8
            );
            rtTemp.filterMode = FilterMode.Point;

            _effectMaterial.SetFloat("_Density", _density);
            _effectMaterial.SetInt("_RandomSeed", _randomSeed);
            _effectMaterial.SetFloat("_Progress", _time);
            _effectMaterial.SetFloat("_Threshold", _sliceLength);
            Graphics.Blit(null, rtTemp, _effectMaterial, 0);

            _blendMaterial.SetColor("_Color", _color);
            _blendMaterial.SetTexture("_MaskTex", rtTemp);
            Graphics.Blit(source, destination, _blendMaterial, 0);

            RenderTexture.ReleaseTemporary(rtTemp);
        }
    }
}
