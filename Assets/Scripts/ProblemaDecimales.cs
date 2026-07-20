using System.Globalization;
using System.Threading;
using UnityEngine;

public class LocaleFixer : MonoBehaviour
{
    void Awake()
    {
        // Fuerza el uso del punto decimal (.) en todos los hilos de la aplicación
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
        CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
    }
}