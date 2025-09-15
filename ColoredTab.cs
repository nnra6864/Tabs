using System;
using System.Collections;
using NnUtils.Modules.Easings;
using NnUtils.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Tabs
{
    public class ColoredTab : Tab
    {
        [Serializable]
        public class Transition
        {
            public Color Color;
            public float Time;
            public EasingType Easing;

            public Transition(Color color = default, float time = 0.5f, EasingType easing = EasingType.CubicOut)
            {
                Color = color;
                Time = time;
                Easing = easing;
            }
        }

        [SerializeField] private Graphic _graphic;

        [SerializeField]
        private Transition
            _noneTransition = new(new(0, 0, 0, 0.1f)),
            _hoveredTransition = new(new(0, 0, 0, 0.2f)),
            _pressedTransition = new(new(0, 0, 0, 0.3f)),
            _selectedTransition = new(new(0, 0, 0, 0.5f));

        private void Reset()
        {
            _graphic = GetComponent<Graphic>();
        }

        protected override void Awake()
        {
            base.Awake();
            OnStateChanged += HandleStateChanged;
        }

        private void HandleStateChanged(State state) =>
            this.RestartRoutine(ref _changeColorRoutine, ChangeColorRoutine());

        private Coroutine _changeColorRoutine;
        private IEnumerator ChangeColorRoutine()
        {
            if (!_graphic)
            {
                _changeColorRoutine = null;
                yield break;
            }

            var transition = State switch
            {
                State.Hovered => _hoveredTransition,
                State.Pressed => _pressedTransition,
                State.Selected => _selectedTransition,
                _ => _noneTransition
            };

            var startColor = _graphic.color;
            float lerpPos = 0;
            while (lerpPos < 1)
            {
                var t = Misc.Tween(ref lerpPos, transition.Time, transition.Easing, unscaled: true);
                _graphic.color = Color.Lerp(startColor, transition.Color, t);
                yield return null;
            }

            _changeColorRoutine = null;
        }
    }
}
