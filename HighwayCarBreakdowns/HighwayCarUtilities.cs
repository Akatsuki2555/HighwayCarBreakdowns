using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using MSCLoader;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable Unity.InstantiateWithoutParent

namespace HighwayCarBreakdowns
{
    internal static class HighwayCarUtilities
    {
        /// <summary>
        /// Chooses a random car from the given array of cars.
        /// </summary>
        /// <returns>The randomly selected car as a string.</returns>
        private static string ChooseRandomCar()
        {
            string[] cars = { "SVOBODA", "LAMORE", "VICTRO", "FITTAN", "POLSA" };

            var selectedCar = cars[Random.Range(0, cars.Length)];
            return selectedCar;
        }

        /// <summary>
        /// Generates a clone of a generic car object.
        /// </summary>
        /// <param name="car">The name of the car to clone.</param>
        /// <returns>The cloned GameObject.</returns>
        private static GameObject CarClone(string car)
        {
            switch (car)
            {
                case "SVOBODA":
                    var cloneSvoboda = CloneSvoboda();
                    var colliderSvoboda = cloneSvoboda.AddComponent<BoxCollider>();
                    colliderSvoboda.size = new Vector3(1.1f, 1, 3.4f);
                    colliderSvoboda.center = Vector3.up * 0.2f;
                    var rigidbodySvoboda = cloneSvoboda.AddComponent<Rigidbody>();
                    rigidbodySvoboda.mass = 10000;
                    return cloneSvoboda;

                case "LAMORE":
                    var cloneLamore = CloneLamore();
                    var colliderLamore = cloneLamore.AddComponent<BoxCollider>();
                    colliderLamore.size = new Vector3(1.1f, 1, 3.4f);
                    colliderLamore.center = Vector3.up * 0.2f;
                    var rigidbodyLamore = cloneLamore.AddComponent<Rigidbody>();
                    rigidbodyLamore.mass = 10000;
                    return cloneLamore;

                case "VICTRO":
                    var cloneVictro = CloneVictro();
                    var colliderVictro = cloneVictro.AddComponent<BoxCollider>();
                    colliderVictro.size = new Vector3(1.1f, 1, 3.4f);
                    colliderVictro.center = Vector3.up * 0.2f;
                    var rigidbodyVictro = cloneVictro.AddComponent<Rigidbody>();
                    rigidbodyVictro.mass = 10000;
                    return cloneVictro;

                case "FITTAN":
                    var cloneFittan = CloneFittan();
                    var colliderFittan = cloneFittan.AddComponent<BoxCollider>();
                    colliderFittan.size = new Vector3(1.1f, 1, 3.4f);
                    colliderFittan.center = Vector3.up * 0.2f;
                    var rigidbodyFittan = cloneFittan.AddComponent<Rigidbody>();
                    rigidbodyFittan.mass = 10000;
                    return cloneFittan;

                case "POLSA":
                    var clonePolsa = ClonePolsa();
                    var colliderPolsa = clonePolsa.AddComponent<BoxCollider>();
                    colliderPolsa.size = new Vector3(1.1f, 1, 3.4f);
                    colliderPolsa.center = Vector3.up * 0.2f;
                    var rigidbodyPolsa = clonePolsa.AddComponent<Rigidbody>();
                    rigidbodyPolsa.mass = 10000;
                    return clonePolsa;

                default:
                    throw new Exception("Car not found or not implemented: " + car);
            }
        }

        private static GameObject CloneSvoboda()
        {
            var prefab = GetPrefabFor("SVOBODA");
            var car = new GameObject("SVOBODA(Clone)");

            for (var i = 2; i < 7; i++)
            {
                CopyPartAtIndex(prefab, car, i, out var part);
                if (part.GetComponent<Wheel>() != null) Object.Destroy(part.GetComponent<Wheel>());
            }

            CopyPartAtIndex(prefab, car, 10, out var lod);
            Object.Destroy(lod.transform.GetChild(4).gameObject);
            var driver = Object.Instantiate(lod.transform.GetChild(6).GetChild(0).gameObject);
            driver.transform.parent = car.transform;
            driver.transform.localPosition = new Vector3(0, 0.5f, -3f);
            driver.transform.localRotation = Quaternion.Euler(0, 90, 0);
            SetupDriver(driver.gameObject);

            Object.Destroy(lod.transform.GetChild(6).gameObject);

            return car;
        }

