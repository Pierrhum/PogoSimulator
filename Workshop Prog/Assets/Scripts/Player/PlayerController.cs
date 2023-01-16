using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
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
    [System.NonSerialized]
    public Camera Camera;

    private CameraController _cameraController;

    private float yaw = 0f;
    private float pitch = 0f;
    private Vector2 inputVector;

    private bool isPushing = false;
    private bool isStun = false;
    public bool CanPlay = false;

    public int Ragdolls = 3;
    private int RagdollCount = 0;
    public int KeyToGetUp = 5;
    private int GetUpKeyHit = 0;
    [NonSerialized] public int Score = 0;
    private void Awake()
    {
        Animator = GetComponentInChildren<Animator>();
        Camera = GetComponentInChildren<Camera>();
        _cameraController = Camera.gameObject.GetComponent<CameraController>();
        Rigidbody = GetComponent<Rigidbody>();
        CurrentLife = MaxLife;
        InitPos = transform.position;
        InitEul = transform.eulerAngles;
        
        DisableRagdolls();

        
        pitch = Camera.transform.eulerAngles.x;
        yaw = Camera.transform.eulerAngles.y;
    }

    private void Start()
    {
        HUD.instance.SetScore(Score);
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
                GameManager.instance.FreezePeople();
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
                GameManager.instance.UnFreezePeople();
            }
        }
    }

    private IEnumerator GetUpAnimCoroutine(float duration)
    {
        float timer = 0f;
        float start = Animator.GetFloat("GettingUpMotionTime");
        while (timer < duration)
        {
            float interpolation = Mathf.Lerp(start, (float)GetUpKeyHit / KeyToGetUp, timer / duration);
            PlayerHUD.SetGetUp(interpolation);
            Animator.SetFloat("GettingUpMotionTime", interpolation);
            timer += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
    private void Update()
    {
        if (CanPlay)
        {
            if (isStun)
            {
                // TODO : Check CameraController instead
                //Camera.transform.position = Mesh.position + (new Vector3(0,1,-1) * 2);
                //Camera.transform.LookAt(Mesh);
            }
            else
            {
                Animator.SetFloat("Life", CurrentLife / MaxLife);
                yaw += Input.GetAxis("Mouse X");
                pitch -= Input.GetAxis("Mouse Y");
                Camera.transform.eulerAngles = new Vector3(Mathf.Clamp(pitch, -50,50), Camera.transform.eulerAngles.y, Camera.transform.eulerAngles.z);
                transform.eulerAngles = new Vector3(0, yaw, 0);
                Rigidbody.velocity = transform.TransformDirection(new Vector3(inputVector.x, 0, inputVector.y));
            }
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
        if (!isStun)
        {
            CurrentLife -= damage;
            HUD.instance.UpdateHealthBar(CurrentLife / MaxLife);
            
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
                EnableRagdolls();
                isStun = true;
            }
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
}
