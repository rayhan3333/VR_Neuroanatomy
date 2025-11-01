using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using Microsoft.MixedReality.Toolkit.UI;

namespace VolumeRendering
{

    public class VolumeRenderingController : MonoBehaviour {

        [SerializeField] protected VolumeRendering volume;
        [SerializeField] protected PinchSlider sliderXMin, sliderXMax, sliderYMin, sliderYMax, sliderZMin, sliderZMax;
        //[SerializeField] protected Transform axis;

        void Start ()
        {
            const float threshold = 0.025f;

            sliderXMin.OnValueUpdated.AddListener((sliderEventData) => {
                float v = sliderEventData.NewValue;
                volume.sliceXMin = Mathf.Min(v, volume.sliceXMax - threshold);
            });
            sliderXMax.OnValueUpdated.AddListener((sliderEventData) => {
                float v = sliderEventData.NewValue;
                volume.sliceXMax = Mathf.Max(v, volume.sliceXMin + threshold);
            });
            
            sliderYMin.OnValueUpdated.AddListener((sliderEventData) => {
                float v = sliderEventData.NewValue;
                volume.sliceYMin = Mathf.Min(v, volume.sliceYMax - threshold);
            });
            sliderYMax.OnValueUpdated.AddListener((sliderEventData) => {
                float v = sliderEventData.NewValue;
                volume.sliceYMax = Mathf.Max(v, volume.sliceYMin + threshold);
            });

            sliderZMin.OnValueUpdated.AddListener((sliderEventData) => {
                float v = sliderEventData.NewValue;
                volume.sliceZMin = Mathf.Min(v, volume.sliceZMax - threshold);
            });
            sliderZMax.OnValueUpdated.AddListener((sliderEventData) => {
                float v = sliderEventData.NewValue;
                volume.sliceZMax = Mathf.Max(v, volume.sliceZMin + threshold);
            });
            
        }

        /*void Update()
        {
            volume.axis = axis.rotation;
        }*/
        /*
        public void OnIntensity(float v)
        {
            volume.intensity = v;
        }

        public void OnThreshold(float v)
        {
            volume.threshold = v;
        }
        */
    }

}


