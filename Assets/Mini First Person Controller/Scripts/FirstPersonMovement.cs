using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    public float speed = 5;

    [Header("Running")]
    public bool canRun = true;
    public bool IsRunning { get; private set; }
    public float runSpeed = 9;
    public KeyCode runningKey = KeyCode.LeftShift;

    Rigidbody rigidbody;
    /// <summary> Functions to override movement speed. Will use the last added override. </summary>
    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();
    public Vector3 lastPosition;
    public AudioSource audioSource;
    public AudioSource audioSource3;
    public AudioClip audioSourceClip;
    public float pMin, pMax, vMin, vMax;
    void Awake()
    {
        // Get the rigidbody on this.
        rigidbody = GetComponent<Rigidbody>();
        lastPosition = transform.position;
    }

    void FixedUpdate()
    {
        // Update IsRunning from input.
        IsRunning = canRun && Input.GetKey(runningKey);

        // Get targetMovingSpeed.
        float targetMovingSpeed = IsRunning ? runSpeed : speed;
        if (speedOverrides.Count > 0)
        {
            targetMovingSpeed = speedOverrides[speedOverrides.Count - 1]();
        }

        // Get targetVelocity from input.
        Vector2 targetVelocity =new Vector2( Input.GetAxis("Horizontal") * targetMovingSpeed, Input.GetAxis("Vertical") * targetMovingSpeed);

        // Apply movement.
        rigidbody.velocity = transform.rotation * new Vector3(targetVelocity.x, rigidbody.velocity.y, targetVelocity.y);
    }

    void Update()
    {
        // If the character has moved and the audio is not currently playing
        if (transform.position != lastPosition && !audioSource.isPlaying)
        {
            // Randomly change the playback speed a small amount
            audioSource.pitch = Random.Range(pMin, pMax);
            // Randomly change the volume a small amount
            audioSource.volume = Random.Range(vMin, vMax);
            // Play the step sound
            audioSource.Play();
        }
        // Update lastPosition for the next frame
        lastPosition = transform.position;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "smallPowerup")
        {
            audioSource3.pitch = 6f;
            audioSource3.PlayOneShot(audioSourceClip, 1f);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "meduimPowerup")
        {
            audioSource3.pitch = 2.5f;
            audioSource3.PlayOneShot(audioSourceClip, 1f);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "largePowerup")
        {
            audioSource3.pitch = 1f;
            audioSource3.PlayOneShot(audioSourceClip, 1f);
            Destroy(collision.gameObject);
        }
    }
}