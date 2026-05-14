using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//彩金相关
public partial class IOCanvasModel
{



    private float releaseParams = 1.1f;

    public int MINI_MIN_BASE_VALUE
    { get { return tempJackpotCfgData.BaseSetValue; } }

    public int MINI_MAX_BASE_VALUE
    { get { return tempJackpotCfgData.BaseSetValue * 10; } }

    public int MINI_MIN_MIN_TRIGGER_VALUE
    { get { return (int)(TempMiniBaseValue * releaseParams); } }

    public int MINI_MAX_MIN_TRIGGER_VALUE
    { get { return int.Parse((TempMiniMaxTriggerValue * 0.9f).ToString()); } }

    public int MINI_MIN_MAX_TRIGGER_VALUE
    { get { return (int)(TempMiniBaseValue * 1.5f); } }

    public int MINI_MAX_MAX_TRIGGER_VALUE
    { get { return (TempMiniBaseValue * 10); } }

    public int MINI_MIN_MAX_BET
    { get { return TempMiniMinBet; } }


    public int MINOR_MIN_BASE_VALUE
    { get { return TempMiniBaseValue; } }

    public int MINOR_MAX_BASE_VALUE
    { get { return TempMiniBaseValue * 10; } }

/*#seaweed#  
    public int MINOR_MIN_BASE_VALUE
    { get { return TempMiniBaseValue > MINI_MIN_BASE_VALUE ? TempMiniBaseValue : MINI_MIN_BASE_VALUE; } }

    public int MINOR_MAX_BASE_VALUE
    { get { return TempMiniBaseValue * 10  > MINI_MAX_BASE_VALUE? TempMiniBaseValue * 10 : MINI_MAX_BASE_VALUE; } }
*/


    public int MINOR_MIN_MIN_TRIGGER_VALUE
    { get { return (int)(TempMinorBaseValue * releaseParams); } }

    public int MINOR_MAX_MIN_TRIGGER_VALUE
    { get { return int.Parse((TempMinorMaxTriggerValue * 0.9f).ToString()); } }

    public int MINOR_MIN_MAX_TRIGGER_VALUE
    { get { return (int)(TempMinorBaseValue * 1.5f); } }

    public int MINOR_MAX_MAX_TRIGGER_VALUE
    { get { return (TempMinorBaseValue * 10); } }

    public int MINOR_MIN_MAX_BET
    { get { return TempMinorMinBet; } }

    public int MAJOR_MIN_BASE_VALUE
    { get { return TempMinorBaseValue; } }

    public int MAJOR_MAX_BASE_VALUE
    { get { return TempMinorBaseValue * 10; } }

    public int MAJOR_MIN_MIN_TRIGGER_VALUE
    { get { return (int)(TempMajorBaseValue * releaseParams); } }

    public int MAJOR_MAX_MIN_TRIGGER_VALUE
    { get { return int.Parse((TempMajorMaxTriggerValue * 0.9f).ToString()); } }

    public int MAJOR_MIN_MAX_TRIGGER_VALUE
    { get { return (int)(TempMajorBaseValue * 1.5f); } }

    public int MAJOR_MAX_MAX_TRIGGER_VALUE
    { get { return (TempMajorBaseValue * 10); } }

    public int MAJOR_MIN_MAX_BET
    { get { return TempMajorMinBet; } }

    public int GRAND_MIN_BASE_VALUE
    { get { return TempMajorBaseValue; } }

    public int GRAND_MAX_BASE_VALUE
    { get { return TempMajorBaseValue * 10; } }

    public int GRAND_MIN_MIN_TRIGGER_VALUE
    { get { return (int)(TempGrandBaseValue * releaseParams); } }

    public int GRAND_MAX_MIN_TRIGGER_VALUE
    { get { return int.Parse((TempGrandMaxTriggerValue * 0.9f).ToString()); } }

    public int GRAND_MIN_MAX_TRIGGER_VALUE
    { get { return (int)(TempGrandBaseValue * 1.5f); } }

    public int GRAND_MAX_MAX_TRIGGER_VALUE
    { get { return (TempGrandBaseValue * 10); } }

