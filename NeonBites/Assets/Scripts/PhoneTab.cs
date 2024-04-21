using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.ReloadAttribute;

public class PhoneTab : MonoBehaviour
{
    public GameObject phonePanel;

    public GameObject player;

    private AudioSource myAudioSource;

    private AudioSource[] allAudioSources;

    public AudioClip phoneOut;

    public AudioClip pauseClip;
    public AudioClip resumeClip;

    public Transform packageTransform;

    public GameObject notificationP;
    public TextMeshProUGUI notificationTxt;
    public Transform phoneTransform; // Assign the transform of the phone UI


    public TextMeshProUGUI currenttaskTxt;

    private bool phoneActive = false;
    private bool transition = false;


    public GameObject pauseMenu;  // Assign the Pause Menu panel in the Inspector
    private bool isPaused = false;


    void Start()
    {
        // Get the AudioSource component attached to this GameObject
        myAudioSource = GetComponent<AudioSource>();
        allAudioSources = FindObjectsOfType<AudioSource>();


        phoneTransform = phonePanel.transform;

        // Get the current scene's name
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Check if the current scene's name is equal to the desired scene name
        if (currentSceneName == "Minimaps")
        {
            // Set the time scale to 1
            Time.timeScale = 1f;

            // Lock and hide the cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        // Check if the Tab key is pressed
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!player.GetComponent<Attach2Bike>().isAttachedToBike && player.GetComponent<PlayerNavMesh>().currentPackage == null && !transition && !isPaused)
            {
                PhoneTabAct();
            }

        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void PhoneTabAct()
    {
        if(phoneActive)
        {
            phoneActive = false;
        }
        else phoneActive = true;

        // Toggle the active state of the object
        //phonePanel.SetActive(!phonePanel.activeSelf);

        // If the object is active, show the cursor
        if (phoneActive)
        {
            phonePanel.SetActive(true);

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None; // Unlock cursor movement

            player.GetComponent<PlayerInput>().enabled = false;
            player.GetComponent<ThirdPersonController>().enabled = false;

            // Start the rotation animation coroutine
            StartCoroutine(RotatePhoneToZero(Quaternion.Euler(0,90,0), Quaternion.Euler(0,0,0)));

            StartCoroutine(PlayForLimitedTime(1.0f));

            //player.GetComponent<Animator>().Play(Idle);

        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked; // Lock cursor movement

            // Start the rotation animation coroutine
            StartCoroutine(RotatePhoneToZero(Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, -90, 0)));


            player.GetComponent<PlayerInput>().enabled = true;
            player.GetComponent<ThirdPersonController>().enabled = true;

            StartCoroutine(DeactivateCoroutine(phonePanel));


        }
    }

    // Function to play the AudioClip one time
    public void PlayOneTime(AudioClip clip)
    {
        // Set the AudioClip to play
        myAudioSource.clip = clip;

        // Play the AudioClip
        myAudioSource.Play();

    }

    public void SetPackage(Transform packageTr)
    {
        packageTransform = packageTr;
    }

    public void AssignPackage()
    {
        player.GetComponent<PlayerNavMesh>().GenerateRouteToPackage(packageTransform);

        notificationP.SetActive(true);
        notificationTxt.text = "Delivery Assigned!";


        currenttaskTxt.text = "Pickup order from location";

        StartCoroutine(DeactivateCoroutine(notificationP));
    }

    IEnumerator DeactivateCoroutine(GameObject deactivateGO)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(2f);

        // Deactivate the GameObject
        deactivateGO.SetActive(false);
    }

    IEnumerator PlayForLimitedTime(float duration)
    {
        myAudioSource.clip = phoneOut;
        myAudioSource.Play();
        yield return new WaitForSeconds(duration);
        myAudioSource.Stop();
    }

    IEnumerator RotatePhoneToZero(Quaternion startRotation, Quaternion endRotation)
    {
        float duration = 0.5f; // Duration in seconds
        float time = 0;

        transition = true;

        while (time < duration)
        {
            // Increment the time step per frame
            time += Time.deltaTime;
            // Calculate the rotation for this frame
            phoneTransform.rotation = Quaternion.Lerp(startRotation, endRotation, time / duration);
            yield return null; // Wait until the next frame
        }

        transition = false;
        // Ensure the rotation is set exactly to the final target rotation
        phoneTransform.rotation = endRotation;
    }

    /// <summary>
    /// //////////////////////////////////////////////   PAUSE MENU FUNCTIONS \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    /// </summary>

    public void TogglePause()
    {
        isPaused = !isPaused;
        pauseMenu.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : 1;

        if (isPaused)
        {

            PlayOneTime(pauseClip);
            DisablePlayerInputs();
            PauseAllAudioExceptMine();
        }
        else
        {
            PlayOneTime(resumeClip);
            EnablePlayerInputs();
            ResumeAllAudio();
        }
    }

    private void DisablePlayerInputs()
    {
        // Disable player inputs. You need to implement this according to your game's input handling.
        // For example:
        // player.GetComponent<PlayerController>().enabled = false;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None; // Unlock cursor movement

        player.GetComponent<PlayerInput>().enabled = false;
        player.GetComponent<ThirdPersonController>().enabled = false;

    }

    private void EnablePlayerInputs()
    {
        // Enable player inputs. Implement this according to your game's input handling.
        // For example:
        // player.GetComponent<PlayerController>().enabled = true;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked; // Unlock cursor movement

        player.GetComponent<PlayerInput>().enabled = true;
        player.GetComponent<ThirdPersonController>().enabled = true;

    }

    private void PauseAllAudioExceptMine()
    {
        foreach (var source in allAudioSources)
        {
            if (source != myAudioSource && source.isPlaying)
            {
                source.Pause();  // Pause all audio sources except the one attached to this GameObject
            }
        }
    }

    private void ResumeAllAudio()
    {
        foreach (var source in allAudioSources)
        {
            if (source != myAudioSource)
            {
                source.UnPause();  // Resume all audio sources that were paused
            }
        }
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
				Application.Quit();
#endif
    }

    // Function to load a scene by name
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void SetGOActive(GameObject gameObject)
    {
        if(gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
        else gameObject.SetActive(true);
    }

}
