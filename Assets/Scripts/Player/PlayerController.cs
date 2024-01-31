using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rayqdr.CatInputs;
using System;

public class PlayerController : MonoBehaviour, IDamageable
{
    public static event Action<int,bool> OnChangeHealth;

    [SerializeField] private int _maxHealth = 3;
    [SerializeField] private ParticleSystem _glitterLoss;
    [SerializeField] private ParticleSystem _starsEmitter;
    [SerializeField] private Transform _catBody;

    public int MaxHealth { get { return _maxHealth; } }

    private MInputsAction _inputs;
    public MInputsAction Inputs {
        get 
        {
            if( _inputs == null)
            {
                _inputs = new MInputsAction();
                _inputs.Enable();
            }

            return _inputs;
        }
    }

    private Rigidbody _rb;
    private Animator _animator;
    private PlayerMovement _movement;

    private bool _isTransformed;
    public bool IsTransformed { get { return _isTransformed; } }
    private int _health = 3;
    private bool _isInvincible = false;
    

    private void Start()
    {
        if(_inputs == null)
        {
            _inputs = new MInputsAction();
            _inputs.Enable();
        }

        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _movement = GetComponent<PlayerMovement>();

        GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;
        GlitterManager.OnGlitterTresholdUpdate += GlitterManager_OnGlitterTresholdUpdate;
        GameManager.Instance.SetPlayer(this);

        _inputs.Player.Pause.performed += Pause_performed;
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if(GameManager.Instance.CurrentState != GameState.Pause)
            GameManager.Instance.ChangeGameState(GameState.Pause);
    }

    private void GlitterManager_OnGlitterTresholdUpdate(bool tresholdAchieved)
    {
        if(!_isTransformed && tresholdAchieved)
        {
            _isTransformed = true;
            //Change anims;
            _animator.SetTrigger("Transform");
            SoundManager.Instance.Play("LevelUp");
            _starsEmitter.Play();
            _catBody.localPosition = new Vector3(0, 0.25f, 0);
            _movement.ChangeMaxSpeed(true);
        }
        else if(_isTransformed && !tresholdAchieved)
        {
            _isTransformed = false;
            _starsEmitter.Stop();
            _catBody.localPosition = new Vector3(0, -0.25f, 0);
            _movement.ChangeMaxSpeed(false);
        }
    }

    private void GameManager_OnGameStateChanged(GameState newState)
    {
        switch (newState)
        {
            case GameState.MainMenu:
                break;
            case GameState.BeforeStartGame:
                _rb.isKinematic = true;
                GameManager.Instance.SetPlayer(this);
                break;
            case GameState.StartGame:
                _rb.isKinematic = false;
                break;
            case GameState.InGame:
                _rb.isKinematic = false;
                break;
            case GameState.GameOver:
                break;
            case GameState.Pause:
                break;
        }
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= GameManager_OnGameStateChanged;
        GlitterManager.OnGlitterTresholdUpdate -= GlitterManager_OnGlitterTresholdUpdate;
        _inputs.Player.Pause.performed -= Pause_performed;

    }

    public void TryTakeDamage(int damage, IDamageSource source)
    {
        if (_isInvincible)
            return;
        //Blowback in the air
        _rb.AddForce(((this.transform.position - source.Transform.position).normalized + Vector3.up) * 30.0f,ForceMode.Impulse);
        //Lose some glitter and health
        _health--;
        OnChangeHealth?.Invoke(_health,true);
        SoundManager.Instance.Play("Hurt");
        SoundManager.Instance.Play("Paf");
        _glitterLoss.Play();

        if(_health <= 0)
        {
            //dead
            GameManager.Instance.ChangeGameState(GameState.GameOver);
        }

        //Play invincible state
        StartCoroutine(InvincibleCoroutine());
        _animator.SetTrigger("Hurt");
    }

    private void Update()
    {
        _animator.SetBool("IsTransformed", _isTransformed);
    }

    private IEnumerator InvincibleCoroutine()
    {
        _isInvincible = true;
        //play fade anim
        _animator.SetLayerWeight(1, 1);
        yield return new WaitForSeconds(2.0f);
        _isInvincible = false;
        _animator.SetLayerWeight(1, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "End")
        {
            GameManager.Instance.ChangeGameState(GameState.Win);
        }
    }
}
