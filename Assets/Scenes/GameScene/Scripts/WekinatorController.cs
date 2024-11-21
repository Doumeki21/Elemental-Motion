using System.Collections.Generic;
using UnityEngine;
using extOSC;

public class WekinatorController : MonoBehaviour
{
    [SerializeField] private List<GameObject> controlledObjects; // Drag & drop objects in the inspector

    public void OnReceiveOutputs(List<float> outputs)
    {
        if (outputs.Count != controlledObjects.Count)
        {
            Debug.LogWarning("Mismatch between Wekinator outputs and controlled objects.");
            return;
        }

        // Assign each Wekinator output to a Unity object
        for (int i = 0; i < outputs.Count; i++)
        {
            float value = outputs[i];

            // Example: Control object scale
            controlledObjects[i].transform.localScale = Vector3.one * value;

            // Example: Control object color
            // MeshRenderer meshRenderer = controlledObjects[0].GetComponent<MeshRenderer>();

            // meshRenderer.material.SetFloat("_DisplacementStrength", outputs[0]);


        }
    }
}

