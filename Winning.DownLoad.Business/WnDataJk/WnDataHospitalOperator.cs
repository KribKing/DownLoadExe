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
    public class WnDataHospitalOperator : JkInterface
    {
        public WnDataHospitalOperator(string jkcode, string url)
            : base(jkcode, url, "", "")
        {

        }
        public override ResultInfo Run(string jkcode, JObject jobj)
        {
            ResultInfo retInfo = new ResultInfo();
            try
            {
                retInfo = base.PostResponse();
                if (retInfo.ackflg)
                {
                    WnDataHospitalInfoList info = JsonConvert.DeserializeObject<WnDataHospitalInfoList>(retInfo.ackmsg);
                    if (info != null)
                    {
                        if (info.head.result == "success")
                        {
                            DataTable dt = Tools.ToDataTable<WnDataHospitalInfo>(info.body);
                            base.ExcuteDataBase(dt, ref retInfo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                retInfo.ackmsg = ex.Message;
                retInfo.ackflg = false;
            }
            return retInfo;
        }
    }
}