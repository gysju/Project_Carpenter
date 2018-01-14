using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public static Player Instance;

    [Range(0.01f, 1.0f)]
    [SerializeField] float _moveSpeed = 1.0f;
    [SerializeField] AnimationCurve _speedCurve;

    [Space(10.0f)]
    [Range(0.01f, 2.0f)]
    [SerializeField] float _crouchDuration = 1.0f;
    [Range(0.01f, 1.0f)]
    [SerializeField] float _crouchSpeed = 1.0f;
    [SerializeField] AnimationCurve _scaleCurve;

    [Space(10.0f)]
    private Transform _transform;
    private bool _isCrounched = false;
    private int _currentAnchor = 0;

	void Awake ()
    {
        if (Instance == null)
        {
            Instance = this;
            _transform = transform;
        }
        else
        {
            Destroy(gameObject);
        }
	}

    public void InitPlayer(int originPos)
    {
        _currentAnchor = originPos;
        _transform.position = GameManager.Instance.PlayerAnchors[_currentAnchor].position;
    }

    void Update ()
    {
        if (!_isCrounched)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                StartCoroutine(Crouch(Vector3.one, new Vector3(1.0f, 0.3f, 1.0f)));
            }
            else if (_currentAnchor > 0 && Input.GetKeyDown(KeyCode.LeftArrow))
            {
                StartCoroutine(Move(GameManager.Instance.PlayerAnchors[_currentAnchor], GameManager.Instance.PlayerAnchors[--_currentAnchor]));
            }
            else if (_currentAnchor < (GameManager.Instance.PlayerAnchors.Count - 1) && Input.GetKeyDown(KeyCode.RightArrow))
            {
                StartCoroutine(Move(GameManager.Instance.PlayerAnchors[_currentAnchor], GameManager.Instance.PlayerAnchors[++_currentAnchor]));
            }
        }
    }

    private IEnumerator Move( Transform origin, Transform destination)
    {
        float t = 0.0f;
        while ( t < _moveSpeed)
        {
            _transform.position = Vector3.Lerp( origin.position, destination.position, _speedCurve.Evaluate( t / _moveSpeed));
            t += Time.deltaTime;
            yield return null;
        }
        _transform.position = destination.position;
    }

    private IEnumerator Crouch( Vector3 origin, Vector3 target)
    {
        _isCrounched = true;

        float t = 0.0f;
        while ( t < _crouchSpeed)
        {
            transform.localScale = Vector3.Lerp(origin, target, _scaleCurve.Evaluate( t / _crouchSpeed));
            t += Time.deltaTime;
            yield return null;
        }
        _transform.localScale = target;

        yield return new WaitForSeconds(_crouchDuration);

        t = 0.0f;
        while (t < _crouchSpeed)
        {
            transform.localScale = Vector3.Lerp(target, origin, _scaleCurve.Evaluate(t / _crouchSpeed));
            t += Time.deltaTime;
            yield return null;
        }
        _transform.localScale = origin;
        _isCrounched = false;
    }
}
