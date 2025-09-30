/**
 * @file    
 * @author  Huang Wen <Email:ww1383@163.com, QQ:214890094, WeChat:w18926268887>
 * @version 1.0
 *
 * @section LICENSE
 *
 * Permission is hereby granted, free of charge, to any person obtaining a
 * copy of this software and associated documentation files (the "Software"),
 * to deal in the Software without restriction, including without limitation
 * the rights to use, copy, modify, merge, publish, distribute, sublicense,
 * and/or sell copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included
 * in all copies or substantial portions of the Software.
 *
 * @section DESCRIPTION
 *
 * This file is ...
 */
using Hal;
using Newtonsoft.Json;
using UnityEngine;

namespace SBoxApi
{
    /*
	* 	JACKPOT_MAX = 4
	* 	3 小彩金
	* 	2 中彩金
	* 	1 大彩金
	* 	0 巨大彩金
	*/

    //从硬件返回的彩金信息
    public class SBoxJackpotData
    {

        public int result;
        public int MachineId;                       // 机台号
        public int SeatId;                          // 分机号/座位号
        public int ScoreRate;                       // 分值比，1分多少钱.直接返回app传下来的值
        public int JpPercent;                       // 分机彩金百分比，每次押分贡献给彩金的比例。直接返回app传下来的值

        //
        public int[] Lottery;                       // 0:表示没有开出彩金，1:表示已开出彩金
        public int[] Jackpotlottery;				// 开出的彩金注意:此处的单位是钱的单位，而且是乘以了100的，分机收到这个值要根据分机的分值比来转成成对应的分数，而且还要将此值除以100
        public int[] JackpotOut;                    // 彩金显示积累分,用于显示当前的彩金值
        public int[] JackpotOld;					// 开出彩金前的显示积累分
    }

    //app端传到硬件的信息
    public class SBoxJackpotBet
    {
        public int MachineId;                       // 机台号
        public int SeatId;                          // 分机号/座位号
        public int Bet;                             // 当前的押分,为了避免丢失小数，需要乘以100，硬件读取这个值会除以100后使用
        public int BetPercent;                      // 押分比例，目前拉霸默认值传1，同样需要乘以100
        public int ScoreRate;                       // 分值比，1分多少钱，需要乘以1000再往下传
        public int JpPercent;						// 分机彩金百分比，每次押分贡献给彩金的比例。需要乘以1000再往下传
    }



    public class SBoxJackpotBetCoinPush
    {
        public int MachineId;        // 机台号
        public int SeatId;           // 分机号/座位号
        public int majorBet;              // gtand, major  //   当前的押分,为了避免丢失小数，需要乘以100，硬件读取这个值会除以100后使用
        public int BetPercent;       // 100    押分比例，目前拉霸(推币机)默认值传1，同样需要乘以100
        public int ScoreRate;        // 1000   分值比，1分多少钱，需要乘以1000再往下传
        public int JpPercent;		// 1000   分机彩金百分比，每次押分贡献给彩金的比例。需要乘以1000再往下传

        public int grandBet; // grand 0 ; major 1
    }



    public class SBoxJackpotConfigDataItem
    {
        public int JpType;                          //彩金类型
        public int BaseValue;                       //基本值		需要乘以100
        public int MinTriggerValue;                 //最小触发值	需要乘以100
        public int MaxTriggerValue;                 //最大触发值	需要乘以100
        public int Weight;                          //积分比重		需要乘以100
        public int MinBet;                          //拉彩金最低押分		需要乘以100
        public int MaxBet;                          //拉彩金最高押分		需要乘以100
    }

    public class SBoxJackpotConfigData
    {
        public int result;
        public int BaseSetValue;                    //小彩金的基本值		需要乘以100
        public int TotalWeight;						//总份数				需要乘以100
        public SBoxJackpotConfigDataItem[] sBoxJackpotConfigDataItem; //4个彩金的设定参数
    }

    public class SBoxJackpotInfoData
    {
        public int result;
        public ulong[] WithdrawalPoints;                    //彩金累计提拔分
        public ulong[] RewardPoints;                        //彩金开奖累积分
        public int[] Triggers;                              //当前彩金触发值
    }

