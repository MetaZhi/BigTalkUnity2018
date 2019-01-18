using UnityEngine;

public class ParticleAPI : MonoBehaviour
{
    ParticleSystem particle;

    void Start()
    {
        ParticleSystem.Particle[] array = new ParticleSystem.Particle[1000];
        particle.GetParticles(array, 1000, 500);

        particle.SetParticles(array, 1000, 1000);
    }
}
