using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public static Player Instance;

    [Range(0.01f, 1.0f)]
    [SerializeField] float _speed;
    [SerializeField] AnimationCurve _speedCurve;

    private Transform _transform;
    private Spawner _currentLine;

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
	
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            StartCoroutine( Move(_transform.position, _transform.position - new Vector3(GameManager.Instance.SpaceBetweenToSpawn, 0.0f, 0.0f)));
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            StartCoroutine( Move(_transform.position, _transform.position + new Vector3(GameManager.Instance.SpaceBetweenToSpawn, 0.0f, 0.0f)));
        }
    }

    private IEnumerator Move( Vector3 origin, Vector3 destination)
    {
        float t = 0.0f;
        while ( t < _speed)
        {
            _transform.position = Vector3.Lerp( origin, destination, _speedCurve.Evaluate( t / _speed ));
            t += Time.deltaTime;
            yield return null;
        }
        _transform.position = destination;
    }
}
