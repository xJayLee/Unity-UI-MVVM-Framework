﻿namespace MMUISystem.UIButton
{
    public class ElapsedTimeIsLowerThan : ConditionBase
    {
        float _targetMilliSeconds;

        public ElapsedTimeIsLowerThan(int targetMilliSeconds)
        {
            _targetMilliSeconds = targetMilliSeconds;
        }

        public override bool CheckCondition(params object[] param)
        {
            int passedTime = (int)param[0];

            return passedTime <= _targetMilliSeconds;
        }
    }
}
