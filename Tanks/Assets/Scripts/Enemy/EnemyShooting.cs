using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyShooting : MonoBehaviour
{
    public int m_PlayerNumber = 1;
    public Rigidbody m_Shell;
    public Transform m_FireTransform;
    public Slider m_AimSlider;
    public AudioSource m_ShootingAudio;
    public AudioClip m_ChargingClip;
    public AudioClip m_FireClip;
    public float m_MinLaunchForce = 15f;
    public float m_MaxLaunchForce = 30f;
    public float m_MaxChargeTime = 0.75f;


    //private string m_FireButton;
    private float m_CurrentLaunchForce;
    private float m_ChargeSpeed;
    private bool m_Fired;
    private NavMeshAgent nav;


    private void OnEnable()
    {
        m_CurrentLaunchForce = m_MinLaunchForce;
        m_AimSlider.value = m_MinLaunchForce;
    }


    private void Start()
    {
       // m_FireButton = "Fire" + m_PlayerNumber;

        m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;
        nav = GetComponent<NavMeshAgent>();
    }


    private void Update()
    {
        // Track the current state of the fire button and make decisions based on the current launch force.
        m_AimSlider.value = m_MinLaunchForce; //?

        if (m_CurrentLaunchForce >= m_MaxLaunchForce && !m_Fired)
        {
            //si carga maxima pero encara no hem disparat... --> disparem a maxim (no se pot aguantar la bala)
            m_CurrentLaunchForce = m_MaxLaunchForce; //per a no pasarnos de la força maxima
            Fire();
        }
        else if (nav.isStopped) //nav.stoppingDistance <-- esto segun chover
        {
            Fire();
        }
        /*else if (Input.GetButtonDown(m_FireButton))
        {
            //si apretem disparar per primera vegada... --> preparem tot

            m_Fired = false;
            m_CurrentLaunchForce = m_MinLaunchForce;

            m_ShootingAudio.clip = m_ChargingClip;
            m_ShootingAudio.Play();

        }
        else if (Input.GetButton(m_FireButton) && !m_Fired)
        {
            //si estem aguantant el boto de disparar pero encara no hem disparat... --> augmentem valors
            m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;

            m_AimSlider.value = m_CurrentLaunchForce;
        }
        else if (Input.GetButtonUp(m_FireButton) && !m_Fired)
        {
            //si hem soltat el boto i encara no hem disparat --> disparem

            Fire();
        }*/
    }


    private void Fire()
    {
        // Instantiate and launch the shell.

        m_Fired = true;

        Rigidbody shellInstance = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody; //instanciate mos torna un objecte, pero mosatros volem que mos done el rigidbody de eixe objete, per aixo el "as rigidbody"

        shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward;

        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();

        m_CurrentLaunchForce = m_MinLaunchForce;
    }
}