    public int GRAND_MIN_MAX_BET
    { get { return TempGrandMinBet; } }

    public int TempMiniBaseValue
    {
        set
        {
            var minJackpot = tempJackpotCfgData.sBoxJackpotConfigDataItem[3];
            minJackpot.BaseValue = value;
            minJackpot.MinTriggerValue = (int)(minJackpot.BaseValue * releaseParams);
            minJackpot.MaxTriggerValue = (int)(minJackpot.BaseValue * 1.5f);

            TempMinorBaseValue = value;

            IOCanvasManager.Instance.UpdateParamsPanle();
        }
        get { return tempJackpotCfgData.sBoxJackpotConfigDataItem[3].BaseValue; }
    }

    public int TempMiniMinTriggerValue //#seaweed新加
    {
        set
        {
            var minJackpot = tempJackpotCfgData.sBoxJackpotConfigDataItem[3];
            minJackpot.MinTriggerValue = value;
            IOCanvasManager.Instance.UpdateParamsPanle();
        }
        get { return tempJackpotCfgData.sBoxJackpotConfigDataItem[3].MinTriggerValue; }
    }


    public int TempMiniMaxTriggerValue
    {
        set
        {
            var minJackpot = tempJackpotCfgData.sBoxJackpotConfigDataItem[3];
            minJackpot.MaxTriggerValue = value;
            minJackpot.MinTriggerValue = minJackpot.MinTriggerValue > minJackpot.MaxTriggerValue * 0.9f ? int.Parse((minJackpot.MaxTriggerValue * 0.9f).ToString()) : minJackpot.MinTriggerValue;
            IOCanvasManager.Instance.UpdateParamsPanle();
        }
        get { return tempJackpotCfgData.sBoxJackpotConfigDataItem[3].MaxTriggerValue; }
    }


    public int TempMiniWeight //#seaweed新加
    {
        set
        {
            var minJackpot = tempJackpotCfgData.sBoxJackpotConfigDataItem[3];
            minJackpot.Weight = value;
            IOCanvasManager.Instance.UpdateParamsPanle();
        }
        get { return tempJackpotCfgData.sBoxJackpotConfigDataItem[3].Weight; }
    }


    public int TempMiniMinBet
    {
        set
        {
            var minJackpot = tempJackpotCfgData.sBoxJackpotConfigDataItem[3];
            minJackpot.MinBet = value;
            if (value == 0)
                minJackpot.MaxBet = 0;
            else if (minJackpot.MaxBet < value)
                minJackpot.MaxBet = value;
            IOCanvasManager.Instance.UpdateParamsPanle();
        }
        get { return tempJackpotCfgData.sBoxJackpotConfigDataItem[3].MinBet; }
    }

    public int TempMiniMaxBet
    {
        set
        {
            var minJackpot = tempJackpotCfgData.sBoxJackpotConfigDataItem[3];
            //if (minJackpot.MinBet > 0)
            {
                minJackpot.MaxBet = value;
                IOCanvasManager.Instance.UpdateParamsPanle();
            }
        }
        get { return tempJackpotCfgData.sBoxJackpotConfigDataItem[3].MaxBet; }
    }

    public int TempMinorBaseValue
    {
        set
        {
            var minorJackpot = tempJackpotCfgData.sBoxJackpotConfigDataItem[2];
            minorJackpot.BaseValue = value;
            minorJackpot.MinTriggerValue = (int)(minorJackpot.BaseValue * releaseParams);
            minorJackpot.MaxTriggerValue = (int)(minorJackpot.BaseValue * 1.5f);

            TempMajorBaseValue = value;
            IOCanvasManager.Instance.UpdateParamsPanle();
        }
        get { return tempJackpotCfgData.sBoxJackpotConfigDataItem[2].BaseValue; }
    }

    public int TempMinorMinTriggerValue //#seaweed新加
    {
        set
        {
            var minorJackpot = tempJackpotCfgData.sBoxJackpotConfigDataItem[2];
            minorJackpot.MinTriggerValue = value;
            IOCanvasManager.Instance.UpdateParamsPanle();
        }
        get { return tempJackpotCfgData.sBoxJackpotConfigDataItem[2].MinTriggerValue; }
    }