    public class SBoxJackpotSetResult
    {
        public int result;                    //为0时成功，其它则为失败
    }

    public partial class SBoxIdea
    {
        public const int JACKPOT_MAX = 4;
        /**
		 *  @brief          彩金主机app初始化时调用这个接口获取当前4个彩金分数
		 *  @param          
		 *  @return         
		 *                  
		 *                  
		 *  @details        
		 */

        public static void JackpotHostInit()
        {
            SBoxPacket sBoxPacket = new SBoxPacket(cmd: 20018, source: 1, target: 2, size: 2);

            sBoxPacket.data[0] = 0;
            sBoxPacket.data[1] = 0;

            SBoxIOEvent.AddListener(sBoxPacket.cmd, JackpotHostInitR);
            SBoxIOStream.Write(sBoxPacket);
        }
        private static void JackpotHostInitR(SBoxPacket sBoxPacket)
        {
            SBoxJackpotData sBoxJackpotData = new SBoxJackpotData();
            int pos = 0;
            sBoxJackpotData.JackpotOut = new int[JACKPOT_MAX];

            for (int i = 0; i < JACKPOT_MAX; i++)
            {
                sBoxJackpotData.JackpotOut[i] = sBoxPacket.data[pos++];
            }

            EventCenter.Instance.EventTrigger(SBoxEventHandle.SBOX_JACKPOT_HOST_INIT, sBoxJackpotData);
        }

        /**
		 *  @brief          彩金主机app收到分机押注信息时调用这个接口
		 *  @param          
		 *  @return         
		 *                  
		 *                  
		 *  @details        
		 */
        public static void JackpotBetHost(SBoxJackpotBetCoinPush sBoxJackpotBet)//(SBoxJackpotBet sBoxJackpotBet)
        {
            SBoxPacket sBoxPacket = new SBoxPacket(cmd: 20019, source: 1, target: 2, size: 7);

            sBoxPacket.data[0] = sBoxJackpotBet.MachineId;
            sBoxPacket.data[1] = sBoxJackpotBet.SeatId;
            sBoxPacket.data[2] = sBoxJackpotBet.majorBet;
            //sBoxPacket.data[3] = sBoxJackpotBet.BetPercent;  
            //sBoxPacket.data[4] = sBoxJackpotBet.ScoreRate;
            //sBoxPacket.data[5] = sBoxJackpotBet.JpPercent;
            sBoxPacket.data[3] = sBoxJackpotBet.grandBet;
            sBoxPacket.data[4] = sBoxJackpotBet.BetPercent;
            sBoxPacket.data[5] = sBoxJackpotBet.ScoreRate;
            sBoxPacket.data[6] = sBoxJackpotBet.JpPercent;

            SBoxIOEvent.AddListener(sBoxPacket.cmd, JackpotBetHostR);
            SBoxIOStream.Write(sBoxPacket);
        }

        private static void JackpotBetHostR(SBoxPacket sBoxPacket)
        {
            int pos = 0;
            SBoxJackpotData sBoxJackpotData = new SBoxJackpotData();
            sBoxJackpotData.result = sBoxPacket.data[pos++];
            sBoxJackpotData.MachineId = sBoxPacket.data[pos++];
            sBoxJackpotData.SeatId = sBoxPacket.data[pos++];
            sBoxJackpotData.ScoreRate = sBoxPacket.data[pos++];
            sBoxJackpotData.JpPercent = sBoxPacket.data[pos++];

            sBoxJackpotData.Lottery = new int[JACKPOT_MAX];
            sBoxJackpotData.Jackpotlottery = new int[JACKPOT_MAX];
            sBoxJackpotData.JackpotOut = new int[JACKPOT_MAX];  // 当前值
            sBoxJackpotData.JackpotOld = new int[JACKPOT_MAX];  // 开彩金前

            //
            for (int i = 0; i < JACKPOT_MAX; i++)
            {
                sBoxJackpotData.Lottery[i] = sBoxPacket.data[pos++];
            }

            //
            for (int i = 0; i < JACKPOT_MAX; i++)
            {
                sBoxJackpotData.Jackpotlottery[i] = sBoxPacket.data[pos++];
            }

            //
            for (int i = 0; i < JACKPOT_MAX; i++)
            {
                sBoxJackpotData.JackpotOut[i] = sBoxPacket.data[pos++];
            }

            //
            for (int i = 0; i < JACKPOT_MAX; i++)
            {
                sBoxJackpotData.JackpotOld[i] = sBoxPacket.data[pos++];
            }

            curSBoxJackpotData = sBoxJackpotData;

            EventCenter.Instance.EventTrigger(SBoxEventHandle.SBOX_JACKPOT_BET_HOST, sBoxJackpotData);
        }

