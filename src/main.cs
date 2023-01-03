using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using UnityEngine.Events;

namespace Mod
{
    public class Mod
    {
        public static void Main()
        {
            ModAPI.Register(
                new Modification()
                {
                    OriginalItem = ModAPI.FindSpawnable("Pistol"),
                    NameOverride = "Explosive Pistol",
                    DescriptionOverride = "Explosive Pistol!!",
                    CategoryOverride = ModAPI.FindCategory("Firearms"), 
                    ThumbnailOverride = ModAPI.LoadSprite("PistolTexture.png"),
                    AfterSpawn = (Instance) =>
                    {
                        Instance.AddComponent<ChangeBallistics>();
                        Instance.GetComponent<SpriteRenderer>().sprite = ModAPI.LoadSprite("PistolTexture.png");
                        var firearm = Instance.GetComponent<FirearmBehaviour>();

                        Cartridge customCartridge = ModAPI.FindCartridge("9mm"); 
                        customCartridge.name = "Explosive 9mm";
                        customCartridge.Damage *= 15f;
                        customCartridge.StartSpeed *= 457368f;
                        customCartridge.PenetrationRandomAngleMultiplier *= 1.4f;
                        customCartridge.Recoil *= 25f;
                        customCartridge.ImpactForce *= 10f;
                        firearm.Cartridge = customCartridge;

                        Instance.FixColliders();
                    }
                }
            );
        }
    }

    public class ChangeBallistics : MonoBehaviour 
    {
        FirearmBehaviour firearm;

        private void Start()
        {
            firearm = GetComponent<FirearmBehaviour>();
        }

        private void Update()
        {
            if (firearm.BallisticsEmitter != null)
            {
                ModAPI.Notify("it has loaded");
                var ballistics = firearm.BallisticsEmitter;
                ballistics.ImpactForceMultiplier *= 2.5f;
                ballistics.ExplosiveRoundParams = new ExplosionCreator.ExplosionParameters
                {
                    CreateParticlesAndSound = true,
                    LargeExplosionParticles = false,
                    DismemberChance = 1f,
                    FragmentForce = 8,
                    FragmentationRayCount = 32,
                    Range = 10
                };
                ballistics.ExplosiveRound = true;
                ModAPI.Notify("Changed stuff!");
                Destroy(this);
            }
        }
    }
}
