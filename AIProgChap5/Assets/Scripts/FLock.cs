﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FLock : MonoBehaviour
{
    internal FlockController controller;

    private Rigidbody rigidBody;
    // Start is called before the first frame update
    void Start()
    {
        this.rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (controller)
        {
            Vector3 relativePos = steer() * Time.deltaTime;
            if (relativePos != Vector3.zero)
            {
                rigidBody.velocity = relativePos;

                //enforce minimum and maxium speeds of the boids
                float speed = rigidBody.velocity.magnitude;
                if (speed > controller.maxVelocity)
                {
                    GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized * controller.maxVelocity;
                }
                else if (speed < controller.minVelocity)
                {
                    GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized * controller.minVelocity;
                }
            }
        }
    }
        //Calculate flock steering Vector based on the Craig Reynold's algorithm (Cohesion, Alignment, Follow leader and Seperation)
        private Vector3 steer()
        {
            Vector3 center = controller.flockCenter - transform.localPosition;          // cohesion
            Vector3 velocity = controller.flockVelocity - GetComponent<Rigidbody>().velocity;           // alignment
            Vector3 follow = controller.target.localPosition - transform.localPosition; // follow leader
            Vector3 separation = Vector3.zero;                                          // separation

            foreach (FLock flock in controller.flockList)
            {
                if (flock != this)
                {
                    Vector3 relativePos = transform.localPosition - flock.transform.localPosition;
                    separation += relativePos / (relativePos.sqrMagnitude);
                }
            }

            // randomize
            Vector3 randomize = new Vector3((Random.value * 2) - 1, (Random.value * 2) - 1, (Random.value * 2) - 1);

            randomize.Normalize();

            return (controller.centerWeight * center +
                    controller.velocityWeight * velocity +
                    controller.separationWeight * separation +
                    controller.followWeight * follow +
                    controller.randomizeWeight * randomize);
        }
    }