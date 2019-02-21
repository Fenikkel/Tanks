using UnityEngine;

public class ShellExplosion : MonoBehaviour
{
    public LayerMask m_TankMask;
    public ParticleSystem m_ExplosionParticles;       
    public AudioSource m_ExplosionAudio;              
    public float m_MaxDamage = 100f;                  
    public float m_ExplosionForce = 1000f;            
    public float m_MaxLifeTime = 2f;   //per si decas no xoca en un colider desitjat i no volem que estiga en la escena infinitament                
    public float m_ExplosionRadius = 5f;              


    private void Start()
    {
        Destroy(gameObject, m_MaxLifeTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        // Find all the tanks in an area around the shell and damage them.
        Collider[] colliders = Physics.OverlapSphere(transform.position, m_ExplosionRadius, m_TankMask); //despres de entrar en el trigger, mirem quins tanks que tenen el tankMask estan o toquen la esfera que esta posicionada al punt transform.position (on esta la shell) i amb un radi de m_ExplodeRadius

        for (int i =0; i < colliders.Length; i++ )
        {
            Rigidbody targetRidbody = colliders[i].GetComponent<Rigidbody>(); 

            if (!targetRidbody) //se fa per si decas, en teoria tots els tanks deurien tindre rigidbody
            {
                Debug.Log(targetRidbody); //te diu quin objecte no te rigidbody pero si la layer mask
                continue;
            }

            targetRidbody.AddExplosionForce(m_ExplosionForce, transform.position, m_ExplosionRadius);

            TankHealth targetHealth = targetRidbody.GetComponent<TankHealth>(); //targetRigitbody comparteix el mateix gameObject que el script, per aixo podem obtindre tankHealth desde el rigidbody

            float damage = CalculateDamage(targetRidbody.position);

            targetHealth.TakeDamage(damage);

            m_ExplosionParticles.transform.parent = null; //Aixina del desattachem del pare perque volem que es continue reproduint el so i les particules encara que destruim la bala. Sino destruiriem la bala i tots els seus fills

            m_ExplosionParticles.Play();

            m_ExplosionAudio.Play();

            Destroy(m_ExplosionParticles.gameObject, m_ExplosionParticles.main.duration); //destruirem el gameobject de les particules quan pase el temps de duracio de estes (que NO el lifetime de les particules)

            Destroy(gameObject);//destrueix la bala (recorda que ja no te com a fill el sistema de particules)
        }

    }


    private float CalculateDamage(Vector3 targetPosition)
    {
        // Calculate the amount of damage a target should take based on it's position.

        Vector3 explosionToTarget = targetPosition - transform.position; //creem un vector desde el target(tank), fins la bala utilitzant les seues dos posicions


        float explosionDistance = explosionToTarget.magnitude;//com de gran des eixe vector (algo entre 0 i el m_ExplosionRadius)

        //ara volem pasar eixa distancia de algo entre 0 i el m_ExplosionRadius a algo relatiu com entre 0 i 1

        float relativeDistance = (m_ExplosionRadius - explosionDistance) / m_ExplosionRadius; //si el objectiu esta lluny del centre de la bala, aço donara algo proxim a 0, mentres que si esta prop del centre de la bala, aço donara un valor proxim a 1

        float damage = relativeDistance * m_MaxDamage;

        damage = Mathf.Max(0f, damage); //per si hi ha damage negatiu, li posem 0 (no fa damage)
        //el damage negatiu seria en el cas de que el que el radi de explosio si que estiguera tocant el collider del tank pero el centre del tank estiguera fora. Per tant, la explosio soles afectara al tank si el seu centre esta dins del radi de explosio (overlap sphere)

        return damage;
    }
}