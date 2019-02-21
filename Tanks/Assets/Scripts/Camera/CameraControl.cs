using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float m_DampTime = 0.2f;      //temps que te la camera per a fer el recorregut que se li ha asignat           
    public float m_ScreenEdgeBuffer = 4f;           //el numero que anyadim als costats per asegurarnos que els tanks(jugadors no sen ixen de la pantalla)
    public float m_MinSize = 6.5f;     //el minim que es pot minimitzar la camera (per a que no faja un zoom brutal)             
    [HideInInspector] public Transform[] m_Targets;  //HideInInspector serveix per a que esta variable publica no ixca en el inspector pero continua sent accesible desde altres scripts perque es una variable publica 


    private Camera m_Camera;                        
    private float m_ZoomSpeed;                      
    private Vector3 m_MoveVelocity;                 
    private Vector3 m_DesiredPosition;         //la posicio que la camara vol conseguir (sera la mitja de les posicions dels tanks(jugadors))     //que la CameraRig(pare de la main camera que esta a 0,0,0) estiga en el terra ho fa facil aço


    private void Awake()
    {
        m_Camera = GetComponentInChildren<Camera>(); //pillara la primera camera que encontrara en els fills
    }


    private void FixedUpdate()
    {
        Move();
        Zoom();
    }


    private void Move()
    {
        FindAveragePosition();

        transform.position = Vector3.SmoothDamp(transform.position, m_DesiredPosition, ref m_MoveVelocity, m_DampTime); //SmoothDamp mou suaument la camera fins la posicio designada en el temps que se li asigna //ref significa que will write back to that variable la velocitat en que va
    }


    private void FindAveragePosition()
    {
        Vector3 averagePos = new Vector3();
        int numTargets = 0;

        for (int i = 0; i < m_Targets.Length; i++)
        {
            if (!m_Targets[i].gameObject.activeSelf) //si el tank no esta activat
                continue; //continue in the next iteration of the loop

            averagePos += m_Targets[i].position;
            numTargets++;
        }

        if (numTargets > 0)
            averagePos /= numTargets;

        averagePos.y = transform.position.y; //per si els mosques, posem a 0 la y que es on deurien estar tots els tanks

        m_DesiredPosition = averagePos;
    }


    private void Zoom()
    {
        float requiredSize = FindRequiredSize();
        m_Camera.orthographicSize = Mathf.SmoothDamp(m_Camera.orthographicSize, requiredSize, ref m_ZoomSpeed, m_DampTime); //si la camara no fa el move en el mateix temps (DampTime) que el que fa el zoom, fara coses rares
    }

/*  Per a fer el zoom, el que fem es incrementar i decrementar el tamany de la camara ortografica. El tamany de esta sempre es desce el 
    centre fins a dalt de tot es el size, i desde el centre fins a la dreta del tot es Size * AspectRatio. Si el aspect ratio es 16:9 (16/9), a les hores aixo dona que es Size * 1'77  */

    private float FindRequiredSize()
    {
        Vector3 desiredLocalPos = transform.InverseTransformPoint(m_DesiredPosition); //transforms position from world space to local space //aço ho esta ficant a una posició local... del camera rig (pare de la main camera)

        float size = 0f;

        for (int i = 0; i < m_Targets.Length; i++)
        {
            if (!m_Targets[i].gameObject.activeSelf)
                continue;

            Vector3 targetLocalPos = transform.InverseTransformPoint(m_Targets[i].position);

            Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos; //distancia entre el tank y la posicio mitja? No me cuadra... No deuria ser desde el tank i el centre?

            size = Mathf.Max (size, Mathf.Abs (desiredPosToTarget.y)); //si es mes gran la nova posició en Y

            size = Mathf.Max (size, Mathf.Abs (desiredPosToTarget.x) / m_Camera.aspect); // si anteriorment ha guanyat la posició nova, comparara la posicio nova en la posicio nova pero en X
        }
        
        size += m_ScreenEdgeBuffer; //fiquem el padding per a que els tanks no sen ixquen de la pantalla

        size = Mathf.Max(size, m_MinSize); //per a que no faja massa zoom

        return size;
    }


    public void SetStartPositionAndSize() //es public perque aixina el game manajer pot cridarlos cada vegada que hi ha una ronda nova i hi ha que fer reset a tot
    {
        FindAveragePosition();

        transform.position = m_DesiredPosition;

        m_Camera.orthographicSize = FindRequiredSize();
    }
}