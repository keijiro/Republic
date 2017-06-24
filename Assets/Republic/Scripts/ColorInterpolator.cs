using UnityEngine;

namespace Klak.Wiring
{
    [AddComponentMenu("Klak/Wiring/Filter/Color Interpolator")]
    public class ColorInterpolator : NodeBase
    {
        #region Editable properties

        [SerializeField, Range(0.1f, 50)]
        float _speed = 10;

        #endregion

        #region Node I/O

        [Inlet]
        public Color input
        {
            set
            {
                _target = value;

                if (!_initialized)
                {
                    _current = value;
                    _initialized = true;
                }
            }
        }

        [SerializeField, Outlet]
        ColorEvent _outputEvent = new ColorEvent();

        #endregion

        #region Private members

        Color _target;
        Color _current;
        bool _initialized;

        void Update()
        {
            if (!_initialized) return;

            var exp = Mathf.Exp(-_speed * Time.deltaTime);
            _current = Color.Lerp(_target, _current, exp);

            _outputEvent.Invoke(_current);
        }

        #endregion
    }
}
