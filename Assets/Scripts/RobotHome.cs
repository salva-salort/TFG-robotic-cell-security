using System.Collections;
using UnityEngine;

public class RobotHome : MonoBehaviour
{
    [System.Serializable]
    public struct JointConfig
    {
        public string nombreArticulacion; 
        public ArticulationBody articulationBody; 
        public float anguloHomeGrados; 
    }

    [Header("Configuración de Articulaciones")]
    public JointConfig[] articulaciones;

    void Start()
    {
        StartCoroutine(EjecutarFixYHomeConRetraso());
    }

    IEnumerator EjecutarFixYHomeConRetraso()
    {
        yield return new WaitForSeconds(0.2f);
        IrAPosicionHome();
    }

    public void IrAPosicionHome()
    {
        if (articulaciones == null || articulaciones.Length == 0) return;

        foreach (var joint in articulaciones)
        {
            if (joint.articulationBody != null)
            {
                string nameLower = joint.articulationBody.name.ToLower();
                var drive = joint.articulationBody.xDrive;

                // 1. Configuración de precisión global para la APK
                joint.articulationBody.solverIterations = 30;
                joint.articulationBody.solverVelocityIterations = 10;
                joint.articulationBody.WakeUp();

                // 2. FILTRADO: ¿Es una pinza/dedo o un joint del brazo?
                if (nameLower.Contains("finger") || nameLower.Contains("gripper") || nameLower.Contains("hand"))
                {
                    // CONFIGURACIÓN PARA LAS PINZAS (Fuerza suave para evitar vibraciones)
                    joint.articulationBody.jointFriction = 1f;
                    joint.articulationBody.angularDamping = 2f;

                    drive.stiffness = 2000f;   // Mucho más bajo, no necesitan fuerza de camión
                    drive.damping = 200f;      // Amortiguación proporcional (Relación 10:1)
                    drive.forceLimit = 200f;   // Límite de fuerza bajo para que no vibren
                }
                else
                {
                    // CONFIGURACIÓN PARA EL BRAZO (Fuerza firme pero amortiguada para evitar el efecto látigo)
                    joint.articulationBody.jointFriction = 0.2f;   
                    joint.articulationBody.angularDamping = 5f;    // Subimos la amortiguación estructural (freno)

                    drive.stiffness = 20000f;  // Bajamos de 40000 a 20000 (firme pero natural)
                    drive.damping = 2000f;     // Subimos drásticamente el damping del motor (Relación 10:1)
                    drive.forceLimit = 1500f;  // Fuerza de sobra para levantar los eslabones
                }

                // 3. Asignamos el ángulo objetivo de tu Home
                drive.target = joint.anguloHomeGrados;
                joint.articulationBody.xDrive = drive;
            }
        }
        
        Debug.Log("[RobotHome] Ajustes de dinámica equilibrados. Robot enviado a Home.");
    }
}