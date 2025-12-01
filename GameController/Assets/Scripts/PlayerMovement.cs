using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    public CharacterController2D controller;
    public Animator animator;
    public PrefabWeapon playerWeapon;

    [Header("Movement Settings")]
    public float runSpeed = 40f;
    public bool useCursorInput = false;
    public bool useArduinoInput = false;    // ← mode baru

    [Header("Arduino Settings")]
    public ArduinoReader arduino;           // drag dari scene
    public float arduinoSensitivity = 0.1f; // ubah sesuai kebutuhan

    [Header("Cursor Control Settings")]
    public float deadZone = 1.5f;

    private float horizontalMove = 0f;
    private bool jump = false;
    private bool crouch = false;
    private float faceDir = 1f;

    void Update()
    {
        // 🎮 PILIH MODE INPUT
        if (useArduinoInput && arduino != null)
        {
            horizontalMove = GetArduinoInput() * runSpeed;
        }
        else if (useCursorInput)
        {
            horizontalMove = GetCursorHorizontalInput() * runSpeed;
        }
        else
        {
            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        }

        // Animasi kecepatan
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        // Jump (keyboard)
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("IsJumping", true);
        }

        // Jump (cursor mode)
        if (useCursorInput && Input.GetMouseButtonDown(1))
        {
            jump = true;
            animator.SetBool("IsJumping", true);
        }

        // Crouch
        if (Input.GetButtonDown("Crouch"))
            crouch = true;
        else if (Input.GetButtonUp("Crouch"))
            crouch = false;

        // Shoot
        if (Input.GetKeyDown(KeyCode.F) && playerWeapon != null)
            playerWeapon.Shoot();
    }

    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
    }

    public void OnCrouching(bool isCrouching)
    {
        animator.SetBool("IsCrouching", isCrouching);
    }

    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }

    // ============================================
    //                ARDUINO INPUT
    // ============================================
    private float GetArduinoInput()
    {
        float x = arduino.acceleration.x;

        // Threshold kecil biar ga goyang terus
        if (Mathf.Abs(x) < 0.1f)
            return 0f;

        // Sensitivitas
        return x * arduinoSensitivity;
    }

    // ============================================
    //             CURSOR INPUT (asli)
    // ============================================
    private float GetCursorHorizontalInput()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float diff = mouseWorldPos.x - transform.position.x;

        if (Mathf.Abs(diff) < deadZone)
        {
            FaceCursor(diff);
            return 0f;
        }

        FaceCursor(diff);
        return Mathf.Sign(diff);
    }

    private void FaceCursor(float diff)
    {
        if (diff > 0) faceDir = 1f;
        else if (diff < 0) faceDir = -1f;

        animator.SetFloat("FaceDir", faceDir);
    }

    private void OnDrawGizmosSelected()
    {
        if (!useCursorInput) return;

        Gizmos.color = new Color(1f, 0.8f, 0f, 0.3f);
        Gizmos.DrawCube(transform.position + Vector3.up * 1f, new Vector3(deadZone * 2, 2f, 0.1f));
    }
}