        private static GameObject CloneLamore()
        {
            var prefab = GetPrefabFor("LAMORE");
            var car = new GameObject("LAMORE(Clone)");

            for (var i = 3; i < 7; i++)
            {
                CopyPartAtIndex(prefab, car, i, out var part);
                if (part.GetComponent<Wheel>() != null) Object.Destroy(part.GetComponent<Wheel>());
            }

            for (var i = 9; i < 12; i++)
            {
                CopyPartAtIndex(prefab, car, i, out var part);
                if (part.GetComponent<Wheel>() != null) Object.Destroy(part.GetComponent<Wheel>());
            }

            CopyPartAtIndex(prefab, car, 15, out var lod);
            Object.Destroy(lod.transform.GetChild(0).gameObject);
            var driver = Object.Instantiate(lod.transform.GetChild(2).GetChild(0).gameObject);
            driver.transform.parent = car.transform;
            driver.transform.localPosition = new Vector3(0, 0.5f, -3f);
            driver.transform.localRotation = Quaternion.Euler(0, 90, 0);
            SetupDriver(driver);

            Object.Destroy(lod.transform.GetChild(2).GetChild(0).gameObject);

            return car;
        }

        private static GameObject CloneVictro()
        {
            var prefab = GetPrefabFor("VICTRO");
            var car = new GameObject("VICTRO(Clone)");

            for (var i = 2; i < 6; i++)
            {
                CopyPartAtIndex(prefab, car, i, out var part);
                if (part.GetComponent<Wheel>() != null) Object.Destroy(part.GetComponent<Wheel>());
            }

            CopyPartAtIndex(prefab, car, 8, out _);
            CopyPartAtIndex(prefab, car, 9, out _);
            CopyPartAtIndex(prefab, car, 11, out _);

            CopyPartAtIndex(prefab, car, 15, out var lod);

            var driver = Object.Instantiate(lod.transform.GetChild(0).GetChild(0).gameObject);
            driver.transform.parent = car.transform;
            driver.transform.localPosition = new Vector3(0, 0.5f, -3f);
            driver.transform.localRotation = Quaternion.Euler(0, 90, 0);
            SetupDriver(driver);

            Object.Destroy(lod.transform.GetChild(0).gameObject);
            Object.Destroy(lod.transform.GetChild(2).gameObject);

            return car;
        }

        private static GameObject CloneFittan()
        {
            var prefab = GetPrefabFor("FITTAN");
            var car = new GameObject("FITTAN(Clone)");

            for (var i = 3; i < 8; i++)
            {
                CopyPartAtIndex(prefab, car, i, out var part);
                if (part.GetComponent<Wheel>() != null) Object.Destroy(part.GetComponent<Wheel>());
            }

            for (int i = 10; i < 14; i++)
            {
                CopyPartAtIndex(prefab, car, i, out _);
            }

            CopyPartAtIndex(prefab, car, 18, out var lod);
            Object.Destroy(lod.transform.GetChild(3).gameObject);
            var driver = Object.Instantiate(lod.transform.GetChild(5).GetChild(0).gameObject);
            driver.transform.parent = car.transform;
            driver.transform.localPosition = new Vector3(0, 0.5f, -3f);
            driver.transform.localRotation = Quaternion.Euler(0, 90, 0);
            SetupDriver(driver);

            Object.Destroy(lod.transform.GetChild(5).gameObject);

            return car;
        }

        private static GameObject ClonePolsa()
        {
            var prefab = GetPrefabFor("POLSA");
            var car = new GameObject("POLSA(Clone)");

            for (var i = 7; i < 12; i++)
            {
                CopyPartAtIndex(prefab, car, i, out var part);
                if (part.GetComponent<Wheel>() != null) Object.Destroy(part.GetComponent<Wheel>());
            }

            CopyPartAtIndex(prefab, car, 15, out var lod);

            var driver = Object.Instantiate(lod.transform.GetChild(3).GetChild(0).gameObject);
            driver.transform.parent = car.transform;
            driver.transform.localPosition = new Vector3(0, 0.5f, -3f);
            driver.transform.localRotation = Quaternion.Euler(0, 90, 0);
            SetupDriver(driver);

            Object.Destroy(lod.transform.GetChild(3).gameObject);
            Object.Destroy(lod.transform.GetChild(4).gameObject);

            return car;
        }

