using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Transform FirePolong;
    [SerializeField] private GameObject BulletPrefab;
    private PlayerInputs inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputs();
        inputActions.PlayerInputActions.Enable();
    }

    // Update is called once per frame
    private void Update()
    {
        
            if (inputActions.PlayerInputActions.Shoot.triggered == true) {
                Instantiate(BulletPrefab, FirePolong.position, Quaternion.Euler(0, 0, 90));
                
                
            }
        
        
    }}
    
