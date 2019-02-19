using UnityEngine;

public class TankMovement : MonoBehaviour
{
    public int m_PlayerNumber = 1;         
    public float m_Speed = 12f;            
    public float m_TurnSpeed = 180f;       
    public AudioSource m_MovementAudio;    
    public AudioClip m_EngineIdling;       
    public AudioClip m_EngineDriving;      
    public float m_PitchRange = 0.2f;

    
    private string m_MovementAxisName;     
    private string m_TurnAxisName;         
    private Rigidbody m_Rigidbody;         
    private float m_MovementInputValue;    
    private float m_TurnInputValue;        
    private float m_OriginalPitch;

    Vector3 movement=Vector3.zero;
    public float friction = 0.7f;


    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }


    private void OnEnable ()
    {
        m_Rigidbody.isKinematic = false;
        m_MovementInputValue = 0f;
        m_TurnInputValue = 0f;
    }


    private void OnDisable ()
    {
        m_Rigidbody.isKinematic = true; //Kinematic es si vols fisiques pero que no vols que se li apliquen forçes al objecte (en el pinball les botes reboten contra objectes pero estos objectes no se mouen)
    }


    private void Start()
    {
        m_MovementAxisName = "Vertical" + m_PlayerNumber;
        m_TurnAxisName = "Horizontal" + m_PlayerNumber;

        m_OriginalPitch = m_MovementAudio.pitch;
    }
    

    private void Update()
    {
        // Store the player's input and make sure the audio for the engine is playing.
        m_MovementInputValue = Input.GetAxis(m_MovementAxisName); //vertical? El que pasa esque no sabem quin jugador esta utilitzant el scrip
        m_TurnInputValue = Input.GetAxis(m_TurnAxisName); // horizontal

        EngineAudio();
    }


    private void EngineAudio()
    {
        // Play the correct audio clip based on whether or not the tank is moving and what audio is currently playing.

        if (Mathf.Abs(m_MovementInputValue) < 0.1f && Mathf.Abs(m_TurnInputValue) < 0.1) //Abs es valor Absolut. Osiga que llevem el negatiu per a poder fer la comparacio (osiga, preguntem, si el valor esta entre -1 i 1)
        {

            if (m_MovementAudio.clip == m_EngineDriving) //si esta reproduintse el so de de conduir
            {

                m_MovementAudio.clip = m_EngineIdling;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange); //valor entre 0.8 i 1.2 //se fa eixina per a no tindre magic numbers en el cas de que es restructurara el codic
                m_MovementAudio.Play();
            }

        }
        else //Si esta movent-se
        {
            if (m_MovementAudio.clip == m_EngineIdling) //si esta reproduintse el so de estar quiet
            {

                m_MovementAudio.clip = m_EngineDriving;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange); //valor entre 0.8 i 1.2 //se fa eixina per a no tindre magic numbers en el cas de que es restructurara el codic
                m_MovementAudio.Play();
            }
        }

    }


    private void FixedUpdate()
    {
        // Move and turn the tank.
        Move();
        Turn();
    }


    private void Move()
    {
        // Adjust the position of the tank based on the player's input.

        //Vector3 movement;

        if (Mathf.Abs(m_MovementInputValue) > 0.5f)
        {
            movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;
            //print("hola");
        }
        else
        {
            movement = movement * friction;
            print("adeu");

        }

        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
    }


    private void Turn()
    {
        // Adjust the rotation of the tank based on the player's input.

        float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);

        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
    }
}