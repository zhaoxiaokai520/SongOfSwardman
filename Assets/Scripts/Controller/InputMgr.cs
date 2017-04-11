using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Managers
{
    class InputMgr : Singleton<InputMgr>
    {
        int mCurrLevel;//current input level
        int mPrevLevel = -1;// negative level is no meanings

        public void SetLevel(int inputLvl)
        {
            if (inputLvl != mCurrLevel)
            {
                if (mPrevLevel < 0)
                {
                    mPrevLevel = mCurrLevel;
                }

                mCurrLevel = inputLvl;
            }
        }

        public int GetLevel()
        {
            return mCurrLevel;
        }

        public void RestoreLevel()
        {
            if (mPrevLevel >= 0)
            {
                mCurrLevel = mPrevLevel;
                mPrevLevel = -1;
            }
        }
    }
}
