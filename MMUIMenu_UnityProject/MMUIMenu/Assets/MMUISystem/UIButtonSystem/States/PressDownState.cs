﻿using System;

namespace MMUISystem.UIButton
{
    public class PressDownState : StateBase
    {
        public PressDownState()
        {
            StateEnum = InteractionStateEnum.PressDown;
        }

        public override void EnterStateHandler(params object[] addParams)
        {
            StateEnterTime = DateTime.Now;

            FireOnEnterStateHandled();
        }

        public override void ExitStateHandler()
        {
            FireOnExitStateHandled();
        }

        public override void StateHandler()
        {
            FireOnStateHandled();

            FireOnNewStateRequested(CommandEnum.PressDown);
        }
    }
}
