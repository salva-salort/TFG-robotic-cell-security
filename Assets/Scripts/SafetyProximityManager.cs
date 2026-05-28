using UnityEngine;

public class SafetyProximityManager : MonoBehaviour
{
    [Header("Referencias del Robot")]
    [Tooltip("Arrastra aquí TODOS los eslabones del robot que quieras monitorizar")]
    public Transform[] eslabonesRobot; // <-- CAMBIO CLAVE: Ahora es una lista de eslabones

    [Header("Configuración de Distancias (en metros)")]
    public float distanciaAvisoTorso = 0.8f;   
    public float distanciaAvisoCabeza = 0.6f;  

    [Header("Eventos bHaptics")]
    public string eventoVibracionTorso = "vibrar_chaleco";
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

        // Recorremos todos los eslabones del robot para encontrar cuál es el más cercano al operario
        foreach (Transform eslabon in eslabonesRobot)
        {
            if (eslabon == null) continue;

            // Distancia del torso a ESTE eslabón en concreto
            if (transformTorso != null)
            {
                float dTorso = Vector3.Distance(eslabon.position, transformTorso.position);
                if (dTorso < distanciaMinimaTorso) distanciaMinimaTorso = dTorso;
            }

            // Distancia de la cabeza a ESTE eslabón en concreto
            if (transformCabeza != null)
            {
                float dCabeza = Vector3.Distance(eslabon.position, transformCabeza.position);
                if (dCabeza < distanciaMinimaCabeza) distanciaMinimaCabeza = dCabeza;
            }
        }

        // --- CONTROL DE PROXIMIDAD DEL TORSO (Usando la distancia al eslabón más cercano) ---
        if (transformTorso != null && distanciaMinimaTorso != float.MaxValue)
        {
            if (distanciaMinimaTorso <= distanciaAvisoTorso)
            {
                if (!torsoYaEnPeligro)
                {
                    CanvasNotifications.Instance.SetPeligroTorso(true);
                    torsoYaEnPeligro = true;
                }
                // BhapticsLibrary.Play(eventoVibracionTorso);
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
                }
                // BhapticsLibrary.Play(eventoVibracionCabeza);
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