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

        // Subscribe to the OSC address
        oscReceiver.Bind(OSCAddress, HandleWekinatorOutputs);
    }

    private void HandleWekinatorOutputs(OSCMessage message)
    {
        // Ensure the message contains float values
        if (message.Values.Count > 0 && message.Values[0].Type == OSCValueType.Float)
        {
            float[] outputs = new float[message.Values.Count];

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

            // This changes the panning speed of the mist over the water in the center of the map
            if (i == 3)
            {
                MeshRenderer meshRenderer = controlledObjects[i].GetComponent<MeshRenderer>();
                meshRenderer.material.SetFloat("_PanSpeed", value);
            }

            // This Changes the scale of the water texture
            if (i == 4)
            {
                MeshRenderer meshRenderer = controlledObjects[i].GetComponent<MeshRenderer>();
                Vector2 UVScaleChange = new Vector2(value, value);
                meshRenderer.material.SetVector("Vector2_37B21477", UVScaleChange);
            }

            // This changes the contrast of the water
            if (i == 5)
            {
                MeshRenderer meshRenderer = controlledObjects[i].GetComponent<MeshRenderer>();
                meshRenderer.material.SetFloat("Vector1_B9F56378", value);
            }

            // This changes the height of the ocean waves
            if (i == 6)
            {
                MeshRenderer meshRenderer = controlledObjects[i].GetComponent<MeshRenderer>();
                meshRenderer.material.SetFloat("_WaveScale", value);
            }

            // This changes the water's scroll speed
            if (i == 7)
            {
                MeshRenderer meshRenderer = controlledObjects[i].GetComponent<MeshRenderer>();
                meshRenderer.material.SetFloat("Vector1_244B0600", value);
            }

            // This raises or flattens the terrain
            if (i == 8)
            {
                int i2 = (int)value;
                Vector3 heightChange = new Vector3(600, 100 * value, 600);
                terrain.terrainData.size = heightChange;
            }

            // This rotates the direction of the light
            if (i == 9)
            {
                Light.transform.Rotate(0, value, 0);
            }

        }
    }

    // This Function stores the terrainData so that we don't need to constantly recall it
    void Awake()
    {
        terrainData = terrain.terrainData;
    }
}