        internal static void SetupDriver(GameObject driver)
        {
            // driver is a rig

            // get the skeleton
            var skeleton = driver.transform.GetChild(1).gameObject;

            // set spine_middle
            skeleton.transform.GetChild(0).GetChild(0).transform.localRotation = Quaternion.Euler(0, 0, 0);
            // set pelvis
            skeleton.transform.GetChild(0).transform.localRotation = Quaternion.Euler(90, 0, -90);
            var spineUpper = skeleton.transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
            // set spine_upper
            spineUpper.transform.localRotation = Quaternion.Euler(0, 0, 0);

            SetupDriverUpperSpine(spineUpper);
            SetupDriverSkeleton(skeleton);
        }

        internal static void SetupDriverSkeleton(GameObject skeleton)
        {
            // left leg
            skeleton.transform.GetChild(1).transform.localRotation = Quaternion.Euler(-90, -90, 0);
            skeleton.transform.GetChild(1).GetChild(0).transform.localRotation = Quaternion.Euler(0, 0, 0);
            skeleton.transform.GetChild(1).GetChild(0).GetChild(0).transform.localRotation =
                Quaternion.Euler(0, 0, -90);

            // right leg
            skeleton.transform.GetChild(2).transform.localRotation = Quaternion.Euler(-90, 90, 180);
            skeleton.transform.GetChild(2).GetChild(0).transform.localRotation = Quaternion.Euler(0, 0, 0);
            skeleton.transform.GetChild(2).GetChild(0).GetChild(0).transform.localRotation =
                Quaternion.Euler(0, 0, -90);
        }

        internal static void SetupDriverUpperSpine(GameObject spineUpper)
        {
            // head
            spineUpper.transform.GetChild(2).transform.localRotation = Quaternion.Euler(-90,
                180, 90);
            spineUpper.transform.GetChild(2).GetChild(0).transform.localRotation =
                Quaternion.Euler(180, -90, -90);

            // left arm
            spineUpper.transform.GetChild(0).transform.localRotation = Quaternion.Euler(-180 + Random.Range(-5f, 5f),
                -80 + Random.Range(-5f, 5f), 180 + Random.Range(-5f, 5f));
            spineUpper.transform.GetChild(0).GetChild(0).transform.localRotation =
                Quaternion.Euler(0 + Random.Range(-5f, 5f), 65 + Random.Range(-5f, 5f), 0 + Random.Range(-5f, 5f));
            spineUpper.transform.GetChild(0).GetChild(0).GetChild(0).transform.localRotation =
                Quaternion.Euler(0 + Random.Range(-5f, 5f), 15 + Random.Range(-5f, 5f), 0 + Random.Range(-5f, 5f));

            // right arm
            spineUpper.transform.GetChild(1).transform.localRotation = Quaternion.Euler(180 + Random.Range(-5f, 5f),
                80 + Random.Range(-5f, 5f), 180 + Random.Range(-5f, 5f));
            spineUpper.transform.GetChild(1).GetChild(0).transform.localRotation =
                Quaternion.Euler(0 + Random.Range(-5f, 5f), -65 + Random.Range(-5f, 5f), 0 + Random.Range(-5f, 5f));
            var rightHand = spineUpper.transform.GetChild(1).GetChild(0).GetChild(0);
            rightHand.transform.localRotation =
                Quaternion.Euler(0 + Random.Range(-5f, 5f), -15 + Random.Range(-5f, 5f), 0 + Random.Range(-5f, 5f));

            var interaction = GameObject.CreatePrimitive(PrimitiveType.Cube);
            interaction.transform.localScale = Vector3.one * 0.1f;
            interaction.transform.parent = rightHand.GetChild(0);
            interaction.transform.localPosition = Vector3.zero;
            HighwayCarBreakdowns.PaymentScript = interaction.AddComponent<Payment>();
        }

        [CanBeNull]
        internal static GameObject GetDriver(GameObject car)
        {
            // go through the children and find "Driver(Clone)" and return it
            for (var i = 0; i < car.transform.childCount; i++)
            {
                if (car.transform.GetChild(i).gameObject.name == "Driver(Clone)")
                {
                    return car.transform.GetChild(i).gameObject;
                }
            }

            ModConsole.Warning($"Couldn't find Driver(Clone) on {car.name}!");
            return null;
        }

