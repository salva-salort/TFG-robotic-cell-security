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
    public TextMeshProUGUI textoTorso;   
    public TextMeshProUGUI textoCabeza;  

    // Memoria del estado actual de cada zona háptica
    private bool derechaEnPeligro = false;
    private bool izquierdaEnPeligro = false;
    private bool torsoEnPeligro = false;    
    private bool cabezaEnPeligro = false;   
    
    private bool torsoDelante = true; 

    void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); return; }

        if (fondoPanel != null) fondoPanel.SetActive(true);

        LimpiarPantalla();
    }

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

    public void SetPeligroTorso(bool enPeligro, bool esDelante = true) 
    {
        torsoEnPeligro = enPeligro;
        torsoDelante = esDelante; // Guardamos la dirección
        ActualizarDashboard();
    }

    public void SetPeligroCabeza(bool enPeligro) 
    {
        cabezaEnPeligro = enPeligro;
        ActualizarDashboard();
    }

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

        // 3. Torso 
        if (textoTorso != null)
        {
            if (torsoEnPeligro)
            {
                textoTorso.text = torsoDelante ? "[X] TORSO: VIBRANDO DELANTE" : "[X] TORSO: VIBRANDO DETRÁS";
                textoTorso.color = Color.red;
            }
            else
            {
                textoTorso.text = "[ ] TORSO: SEGURO";
                textoTorso.color = Color.white;
            }
        }

        // 4. Cabeza
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

        if (fondoPanel != null)
        {
            if (derechaEnPeligro || izquierdaEnPeligro || torsoEnPeligro || cabezaEnPeligro)
            {
                fondoPanel.GetComponent<Image>().color = new Color(0.3f, 0f, 0f); 
            }
            else
            {
                fondoPanel.GetComponent<Image>().color = Color.black; 
            }
        }
    }

    public void LimpiarPantalla()
    {
        derechaEnPeligro = false;
        izquierdaEnPeligro = false;
        torsoEnPeligro = false;
        cabezaEnPeligro = false;
        torsoDelante = true; // Reset por defecto
        ActualizarDashboard();
    }
}