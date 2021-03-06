using UnityEngine;

namespace Republic.WallFx
{
    class SlidingLine : MonoBehaviour
    {
        [SerializeField] Color _color = Color.white;
        [SerializeField, Range(1, 50)] float _density = 8;
        [SerializeField, Range(1, 256)] float _rows = 1;
        [SerializeField, Range(0, 10)] float _speed = 1;
        [SerializeField, Range(0, 1)] float _thickness = 0.05f;

        public Color color { set { _color = value; } }
        public float density { set { _density = value; } }
        public float rows { set { _rows = value; } }
        public float speed { set { _speed = value; } }
        public float thickness { set { _thickness = value; } }

        [SerializeField, HideInInspector] Shader _effectShader;
        [SerializeField, HideInInspector] Shader _blendShader;

        Material _effectMaterial;
        Material _blendMaterial;
        float _time;

        void Start()
        {
            _effectMaterial = new Material(_effectShader);
            _blendMaterial = new Material(_blendShader);
            _time = Random.value * 100;
        }

        void Update()
        {
            _time += _speed * Time.deltaTime;
        }

        void OnDestroy()
        {
            if (_effectMaterial != null) Destroy(_effectMaterial);
            if (_blendMaterial != null) Destroy(_blendMaterial);
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            var rtTemp = RenderTexture.GetTemporary(
                source.width, (int)_rows, 0, RenderTextureFormat.R8
            );
            rtTemp.filterMode = FilterMode.Point;

            _effectMaterial.SetFloat("_Density", _density);
            _effectMaterial.SetFloat("_Offset", _time);
            _effectMaterial.SetFloat("_Thickness", _thickness);
            Graphics.Blit(null, rtTemp, _effectMaterial, 0);

            _blendMaterial.SetColor("_Color", _color);
            _blendMaterial.SetTexture("_MaskTex", rtTemp);
            Graphics.Blit(source, destination, _blendMaterial, 0);

            RenderTexture.ReleaseTemporary(rtTemp);
        }
    }
}
