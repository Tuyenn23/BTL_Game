using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExampleArmy : MonoBehaviour {
    public FormationBase _formation;

    public FormationBase Formation
    {
        get
        {
            if (_formation == null) _formation = GetComponent<FormationBase>();
            return _formation;
        }
        set => _formation = value;
    }

    [SerializeField] private GameObject _unitPrefab;
    [SerializeField] private GameObject _unitPrefab1;
    
    [SerializeField] private float _unitSpeed;

    public List<GameObject> _spawnedUnits = new List<GameObject>();
    public List<Vector3> _points = new List<Vector3>();
    private Transform _parent;

    public float spawnDelay = 2.0f;
    private float lastSpawnTime = 0.0f;

    private void Start()
    {
        lastSpawnTime = Time.time;
    }

    private void Awake()
    {
        _parent = new GameObject("Unit Parent").transform;
    }

    private void Update()
    {
        SpawnObj();
        SetFormation();
    }

    public void SetFormation()
    {
        for (var i = 0; i < _spawnedUnits.Count; i++)
        {
            _spawnedUnits[i].transform.position = Vector3.MoveTowards(_spawnedUnits[i].transform.position, transform.position + _points[i], _unitSpeed * Time.deltaTime);
        }
    }

    private void SpawnObj()
    {
        _points = Formation.EvaluatePoints().ToList();

        if (_points.Count > _spawnedUnits.Count)
        {
            var remainingPoints = _points.Skip(_spawnedUnits.Count);
            Spawn(remainingPoints);
        }
        else if (_points.Count < _spawnedUnits.Count)
        {
            Kill(_spawnedUnits.Count - _points.Count);
        }
    }

    private void Spawn(IEnumerable<Vector3> points)
    {
        foreach (var pos in points)
        {
            if (Time.time - lastSpawnTime >= spawnDelay)
            {
                var unit = Instantiate(_unitPrefab, transform.position + pos, Quaternion.identity, _parent);
                var unit1 = Instantiate(_unitPrefab1, transform.position + pos, Quaternion.identity, _parent);
                
                _spawnedUnits.Add(unit);
                _spawnedUnits.Add(unit1);
                
                lastSpawnTime = Time.time;
            }
        }
    }

    private void Kill(int num)
    {
        for (var i = 0; i < num; i++)
        {
            var unit = _spawnedUnits.Last();
            _spawnedUnits.Remove(unit);
            Destroy(unit.gameObject);
        }
    }
}