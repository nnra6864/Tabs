using System;
using UnityEngine;
using UnityEngine.Events;

namespace Tabs
{
    public class Tab : MonoBehaviour
    {
        [SerializeField] private bool _selectOnStart;
        [SerializeField, HideInInspector] protected Tabs _tabs;
        public UnityEvent OnSelected;
        
        private State _state;
        public event Action<State> OnStateChanged;

        public State State
        {
            get => _state;
            set
            {
                if (_state == value) return;
                _state = value;
                OnStateChanged?.Invoke(_state);
                if (_state == State.Selected) OnSelected?.Invoke();
            }
        }

        private bool _isHovered;
        private bool _isPressed;

        protected void OnEnable()
        {
            _tabs = GetComponentInParent<Tabs>();
            _tabs?.TabsList.Add(this);
        }

        protected virtual void Awake()
        {
            if (!_tabs)
            {
                Debug.LogWarning("Tab must be a child of an object with a Tabs component");
                gameObject.SetActive(false);
            }
        }

        protected virtual void Start()
        {
            if (_selectOnStart) Select();
        }

        protected virtual void Update()
        {
            if (_isPressed && Input.GetMouseButtonUp(0))
            {
                _isPressed = false;
                EvaluateState();
            }
        }

        public virtual void PointerEnter()
        {
            _isHovered = true;
            EvaluateState();
        }

        public virtual void PointerExit()
        {
            _isHovered = false;
            EvaluateState();
        }

        public virtual void PointerDown()
        {
            _isPressed = true;
            EvaluateState();
        }

        public virtual void PointerUp()
        {
            _isPressed = false;
            EvaluateState();
        }

        public virtual void PointerClick() => Select();

        private void EvaluateState() =>
            State = _tabs.CurrentTab == this ? State.Selected
                : _isPressed ? State.Pressed
                : _isHovered ? State.Hovered : State.None;

        public virtual void Select()
        {
            _tabs.CurrentTab = this;
            EvaluateState();
        }

        public virtual void Deselect()
        {
            EvaluateState();
        }
    }
}