        public static SBoxJackpotData curSBoxJackpotData;


        /*
		*	设置彩金的配置信息
		*/
        public static void JackpotWriteConfig(SBoxJackpotConfigData sBoxJackpotConfigData)
        {
            SBoxPacket sBoxPacket = new SBoxPacket(cmd: 20020, source: 1, target: 2, size: 30);

            int pos = 0;
            sBoxPacket.data[pos++] = sBoxJackpotConfigData.BaseSetValue;
            sBoxPacket.data[pos++] = sBoxJackpotConfigData.TotalWeight;
            for (int i = 0; i < sBoxJackpotConfigData.sBoxJackpotConfigDataItem.Length; i++)
            {
                sBoxPacket.data[pos++] = sBoxJackpotConfigData.sBoxJackpotConfigDataItem[i].JpType;
                sBoxPacket.data[pos++] = sBoxJackpotConfigData.sBoxJackpotConfigDataItem[i].BaseValue;
                sBoxPacket.data[pos++] = sBoxJackpotConfigData.sBoxJackpotConfigDataItem[i].MinTriggerValue;
                sBoxPacket.data[pos++] = sBoxJackpotConfigData.sBoxJackpotConfigDataItem[i].MaxTriggerValue;
                sBoxPacket.data[pos++] = sBoxJackpotConfigData.sBoxJackpotConfigDataItem[i].Weight;
                sBoxPacket.data[pos++] = sBoxJackpotConfigData.sBoxJackpotConfigDataItem[i].MinBet;
                sBoxPacket.data[pos++] = sBoxJackpotConfigData.sBoxJackpotConfigDataItem[i].MaxBet;
            }

            SBoxIOEvent.AddListener(sBoxPacket.cmd, JackpotWriteConfigR);
            SBoxIOStream.Write(sBoxPacket);
        }

        private static void JackpotWriteConfigR(SBoxPacket sBoxPacket)
        {
            SBoxJackpotSetResult sBoxJackpotSetResult = new SBoxJackpotSetResult();
            sBoxJackpotSetResult.result = sBoxPacket.data[0];
            EventCenter.Instance.EventTrigger(SBoxEventHandle.SBOX_JACKPOT_WRITE_CONFIG, sBoxJackpotSetResult);
        }

        /*
		*	读取彩金的配置信息
		*/
        public static void JackpotReadConfig()
        {
            SBoxPacket sBoxPacket = new SBoxPacket(cmd: 20021, source: 1, target: 2, size: 2);

            sBoxPacket.data[0] = 0;
            sBoxPacket.data[1] = 0;

            SBoxIOEvent.AddListener(sBoxPacket.cmd, JackpotReadConfigR);
            SBoxIOStream.Write(sBoxPacket);
        }

