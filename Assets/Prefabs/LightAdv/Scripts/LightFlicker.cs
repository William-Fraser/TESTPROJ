using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    [Header("Target Light")]
    public Light targetLight;

    [Header("Abilities")]
    public bool trigger = false;
    public bool pulsate = false;
    public bool flicker = false;

    [Header("Trigger")]
    [Tooltip("Runs on Start. \nTurns off the MeshRenderer for the Trigger area that turns on the Light.")]
    public bool turnOffTriggerArea;
    
    [Header("Pulsate")]
    public float waitTime = .5f;
    
    [Header("Flicker")]
    [Range(0.5f, 25f)]
    public float flickerRate = 5;

    private bool activate;
    private bool deactivate;
    private bool activateWaitTimeFlicker;

    // Start is called before the first frame update
    void Start()
    {
        targetLight = targetLight.GetComponent<Light>();

        if (trigger == false) // disables trigger area
        {
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
        }
        else if (trigger)
        { 
            targetLight.enabled = !targetLight.enabled;
            
            if (turnOffTriggerArea)
            GetComponent<MeshRenderer>().enabled = false;
        }

        if (pulsate)
        {
            deactivate = true;
        }

        if (flicker)
        {
            activateWaitTimeFlicker = true;
            deactivate = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (activate)
        {
            targetLight.enabled = true;
            StartCoroutine("CountDownAndDeactivate");
            activate = false;
        }
        else if (deactivate)
        {
            targetLight.enabled = false;
            StartCoroutine("CountDownAndActivate");
            deactivate = false;
        }

        if (activateWaitTimeFlicker)
        {
            activateWaitTimeFlicker = false;
            StartCoroutine("RandomizeWaitTime");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        targetLight.enabled = true;   
    }
    private void OnTriggerExit(Collider other)
    {
        targetLight.enabled = false;
        
        if (pulsate || flicker)
        { 
            StopCoroutine("CountDownAndActivate");
            StopCoroutine("CountDownAndDeactivate");
        }
    }
    private IEnumerator CountDownAndActivate()
    {
        yield return new WaitForSeconds(waitTime);
        activate = true;
    }
    private IEnumerator CountDownAndDeactivate()
    {
        yield return new WaitForSeconds(waitTime);
        deactivate = true;
    }
    private IEnumerator RandomizeWaitTime()
    {
        yield return new WaitForSeconds(waitTime);
        waitTime = Random.Range(0.5f, flickerRate);
        activateWaitTimeFlicker = true;
    }
}
