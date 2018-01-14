using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public static Player Instance;

    [Range(0.01f, 1.0f)]
    [SerializeField] float _speed;
    [SerializeField] AnimationCurve _speedCurve;

    private Transform _transform;
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
        if (_currentAnchor > 0 && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            StartCoroutine( Move(GameManager.Instance.PlayerAnchors[_currentAnchor], GameManager.Instance.PlayerAnchors[--_currentAnchor]));
        }
        else if (_currentAnchor < (GameManager.Instance.PlayerAnchors.Count - 1) && Input.GetKeyDown(KeyCode.RightArrow))
        {
            StartCoroutine( Move(GameManager.Instance.PlayerAnchors[_currentAnchor], GameManager.Instance.PlayerAnchors[++_currentAnchor]));
        }
    }

    private IEnumerator Move( Transform origin, Transform destination)
    {
        float t = 0.0f;
        while ( t < _speed)
        {
            _transform.position = Vector3.Lerp( origin.position, destination.position, _speedCurve.Evaluate( t / _speed ));
            t += Time.deltaTime;
            yield return null;
        }
        _transform.position = destination.position;
    }
}
