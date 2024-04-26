using JetBrains.Annotations;
using MSCLoader;
using UnityEngine;

// ReSharper disable MemberCanBePrivate.Global

// ReSharper disable Unity.InstantiateWithoutParent

namespace HighwayCarBreakdowns
{
    [UsedImplicitly]
    internal class HighwayCarBreakdowns : Mod
    {
        public override string Name => "Highway Car Breakdowns";

        public override string ID => "highwaycarbreakdowns";

        public override string Version => "1.2.0";

        public override string Author => "アカツキ";

        public override string Description =>
            "Cars on the highway can break down and you can fix it for some extra cash.";

        internal static SettingsSlider SpawnMultiplier;

        public override void ModSetup()
        {
            base.ModSetup();

            SetupFunction(Setup.OnLoad, Mod_OnLoad);
            SetupFunction(Setup.Update, Mod_OnUpdate);
            SetupFunction(Setup.OnGUI, Mod_OnGUI);
        }

        private void Mod_OnGUI()
        {
            if (!_enableGUI.GetValue()) return;
            if (Current is null) return;
            if (PaymentScript is null) return;
            if (Current.CarFixed || Current.InspectionTime < Current.TimeToInspect) return;

            GUI.color = Color.yellow;
            GUI.Label(new Rect(20, 120, 1000, 20), "Highway Car Breakdowns - Car Problem to Fix:");
            switch (Current.Problem)
            {
                case BrokenDownCarProblem.NoBattery:
                    GUI.Label(new Rect(20, 140, 1000, 20), "- Battery 0/1");
                    break;
                case BrokenDownCarProblem.NoCoolant:
                    GUI.Label(new Rect(20, 140, 1000, 20), "- Coolant 0/1");
                    break;
                case BrokenDownCarProblem.NoFuel:
                    GUI.Label(new Rect(20, 140, 1000, 20), "- Gasoline 0/1");
                    break;
                case BrokenDownCarProblem.NoOil:
                    GUI.Label(new Rect(20, 140, 1000, 20), "- Oil 0/1");
                    break;
                case BrokenDownCarProblem.NoClutchFluid:
                    GUI.Label(new Rect(20, 140, 1000, 20), "- Brake Fluid 0/1");
                    break;
                case BrokenDownCarProblem.NoOilFilter:
                    GUI.Label(new Rect(20, 140, 1000, 20), "- Oil Filter 0/1");
                    GUI.Label(new Rect(20, 160, 1000, 20), "- Oil 0/1");
                    break;
                default:
                    throw new System.ArgumentOutOfRangeException();
            }
        }

        public override void ModSettings()
        {
            base.ModSettings();
            // add a button for spawning a car manually
#if DEBUG
            Settings.AddButton(this, "Debug Spawn Car", () =>
            {
                if (HighwayCarUtilities.DetermineIfSpawn()) HighwayCarUtilities.SpawnRandomCar();
            });
            // add a button to teleport the player to the current car
            Settings.AddButton(this, "Debug Teleport Player", () =>
            {
                var player = GameObject.Find("PLAYER");
                var carTransform = Current.transform;
                player.transform.position = carTransform.position - carTransform.forward * 20;
            });
            // Button for manually triggering Payment script
            Settings.AddButton(this, "Debug Process Payment", () => { PaymentScript.ProcessingPayment = true; });
#endif

            Settings.AddText(this,
                "Here you can configure the spawn multiplier. Note that increasing the spawn multiplier will decrease the payment in order to keep the mod balanced.");
            SpawnMultiplier = Settings.AddSlider(this, "spawnMultiplier", "Spawn Multiplier", 1f, 5f, 1f);

            _enableGUI = Settings.AddCheckBox(this, "enablegui", "Enable GUI", true);

            Settings.AddButton(this, "Author Socials",
                () => { Application.OpenURL("https://linktr.ee/akatsuki2555"); });
        }

        private SettingsCheckBox _enableGUI;
        private float _timer;

        private void Mod_OnUpdate()
        {
            Update_SpawnCar();
        }

        private void Update_SpawnCar()
        {
            _timer += Time.deltaTime;
            if (_timer < 60 / SpawnMultiplier.GetValue()) return;
            _timer = 0;
            if (!HighwayCarUtilities.DetermineIfSpawn()) return;
            HighwayCarUtilities.SpawnRandomCar();
        }

        internal static BrokenDownCar Current { get; set; }
        internal static Payment PaymentScript { get; set; }

        internal void Mod_OnLoad()
        {
#if DEBUG
            HighwayCarUtilities.SpawnRandomCar();
            var player = GameObject.Find("PLAYER");
            player.transform.position = Current.transform.position - Current.transform.forward * 20;
#else
            // ModConsole.Log($"DetermineIfSpawn: {HighwayCarUtilities.DetermineIfSpawn()}");
            if (!HighwayCarUtilities.DetermineIfSpawn()) return;
            // ModConsole.Log("Spawning car");
            HighwayCarUtilities.SpawnRandomCar();
#endif
        }
    }
}