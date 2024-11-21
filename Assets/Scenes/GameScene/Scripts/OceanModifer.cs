using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OceanModifer : MonoBehaviour
{

    public float DisplacementStrength = 7;
    public Texture albedotexture;
    MeshRenderer meshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.color = Color.cyan;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DisplacementStrength += 0.1f;
            if (DisplacementStrength > 10) DisplacementStrength = 10;
            meshRenderer.material.SetFloat("_DisplacementStrength", DisplacementStrength);
            // meshRenderer.material.SetFloat("_Smoothness", DisplacementStrength);
        }

        else if (Input.GetMouseButtonDown(1))
        {
            DisplacementStrength -= 0.1f;
            if (DisplacementStrength < 0) DisplacementStrength = 0.1f;
            meshRenderer.material.SetFloat("_DisplacementStrength", DisplacementStrength);
            // meshRenderer.material.SetFloat("_Smoothness", DisplacementStrength);
        }
    }

    public void OceanDisplacementSlider(float Displacement)
    {
        meshRenderer.material.SetFloat("_DisplacementStrength", Displacement);
    }

    public void ChangeNormalTexture()
    {
        meshRenderer.material.SetTexture("_MainTex", albedotexture);
    }
}
