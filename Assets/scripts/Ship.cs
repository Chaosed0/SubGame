using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Ship : MonoBehaviour {
    private float _depth = 0.0f;
    private float moveFactor = 0.0f;

    public float moveSpeed = 10.0f;
    public float maxDepth = 100.0f;

    public Level level;
    public Unit[] crew;

    [System.Serializable]
    public class StationEvent : UnityEvent<Unit, TileType> { };
    public StationEvent onStationEntered = new StationEvent();
    public StationEvent onStationExited = new StationEvent();

    public bool isMoving;
    public float shipStoppedBubbleSpeed;
    public float shipMovingBubbleSpeed;
    public GameObject[] bubbleParticles;

    public bool lostPower;

    //public bool movingDown;

    public float depth {
        get { return _depth; }
    }

	void Start () {
        for (int i = 0; i < crew.Length; i++) {
            Unit member = crew[i];
            Pathfinder pathfinder = member.GetComponent<Pathfinder>();
            pathfinder.onPathStarted.AddListener(() => OnPathStarted(member));
            pathfinder.onPathFinished.AddListener(() => OnPathFinished(member));
        }

        onStationEntered.AddListener(OnPilotStationEntered);
        onStationExited.AddListener(OnPilotStationExited);
	}

    void Update () {

        if (lostPower == false)
        {
            _depth += moveSpeed * moveFactor * Time.deltaTime;

            //if (movingDown == true)
            if (moveFactor > 0)
            {
                MoveShipDown(true);
            }
            else
            {
                MoveShipDown(false);
            }
        }
    }

    public void SetMoveFactor(float moveFactor) {
        this.moveFactor = moveFactor;
    }

    private void OnPathStarted(Unit unit) {
        // Unit is leaving a place, check if it's a station we need to know about
        Tile tile = level.TileAtWorldPosition(unit.transform.position);
        if (tile.tileType != TileType.Normal) {
            if (onStationExited != null) {
                onStationExited.Invoke(unit, tile.tileType);
            }
        }
    }

    private void OnPathFinished(Unit unit) {
        // Unit is entering a place, check if it's a station we need to know about
        Tile tile = level.TileAtWorldPosition(unit.transform.position);
        if (tile.tileType != TileType.Normal) {
            if (unit.IsOperating()) {
                if (onStationEntered != null) {
                    onStationEntered.Invoke(unit, tile.tileType);
                }
            } else {
                // The unit entered the station but he isn't operating it (probably repairing)
                if (onStationExited != null) {
                    onStationExited.Invoke(unit, tile.tileType);
                }
            }
        }
    }

    private void OnPilotStationEntered(Unit unit, TileType type) {
        if (type == TileType.Pilot) {
            moveFactor = 1.0f * unit.unitStats.Steering;
        }
    }

    private void OnPilotStationExited(Unit unit, TileType type) {
        if (type == TileType.Pilot) {
            moveFactor = 0.0f;
        }
    }

    ParticleSystem.Particle[] particles;


    public void MoveShipDown(bool sendMoving)
    {
        //transform.position = new Vector3(transform.position.x, transform.position.y - (moveAmount), transform.position.z);
        //level.transform.position = new Vector3(level.transform.position.x, level.transform.position.y - (moveAmount), level.transform.position.z);

        if (sendMoving == true)
        {
            if (isMoving == false)
            {
                for (int h = 0; h < bubbleParticles.Length; h++)
                {
                    ParticleSystem system = bubbleParticles[h].GetComponent<ParticleSystem>();

                    if (system != null)
                    {
                        if (particles == null || particles.Length - 1 != system.particleCount)
                            particles = new ParticleSystem.Particle[system.particleCount];
                        
                        system.GetParticles(particles);
                    }

                    //Do stuff to particles
                    for (int i = 0; i < particles.Length; i++)
                    {
                        particles[i].velocity = new Vector3(0, 0, shipMovingBubbleSpeed);
                    }
                        
                    system.SetParticles(particles, particles.Length);

                    bubbleParticles[h].GetComponent<ParticleSystem>().startSpeed = shipMovingBubbleSpeed;
                }

                /*for (int i = 0; i < bubbleParticles.Length; i++)
                {
                    bubbleParticles[i].GetComponent<ParticleSystem>().velocityOverLifetime = new Vector3(0, 0, 1);
                }*/

                isMoving = true;
            }
        }
        else
        {
            if (isMoving == true)
            {
                for (int h = 0; h < bubbleParticles.Length; h++)
                {
                    ParticleSystem system = bubbleParticles[h].GetComponent<ParticleSystem>();

                    if (system != null)
                    {
                        if (particles == null || particles.Length - 1 != system.particleCount)
                            particles = new ParticleSystem.Particle[system.particleCount];

                        system.GetParticles(particles);
                    }

                    //Do stuff to particles
                    for (int i = 0; i < particles.Length; i++)
                    {
                        particles[i].velocity = new Vector3(0, 0, shipStoppedBubbleSpeed);
                    }

                    system.SetParticles(particles, particles.Length);

                    bubbleParticles[h].GetComponent<ParticleSystem>().startSpeed = shipStoppedBubbleSpeed;
                }

                /*for (int i = 0; i < bubbleParticles.Length; i++)
                {
                    bubbleParticles[i].GetComponent<ParticleSystem>().velocityOverLifetime = new Vector3(0, 0, 0);
                }*/

                isMoving = false;
            }
        }

        //bubble changing here
       /* if (isMoving == false)
        {
            for (int i = 0; i < bubbleParticles.Length; i++)
            {
                bubbleParticles[i].GetComponent<ParticleSystem>().startSpeed = 1;
            }

            isMoving = true;
        }*/
    }

    public float GetDepthFraction() {
        return depth / maxDepth;
    }
}
