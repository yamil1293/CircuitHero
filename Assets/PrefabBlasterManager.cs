using UnityEngine;
using System.Collections;

public class PrefabBlasterManager : MonoBehaviour {


    [Header("Power Mode Blaster Prefabs")]
    [SerializeField] Transform standardPMPrefab;                   // The standard damage dealt by Power Shots.
    [SerializeField] Transform chargingPMPrefab;                   // The standard damage dealt by Power Shots.
    [SerializeField] Transform chargedPMPrefab;                   // The charged damage dealt by Power Shots.
    [SerializeField] Transform fusionPMPrefab;                   // The fusion strike damage dealt by Power Shots.

    [Header("Magnetic Mode Blaster Prefabs")]
    [SerializeField] Transform standardMMPrefab;                // The standard damage dealt by Magnetic Shots.
    [SerializeField] Transform chargingMMPrefab;                // The charged damage dealt by Magnetic Shots.
    [SerializeField] Transform chargedMMPrefab;                // The fusion strike damage dealt by Magnetic Shots.
    [SerializeField] Transform fusionMMPrefab;                   // The fusion strike damage dealt by Power Shots.

    [Header("Thermal Mode Blaster Prefabs")]
    [SerializeField] Transform standardTMPrefab;                 // The standard damage dealt by Thermal Shots.
    [SerializeField] Transform chargingTMPrefab;                 // The charged damage dealt by Thermal Shots.
    [SerializeField] Transform chargedTMPrefab;                 // The fusion strike damage dealt by Thermal Shots.
    [SerializeField] Transform fusionTMPrefab;                   // The fusion strike damage dealt by Power Shots.

    [Header("Diffusion Mode Blaster Prefabs")]
    [SerializeField] Transform standardDMPrefab;               // The standard damage dealt by Diffusion Shots.
    [SerializeField] Transform chargingDMPrefab;               // The charged damage dealt by Diffusion Shots.
    [SerializeField] Transform chargedDMPrefab;               // The fusion strike damage dealt by Diffusion Shots.
    [SerializeField] Transform fusionDMPrefab;                   // The fusion strike damage dealt by Power Shots.  

    [Header("All Mode Blaster Prefabs")]
    [SerializeField] Transform spiralShotPrefab;                 // The damage dealt by the second charged Shot.   

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
