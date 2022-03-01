using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private int ShotDmg = 1;
    public float FireRate = .25f;
    private float Range = 50;
    public float Impact = 100;

    public Transform GunEnd;

    public Camera fpsCamera;
    private WaitForSeconds ShotDuration = new WaitForSeconds(.07f);
    private LineRenderer laserLine;
    private float nextFire;
    private void Start()
    {
        laserLine = GetComponent<LineRenderer>();
        //fpsCamera = GetComponentInParent<Camera>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time > nextFire)
        {
            nextFire = Time.time + FireRate;
            StartCoroutine(ShotEffect());

            Vector3 rayOrigin = fpsCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
            RaycastHit hit;
            laserLine.SetPosition(0, GunEnd.position);
            if (Physics.Raycast(rayOrigin, fpsCamera.transform.forward, out hit, Range))
            {
                laserLine.SetPosition(1,hit.point);
            }
            else
            {
                laserLine.SetPosition(1,rayOrigin+(fpsCamera.transform.forward*Range));
            }
        }
    }

    IEnumerator ShotEffect()
    {
        laserLine.enabled = true;
        yield return ShotDuration;
        laserLine.enabled = false;
    }
    
}
