using System;
using UnityEngine;
using PixelCrew.Components;
using PixelCrew.Utils;
using UnityEditor.Animations;




namespace PixelCrew
{

    public class Hero : MonoBehaviour
    {

        [SerializeField] private float _speed; // Скорость
        [SerializeField] private float _jumpSpeed; //Сила прыжка
        [SerializeField] private float _damageSpeed; //Ускорение персонажа по Y при получение урона
        [SerializeField] private int _damage;
        [SerializeField] private LayerCheck _groundCheck; //Ссылка на слой
        [SerializeField] private float _interactionRadius; //Радиус взаимодействия с объектами
        [SerializeField] private LayerMask _interactionLayer; //Ссылка на слой объектов для взаимодействия
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private AnimatorController _armed;
        [SerializeField] private AnimatorController _disarmed;
        [SerializeField] private ParticleSystem _hitParticle;
        [SerializeField] private float _slamDawnVelocity;

        [SerializeField] private CheckCircleOverlap _attackRange;

        private bool _isArmed;
        

        [SerializeField] private SpawnComponent _footStepParticles;
        [SerializeField] private SpawnComponent _jumpDustParticles;
        [SerializeField] private SpawnComponent _fallDust;
        [SerializeField] private SpawnComponent _attackDust;


        private Collider2D[] _interactionResult = new Collider2D[1]; //Массив коллайдеров для взаимодействия

        private int _coins; //Значение собранных монет
        private float _flyingTime;


        private Vector2 _direction; // Направление
        private Rigidbody2D _rigidbody; // Ссылка на риджетбоди
        private Animator _animator; //Ссылка на аниматор
        private bool _isGrounded; //Переменная для  определения положения объекта на земле или нет
        private bool _allowDoubleJump; //Возможность совершить двойной прыжок

        private static readonly int IsGround = Animator.StringToHash("is-ground");
        private static readonly int IsRunning = Animator.StringToHash("is-running");
        private static readonly int IsFall = Animator.StringToHash("vertical-velocity");
        private static readonly int Hit = Animator.StringToHash("hit");
        private static readonly int AttackKey = Animator.StringToHash("attack");

        //Инициализирование ссылок
        private void Awake() 
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        //Получаем направление движения из компонента HeroInputReader
        public void SetDirection(Vector2 direction)
        {
            _direction = direction;
        }


        private void Update() 
        {
            _isGrounded = IsGrounded(); //Проверка на земле ли герой
        }

        private void FixedUpdate() 
        {
            var xVelocity = _direction.x * _speed;
            var yVelocity = CalculateYVelocity();
            _rigidbody.velocity = new Vector2(xVelocity, yVelocity);
            _flyingTime += Time.deltaTime;

            if(_isGrounded)
            {
                _flyingTime = 0;
            }


            //Запускаем аниматоры
            _animator.SetBool(IsGround, _isGrounded);
            _animator.SetFloat(IsFall, _rigidbody.velocity.y);
            _animator.SetBool(IsRunning, _direction.x != 0);


            UpdateSpriteDirection();
        }

        //Вычисление изменения кардинаты Y
        private float CalculateYVelocity()
        {
            var yVelocity = _rigidbody.velocity.y;
            var isJumping = _direction.y > 0;
            
            if(_isGrounded) _allowDoubleJump = true;

            if (isJumping)
            {
                yVelocity = CalculateJumpvelocity(yVelocity);
            } 
            else if(_rigidbody.velocity.y > 0)
            {
                yVelocity *= 0.5f;

            }

            return yVelocity;
        }

        //Сила прыжка
        private float CalculateJumpvelocity(float yVelocity)
        {
            var isFalling = _rigidbody.velocity.y <= 0.001f;
            if (!isFalling) return yVelocity;

            if(_isGrounded)
            {
                yVelocity += _jumpSpeed;
                _jumpDustParticles.Spawn();
            } else if (_allowDoubleJump)
            {
                yVelocity = _jumpSpeed;
                _jumpDustParticles.Spawn();
                _allowDoubleJump = false;
            }
            return yVelocity;
        }

        //Изменения спрайта при движение
        private void UpdateSpriteDirection()
        {
               if(_direction.x > 0)
            {
                transform.localScale = Vector3.one;

            }
            else if (_direction.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }

        //Проверка положения персонажа на земле ли он
        private bool IsGrounded()
        {
            return _groundCheck.IsTouchingLayer;
            //var hit = Physics2D.CircleCast(transform.position + _groundCheckPositionDelta, _groundCheckRadius, Vector2.down, 0, _groundLayer);
            //return hit.collider != null;
            
        }

       /* private void OnDrawGizmos() 
        {
            Gizmos.color = IsGrounded() ? Color.green : Color.red;
            Gizmos.DrawSphere(transform.position, 0.3f);
        }*/

        //
        public void SaySomething()
        {
            Debug.Log("Something!");
        }

        //Расчет изменения количества собранных монет
        public void GoldCoin()
        {
            _coins += 10;
            Debug.Log(_coins);
        }

        //Расчет изменения количества собранных монет
        public void SilverCoin()
        {
            _coins +=1;
            Debug.Log(_coins);
        }

        //Метод получения урона игроком
        public void TakeDamage()
        {
            _animator.SetTrigger(Hit);
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _damageSpeed);

            if(_coins > 0)
            {
            SpawnCoins();
            }
        }

        private void SpawnCoins()
        {
            var numCoinsToDispose = Math.Min(_coins, 5);
            _coins -= numCoinsToDispose;

            var burst = _hitParticle.emission.GetBurst(0);
            burst.count = numCoinsToDispose;
            _hitParticle.emission.SetBurst(0, burst);
            
            _hitParticle.gameObject.SetActive(true);
            _hitParticle.Play();
        }

        //Взаимодействие с объектами
        public void Interact()
        {
            var size = Physics2D.OverlapCircleNonAlloc(transform.position, _interactionRadius, _interactionResult, _interactionLayer);

            for (int i = 0; i < size; i++)
            {
                var interactable = _interactionResult[i].GetComponent<InteractableComponent>();
                if(interactable != null)
                {
                    interactable.Interact();
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D other) 
        {
            if (other.gameObject.IsInLayer(_groundLayer))
            {
                var contact = other.contacts[0];
                if(contact.relativeVelocity.y >= _slamDawnVelocity)
                {
                    _fallDust.Spawn();
                }
            }
        }


        public void SpawnFootDust()
        {
            // FindObjectOfType<AnimationComponent>().SetClip("run");
            _footStepParticles.Spawn();
        }


        public void Attack()
        {
            if(!_isArmed) return;
            _animator.SetTrigger(AttackKey);
            _attackDust.Spawn();

        }

        public void OnAttack()
        {
            var gos = _attackRange.GetObjectsInRange();
            foreach (var go in gos)
            {
                var hp = go.GetComponent<HealthComponent>();
                if(hp != null && go.CompareTag("Enemy"))
                {
                    hp.ApplyDamage(_damage);
                }
            }
        }

        public void ArmHero()
        {
            _isArmed = true;
            _animator.runtimeAnimatorController = _armed;
            
        }
    }
}

