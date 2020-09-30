using UnityEngine;
using System.Collections.Generic;
using DaggerfallWorkshop;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Utility;
using DaggerfallWorkshop.Game.Items;

namespace SpellcastStudios.BetterFootsteps
{
    public class BetterFootstepsComponentPlayer : BetterFootstepsComponent
    {
        PlayerMotor playerMotor;

        protected override void Start()
        {
            playerMotor = GetComponent<PlayerMotor>();
            base.Start();
        }

        protected override bool IsOnExteriorWater()
        {
            return playerMotor.OnExteriorWater == PlayerMotor.OnExteriorWaterMethod.Swimming || playerMotor.OnExteriorWater == PlayerMotor.OnExteriorWaterMethod.WaterWalking;
        }

        protected override bool IsOnExteriorPath()
        {
            return playerMotor.OnExteriorPath;
        }

        protected override bool IsOnStaticGeometry()
        {
            return playerMotor.OnExteriorStaticGeometry;
        }

        protected override bool IsRunning()
        {
            return playerMotor.IsRunning;
        }

        protected override bool IsLevitating()
        {
            return playerMotor.IsLevitating;
        }

        protected override bool IsMovingLessThanHalfSpeed()
        {
            return playerMotor.IsMovingLessThanHalfSpeed;
        }

        protected override bool IsGrounded()
        {
            return playerMotor.IsGrounded;
        }

        protected override bool IsSwimming()
        {
            return playerMotor.IsSwimming;
        }

        protected override bool IsStandingStill()
        {
            return playerMotor.IsStandingStill;
        }
    }
}