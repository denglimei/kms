﻿using KmsService.DAL;
using KmsService.Entity;
using System.Configuration;

namespace KmsService.KeyBLL.ManagerGetKeyHandler
{
    /// <summary>
    /// 判断用户是否有正在使用中的用户
    /// </summary>
    public class UserOccupiedHandler : GetRoomHandler
    {

        /// <summary>
        /// 判断用户是否正在使用，如果使用则提醒管理员无法使用并告知管理员那个用户正在使用。
        /// </summary>
        /// <param name="roomName"></param>
        /// <param name="managerID"></param>
        /// <returns></returns>
        public override string GetRoom(string roomName, string managerID)
        {
            SelectCalendarInfoDAL calendarInfoDAL = new SelectCalendarInfoDAL();
            CalendarInfoEntity calendarInfoEntity = calendarInfoDAL.SelectOccupiedRecord(roomName);
            if (calendarInfoEntity.Organizer != null)
            {


                string strText = string.Format("您要使用的会议室现在正在被用户：{0}使用中，结束时间为{1}。", calendarInfoEntity.Organizer, calendarInfoEntity.EndTime);
                sendMessages.SendBotbotText(managerID, strText);
                return null;
            }
            else
            {
                return successor.GetRoom(roomInfo.RoomName, managerID);
            }
        }
    }
}