using UnityEngine;
using Klak.Math;

namespace Republic.WallFx
{
    class SimpleFill : MonoBehaviour
    {
        [SerializeField] Color _color = Color.white;
        [SerializeField, Range(1, 30)] float _noiseFrequency = 10;
        [SerializeField, Range(0, 2)] float _noiseAmplitude = 0;

        public Color color { set { _color = value; } }
        public float noiseFrequency { set { _noiseFrequency = value; } }
        public float noiseAmplitude { set { _noiseAmplitude = value; } }

        [SerializeField, HideInInspector] Shader _shader;

        NoiseGenerator _noise;
        Material _material;

        void Start()
        {
            _noise = new NoiseGenerator() {
                Frequency = _noiseFrequency,
                FractalLevel = 2
            };
            _material = new Material(_shader);
        }

        void Update()
        {
            _noise.Frequency = _noiseFrequency;
            _noise.Step();
        }

        void OnDestroy()
        {
            if (_material != null) Destroy(_material);
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            var c = _color;
            c.a *= Mathf.Lerp(1.0f, _noise.Value(1) + 0.5f, _noiseAmplitude);
            _material.SetColor("_Color", c);
            Graphics.Blit(source, destination, _material, 0);
        }
    }
}
