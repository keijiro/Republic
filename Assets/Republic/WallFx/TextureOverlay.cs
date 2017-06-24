using UnityEngine;

namespace Republic.WallFx
{
    class TextureOverlay : MonoBehaviour
    {
        [SerializeField] Texture [] _textures;

        [SerializeField] Color _color = Color.white;
        [SerializeField] int _textureIndex;

        public Color color { set { _color = value; } }
        public int textureIndex { set { _textureIndex = value; } }

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
            var i = Mathf.Clamp(_textureIndex, 0, _textures.Length);
            _material.SetColor("_Color", _color);
            _material.SetTexture("_OverlayTex", _textures[i]);
            Graphics.Blit(source, destination, _material, 0);
        }
    }
}
