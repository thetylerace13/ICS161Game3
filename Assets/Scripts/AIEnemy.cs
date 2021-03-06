﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class AIEnemy : MonoBehaviour {

    // Use this for initialization

    public GameObject afterEffect;      // The after-effect to use
    private Rigidbody2D _rigidbody2D;   // The Rigidbody component attached
    private AudioSource _audioSource;  // The Audio Source component attached

    [Range(0.5f, 5)]
    public float _speed;

    private bool moving;
    private int start;
    private int destination;
    private List<int> visited;          // Tracks visited nodes
    private List<int> myPath;
    private int index;
    
	void Awake() {
        start = (int)transform.position.x;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
        GetComponent<AudioSource>().volume = GameController.volume;
        visited = new List<int>();
        index = 0;
        moving = false;
        _speed = ((float)GameController.difficulty + 2) / 4;
	}

    void Start ()
    {
        getLocations();
        myPath = Maze.instance.FindPathFast(start, destination);
    }

    // Update is called once per frame
    void Update () {
        if (index >= myPath.Count)
        {
            start = myPath[myPath.Count - 1];
            index = 0;
            myPath.Clear();
            getLocations();
            myPath = Maze.instance.FindPathFast(start, destination);
        }
        else if (!moving)
        {
            StartCoroutine(MoveTo(Maze.instance.PositionAt(myPath[index]), 1f / _speed));
        }

       /* GetComponent<AudioSource>().volume = GameController.volume;
        if (index >= myPath.Count)
        {
            index = 0;
            myPath.Clear();
            getLocations();
            myPath = Maze.instance.FindPathFast(start, destination);
        }
        else
        {
            Move();
        }*/
        //if (index == myPath.Count)
        //    MenuController.instance.GameLost();
	}
    void getLocations()
    {
        float minDistance = Mathf.Infinity;
        for (int i = EnemyController.instance.taken.Count - 1; i >= 1; --i)
        {
            int loc = EnemyController.instance.taken[i].y * GameController.width + EnemyController.instance.taken[i].x;
            if (!visited.Contains(loc) && Maze.instance.GetDistance(start, loc) < minDistance)
            {
                destination = loc;
                minDistance = Maze.instance.GetDistance(start, loc);
            }
        }
    }

    private void Move()
    {
        if (!moving)
        {
            StartCoroutine(MoveTo(Maze.instance.PositionAt(myPath[index]), 1f / _speed));
        }
    }
    private IEnumerator MoveTo(Vector2 destination, float time)
    {
        moving = true;
        Vector2 start = _rigidbody2D.position;
        for (float i = 0; i < time; i += Time.deltaTime)
        {
            _rigidbody2D.MovePosition(Vector2.Lerp(start, destination, i / time));
            yield return null;
        }
        _rigidbody2D.MovePosition(destination);
        ++index;
        visited.Add((int)destination.y * Maze._width + (int)destination.x);
        moving = false;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
    }
}
