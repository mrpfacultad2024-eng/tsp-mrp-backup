using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Threading;
using Unity.VisualScripting;
using System.IO;


public class vuelonosync : MonoBehaviour {
    public float speed = 50f;
    public float rotationSpeed = 100f;
    public Transform cameraTransform;
    public Vector2 movementInput;

    //Control de iteraciones
    public int turbulenceIterations = 1000000;


    //Lista de vectores de posición calculados
    private List<Vector3> turbulenceForces = new List<Vector3>();

    //Variables para manipular el hilo secundario
    private Thread turbulenceThread;
    private bool isTurbulenceRunning = false;
    private bool stopTurbulenceThread = false;
    private float capturedTime;

    //Bandera de control sobre lectura
    public bool read = false;
    public bool write = false;  
    //Ruta de almacenamiento de archivo
    string filepath;

    //Método para mover la nave
    public void OnMovement(InputValue Value) {
        movementInput = Value.Get<Vector2>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() 
    {
        filepath = Application.dataPath + "/TurbulenceData.txt";
        Debug.Log("Ruta del archivo: " + filepath);
    }

    // Update is called once per frame
    void Update() {
        if (cameraTransform == null) {
            Debug.LogError("No hay camara asignada");
            return;
        }

        //Actividad 1: Proceso pesado que consume recursos
        //SimulateTurbulence();

        //Tiempo transcurrido 
        capturedTime = Time.time;

        if (!isTurbulenceRunning) {
            isTurbulenceRunning = true;
            stopTurbulenceThread = false;

            turbulenceThread = new Thread(() =>
            SimulateTurbulence(capturedTime));

            turbulenceThread.Start();
        }

        //Mover la nave linealmente
        Vector3 moveDirection = cameraTransform.forward * movementInput.y * speed * Time.deltaTime;
        this.transform.position += moveDirection;

        //Mover la nave en rotación
        float yaw = movementInput.x * rotationSpeed * Time.deltaTime;
        this.transform.Rotate(0, yaw, 0);

        //Actividad 3

        TryReadFile();
    }

    public void SimulateTurbulence(float time) {
        turbulenceForces.Clear();

        //Repeticiones

        for (int i = 0; i < turbulenceIterations; i++) {
            //Verificar si se debe detener el hilo
            if (stopTurbulenceThread) {
                break;
            }

            Vector3 force = new Vector3
            (
                Mathf.PerlinNoise(i * 0.001f, time) * 2 - 1,
                Mathf.PerlinNoise(i * 0.002f, time) * 2 - 1,
                Mathf.PerlinNoise(i * 0.003f, time) * 2 - 1
            );

            turbulenceForces.Add(force);

        }

        Debug.Log("Iniciando simulación");

        //Escritura en archivo

        using (StreamWriter writer = new StreamWriter(filepath, false)) 
        {
            foreach (var force in turbulenceForces)
            {
                writer.WriteLine(force.ToString());
            }
            writer.Flush();
        }

        Debug.Log("Archivo escrito");

        isTurbulenceRunning = false;
    }

    //Actividad 3 Escritura

    void TryReadFile() 
    {
        try 
        {
            string content = File.ReadAllText(filepath);
            Debug.Log("Archivo leído" + content);
        }
        catch(IOException ex) 
        {
            Debug.LogError("Error de acceso al archivo" + ex.Message);
        }
    }

    private void OnDestroy() {
        stopTurbulenceThread = true;
        if (turbulenceThread != null && turbulenceThread.IsAlive) {
            turbulenceThread.Join();
        }
    }
}
