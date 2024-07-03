using System.Collections;
using System.Collections.Generic;
using GLU.SteeringBehaviors;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(Steering))]
public class SeekBrain : MonoBehaviour
{
    [SerializeField] private GameObject target; // Het doelwit waar naartoe gestuurd wordt

    // Wordt aangeroepen bij de start van het script
    void Start()
    {
        // Haalt de Steering component op 
        Steering steering = GetComponent<Steering>();

        // Maakt een lijst van behaviors aan
        List<IBehavior> behaviors = new List<IBehavior>();

        // Voegt het Seek gedrag toe aan de lijst en geef het het opgegeven doelwit
        behaviors.Add(new Seek(target));

        // Stelt de lijst van behaviors in op de Steering component
        steering.SetBehaviors(behaviors);
    }
}
