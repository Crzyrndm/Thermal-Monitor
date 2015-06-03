using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Thermal_Monitor
{
    public class ThermalMonitor : PartModule
    {
        [KSPField(guiActive = true, guiActiveEditor = false, guiName = "Temperature", guiUnits = " K", guiFormat = "F2")]
        public float temp;
        [KSPField(guiActive = true, guiActiveEditor = false, guiName = "Degrees / s", guiUnits = " K", guiFormat = "F3")]
        public float heatingRate;
        
        public void Update()
        {
            if (!HighLogic.LoadedSceneIsFlight)
                return;

            heatingRate = (float)((part.thermalConductionFlux + part.thermalConvectionFlux + part.thermalRadiationFlux + part.thermalInternalFluxPrevious) / part.thermalMass);
            temp = (float)(part.temperature);
        }
    }
}
