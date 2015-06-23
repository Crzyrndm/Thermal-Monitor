using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Thermal_Monitor
{
    public class ThermalMonitor : PartModule
    {
        [KSPField(guiActive = true, guiActiveEditor = false, guiName = "Part")]
        public string temp;
        [KSPField(guiActive = true, guiActiveEditor = false, guiName = "Part")]
        public string heatingRate;

        double lastSkinTemp, lastIntTemp;
        double skinRate, internalRate;
        public void FixedUpdate()
        {
            if (!HighLogic.LoadedSceneIsFlight)
                return;
            // a hack because how the heating rates should be calculated is not obvious
            internalRate = internalRate * 0.9 + 0.1 * (part.temperature - lastIntTemp) / TimeWarp.fixedDeltaTime ;
            lastIntTemp = part.temperature;

            skinRate = skinRate * 0.99 + 0.01 * (part.skinTemperature - lastSkinTemp) / TimeWarp.fixedDeltaTime;
            lastSkinTemp = part.skinTemperature;
            //double internalRate = (part.thermalConductionFlux + part.thermalInternalFluxPrevious + part.skinInternalConductionMult * (part.skinTemperature - part.temperature)) / part.thermalMass;
            //double skinRate = (part.thermalConvectionFlux + part.thermalRadiationFlux + part.skinInternalConductionMult * (part.temperature - part.skinTemperature)) / part.skinThermalMass;
            temp = string.Format("{0:0.00}K | Skin: {1:0.00}K", part.temperature, part.skinTemperature);
            heatingRate = string.Format("{0:0.00}K/s | Skin: {1:0.00}K/s", internalRate, skinRate);
        }
    }
}
