using System;
using System.Reflection;
using HutongGames.PlayMaker;
using UnityEngine;
using Random = UnityEngine.Random;

// ReSharper disable MemberCanBePrivate.Global

namespace HighwayCarBreakdowns
{
    internal class BrokenDownCar : MonoBehaviour
    {
        internal GameObject Player;
        internal Camera MainCamera;
        internal float InspectionTime;
        internal BrokenDownCarProblem Problem;
        internal bool CarFixed;
        internal bool HasShowedProblem;
        internal GameObject Driver;

        internal float TimeToInspect = Random.Range(1f, 10f);

        [Obfuscation(Exclude = true)]
        internal void Start()
        {
            _Start();
        }

        private void _Start()
        {
            Player = GameObject.Find("PLAYER");
            Problem = (BrokenDownCarProblem)Random.Range(0, 5);
            Driver = HighwayCarUtilities.GetDriver(gameObject);
        }

        [Obfuscation(Exclude = true)]
        internal void Update()
        {
            _Update();
        }

        private void _Update()
        {
            VerifyNullVariables();

            ShowTheProblem();

            WaitForPlayerToInspect();

            if (InspectionTime > TimeToInspect) LookForItems();

            DestroyCarIfFarAway();
        }

        private void DestroyCarIfFarAway()
        {
            if (CarFixed && Vector3.Distance(Player.transform.position, transform.position) > 500f)
                Destroy(gameObject);
        }

        internal void VerifyNullVariables()
        {
            if (MainCamera is null) MainCamera = Camera.main;
            if (Player is null) Player = GameObject.Find("PLAYER");
            if (Driver is null) Driver = HighwayCarUtilities.GetDriver(gameObject);
        }

        internal void ShowTheProblem()
        {
            if (Vector3.Distance(Player.transform.position, Driver.transform.position) >= 2 ||
                HasShowedProblem) return;
            FsmVariables.GlobalVariables.GetFsmString("GUIsubtitle").Value =
                "\"Hello there, I need your help, my car is broken.\"";

            HasShowedProblem = true;
        }

        internal void LookForItems()
        {
            if (InspectionTime < TimeToInspect) return;
            if (CarFixed) return;

            foreach (var collider in Physics.OverlapSphere(Driver.transform.position, 1f))
            {
                if (collider.gameObject.name != "motor oil(itemx)" &&
                    collider.gameObject.name != "coolant(itemx)" &&
                    collider.gameObject.name != "brake fluid(itemx)" &&
                    collider.gameObject.name != "battery(Clone)" &&
                    collider.gameObject.name != "gasoline(itemx)" &&
                    collider.gameObject.name != "oil filter(Clone)") continue;
                var result = HighwayCarUtilities.DetermineGivenItem(collider.gameObject);
                DisplayMessageForGivenItem(result);
            }
        }

        internal void DisplayMessageForGivenItem(HighwayCarBreakdownsItemDetermination result)
        {
            switch (result)
            {
                case HighwayCarBreakdownsItemDetermination.ItemIsNotItem:
                    return;
                case HighwayCarBreakdownsItemDetermination.OilGiven:
                case HighwayCarBreakdownsItemDetermination.CoolantGiven:
                case HighwayCarBreakdownsItemDetermination.GasolineGiven:
                case HighwayCarBreakdownsItemDetermination.BrakeFluidGiven:
                case HighwayCarBreakdownsItemDetermination.BatteryGiven:
                    FsmVariables.GlobalVariables.GetFsmString("GUIsubtitle").Value =
                        "\"Thank you so much for helping me! Here's a bit of money for you.\"";
                    CarFixed = true;
                    // FsmVariables.GlobalVariables.GetFsmFloat("PlayerMoney").Value += Random.Range(500, 1500) /
                    //     HighwayCarBreakdowns.SpawnMultiplier.GetValue();
                    HighwayCarBreakdowns.PaymentScript.ProcessingPayment = true;
                    break;
                case HighwayCarBreakdownsItemDetermination.OilFilterGiven:
                    FsmVariables.GlobalVariables.GetFsmString("GUIsubtitle").Value =
                        "\"Thank you for the oil filter. Can you also provide me with oil to replace it as well?\"";
                    HighwayCarBreakdowns.Current.Problem = BrokenDownCarProblem.NoOil;
                    break;
                case HighwayCarBreakdownsItemDetermination.OilTooLittle:
                    FsmVariables.GlobalVariables.GetFsmString("GUIsubtitle").Value =
                        "\"I am afraid that this little oil is not enough to fix the car.\"";
                    break;
                case HighwayCarBreakdownsItemDetermination.CoolantTooLittle:
                    FsmVariables.GlobalVariables.GetFsmString("GUIsubtitle").Value =
                        "\"I am afraid that this little coolant is not enough to fix the car.\"";
                    break;
                case HighwayCarBreakdownsItemDetermination.GasolineTooLittle:
                    FsmVariables.GlobalVariables.GetFsmString("GUIsubtitle").Value =
                        "\"I am afraid that this little gasoline is not enough to get me to the store.\"";
                    break;
                case HighwayCarBreakdownsItemDetermination.BrakeFluidTooLittle:
                    FsmVariables.GlobalVariables.GetFsmString("GUIsubtitle").Value =
                        "\"I am afraid that this little clutch fluid is not enough to fix the car.\"";
                    break;
                case HighwayCarBreakdownsItemDetermination.BatteryTooLittle:
                    FsmVariables.GlobalVariables.GetFsmString("GUIsubtitle").Value =
                        "\"I am afraid that this little charge in the battery is not enough to fix the car.\"";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        internal void WaitForPlayerToInspect()
        {
            foreach (var raycastHit in Physics.RaycastAll(Player.transform.position, Player.transform.forward, 3f))
                if (raycastHit.collider.gameObject == gameObject)
                {
                    var interact = FsmVariables.GlobalVariables.GetFsmString("GUIinteraction");
                    var use = FsmVariables.GlobalVariables.GetFsmBool("GUIuse");

                    if (InspectionTime < TimeToInspect)
                    {
                        interact.Value = "Inspect the car";
                        use.Value = true;
                    }

                    if (InspectionTime < TimeToInspect && Input.GetKey(KeyCode.F))
                    {
                        InspectionTime += Time.deltaTime;
                        interact.Value = $"Inspecting: {InspectionTime / TimeToInspect:P0}";
                        use.Value = true;
                    }

                    if (InspectionTime < TimeToInspect && Input.GetKeyUp(KeyCode.F)) InspectionTime = 0;

                    if (InspectionTime > TimeToInspect && !CarFixed)
                    {
                        ShowRightProblem(interact);
                    }
                }
        }

        internal void ShowRightProblem(FsmString interact)
        {
            switch (Problem)
            {
                case BrokenDownCarProblem.NoFuel:
                    interact.Value = "The car has no fuel and therefore it can't start.";
                    break;
                case BrokenDownCarProblem.NoOil:
                    interact.Value = "The car has no oil and therefore the driver can't continue.";
                    break;
                case BrokenDownCarProblem.NoCoolant:
                    interact.Value = "The car has no coolant and therefore the driver can't continue.";
                    break;
                case BrokenDownCarProblem.NoClutchFluid:
                    interact.Value = "The car has no clutch fluid and therefore the driver can't continue.";
                    break;
                case BrokenDownCarProblem.NoBattery:
                    interact.Value = "The car has a dead battery and therefore can't start.";
                    break;
                case BrokenDownCarProblem.NoOilFilter:
                    interact.Value = "The car has a seized oil filter and therefore the driver can't continue.";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}