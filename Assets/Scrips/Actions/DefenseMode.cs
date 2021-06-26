using System.Collections;
using UniBT;
using UniBT.Examples.Scripts;
using UnityEngine;

namespace Assets.Scrips.Actions
{
    public class DefenseMode: AttackMode
    {

        protected override bool IsUpdatable() => !base.IsUpdatable();
    }
}