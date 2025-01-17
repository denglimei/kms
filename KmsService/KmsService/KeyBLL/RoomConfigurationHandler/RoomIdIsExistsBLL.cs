﻿using KmsService.Entity;
using KmsService.Log4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KmsService.KeyBLL.RoomConfigurationHandler
{
    //判断是否存在此id的会议室名称
    //true：存在，执行更新；false：不存在，执行插入
    class RoomIdIsExistsBLL : RoomConfigurationHandlerBLL
    {
        public override bool ModifyRoom(string basicDataStr, BasicDataEntity newBasicData, BasicDataEntity oldBasicData, List<string> allLockNumber)
        {
            //获取t_room表已经使用了的锁的编号
            lockNumbers = modifyConfigurationDAL.GetLockNumber();
            foreach (var item in lockNumbers)
            {
                if (allLockNumber.Contains(item))
                {
                    allLockNumber.Remove(item);
                }
            }
            if (allLockNumber.Count == 0)
            {
                throw new Exception("锁已被用完，请联系相关负责人员");
            }
            LoggerHelper.Info("【管理员会议室配置】判断还有几把锁号可以使用：" + allLockNumber.Count);

            bool result = modifyConfigurationDAL.RoomIdIsExists(Convert.ToString(newBasicData.ID));
            LoggerHelper.Info("【管理员会议室配置】判断是否存在" + Convert.ToString(newBasicData.ID) + "的会议室名称，结果为：" + result);

            if (!result)
            {
                //调用下一个职责链
                successor.ModifyRoom(basicDataStr, newBasicData, oldBasicData, allLockNumber);
            }
            else
            {
                LoggerHelper.Info("【管理员会议室配置】存在id为" + Convert.ToString(newBasicData.ID) + "的会议室，说明管理员是要更新会议室信息，更新t_basicdata表和t_room表");
                //更新t_basicdata表会议室信息
                modifyConfigurationDAL.UpdateBasicData(newBasicData);
                //更新t_room表会议室信息
                modifyConfigurationDAL.UpdateRoomName(newBasicData.ID, newBasicData.RoomName);
            }
            return true;
        }
    }
}