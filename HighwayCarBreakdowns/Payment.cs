using System;
using System.Reflection;
using HutongGames.PlayMaker;
using MSCLoader;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HighwayCarBreakdowns
{
    internal class Payment : MonoBehaviour
    {
        [Obfuscation(Exclude = true)]
        private void Start()
        {
            _Start();
        }

        [Obfuscation(Exclude = true)]
        private void Update()
        {
            _Update();
        }

        private GameObject _shoulderRight;
        private Quaternion _initialArmRotation;
        private Quaternion _openArmRotation;
        private Quaternion _initialHandRotation;
        private Quaternion _openHandRotation;
        internal bool ProcessingPayment { get; set; }
        internal float PaymentValue { get; set; }
        internal GameObject Player;
        
        private void _Start()
        {
            Player = GameObject.Find("PLAYER");
            gameObject.name = "HighwayCarBreakdowns_Payment";
            Destroy(GetComponent<Renderer>());
            _shoulderRight = transform.parent.parent.parent.gameObject;
            _initialArmRotation = _shoulderRight.transform.localRotation;
            _openArmRotation = Quaternion.Euler(_initialArmRotation.eulerAngles + new Vector3(0, 0, -70));
            _initialHandRotation = transform.parent.localRotation;
            _openHandRotation = Quaternion.Euler(_initialHandRotation.eulerAngles + new Vector3(90, 0, 0));
            PaymentValue = Random.Range(500f, 1500f) / HighwayCarBreakdowns.SpawnMultiplier.GetValue();
        }


        private void _Update()
        {
            _shoulderRight.transform.localRotation = Quaternion.Lerp(_shoulderRight.transform.localRotation,
                ProcessingPayment ? _openArmRotation : _initialArmRotation, Time.deltaTime * 3f);
            transform.parent.localRotation = Quaternion.Lerp(transform.parent.localRotation,
                ProcessingPayment ? _openHandRotation : _initialHandRotation, Time.deltaTime * 3f);

            if (!ProcessingPayment || !(Vector3.Distance(Player.transform.position, transform.position) < 2f)) return;
            FsmVariables.GlobalVariables.GetFsmString("GUIinteraction").Value = $"TAKE MONEY {PaymentValue:0} MK";
            FsmVariables.GlobalVariables.GetFsmBool("GUIuse").Value = true;
            if (!Input.GetMouseButtonDown(0)) return;
            FsmVariables.GlobalVariables.GetFsmFloat("PlayerMoney").Value += PaymentValue;
            ProcessingPayment = false;
        }
    }
}