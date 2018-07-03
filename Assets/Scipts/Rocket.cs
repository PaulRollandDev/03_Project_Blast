using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    enum State { Alive, Dying, Transcending }

    //State state = State.Alive;
    bool isTransitioning = false;
    
    [SerializeField] float rcsTrust = 100f;
    [SerializeField] float mainTrust = 100f;
    [SerializeField] float levelTimeDelay = 2f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip nextLevel;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] ParticleSystem nextLevelParticles;

    bool debugMode = false;

    Rigidbody rigidBody;
    AudioSource audioSource;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTransitioning)
        {
            RespondToThrustInput();
            RespondToRotate();
        }
        if (Debug.isDebugBuild)
        {
            DebugMode();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isTransitioning) { return; }

        switch (collision.gameObject.tag)
        {
            case "Friendly":

                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                if (debugMode == false)
                {
                    StartDeathSequence();
                } 
                 break;
        }
    }

    private void StartSuccessSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(nextLevel);
        nextLevelParticles.Play();
        Invoke("LoadNextLevel", levelTimeDelay);
    }

    private void StartDeathSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(death);
        deathParticles.Play();
        Invoke("LoadLevelOne", levelTimeDelay);
    }

    private void LoadNextLevel()
    {
        int maxLevels = SceneManager.sceneCountInBuildSettings;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == maxLevels)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex); //todo allow for more than 2 levels
    }

    private void LoadLevelOne()
    {

        SceneManager.LoadScene(0);
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            StopApplyingTrust();
        }
    }

    private void StopApplyingTrust()
    {
        audioSource.Stop();
        mainEngineParticles.Stop();
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainTrust * Time.deltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        mainEngineParticles.Play();
    }

    private void RespondToRotate()
    {
        rigidBody.angularVelocity = Vector3.zero; // remove rotation due to phsyics engine
        float rotationThisFrame = rcsTrust * Time.deltaTime;
    
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
    }

    void DebugMode()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
            Debug.Log("Debug Load Next level");
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            debugMode = !debugMode;
            print(debugMode);
        }
    }


 }