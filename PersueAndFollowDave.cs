using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GLU.SteeringBehaviors;
using static UnityEngine.GraphicsBuffer;
using System;

[RequireComponent(typeof(Steering))]
public class PersueAndFollowDave : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private List<IBehavior> _behaviors;
    private Steering _steering;
    private bool _startPursue;
    void Start()
    {
        _steering = GetComponent<Steering>();
        _behaviors = new List<IBehavior>();
        //Player
        //_behaviors.Add(new Keyboard());
        //_behaviors.Add(new SeekClickPoint());
        //Basic
        //_behaviors.Add(new Seek(_player));
        //_behaviors.Add(new Flee(_player));
        //_behaviors.Add(new Arrive(_player));
        _behaviors.Add(new Pursue(_player));
        //_behaviors.Add(new Evade(_player));
        _behaviors.Add(new Wander(transform));
        //_behaviors.Add(new Idle());
        //_behaviors.Add(new Hide(_player));
        //Advanced
        _behaviors.Add(new FollowWall() { m_offset = 0.0f, m_angle = 0.0f, m_scale = 1.0f });
        _behaviors.Add(new AvoidObstacle() { m_offset = 0.0f, m_angle = 80.0f, m_scale = 0.1f });
        _behaviors.Add(new AvoidWall() { m_offset = 0.0f, m_angle = -80.0f, m_scale = 0.1f });
        _behaviors.Add(new AvoidWall() { m_offset = 0.0f, m_angle = 80.0f, m_scale = 0.1f });
        //_behaviors.Add(new Flock(gameObject));
        _behaviors.Add(new AvoidObstacle());
        //_behaviors.Add(new FollowPath());
        _steering.SetBehaviors(_behaviors);
    }
    private void Update()
    {
        if (Vector3.Distance(transform.position, _player.transform.position) < 3)
        {
            _behaviors.Clear();
            _behaviors.Add(new Wander(transform));
            _behaviors.Add(new AvoidObstacle());
            _behaviors.Add(new FollowWall() { m_offset = 0.0f, m_angle = 0.0f, m_scale = 1.0f });
        }
        if (!_startPursue & Vector3.Distance(transform.position, _player.transform.position) <= 5)
        {
            _startPursue = true;
            _behaviors.Clear();
            _behaviors.Add(new AvoidObstacle());
            _behaviors.Add(new FollowWall() { m_offset = 0.0f, m_angle = 0.0f, m_scale = 1.0f });
            _behaviors.Add(new Pursue(_player));
        }
        if (_startPursue & Vector3.Distance(transform.position, _player.transform.position) >= 7)
        {
            _startPursue = false;
            _behaviors.Clear();
            _behaviors.Add(new Wander(transform));
            _behaviors.Add(new AvoidObstacle());
            _behaviors.Add(new FollowWall() { m_offset = 0.0f, m_angle = 0.0f, m_scale = 1.0f });
        }
    }
}
