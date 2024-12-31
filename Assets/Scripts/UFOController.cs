using UnityEngine;
using UnityEngine.UI;

public class UFOController : MonoBehaviour
{
    [SerializeField] private float _maxFuel = 100f;
    [SerializeField] private float _fuelConsumption = 5f;
    [SerializeField] private float _fuelOnAbduct = 10f;
    [SerializeField] private float _moveSpeed;

    [Space]

    [SerializeField] private float _abductSpeed = 5f;
    [SerializeField] private float _maxRayDistance = 10f;

    [Space]

    [SerializeField] private Slider _fuelSlider;
    [SerializeField] private Light _spotlight;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private FixedJoystick _joystick;
    [SerializeField] private Animator _UFOAnimator;
    [SerializeField] private ParticleSystem _UFOCrashParticles;

    private float _currentFuel;
    private Transform _sheep;
    private bool _sheepHit = false;

    private const string _UFOReset = "UFOReset";
    private const string _UFOCrash = "UFOCrash";

    private void Start()
    {
        LevelController.Instance.GameReset.AddListener(ResetStuff);
        ResetStuff();
    }

    private void FixedUpdate()
    {
        if (LevelController.Instance.GamePlaying)
            _rb.velocity = new Vector3(_joystick.Horizontal * _moveSpeed, _rb.velocity.y, _joystick.Vertical * _moveSpeed);
    }

    public void OnCapture()
    {
        if (_sheepHit || !LevelController.Instance.GamePlaying)
            return;

        RaycastHit hit;
        Ray ray = new(_spotlight.transform.position, _spotlight.transform.forward);

        if (Physics.Raycast(ray, out hit, _maxRayDistance))
        {
            if (hit.collider.CompareTag("Sheep"))
            {
                _sheep = hit.collider.transform;
                _sheep.parent = gameObject.transform;
                _sheepHit = true;
            }
        }
    }

    private void Update()
    {
        if (!LevelController.Instance.GamePlaying)
            return;

        _currentFuel -= _fuelConsumption * Time.deltaTime;
        _fuelSlider.value = _currentFuel;
        if (_currentFuel <= 0)
        {
            LevelController.Instance.OnGameOver();
            _UFOAnimator.Play(_UFOCrash);
            _UFOCrashParticles.Play();
        }

        if (_sheepHit)
        {
            Debug.Log(_sheep.position.y);
            _sheep.position += _abductSpeed * Time.deltaTime * Vector3.up;
            if (_sheep.position.y > transform.position.y) // relative to UFO position as parent
            {
                _sheepHit = false;
                LevelController.Instance.AddSheep();
                _currentFuel = Mathf.Min(_currentFuel + _fuelOnAbduct, _maxFuel);
                _fuelSlider.value = _currentFuel;
                Destroy(_sheep.gameObject);
            }
        }
    }

    private void ResetStuff()
    {
        _UFOAnimator.Play(_UFOReset);
        _UFOCrashParticles.Stop();
        _fuelSlider.maxValue = _maxFuel;
        _fuelSlider.value = _maxFuel;
        _currentFuel = _maxFuel;
    }
}
