using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{   
    [RequireComponent(typeof(SpriteRenderer))]

    public class AnimationComponent : MonoBehaviour
    {

        [SerializeField] [Range(1, 30)] private int _frameRate = 10; //Максимальное значение фрейм рейта, по дефолту 10
        [SerializeField] private UnityEvent<string> _onComplete; //Запуск ивента
        [SerializeField] private AnimationClip[] _clips; //Массив клипов

        private SpriteRenderer _renderer; //Ссылка на спрайт рендерер

        private float _secPerFrame; 
        private float _nextFrameTime;
        private int _currentFrame;
        private bool _isPlaying = true;


        private int _currentClip;

        private void Start() {
            
                _renderer = GetComponent<SpriteRenderer>();
                _secPerFrame = 1f/_frameRate;

                StartAnimation();
            
        }

        private void OnBecameVisible() {
            enabled = _isPlaying;
        }

        private void OnBecameInvisible() {
            enabled = false;
        }

        public void SetClip(string clipName)
        {
            for (var i = 0; i < _clips.Length; i++)
            {
                if (_clips[i].Name == clipName)
                {
                    _currentClip = i;
                    StartAnimation();
                    return;
                }
            }

            enabled = _isPlaying = false;
        }

        private void StartAnimation()
        {
            _nextFrameTime = Time.time + _secPerFrame;
            _isPlaying = true;
            _currentFrame = 0;
        }

        private void OnEnable()
        {
            _nextFrameTime = Time.time + _secPerFrame;
        }

        private void Update() 
        {
            if (_nextFrameTime > Time.time) return;

            var clip = _clips[_currentClip];
            if (_currentFrame >= clip.Sprites.Length)
            {
                if (clip.Loop)
                {
                    _currentFrame = 0;
                }
                else
                {
                    clip.OnComplete?.Invoke();
                    _onComplete?.Invoke(clip.Name);
                    enabled = _isPlaying = clip.AllowNextClip;
                    if (clip.AllowNextClip)
                    {
                        _currentFrame = 0;
                        _currentClip = (int) Mathf.Repeat(_currentClip + 1, _clips.Length);
                    }
                }
                return;
            }


            _renderer.sprite = clip.Sprites[_currentFrame];

            _nextFrameTime += _secPerFrame;
            _currentFrame++;
        }

        
        [Serializable]
        public class AnimationClip
        {
            [SerializeField] private string _name;
            [SerializeField] private Sprite[] _sprites;
            [SerializeField] private bool _loop;
            [SerializeField] private bool _allowNextClip;
            [SerializeField] private UnityEvent _onComplete;

            public string Name => _name;
            public Sprite[] Sprites => _sprites;
            public bool Loop => _loop;
            public bool AllowNextClip => _allowNextClip;
            public UnityEvent OnComplete => _onComplete;
        }
    }
}