    public int TempMinorMaxTriggerValue
    {
        set
        {
            var minJackpot = tempJackpotCfgData.sBoxJackpotConfigDataItem[2];
            minJackpot.MaxTriggerValue = value;
            minJackpot.MinTriggerValue = minJackpot.MinTriggerValue > minJackpot.MaxTriggerValue * 0.9f ? int.Parse((minJackpot.MaxTriggerValue * 0.9f).ToString()) : minJackpot.MinTriggerValue;
            IOCanvasManager.Instance.UpdateParamsPanle();
        }
        get { return tempJackpotCfgData.sBoxJackpotConfigDataItem[2].MaxTriggerValue; }
    }


    public int TempMinorWeight //#seaweed新加
    {
        set
        {
            var minorJackpot = tempJackpotCfgData.sBoxJackpotConfigDataItem[2];
            minorJackpot.Weight = value;
            IOCanvasManager.Instance.UpdateParamsPanle();
        }
        get { return tempJackpotCfgData.sBoxJackpotConfigDataItem[2].Weight; }
    }

    public int TempMinorMinBet
    {
        set
        {
            var minJackpot = tempJackpotCfgData.sBoxJackpotConfigDataItem[2];
            minJackpot.MinBet = value;
            if (value == 0)
                minJackpot.MaxBet = 0;
            else if (minJackpot.MaxBet < value)
                minJackpot.MaxBet = value;
            IOCanvasManager.Instance.UpdateParamsPanle();
        }
        get { return tempJackpotCfgData.sBoxJackpotConfigDataItem[2].MinBet; }
    }

    public int TempMinorMaxBet
    {
        set
        {
            var minJackpot = tempJackpotCfgData.sBoxJackpotConfigDataItem[2];
            //if (minJackpot.MinBet > 0)
            {
                minJackpot.MaxBet = value;
                IOCanvasManager.Instance.UpdateParamsPanle();
            }
        }
        get { return tempJackpotCfgData.sBoxJackpotConfigDataItem[2].MaxBet; }
    }

    public int TempMajorBaseValue
    {
        set
        {
            var majorJackpot = tempJackpotCfgData.sBoxJackpotConfigDataItem[1];
            majorJackpot.BaseValue = value;
            majorJackpot.MinTriggerValue = (int)(majorJackpot.BaseValue * releaseParams);
            majorJackpot.MaxTriggerValue = (int)(majorJackpot.BaseValue * 1.5f);

            TempGrandBaseValue = value;
            IOCanvasManager.Instance.UpdateParamsPanle();
        }
        get { return tempJackpotCfgData.sBoxJackpotConfigDataItem[1].BaseValue; }
    }

    public int TempMajorMinTriggerValue //#seaweed新加
    {
        set
        {
            var majorJackpot = tempJackpotCfgData.sBoxJackpotConfigDataItem[1];
            majorJackpot.MinTriggerValue = value;
            IOCanvasManager.Instance.UpdateParamsPanle();
        }
        get { return tempJackpotCfgData.sBoxJackpotConfigDataItem[1].MinTriggerValue; }
    }


    public int TempMajorMaxTriggerValue
    {
        set
        {
            var majorJackpot = tempJackpotCfgData.sBoxJackpotConfigDataItem[1];
            majorJackpot.MaxTriggerValue = value;
            majorJackpot.MinTriggerValue = majorJackpot.MinTriggerValue > majorJackpot.MaxTriggerValue * 0.9f ? int.Parse((majorJackpot.MaxTriggerValue * 0.9f).ToString()) : majorJackpot.MinTriggerValue;
            IOCanvasManager.Instance.UpdateParamsPanle();
        }
        get { return tempJackpotCfgData.sBoxJackpotConfigDataItem[1].MaxTriggerValue; }
    }


    public int TempMajorWeight //#seaweed新加
    {
        set
        {
            var majorJackpot = tempJackpotCfgData.sBoxJackpotConfigDataItem[1];
            majorJackpot.Weight = value;
            IOCanvasManager.Instance.UpdateParamsPanle();
        }
        get { return tempJackpotCfgData.sBoxJackpotConfigDataItem[1].Weight; }
    }

