using UnityEngine;
using Bhaptics.SDK2;

public class SafetyProximityManager : MonoBehaviour
{
    [Header("Referencias del Robot")]
    [Tooltip("Arrastra aquí TODOS los eslabones del robot que quieras monitorizar")]
    public Transform[] eslabonesRobot; 

    [Header("Configuración de Distancias (en metros)")]
    public float distanciaAvisoTorso = 0.8f;   
    public float distanciaAvisoCabeza = 0.6f;  

    [Header("Eventos bHaptics")]
    public string eventoVibracionTorsoDelante = "torsodelante";
    public string eventoVibracionTorsoDetras = "torsodetras";
    public string eventoVibracionCabeza = "vibrar_diadema";

    private Transform transformTorso;
    private Transform transformCabeza;

    private bool torsoYaEnPeligro = false;
    private bool cabezaYaEnPeligro = false;

    void Start()
    {
        GameObject objetoTorso = GameObject.FindWithTag("Torso");
        if (objetoTorso != null) transformTorso = objetoTorso.transform;

        GameObject objetoCabeza = GameObject.FindWithTag("Cabeza");
        if (objetoCabeza != null) transformCabeza = objetoCabeza.transform;

        if (eslabonesRobot == null || eslabonesRobot.Length == 0)
        {
            Debug.LogWarning("No has asignado ningún eslabón del robot en el SafetyProximityManager.");
        }
    }

    void Update()
    {
        if (CanvasNotifications.Instance == null || eslabonesRobot == null || eslabonesRobot.Length == 0) return;

        // Buscaremos la distancia más corta del usuario al robot
        float distanciaMinimaTorso = float.MaxValue;
        float distanciaMinimaCabeza = float.MaxValue;

        Transform eslabonMasCercanoTorso = null;

        // Recorremos todos los eslabones del robot
        foreach (Transform eslabon in eslabonesRobot)
        {
            if (eslabon == null) continue;

            if (transformTorso != null)
            {
                float dTorso = Vector3.Distance(eslabon.position, transformTorso.position);
                if (dTorso < distanciaMinimaTorso)
                {
                    distanciaMinimaTorso = dTorso;
                    eslabonMasCercanoTorso = eslabon; // Almacenamos el eslabón culpable
                }
            }

            if (transformCabeza != null)
            {
                float dCabeza = Vector3.Distance(eslabon.position, transformCabeza.position);
                if (dCabeza < distanciaMinimaCabeza) distanciaMinimaCabeza = dCabeza;
            }
        }

        // --- CONTROL DE PROXIMIDAD DEL TORSO ---
        if (transformTorso != null && distanciaMinimaTorso != float.MaxValue)
        {
            if (distanciaMinimaTorso <= distanciaAvisoTorso)
            {
                if (!torsoYaEnPeligro)
                {
                    
                    torsoYaEnPeligro = true;
                    
                    // Calculamos si ese eslabón concreto está delante o detrás del usuario
                    Vector3 direccionAlRobot = (eslabonMasCercanoTorso.position - transformTorso.position).normalized;
                    float resultadoDot = Vector3.Dot(transformTorso.forward, direccionAlRobot);
                    bool esDelante = (resultadoDot >= 0);

                    CanvasNotifications.Instance.SetPeligroTorso(true, esDelante);
                    
                    if (esDelante)
                    {
                        // El robot está en la zona frontal (ángulo de 180º delanteros)
                        BhapticsLibrary.Play(eventoVibracionTorsoDelante); 
                    }
                    else
                    {
                        // El robot está en la espalda
                        BhapticsLibrary.Play(eventoVibracionTorsoDetras); 
                    }
                }
            }
            else
            {
                if (torsoYaEnPeligro)
                {
                    CanvasNotifications.Instance.SetPeligroTorso(false);
                    torsoYaEnPeligro = false;
                }
            }
        }

        // --- CONTROL DE PROXIMIDAD DE LA CABEZA ---
        if (transformCabeza != null && distanciaMinimaCabeza != float.MaxValue)
        {
            if (distanciaMinimaCabeza <= distanciaAvisoCabeza)
            {
                if (!cabezaYaEnPeligro)
                {
                    CanvasNotifications.Instance.SetPeligroCabeza(true);
                    cabezaYaEnPeligro = true;
                    
                    BhapticsLibrary.Play(eventoVibracionCabeza); 
                }
            }
            else
            {
                if (cabezaYaEnPeligro)
                {
                    CanvasNotifications.Instance.SetPeligroCabeza(false);
                    cabezaYaEnPeligro = false;
                }
            }
        }
    }
}