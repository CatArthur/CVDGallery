using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(CVDRenderer), PostProcessEvent.AfterStack, "Custom/CVD")]
public sealed class CVD : PostProcessEffectSettings
{
    public String[] anomalyNames =
    {
        "Normal vision", "Protanopia", "Protanomaly", "Deuteranopia", "Deuteranomaly", "Tritanopia", "Tritanomaly", "Achromatopsia", "Achromatomaly"
    };

    private int N = 9;
    [Range(0, 8), Tooltip("CVD effect intensity.")]
    public IntParameter anomaly = new IntParameter{value = 0};
        
    public Vector4[,] matrices =
    {
        {
            new (1f, 0f, 0f),
            new (0f, 1f, 0f),
            new (0f, 0f, 1f)
        },
        {
            new (0.17f,  0.83f,0),
            new (0.17f,  0.83f,0),
            new (   0f,     0f,1) 
        },
        {
            new (0.436f,  0.581f,0),
            new (0.136f,  0.881f,0),
            new (   0f,     0f,1) 
        },
        {
            new (0.33f,  0.67f, 0),
            new (0.33f,  0.67f, 0),
            new (-0.03f, 0.03f, 1 )
        },
        {
            new (0.531f,  0.441f, 0),
            new (0.231f,  0.741f, 0),
            new (    0f,      0f, 1 )
        },
        {                                                                                  
            new (1, 0.13f, -0.13f),
            new (0, 0.87f, 0.13f),
            new (0, 0.87f, 0.13f)
        },
        {                                                                               
            new (1, 0.091f, -0.091f),
            new (0, 0.909f, 0.091f),
            new (0, 0.609f, 0.391f)
        },
        {
            new (0.30f,0.59f,0.11f),
            new (0.30f,0.59f,0.11f),
            new (0.30f,0.59f,0.11f)
        },
        {
            new (0.51f,0.42f,0.08f),
            new (0.21f,0.72f,0.08f),
            new (0.21f,0.42f,0.38f)
        }
    };

    public String AnomalyName()
    {
        return anomalyNames[anomaly];
    }
    
    public void ChangeAnomaly(int newAnomaly)
    {
        anomaly.value = newAnomaly;
    }
    
    public void NextAnomaly()
    {
        anomaly.value = (anomaly.value + 1) % N;
    }
    public void PrevAnomaly()
    {
        anomaly.value = (anomaly.value + N - 1) % N;
    }
}

public sealed class CVDRenderer : PostProcessEffectRenderer<CVD>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/CVD"));
        sheet.properties.SetVector("R", settings.matrices[settings.anomaly, 0]);
        sheet.properties.SetVector("G", settings.matrices[settings.anomaly, 1]);
        sheet.properties.SetVector("B", settings.matrices[settings.anomaly, 2]);
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}
 