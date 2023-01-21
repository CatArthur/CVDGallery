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
            new (0.567f, 0.433f, 0.000f),
            new (0.558f, 0.442f, 0.000f),
            new (0.000f, 0.242f, 0.758f)
        },
        {
            new (0.817f,0.183f,0.000f),
            new (0.333f,0.667f,0.000f),
            new (0.000f,0.125f,0.875f)
        },
        {
            new (0.625f,0.375f,0.000f),
            new (0.700f,0.300f,0.000f),
            new (0.000f,0.300f,0.700f)
        },
        {
            new (0.800f,0.200f,0.000f),
            new (0.258f,0.742f,0.000f),
            new (0.000f,0.142f,0.858f)
        },
        {
            new (0.950f,0.050f,0.000f),
            new (0.000f,0.433f,0.567f),
            new (0.000f,0.475f,0.525f)
        },
        {
            new (0.967f,0.033f,0.000f),
            new (0.000f,0.733f,0.267f),
            new (0.000f,0.183f,0.817f)
        },
        {
            new (0.299f,0.587f,0.114f),
            new (0.299f,0.587f,0.114f),
            new (0.299f,0.587f,0.114f)
        },
        {
            new (0.618f,0.320f,0.062f),
            new (0.163f,0.775f,0.062f),
            new (0.163f,0.320f,0.516f)
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
 