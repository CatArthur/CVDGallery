using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(CVDRenderer), PostProcessEvent.AfterStack, "Custom/CVD")]
public sealed class CVD : PostProcessEffectSettings
{
    public String[] anomalies =
    {
        "Protanopia",
        "Protanomaly", "Deuteranopia", "Deuteranomaly", "Tritanopia", "Tritanomaly", "Achromatopsia", "Achromatomaly"
    };

    [Range(0, 7), Tooltip("CVD effect intensity.")]
    public IntParameter anomaly = new IntParameter{value = 0};
        
    public Vector4[,] matrices =
    {
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

//float3(0.567,0.433,0.000)); 
//float3(0.558,0.442,0.000)); 
//float3(0.000,0.242,0.758)); 