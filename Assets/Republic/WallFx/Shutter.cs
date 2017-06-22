using UnityEngine;

namespace Republic.WallFx
{
    class Shutter : MonoBehaviour
    {
        [SerializeField] Color _color = Color.white;
        [SerializeField, Range(1, 50)] float _columns = 4;
        [SerializeField, Range(1, 100)] float _rows = 4;
        [SerializeField, Range(10, 240)] float _bpm = 120;

        public Color color { set { _color = value; } }
        public float columns { set { _columns = value; } }
        public float rows { set { _rows = value; } }
        public float bpm { set { _bpm = value; } }

        [SerializeField, HideInInspector] Shader _shader;

        float _time;
        Material _material;

        void Start()
        {
            _time = Random.value * 100;
            _material = new Material(_shader);
        }

        void Update()
        {
            _time += _bpm / 60 * Time.deltaTime;
        }

        void OnDestroy()
        {
            if (_material != null) Destroy(_material);
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            _material.SetColor("_Color", _color);
            _material.SetVector("_Dimension", new Vector4(_columns, _rows));
            _material.SetFloat("_Progress", _time % 1);
            _material.SetInt("_Step", Mathf.FloorToInt(_time) * 400);
            Graphics.Blit(source, destination, _material, 0);
        }
    }
}