        private static void JackpotReadConfigR(SBoxPacket sBoxPacket)
        {
            int pos = 0;
            SBoxJackpotConfigData sBoxJackpotConfigData = new SBoxJackpotConfigData();
            sBoxJackpotConfigData.result = sBoxPacket.data[pos++];
            if (sBoxJackpotConfigData.result == 0)  //成功
            {
                sBoxJackpotConfigData.BaseSetValue = sBoxPacket.data[pos++];
                sBoxJackpotConfigData.TotalWeight = sBoxPacket.data[pos++];

                sBoxJackpotConfigData.sBoxJackpotConfigDataItem = new SBoxJackpotConfigDataItem[JACKPOT_MAX];

                for (int i = 0; i < JACKPOT_MAX; i++)
                {
                    sBoxJackpotConfigData.sBoxJackpotConfigDataItem[i] = new SBoxJackpotConfigDataItem();
                    sBoxJackpotConfigData.sBoxJackpotConfigDataItem[i].JpType = sBoxPacket.data[pos++];
                    sBoxJackpotConfigData.sBoxJackpotConfigDataItem[i].BaseValue = sBoxPacket.data[pos++];
                    sBoxJackpotConfigData.sBoxJackpotConfigDataItem[i].MinTriggerValue = sBoxPacket.data[pos++];
                    sBoxJackpotConfigData.sBoxJackpotConfigDataItem[i].MaxTriggerValue = sBoxPacket.data[pos++];
                    sBoxJackpotConfigData.sBoxJackpotConfigDataItem[i].Weight = sBoxPacket.data[pos++];
                    sBoxJackpotConfigData.sBoxJackpotConfigDataItem[i].MinBet = sBoxPacket.data[pos++];
                    sBoxJackpotConfigData.sBoxJackpotConfigDataItem[i].MaxBet = sBoxPacket.data[pos++];
                }
            }
            EventCenter.Instance.EventTrigger(SBoxEventHandle.SBOX_JACKPOT_READ_CONFIG, sBoxJackpotConfigData);
        }

        /*
		* sel 为0时读取彩金累计提拔分, 彩金开奖累积分和彩金当前触发值
		*/
        public static void JackpotGetWithdrawalPoints(int sel, int[] triggers)
        {
            SBoxPacket sBoxPacket = new SBoxPacket(cmd: 20022, source: 1, target: 2, size: 5);

            sBoxPacket.data[0] = sel;
            sBoxPacket.data[1] = triggers[0];
            sBoxPacket.data[2] = triggers[1];
            sBoxPacket.data[3] = triggers[2];
            sBoxPacket.data[4] = triggers[3];

            SBoxIOEvent.AddListener(sBoxPacket.cmd, JackpotGetWithdrawalPointsR);
            SBoxIOStream.Write(sBoxPacket);
        }

        private static void JackpotGetWithdrawalPointsR(SBoxPacket sBoxPacket)
        {
            int pos = 0;
            int result = sBoxPacket.data[pos++];

            SBoxJackpotInfoData sBoxJackpotInfoData = new SBoxJackpotInfoData();
            if (result == 0)
            {
                sBoxJackpotInfoData.WithdrawalPoints = new ulong[JACKPOT_MAX];
                for (int i = 0; i < JACKPOT_MAX; i++)
                {
                    sBoxJackpotInfoData.WithdrawalPoints[i] = (ulong)sBoxPacket.data[pos++];
                    sBoxJackpotInfoData.WithdrawalPoints[i] += (ulong)sBoxPacket.data[pos++] << 32;
                }

                sBoxJackpotInfoData.RewardPoints = new ulong[JACKPOT_MAX];
                for (int i = 0; i < JACKPOT_MAX; i++)
                {
                    sBoxJackpotInfoData.RewardPoints[i] = (ulong)sBoxPacket.data[pos++];
                    sBoxJackpotInfoData.RewardPoints[i] += (ulong)sBoxPacket.data[pos++] << 32;
                }

                sBoxJackpotInfoData.Triggers = new int[JACKPOT_MAX];
                for (int i = 0; i < JACKPOT_MAX; i++)
                {
                    sBoxJackpotInfoData.Triggers[i] = sBoxPacket.data[pos++];

                }
            }

            EventCenter.Instance.EventTrigger(SBoxEventHandle.SBOX_JACKPOT_GET_WITHDRAWAL_POINTS, sBoxJackpotInfoData);
        }

        public static void JackpotCheat(int macId, int seatId, int jpType)
        {
            SBoxPacket sBoxPacket = new SBoxPacket(cmd: 20023, source: 1, target: 2, size: 3);

            sBoxPacket.data[0] = macId;
            sBoxPacket.data[1] = seatId;
            sBoxPacket.data[2] = jpType;

            SBoxIOEvent.AddListener(sBoxPacket.cmd, JackpotCheatR);
            SBoxIOStream.Write(sBoxPacket);
        }

        public static void JackpotCheatR(SBoxPacket sBoxPacket)
        {
            int result = sBoxPacket.data[0];
            EventCenter.Instance.EventTrigger(SBoxEventHandle.SBOX_JACKPOT_CHEAT, result);
        }
    }
}