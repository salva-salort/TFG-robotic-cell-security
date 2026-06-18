using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar de escena

public class MenuController : MonoBehaviour
{
    // Función para el Botón 1
    public void CargarSimulacionVirtual()
    {
        Debug.Log("Cargando entorno de simulación...");
        // Carga la escena por su nombre exacto
        SceneManager.LoadScene("Simulacion"); 
    }

    // Función para el Botón 2
    public void CargarRealidadMixta()
    {
        Debug.Log("Cargando Realidad Mixta para el laboratorio...");
        SceneManager.LoadScene("Franka_Real");
    }
    
    // Opcional: Un botón para salir de la app
    public void SalirDeLaAplicacion()
    {
        Debug.Log("Cerrando aplicación...");
        Application.Quit();
    }
}