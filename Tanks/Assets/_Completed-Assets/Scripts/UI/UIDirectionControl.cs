using UnityEngine;

namespace Complete
{
    public class UIDirectionControl : MonoBehaviour
    {
        // This class is used to make sure world space UI
        // elements such as the health bar face the correct direction.

        public bool m_UseRelativeRotation = true;       // Use relative rotation should be used for this gameobject?


        private Quaternion m_RelativeRotation;          // The local rotatation at the start of the scene.


        private void Start ()
        {
            m_RelativeRotation = transform.parent.localRotation; //find the local rotation of the canvas
        }


        private void Update ()
        {
            //Aço es per a que la barra de vida no gire conforme gira el Tank, sino que desde la perspectiva del jugador sempre se veu igual (Si la vida es mig cercle, al girar el tank no se girara el mig cercle, el voras igual)(Sense aço si que se giraria)
            if (m_UseRelativeRotation)
                transform.rotation = m_RelativeRotation; //canviant la rotacio a la del pare... no pareix que vajes guanyant vida...
        }
    }
}