        internal static HighwayCarBreakdownsItemDetermination DetermineGivenItem(GameObject item)
        {
            switch (item.name)
            {
                case "coolant(itemx)" when HighwayCarBreakdowns.Current.Problem == BrokenDownCarProblem.NoCoolant:
                {
                    var fluid = item.GetPlayMaker("Use").FsmVariables.GetFsmFloat("Fluid");
                    if (fluid.Value < 5) return HighwayCarBreakdownsItemDetermination.CoolantTooLittle;
                    Object.Destroy(item);
                    return HighwayCarBreakdownsItemDetermination.CoolantGiven;
                }
                case "motor oil(itemx)" when HighwayCarBreakdowns.Current.Problem == BrokenDownCarProblem.NoOil:
                {
                    var fluid = item.GetPlayMaker("Use").FsmVariables.GetFsmFloat("Fluid");
                    if (fluid.Value < 2) return HighwayCarBreakdownsItemDetermination.OilTooLittle;
                    Object.Destroy(item);
                    return HighwayCarBreakdownsItemDetermination.OilGiven;
                }
                case "brake fluid(itemx)"
                    when HighwayCarBreakdowns.Current.Problem == BrokenDownCarProblem.NoClutchFluid:
                {
                    var fluid = item.GetPlayMaker("Use").FsmVariables.GetFsmFloat("Fluid");
                    if (fluid.Value < 0.5f) return HighwayCarBreakdownsItemDetermination.BrakeFluidTooLittle;
                    Object.Destroy(item);
                    return HighwayCarBreakdownsItemDetermination.BrakeFluidGiven;
                }
                case "battery(Clone)"
                    when HighwayCarBreakdowns.Current.Problem == BrokenDownCarProblem.NoBattery:
                {
                    var charge = item.GetPlayMaker("Use").FsmVariables.GetFsmFloat("Charge");
                    if (charge.Value < 11.5f) return HighwayCarBreakdownsItemDetermination.BatteryTooLittle;
                    Object.Destroy(item);
                    return HighwayCarBreakdownsItemDetermination.BatteryGiven;
                }
                case "gasoline(itemx)" when HighwayCarBreakdowns.Current.Problem == BrokenDownCarProblem.NoFuel:
                {
                    var fluid = item.transform.GetChild(0).GetPlayMaker("Data").FsmVariables.GetFsmFloat("Fluid");
                    if (fluid.Value < 5) return HighwayCarBreakdownsItemDetermination.GasolineTooLittle;
                    fluid.Value = 0;
                    return HighwayCarBreakdownsItemDetermination.GasolineGiven;
                }
                case "oil filter(Clone)" when HighwayCarBreakdowns.Current.Problem == BrokenDownCarProblem.NoOilFilter:
                {
                    Object.Destroy(item);
                    return HighwayCarBreakdownsItemDetermination.OilFilterGiven;
                }
                default:
                    return HighwayCarBreakdownsItemDetermination.ItemIsNotItem;
            }
        }

        /// <summary>
        /// Copies a part from the original GameObject at the specified index and attaches it to the cloned GameObject.
        /// </summary>
        /// <param name="original">The original GameObject from which to copy the part.</param>
        /// <param name="cloned">The cloned GameObject to which the copied part will be attached.</param>
        /// <param name="i">The index of the part to copy.</param>
        /// <param name="part">The copied part that was attached to the cloned GameObject.</param>
        internal static void CopyPartAtIndex(GameObject original, GameObject cloned, int i, out GameObject part)
        {
            var origPart = original.transform.GetChild(i).gameObject;
            var clonedPart = Object.Instantiate(origPart);
            clonedPart.transform.parent = cloned.transform;
            clonedPart.transform.localPosition = origPart.transform.localPosition;
            clonedPart.transform.localRotation = origPart.transform.localRotation;
            clonedPart.name = original.transform.GetChild(i).gameObject.name;

            part = clonedPart;
        }

