using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Thermal_Monitor
{
    public class ThermalMonitor : PartModule
    {
        [KSPField(guiActive = true, guiActiveEditor = false, guiName = "Temperature")]
        public float temp;
        [KSPField(guiActive = true, guiActiveEditor = false, guiName = "Flux")]
        public float flux;
        [KSPField(guiActive = true, guiActiveEditor = false, guiName = "Thermal Mass")]
        public float thermalMass;
        
        

        public void Update()
        {
            if (!HighLogic.LoadedSceneIsFlight)
                return;

            thermalMass = (float)part.thermalMass;
            flux = (float)(part.thermalConductionFlux + part.thermalConvectionFlux + part.thermalRadiationFlux + part.thermalInternalFlux);
            temp = (float)(part.temperature);
        }
    }
}
