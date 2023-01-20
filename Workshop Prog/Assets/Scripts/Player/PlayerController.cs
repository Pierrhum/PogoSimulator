using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    public Volume PostProcessVolume;
    public AudioLowPassFilter LowPass;
    public PlayerHUD PlayerHUD;
    public List<Collider> RagDollColliders;
    public List<Rigidbody> RagDollRigidbodies;
    public Transform Mesh;
    private Vector3 InitPos;
    private Vector3 InitEul;
    public float Speed = 5f;
    public float MaxLife = 100f;
    public float CurrentLife;
    private bool isAlive = true;
    private Animator Animator;
    private Rigidbody Rigidbody;
    private SkinnedMeshRenderer _renderer;
    
    public Camera Camera { get { return _cameraController._camera; } }
    public Camera FaceCamera { get { return PlayerHUD.FaceCamera; } }

    public CameraController _cameraController;
    private Vector2 inputVector;

    private bool isPushing = false;
    private bool isStun = false;
    private bool isInvicible = false;
    public bool CanPlay = false;

    public int Ragdolls = 3;
    private int RagdollCount = 0;
    public int KeyToGetUp = 5;
    private int GetUpKeyHit = 0;
    [NonSerialized] public int Score = 0;
    private void Awake()
    {
        Animator = GetComponentInChildren<Animator>();
        Rigidbody = GetComponent<Rigidbody>();
        _renderer = GetComponentInChildren<SkinnedMeshRenderer>();
        CurrentLife = MaxLife;
        InitPos = transform.position;
        InitEul = transform.eulerAngles;
        PostProcessVolume.weight = 0;
        
        DisableRagdolls();
    }

    private void Start()
    {
        HUD.instance.SetScore(Score);
        Camera.enabled = false;
        FaceCamera.enabled = false;
    }

    public void Reset()
    {
        isStun = false;
        Score = 0;
        HUD.instance.SetScore(Score);
        CurrentLife = MaxLife;
        isAlive = true;
        HUD.instance.UpdateHealthBar(CurrentLife / MaxLife);
        transform.position = InitPos;
        transform.eulerAngles = InitEul;
        _cameraController.SetCamera(CameraType.PLAYER);
        Animator.SetFloat("Life", CurrentLife / MaxLife);
        DisableRagdolls();
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (CanPlay)
        {
            inputVector = context.ReadValue<Vector2>() * 5;
            Animator.SetBool("isRunning", context.performed);
            Animator.SetFloat("Left", inputVector.x);
        }
    }
    
    
    public void Fire(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Animator.SetTrigger("isPushing");
        }
    }

    private Coroutine GetUpCoroutine;
    public void GetUp(InputAction.CallbackContext context)
    {
        if (context.started && isStun)
        {
            if (GetUpKeyHit == 0)
            {
                Animator.enabled = true;
                Animator.SetBool("isGettingUp", true);
                Animator.SetFloat("GettingUpMotionTime", 0f);
                transform.position = new Vector3(Mesh.transform.position.x, transform.position.y, Mesh.transform.position.z);
                //transform.eulerAngles = new Vector3(transform.eulerAngles.x, Mesh.eulerAngles.y, transform.eulerAngles.z);
                DisableRagdolls();
            }
            
            GetUpKeyHit++;
            if (GetUpCoroutine != null)
            {
                StopCoroutine(GetUpCoroutine);
                GetUpCoroutine = null;
            }
            GetUpCoroutine = StartCoroutine(GetUpAnimCoroutine(.5f));
            
            if (GetUpKeyHit == KeyToGetUp)
            {
                Animator.SetBool("isGettingUp", false);
                StopCoroutine(GetUpCoroutine);
                isStun = false;
                _cameraController.SetCamera(CameraType.PLAYER);
                GetUpKeyHit = 0;
                PlayerHUD.SetGetUp(1f);
            }
        }
    }

    private IEnumerator GetUpAnimCoroutine(float duration)
    {
        float timer = 0f;
        float start = Animator.GetFloat("GettingUpMotionTime");
        float targetFrequency = Mathf.Lerp(1000f, 12000f, (float)GetUpKeyHit / KeyToGetUp);
        float startFrequency = LowPass.cutoffFrequency;
        while (timer < duration)
        {
            float interpolation = Mathf.Lerp(start, (float)GetUpKeyHit / KeyToGetUp, timer / duration);
            PlayerHUD.SetGetUp(interpolation);
            Animator.SetFloat("GettingUpMotionTime", interpolation);
            LowPass.cutoffFrequency = Mathf.Lerp(startFrequency, targetFrequency, interpolation);
            
            timer += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
    private void Update()
    {
        if (CanPlay && !isStun)
        {
            _cameraController.UseCameraInputs();
            Rigidbody.velocity = transform.TransformDirection(new Vector3(inputVector.x, 0, inputVector.y));
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        PogoAI AI = other.GetComponent<PogoAI>();
        if (AI != null)
        {
            if (isPushing)
            {
                AI.EnableRagdoll();
                Score += AI.Score;
                HUD.instance.SetScore(Score);
            }
            else if(!isStun) Hurt(AI.Strength);
        }
    }

    private void Hurt(float damage)
    {
        if (!isStun && !isInvicible)
        {
            CurrentLife -= damage;
            HUD.instance.UpdateHealthBar(CurrentLife / MaxLife);
            Animator.SetFloat("Life", CurrentLife / MaxLife);
            
            // Death
            if (isAlive && CurrentLife <= 0)
            {
                isAlive = false;
                EnableRagdolls();
                GameManager.instance.EndGame();
            }
            // Ragdoll
            else if (CurrentLife < MaxLife - (MaxLife / (Ragdolls + 1)) * (RagdollCount+1))
            {
                
                RagdollCount++;
                LowPass.cutoffFrequency = 1000f;
                EnableRagdolls();
                isStun = true;
                PlayerHUD.KeySpam.SetActive(true);
            }
            else
                HurtFeeback();
            
            // Post Process
            if (CurrentLife < MaxLife / 4)
                StartCoroutine(PostProcessCoroutine(1f, 2f));
            else if(CurrentLife < MaxLife / 2)
                StartCoroutine(PostProcessCoroutine(0.5f, 4f));
        }
    }

    private void EnableRagdolls()
    {
        _cameraController.SetCamera(CameraType.RAGDOLL);
        RagDollColliders.ForEach(c => c.enabled = true);
        RagDollRigidbodies.ForEach(rb => rb.isKinematic = false);
        Animator.enabled = false;
    }

    private void DisableRagdolls()
    {
        Animator.enabled = true;
        RagDollColliders.ForEach(c => c.enabled = false);
        RagDollRigidbodies.ForEach(rb => rb.isKinematic = true);
    }

    private void EnablePush()
    {
        isPushing = true;
        GetComponent<Collider>().enabled = false;
        GetComponent<Collider>().enabled = true;
    }

    private void DisablePush()
    {
        isPushing = false;
    }

    private void HurtFeeback()
    {
        isInvicible = true;
        HUD.instance.HurtFeedback();
        StartCoroutine(BlinkMeshCoroutine(6));
    }

    private IEnumerator PostProcessCoroutine(float targetWeight, float duration)
    {
        float initWeight = PostProcessVolume.weight;
        float timer = 0f;
        while (timer < duration)
        {
            PostProcessVolume.weight = Mathf.Lerp(initWeight, targetWeight, timer / duration);
            timer += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    private IEnumerator BlinkMeshCoroutine(int blink)
    {
        int blinkCount = 0;
        Color MeshColor = _renderer.material.color;
        while (blinkCount < blink)
        {
            _renderer.enabled = false;
            yield return new WaitForSeconds(.15f);
            _renderer.enabled = true;
            yield return new WaitForSeconds(.15f);
            blinkCount++;
        }
        isInvicible = false;
    }
}
