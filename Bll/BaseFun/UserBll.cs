using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
 *所有用户操作
 */
namespace Bll.BaseFun
{
    public class UserBll
    {
        /// <summary>
        /// 获取可接收短信用户列表
        /// </summary>
        /// <returns></returns>
        public object GetUserList_SMS()
        {
            SQLHelper sqlh = new SQLHelper();
            string sql = @"select UserID,U_LoginName,U_RealName,U_Phone from [dbo].[T_PC_User] where smsallow = 1";
            return sqlh.ExecuteSQLDataSet(sql);
        }
    }
}
