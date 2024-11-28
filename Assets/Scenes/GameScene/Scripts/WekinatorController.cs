using UnityEngine;
using extOSC;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.Rendering.Universal;
using UnityEngine.Experimental.GlobalIllumination;

public class WekinatorController : MonoBehaviour
{
    [SerializeField] private OSCReceiver oscReceiver; // Reference to the OSCReceiver
    [SerializeField] private List<GameObject> controlledObjects; // List of objects to control

    private const string OSCAddress = "/wek/outputs"; // Wekinator's output OSC address

    private TerrainData terrainData;

    public Terrain terrain;

    public GameObject Light;

    private void Start()
    {
        if (oscReceiver == null)
        {
            Debug.LogError("OSC Receiver not assigned. Please assign it in the Inspector.");
            return;
        }

        // Bind to the OSC address
        oscReceiver.Bind(OSCAddress, HandleWekinatorOutputs);
    }

    private void HandleWekinatorOutputs(OSCMessage message)
    {
        // Ensure the message contains float values
        if (message.Values.Count > 0 && message.Values[0].Type == OSCValueType.Float)
        {
            float[] outputs = new float[message.Values.Count];

            // Creates an array to store the float values
            for (int i = 0; i < message.Values.Count; i++)
            {
                outputs[i] = message.Values[i].FloatValue;
            }

            ApplyOutputs(outputs);
        }
        else
        {
            Debug.LogWarning("Received OSC message with no float values.");
        }
        Debug.Log($"Received OSC message with {message.Values.Count} values.");
    }

    private void ApplyOutputs(float[] outputs)
    {
        if (outputs.Length != controlledObjects.Count)
        {
            Debug.LogWarning("Mismatch between Wekinator outputs and controlled objects.");
            return;
        }

        for (int i = 0; i < outputs.Length; i++)
        {
            // Scrolls through each value in the array. Each "if" statement sends the respective value in the array to the correct GameObject
            float value = outputs[i];

            // This changes the sharpness of the mist over the water in the center of the map
            if (i == 1)
            {
                MeshRenderer meshRenderer = controlledObjects[i].GetComponent<MeshRenderer>();
                meshRenderer.material.SetFloat("_CloudSoftness", value);
            }

            // This changes the displacement strength of the normal map of the mist over the water in the center of the map
            if (i == 2)
            {
                MeshRenderer meshRenderer = controlledObjects[i].GetComponent<MeshRenderer>();
                meshRenderer.material.SetFloat("_DisplacementStrength", value);
            }

            // This changes the cutoff the texture of the mist over the water in the center of the map
            if (i == 3)
            {
                MeshRenderer meshRenderer = controlledObjects[i].GetComponent<MeshRenderer>();
                Vector2 CutoffScaleChange = new Vector2(0.07f, value);
                meshRenderer.material.SetVector("_CloudCutoff", CutoffScaleChange);
            }

            // This Changes the Metallicness of the water texture
            if (i == 4)
            {
                MeshRenderer meshRenderer = controlledObjects[i].GetComponent<MeshRenderer>();
                Vector2 UVScaleChange = new Vector2(value, value);
                meshRenderer.material.SetVector("Vector1_3D886DA1", UVScaleChange);
            }

            // This changes the frequency of the waves in the ocean
            if (i == 5)
            {
                MeshRenderer meshRenderer = controlledObjects[i].GetComponent<MeshRenderer>();
                meshRenderer.material.SetFloat("_WaveFrequency", value);
            }

            // This changes the height of the ocean waves
            if (i == 6)
            {
                MeshRenderer meshRenderer = controlledObjects[i].GetComponent<MeshRenderer>();
                meshRenderer.material.SetFloat("_WaveScale", value);
            }

            // This changes the speed of the ocean waves
            if (i == 7)
            {
                MeshRenderer meshRenderer = controlledObjects[i].GetComponent<MeshRenderer>();
                meshRenderer.material.SetFloat("_WaveSpeed", value);
            }

            // This raises or flattens the terrain
            if (i == 8)
            {
                Vector3 heightChange = new Vector3(600, 100 * value, 600);
                terrain.terrainData.size = heightChange;
            }

            // This rotates the direction of the light
            if (i == 9)
            {
                Light.transform.Rotate(0, value, 0);
            }

            // This controls the size of the fog particles
            if (i == 10)
            {
                ParticleSystem particleSystem = controlledObjects[i].GetComponent<ParticleSystem>();
                var mainModule = particleSystem.main;

                //Uses the Wekinator output to control 3D start size of the particles
                mainModule.startSizeX = value;
                mainModule.startSizeY = 1f;
                mainModule.startSizeZ = 1f;
            }

            // This controls the size of the rain particles
            if (i == 11)
            {
                ParticleSystem particleSystem = controlledObjects[i].GetComponent<ParticleSystem>();
                var mainModule = particleSystem.main;

                //Uses the Wekinator output to control 3D start size of the particles
                mainModule.startSizeX = value;
                mainModule.startSizeY = value;
                mainModule.startSizeZ = value;
            }

        }
    }

    // This Function stores the terrainData so that we don't need to constantly recall it
    void Awake()
    {
        terrainData = terrain.terrainData;
    }
}

