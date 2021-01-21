using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Perception.Randomization.Randomizers;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using FloatParameter = UnityEngine.Experimental.Perception.Randomization.Parameters.FloatParameter;

[Serializable]

[AddRandomizerMenu("Perception/My Camera Randomizer")]
public class MyCameraRandomizer : Randomizer
{
    public FloatParameter blurParameter;
    public FloatParameter contrastParameter;
    public FloatParameter saturationParameter;
    public FloatParameter grainAmountParameter;
    
    protected override void OnIterationStart()
    {
        var taggedObjects = tagManager.Query<MyCameraRandomizerTag>();
        foreach (var taggedObject in taggedObjects)
        {
            var volume = taggedObject.GetComponent<Volume>();
            if (volume && volume.profile)
            {
                var dof = (DepthOfField) volume.profile.components.Find(comp => comp is DepthOfField);
                if (dof)
                {
                    var val = blurParameter.Sample();
                    dof.gaussianStart.value = val;
                }

                var colorAdjust = (ColorAdjustments) volume.profile.components.Find(comp => comp is ColorAdjustments);
                if (colorAdjust)
                {
                    var val = contrastParameter.Sample();
                    colorAdjust.contrast.value = val;
                    
                    val = saturationParameter.Sample();
                    colorAdjust.saturation.value = val;
                }
                
                var grain = (FilmGrain) volume.profile.components.Find(comp => comp is FilmGrain);
                if (grain)
                {
                    var val = grainAmountParameter.Sample();
                    grain.intensity.value = Mathf.Clamp(val, 0,1);
                }
            }
        }
    }
}