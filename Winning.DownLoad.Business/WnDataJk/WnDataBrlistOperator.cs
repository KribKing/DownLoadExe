﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Winning.DownLoad.Business.WnDataModel;
using Winning.DownLoad.Core;

namespace Winning.DownLoad.Business.WnDataJk
{
    public class WnDataBrlistOperator: JkInterface
    {
        public WnDataBrlistOperator(string jkcode, string url)
            : base(jkcode, url, "", "")
        {

        }
        public override ResultInfo Run(string jkcode, JObject jobj)
        {
            ResultInfo retInfo = new ResultInfo();
            try
            {
                string dbeginDate = (jobj["dbeginDate"] ?? DateTime.Now.ToString("yyyy-MM-dd")).ToString();
                string dendDate = (jobj["dendDate"] ?? DateTime.Now.AddDays(+1).ToString("yyyy-MM-dd")).ToString();
                base.body = "{\"startTime\": \""+dbeginDate+"\",\"endTime\": \""+dendDate+"\"}";
                retInfo = base.PostResponse();
                if (retInfo.ackflg)
                {
                    WnDataBrlistInfoList info = JsonConvert.DeserializeObject<WnDataBrlistInfoList>(retInfo.ackmsg);
                    if (info != null)
                    {
                        if (info.head.result == "success")
                        {
                            DataTable dt = Tools.ToDataTable<WnDataBrlistInfo>(info.body);
                            base.ExcuteDataBase(dt, ref retInfo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {              
                retInfo.ackcode = "300.1";
                retInfo.ackmsg = ex.Message;
                retInfo.ackflg = false;               
            }
            return retInfo;
        }
    }
}
