using UnityEngine;
using extOSC;
using System.Collections.Generic;

public class WekinatorController : MonoBehaviour
{
    [SerializeField] private OSCReceiver oscReceiver; // Reference to the OSCReceiver
    [SerializeField] private List<GameObject> controlledObjects; // List of objects to control

    private const string OSCAddress = "/wek/outputs"; // Wekinator's output OSC address

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

            // Example: Scale the object
            controlledObjects[i].transform.localPosition = Vector3.one * 100f * value;

            // Example: Change object color
            if (i == 0)
            {
                MeshRenderer meshRenderer = controlledObjects[i].GetComponent<MeshRenderer>();

                meshRenderer.material.SetFloat("_DisplacementStrength", value);
            }

        }
    }
}