        /// <summary>
        /// Retrieves the prefab GameObject for the specified car.
        /// </summary>
        /// <param name="car">The name of the car.</param>
        /// <returns>The prefab GameObject for the specified car, or null if the car is not found.</returns>
        internal static GameObject GetPrefabFor(string car)
        {
            var traffic = GameObject.Find("TRAFFIC");
            // ModConsole.Log("TRAFFIC: " + GeneratePathFor(traffic.transform));
            // ModConsole.Log("TRAFFIC: " + traffic.transform.childCount + " children found.");
            switch (car)
            {
                case "SVOBODA":
                    return traffic.transform.GetChild(7).GetChild(1).gameObject;
                case "LAMORE":
                    return traffic.transform.GetChild(7).GetChild(2).gameObject;
                case "VICTRO":
                    return traffic.transform.GetChild(7).GetChild(4).gameObject;
                case "FITTAN":
                    return traffic.transform.GetChild(7).GetChild(8).gameObject;
                case "POLSA":
                    return traffic.transform.GetChild(7).GetChild(9).gameObject;
                default:
                    return null;
            }
        }

        // Generates a path for the given Transform object by traversing its parent hierarchy and concatenating the names, separated by slashes. Returns the generated path as a string.
        internal static string GeneratePathFor(Transform obj)
        {
            var path = "";
            while (obj != null)
            {
                path = path + "/" + obj.name;
                obj = obj.parent;
            }

            return path;
        }

        internal static readonly List<Vector3> SpawnPoints = new List<Vector3>()
        {
            new Vector3(-1233.104f, 8.665f, -1216.86f),
            new Vector3(-131.39f, 2.93f, -1408.27f),
            new Vector3(572.238f, 3.243f, -1409.199f),
            new Vector3(2227.922f, 6.83f, 39.409f),
            new Vector3(2213.202f, 8.463f, 307.16f),
            new Vector3(1086.36f, 12.81f, 1302.35f)
        };

        internal static readonly List<Quaternion> SpawnRotations = new List<Quaternion>()
        {
            Quaternion.Euler(179.399f, -84.37399f, -178.568f),
            Quaternion.Euler(177.839f, 99.141f, -180.797f),
            Quaternion.Euler(-1.601f, 86.135f, 1.152f),
            Quaternion.Euler(-0.271f, -0.426f, 1.882f),
            Quaternion.Euler(-0.162f, 175.494f, -1.238f),
            Quaternion.Euler(-0.507f, -58.035f, 2.275f)
        };

        /// <summary>
        /// Returns a random index within the range of the SpawnPoints list.
        /// </summary>
        /// <returns>An integer representing the random index.</returns>
        internal static int GetSpawnPointIndex() => Random.Range(0, SpawnPoints.Count);

        /// <summary>
        /// Returns the spawn point for the given index.
        /// </summary>
        /// <param name="index">The index of the spawn point.</param>
        /// <returns>The spawn point at the specified index.</returns>
        internal static Vector3 GetSpawnPointForIndex(int index) => SpawnPoints[index];

        /// <summary>
        /// Returns the spawn rotation for the given index.
        /// </summary>
        /// <param name="index">The index of the spawn rotation to retrieve.</param>
        /// <returns>The spawn rotation for the given index.</returns>
        internal static Quaternion GetSpawnRotationForIndex(int index) => SpawnRotations[index];

        /// <summary>
        /// Determines if a spawn should occur.
        /// </summary>
        /// <returns>True if a spawn should occur, false otherwise.</returns>
        internal static bool DetermineIfSpawn()
        {
            // 10% chance of spawn and if there's not a vehicle spawned already
            var random = Random.Range(0, 100);
            // ModConsole.Log("Current: " + HighwayCarBreakdowns.Current + " Random Range: " + random);
            return random < 10 && HighwayCarBreakdowns.Current == null;
        }

        /// <summary>
        /// Spawns a random car at a random spawn point with a random rotation.
        /// </summary>
        /// <returns>The spawned car with the BrokenDownCar component attached.</returns>
        internal static void SpawnRandomCar()
        {
            var car = ChooseRandomCar();
            var instance = CarClone(car);
            var point = GetSpawnPointIndex();
            instance.transform.position = GetSpawnPointForIndex(point);
            instance.transform.rotation = GetSpawnRotationForIndex(point);
            HighwayCarBreakdowns.Current = instance.AddComponent<BrokenDownCar>();
        }
    }
}