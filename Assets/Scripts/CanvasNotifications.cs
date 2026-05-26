using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasNotifications : MonoBehaviour
{
    public static CanvasNotifications Instance;

    [Header("Componentes de la UI")]
    public GameObject fondoPanel; 
    
    [Header("Indicadores de Texto")]
    public TextMeshProUGUI textoManoDerecha; 
    public TextMeshProUGUI textoManoIzquierda;
    public TextMeshProUGUI textoTorso;   // <-- NUEVO
    public TextMeshProUGUI textoCabeza;  // <-- NUEVO

    // Memoria del estado actual de cada zona háptica
    private bool derechaEnPeligro = false;
    private bool izquierdaEnPeligro = false;
    private bool torsoEnPeligro = false;    // <-- NUEVO
    private bool cabezaEnPeligro = false;   // <-- NUEVO

    void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); return; }

        // El monitor permanece siempre encendido
        if (fondoPanel != null) fondoPanel.SetActive(true);

        LimpiarPantalla();
    }

    // --- FUNCIONES PARA ACTUALIZAR CADA "LUZ" INDEPENDIENTEMENTE ---

    public void SetPeligroManoDerecha(bool enPeligro)
    {
        derechaEnPeligro = enPeligro;
        ActualizarDashboard();
    }

    public void SetPeligroManoIzquierda(bool enPeligro)
    {
        izquierdaEnPeligro = enPeligro;
        ActualizarDashboard();
    }

    public void SetPeligroTorso(bool enPeligro) // <-- NUEVO
    {
        torsoEnPeligro = enPeligro;
        ActualizarDashboard();
    }

    public void SetPeligroCabeza(bool enPeligro) // <-- NUEVO
    {
        cabezaEnPeligro = enPeligro;
        ActualizarDashboard();
    }

    // --- RENDERIZADO DEL DASHBOARD ---
    private void ActualizarDashboard()
    {
        // 1. Mano Derecha
        if (textoManoDerecha != null)
        {
            if (derechaEnPeligro)
            {
                textoManoDerecha.text = "[X] MANO DERECHA: VIBRANDO";
                textoManoDerecha.color = Color.red;
            }
            else
            {
                textoManoDerecha.text = "[ ] MANO DERECHA: SEGURA";
                textoManoDerecha.color = Color.white;
            }
        }

        // 2. Mano Izquierda
        if (textoManoIzquierda != null)
        {
            if (izquierdaEnPeligro)
            {
                textoManoIzquierda.text = "[X] MANO IZQUIERDA: VIBRANDO";
                textoManoIzquierda.color = Color.red;
            }
            else
            {
                textoManoIzquierda.text = "[ ] MANO IZQUIERDA: SEGURA";
                textoManoIzquierda.color = Color.white;
            }
        }

        // 3. Torso (NUEVO)
        if (textoTorso != null)
        {
            if (torsoEnPeligro)
            {
                textoTorso.text = "[X] TORSO: VIBRANDO";
                textoTorso.color = Color.red;
            }
            else
            {
                textoTorso.text = "[ ] TORSO: SEGURO";
                textoTorso.color = Color.white;
            }
        }

        // 4. Cabeza (NUEVO)
        if (textoCabeza != null)
        {
            if (cabezaEnPeligro)
            {
                textoCabeza.text = "[X] CABEZA: VIBRANDO";
                textoCabeza.color = Color.red;
            }
            else
            {
                textoCabeza.text = "[ ] CABEZA: SEGURA";
                textoCabeza.color = Color.white;
            }
        }

        // --- LÓGICA DEL PANEL TRASERO ---
        // Si CUALQUIERA de las partes está en peligro, el fondo se tiñe de alerta
        if (fondoPanel != null)
        {
            if (derechaEnPeligro || izquierdaEnPeligro || torsoEnPeligro || cabezaEnPeligro)
            {
                fondoPanel.GetComponent<Image>().color = new Color(0.3f, 0f, 0f); // Rojo oscuro industrial
            }
            else
            {
                fondoPanel.GetComponent<Image>().color = Color.black; // Negro reposo
            }
        }
    }

    public void LimpiarPantalla()
    {
        derechaEnPeligro = false;
        izquierdaEnPeligro = false;
        torsoEnPeligro = false;
        cabezaEnPeligro = false;
        ActualizarDashboard();
    }
}