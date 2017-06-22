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

        [SerializeField, HideInInspector] Shader _effectShader;
        [SerializeField, HideInInspector] Shader _blendShader;

        Material _effectMaterial;
        Material _blendMaterial;

        void Start()
        {
            _effectMaterial = new Material(_effectShader);
            _blendMaterial = new Material(_blendShader);
        }

        void OnDestroy()
        {
            if (_effectMaterial != null) Destroy(_effectMaterial);
            if (_blendMaterial != null) Destroy(_blendMaterial);
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            var rtTemp = RenderTexture.GetTemporary(
                (int)_columns, (int)_rows, 0, RenderTextureFormat.R8
            );
            rtTemp.filterMode = FilterMode.Point;

            _effectMaterial.SetFloat("_Progress", _transition);
            _effectMaterial.SetInt("_RandomSeed", (int)(_randomSeed * 0x1000000));
            Graphics.Blit(null, rtTemp, _effectMaterial, 0);

            _blendMaterial.SetColor("_Color", _color);
            _blendMaterial.SetTexture("_MaskTex", rtTemp);
            Graphics.Blit(source, destination, _blendMaterial, 0);

            RenderTexture.ReleaseTemporary(rtTemp);
        }
    }
}
