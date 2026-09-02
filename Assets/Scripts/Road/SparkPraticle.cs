using System.Collections.Generic;
using UnityEngine;

public class SparkPraticle : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> _sparks;

    public void TurnOn()
    {
        foreach (ParticleSystem spark in _sparks)
        {
            spark.Play();
        }
    }

    public void TurnOff()
    {
        foreach (ParticleSystem spark in _sparks)
        {
            spark.Stop();
        }
    }
}