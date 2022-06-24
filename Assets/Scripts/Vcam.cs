using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Vcam : MonoBehaviour
{
    private CinemachineBasicMultiChannelPerlin noise;
    private float shakeTimer;

    // Start is called before the first frame update
    void Start()
    {
        var virtualCamera = GetComponent<CinemachineVirtualCamera>();
        noise = virtualCamera
            .GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void Shake() {
        noise.m_AmplitudeGain = 2;
        shakeTimer = 0.5f;
    }

    public void StopShake() {
        shakeTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (shakeTimer > 0) {
            shakeTimer -= Time.deltaTime;
        } else {
            noise.m_AmplitudeGain = 0;
        }
    }
}
