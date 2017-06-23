using UnityEngine;

namespace Republic.WallFx
{
    class RDSystem : MonoBehaviour
    {
        [SerializeField] Color _color = Color.white;
        [SerializeField, Range(0, 0.1f)] float _seed = 0.05f;
        [SerializeField, Range(0, 1)] float _diffU = 1;
        [SerializeField, Range(0, 1)] float _diffV = 0.333f;
        [SerializeField, Range(0, 0.1f)] float _feed = 0.0547f;
        [SerializeField, Range(0, 0.1f)] float _kill = 0.0632f;
        [SerializeField, Range(0, 1)] float _threshold = 0.1f;
        [SerializeField, Range(2, 20)] int _stepCount = 4;

        public Color color { set { _color = value; } }
        public float seed { set { _seed = value; } }
        public float diffU { set { _diffU = value; } }
        public float diffV { set { _diffV = value; } }
        public float feed { set { _feed = value; } }
        public float kill { set { _kill = value; } }
        public float threshold { set { _threshold = value; } }

        public void Reset()
        {
            _resetRequest = true;
        }

        public void Rehash()
        {
            _rehashRequest = true;
        }

        [SerializeField, HideInInspector] Shader _shader;

        Material _material;
        RenderTexture _stateBuffer;

        bool _resetRequest;
        bool _rehashRequest;

        void Start()
        {
            _material = new Material(_shader);

            // We can't allocate the state buffer at this point because
            // the dimensions of the buffer hasn't been known yet.
        }

        void OnDestroy()
        {
            if (_material != null) Destroy(_material);
            if (_stateBuffer != null) Destroy(_stateBuffer);
        }

        void Update()
        {
            if (_stateBuffer == null) return;

            // Reset the state if requested.
            if (_resetRequest)
            {
                _material.SetFloat("_SeedProb", _seed);
                _material.SetInt("_RandomSeed", (int)(Random.value * 0x1000000));
                Graphics.Blit(null, _stateBuffer, _material, 0);
                _resetRequest = false;
            }

            // Temporary buffer used for doble buffering.
            var altBuffer = RenderTexture.GetTemporary(
                _stateBuffer.width, _stateBuffer.height, 0, _stateBuffer.format
            );

            // RD system parameters.
            _material.SetFloat("_Du", _diffU);
            _material.SetFloat("_Dv", _diffV);
            _material.SetFloat("_Feed", _feed);
            _material.SetFloat("_Kill", _kill);

            // Rehash the state if requested.
            if (_rehashRequest)
            {
                _material.SetFloat("_RehashProb", 0.5f);
                _material.SetInt("_RandomSeed", (int)(Random.value * 0x1000000));
                Graphics.Blit(_stateBuffer, altBuffer, _material, 1);
                Graphics.Blit(altBuffer, _stateBuffer, _material, 2);
                _rehashRequest = false;
            }

            // State update iteration.
            for (var i = 0; i < _stepCount / 2; i++)
            {
                Graphics.Blit(_stateBuffer, altBuffer, _material, 2);
                Graphics.Blit(altBuffer, _stateBuffer, _material, 2);
            }

            // Cleaning up.
            RenderTexture.ReleaseTemporary(altBuffer);
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (_stateBuffer == null)
            {
                _stateBuffer = new RenderTexture(
                    source.width / 4, source.height / 4, 0,
                    RenderTextureFormat.RGHalf
                );

                // Clear the state without seeding.
                _material.SetFloat("_SeedProb", 0);
                Graphics.Blit(null, _stateBuffer, _material, 0);
            }

            _material.SetTexture("_StateTex", _stateBuffer);
            _material.SetColor("_Color", _color);
            _material.SetFloat("_Threshold", _threshold);

            Graphics.Blit(source, destination, _material, 3);
        }
    }
}
