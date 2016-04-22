using System;
using System.Collections.Generic;
using System.Collections;
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
        [KSPField(guiActive = false, guiActiveEditor = false, guiName = "Core")]
        public string coreTemp;

        ModuleCoreHeat coreModule;
        double lastSkinTemp, lastIntTemp, lastCoreTemp;
        double skinRate, internalRate, coreRate;

        public void Start()
        {
            StartCoroutine(InitValues());
            coreModule = part.Modules.GetModules<ModuleCoreHeat>().FirstOrDefault();
            if (coreModule != null)
                Fields["coreTemp"].guiActive = true;
        }
        
        /// <summary>
        /// Make it so values don't start from zero causing a few seconds of unreliable output
        /// </summary>
        /// <returns></returns>
        public IEnumerator InitValues()
        {
            while (!HighLogic.LoadedSceneIsFlight || vessel.HoldPhysics)
                yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            lastSkinTemp = part.skinTemperature;
            lastIntTemp = part.temperature;
            if (coreModule != null)
                lastCoreTemp = coreModule.CoreTemperature;
            internalRate = skinRate = coreRate = 0;
        }

        // only needs to be in fixed update for the heating/cooling rate. Would be nice if I could calc that directly from fluxs
        public void FixedUpdate()
        {
            if (!HighLogic.LoadedSceneIsFlight || vessel.HoldPhysics)
                return;

            internalRate *= 0.9;
            internalRate += 0.1 * (part.temperature - lastIntTemp) / TimeWarp.fixedDeltaTime;
            lastIntTemp = part.temperature;

            skinRate *= 0.99;
            skinRate += 0.01 * (part.skinTemperature - lastSkinTemp) / TimeWarp.fixedDeltaTime;
            lastSkinTemp = part.skinTemperature;

            temp = string.Format("{0:0.00}K | Skin: {1:0.00}K", part.temperature, part.skinTemperature);
            heatingRate = string.Format("{0:0.00}K/s | Skin: {1:0.00}K/s", internalRate, skinRate);
            if (coreModule != null)
            {
                coreRate *= 0.9;
                coreRate += 0.1 * (coreModule.CoreTemperature - lastCoreTemp) / TimeWarp.fixedDeltaTime;
                lastCoreTemp = coreModule.CoreTemperature;
                coreTemp = string.Format("{0:0.00}K | Rate: {1:0.00}K/s", coreModule.CoreTemperature, coreRate);
            }
        }
    }
}
