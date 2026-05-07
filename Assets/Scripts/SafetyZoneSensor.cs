using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics; // Obligatorio para usar las listas (HashSets)
// using Bhaptics.SDK2;

public class SafetyZoneSensor : MonoBehaviour
{
    [Header("Configuración Visual")]
    public Material safeMaterial;    // Material Verde
    public Material dangerMaterial;  // Material Rojo
    private MeshRenderer myRenderer;
    
    // EL NUEVO SISTEMA: La "Lista de Asistencia" inteligente
    private HashSet<Collider> objetosDentro = new HashSet<Collider>();

    [Header("Configuración bHaptics")]
    public string eventoManoDerecha = "peligro_mano_derecha";
    public string eventoManoIzquierda = "peligro_mano_izquierda";

    void Start() 
    {
        myRenderer = GetComponent<MeshRenderer>();
        myRenderer.material = safeMaterial; 
    }

    // Usamos el Update para vigilar la zona en tiempo real (por si las manos desaparecen de golpe)
    void Update()
    {
        // Solo gastamos recursos si hay cosas en la lista
        if (objetosDentro.Count > 0)
        {
            ActualizarEstado();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != this.gameObject)
        {
            // Añadimos el objeto a nuestra lista de asistencia
            objetosDentro.Add(other);
            ActualizarEstado(); 

            // --- LÓGICA BHAPTICS ---
            bool esManoDerecha = false;
            bool esManoIzquierda = false;
            Transform ancestroActual = other.transform;

            while (ancestroActual != null)
            {
                string nombre = ancestroActual.name.ToLower();
                if (nombre.Contains("right")) { esManoDerecha = true; break; }
                else if (nombre.Contains("left")) { esManoIzquierda = true; break; }
                ancestroActual = ancestroActual.parent;
            }

            if (esManoDerecha)
            {
                // BhapticsLibrary.Play(eventoManoDerecha);
            }
            else if (esManoIzquierda)
            {
                // BhapticsLibrary.Play(eventoManoIzquierda);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject != this.gameObject)
        {
            // Tachamos el objeto de la lista cuando sale físicamente
            objetosDentro.Remove(other);
            ActualizarEstado();
        }
    }

    // El cerebro del sistema: Evalúa la lista y decide el color
    private void ActualizarEstado()
    {
        // MAGIA PURA: Borramos de la lista cualquier objeto que haya sido destruido o desactivado por perder tracking
        objetosDentro.RemoveWhere(colisionador => colisionador == null || !colisionador.gameObject.activeInHierarchy);

        // Si después de limpiar la lista sigue habiendo algo dentro... es que hay peligro real
        if (objetosDentro.Count > 0)
        {
            myRenderer.material = dangerMaterial;
        }
        else
        {
            // Lista completamente vacía
            myRenderer.material = safeMaterial;
            
            // BhapticsLibrary.Stop(eventoManoDerecha);
            // BhapticsLibrary.Stop(eventoManoIzquierda);
        }
    }
}