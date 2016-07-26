using UnityEngine;
using System.Collections;

public class DiffusionMode : MonoBehaviour {

    [Header("Diffusion Damage")]
    [SerializeField] int diffusionStandard = 10;                       // The standard damage dealt by Diffusion Shots.
    [SerializeField] int diffusionCharged = 10;                       // The charged damage dealt by Diffusion Shots.
    [SerializeField] int diffusionFusion = 10;                       // The fusion strike damage dealt by Diffusion Shots.

    [Header("Diffusion Rapid & Charging")]
    [SerializeField] float diffRapidFireRate = 0;                         // Controls the rapid firing rate of Diffusion Shots.
    [SerializeField] float diffFullyCharged = 0;                      // Controls the time needed to charge lvl2 Diffusion Shots.
    [SerializeField] float diffSpiralCharged = 0;                      // Controls the time needed to charge lvl3 Diffusion Shots.

    /* 
        // [SerializeField] specialProperty = Paralysis, Force, Pierce or NA
        // diffLevel1Special
        // diffLevel2Special
        // diffLevel3Special
        // shotSize = // May just be implemented easily using different prefab/sprite sizes.
    */

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