    public int TempMajorMinBet
    {
        set
        {
            var majorJackpot = tempJackpotCfgData.sBoxJackpotConfigDataItem[1];
            majorJackpot.MinBet = value;
            if (value == 0)
                majorJackpot.MaxBet = 0;
            else if (majorJackpot.MaxBet < value)
                majorJackpot.MaxBet = value;
            IOCanvasManager.Instance.UpdateParamsPanle();
        }
        get { return tempJackpotCfgData.sBoxJackpotConfigDataItem[1].MinBet; }
    }

    public int TempMajorMaxBet
    {
        set
        {
            var majorJackpot = tempJackpotCfgData.sBoxJackpotConfigDataItem[1];
            //if (majorJackpot.MinBet > 0)
            {
                majorJackpot.MaxBet = value;
                IOCanvasManager.Instance.UpdateParamsPanle();
            }
        }
        get { return tempJackpotCfgData.sBoxJackpotConfigDataItem[1].MaxBet; }
    }

    public int TempGrandBaseValue
    {
        set
        {
            var grandJackpot = tempJackpotCfgData.sBoxJackpotConfigDataItem[0];
            grandJackpot.BaseValue = value;
            grandJackpot.MinTriggerValue = (int)(grandJackpot.BaseValue * releaseParams);
            grandJackpot.MaxTriggerValue = (int)(grandJackpot.BaseValue * 1.5f);

            IOCanvasManager.Instance.UpdateParamsPanle();
        }
        get { return tempJackpotCfgData.sBoxJackpotConfigDataItem[0].BaseValue; }
    }


    public int TempGrandMinTriggerValue //#seaweed新加
    {
        set
        {
            var grandJackpot = tempJackpotCfgData.sBoxJackpotConfigDataItem[0];
            grandJackpot.MinTriggerValue = value;
            IOCanvasManager.Instance.UpdateParamsPanle();
        }
        get { return tempJackpotCfgData.sBoxJackpotConfigDataItem[0].MinTriggerValue; }
    }


    public int TempGrandMaxTriggerValue
    {
        set
        {
            var grandJackpot = tempJackpotCfgData.sBoxJackpotConfigDataItem[0];
            grandJackpot.MaxTriggerValue = value;
            grandJackpot.MinTriggerValue = grandJackpot.MinTriggerValue > grandJackpot.MaxTriggerValue * 0.9f ? int.Parse((grandJackpot.MaxTriggerValue * 0.9f).ToString()) : grandJackpot.MinTriggerValue;
            IOCanvasManager.Instance.UpdateParamsPanle();
        }
        get { return tempJackpotCfgData.sBoxJackpotConfigDataItem[0].MaxTriggerValue; }
    }


    public int TempGrandWeight //#seaweed新加
    {
        set
        {
            var grandJackpot = tempJackpotCfgData.sBoxJackpotConfigDataItem[0];
            grandJackpot.Weight = value;
            IOCanvasManager.Instance.UpdateParamsPanle();
        }
        get { return tempJackpotCfgData.sBoxJackpotConfigDataItem[0].Weight; }
    }


    public int TempGrandMinBet
    {
        set
        {
            var grandJackpot = tempJackpotCfgData.sBoxJackpotConfigDataItem[0];
            grandJackpot.MinBet = value;
            if (value == 0)
                grandJackpot.MaxBet = 0;
            else if (grandJackpot.MaxBet < value)
                grandJackpot.MaxBet = value;
            IOCanvasManager.Instance.UpdateParamsPanle();
        }
        get { return tempJackpotCfgData.sBoxJackpotConfigDataItem[0].MinBet; }
    }

    public int TempGrandMaxBet
    {
        set
        {
            var grandJackpot = tempJackpotCfgData.sBoxJackpotConfigDataItem[0];
            //if (grandJackpot.MinBet > 0)
            {
                grandJackpot.MaxBet = value;
                IOCanvasManager.Instance.UpdateParamsPanle();
            }
        }
        get { return tempJackpotCfgData.sBoxJackpotConfigDataItem[0].MaxBet; }
    }



}