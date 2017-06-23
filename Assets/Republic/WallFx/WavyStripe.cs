using UnityEngine;

namespace Republic.WallFx
{
    class WavyStripe : MonoBehaviour
    {
        [SerializeField] Color _color1 = Color.blue;
        [SerializeField] Color _color2 = Color.red;
        [Space]
        [SerializeField, Range(1, 64)] float _frequency = 8;
        [SerializeField, Range(1, 64)] float _rows = 10;
        [Space]
        [SerializeField, Range(-0.5f, 0.5f)] float _waveAmplitude = 0.2f;
        [SerializeField, Range(-5, 5)] float _waveScroll = 0.1f;
        [SerializeField, Range(-5, 5)] float _waveAnimation = 0.1f;
        [Space]
        [SerializeField, Range(-5, 5)] float _rotation = 0.1f;

        public Color color1 { set { _color1 = value; } }
        public Color color2 { set { _color2 = value; } }

        public float frequency { set { _frequency = value; } }
        public float rows { set { _rows = value; } }

        public float waveAmplitude { set { _waveAmplitude = value; } }
        public float waveScroll { set { _waveScroll = value; } }
        public float waveAnimation { set { _waveAnimation = value; } }

        public float rotation { set { _rotation = value; } }

        [SerializeField, HideInInspector] Shader _shader;

        Material _material;

        float _waveScrollTime;
        float _waveAnimationTime;
        float _rotationTime;

        Vector4 RotationMatrixAsVector4()
        {
            var s = Mathf.Sin(_rotationTime);
            var c = Mathf.Cos(_rotationTime);
            return new Vector4(c, -s, s, c);
        }

        void Start()
        {
            _material = new Material(_shader);
        }

        void Update()
        {
            var dt = Time.deltaTime;
            _waveScrollTime += _waveScroll * dt;
            _waveAnimationTime += _waveAnimation * dt;
            _rotationTime += _rotation * dt;
        }

        void OnDestroy()
        {
            if (_material != null) Destroy(_material);
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            var wamp = _waveAmplitude * Mathf.Sin(_waveAnimationTime);
            _material.SetColor("_Color1", _color1);
            _material.SetColor("_Color2", _color2);
            _material.SetFloat("_Frequency", _frequency);
            _material.SetFloat("_Rows", _rows);
            _material.SetFloat("_WaveScroll", _waveScrollTime);
            _material.SetFloat("_WaveAmplitude", wamp);
            _material.SetVector("_Rotation", RotationMatrixAsVector4());
            Graphics.Blit(source, destination, _material, 0);
        }
    }
}
