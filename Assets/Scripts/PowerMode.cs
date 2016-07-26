using UnityEngine;
using System.Collections;

public class PowerMode : MonoBehaviour {

    [Header("Power Damage")]   
    [SerializeField] int powerStandard = 10;                           // The standard damage dealt by Power Shots.
    [SerializeField] int powerCharged = 10;                           // The charged damage dealt by Power Shots.
    [SerializeField] int powerFusion = 10;                           // The fusion strike damage dealt by Power Shots.

    [Header("Power Rapid & Charging")]
    [SerializeField] float powrRapidFireRate = 0;                         // Controls the rapid firing rate of Power Shots.
    [SerializeField] float powrFullyCharged = 0;                      // Controls the time needed to charge lvl2 Power Shots.
    [SerializeField] float powrSpiralCharged = 0;                      // Controls the time needed to charge lvl3 Power Shots.

    /* 
        // [SerializeField] specialProperty = Paralysis, Force, Pierce or NA
        // powrLevel1Special
        // powrLevel2Special
        // powrLevel3Special
        // shotSize = // May just be implemented easily using different prefab/sprite sizes.
    */
      
    void Start() { }
}
