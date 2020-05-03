using System.Collections.Generic;
using CaterDal;
using CaterModel;
using CaterCommon;
using CaterCommon;
using System;

namespace CaterBll {
    /// <summary>
    /// 店员信息业务层
    /// </summary>
    public partial class ManagerInfoBll {
        ManagerInfoDal miDal = new ManagerInfoDal();
        MemberInfoDal memberDal= new MemberInfoDal();
        /// <summary>
        /// 查询用户列表
        /// </summary>
        /// <returns></returns>
        public List<ManagerInfo> GetList() {
            //调用查询方法
            return miDal.GetList();
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="mi"></param>
        /// <returns></returns>
        public bool Add(ManagerInfo mi) {
            //调用dal层的insert方法，完成插入操作
            return miDal.Insert(mi) > 0;
        }

        /// <summary>
        /// 编辑用户信息
        /// </summary>
        /// <param name="mi">要修改的用户实体</param>
        /// <returns></returns>
        public bool Edit(ManagerInfo mi) {
            return miDal.Update(mi) > 0;
        }

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="id">用户Id</param>
        /// <returns></returns>
        public bool Remove(int id) {
            return miDal.Delete(id) > 0;
        }

        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="name">用户名</param>
        /// <param name="pwd">密码</param>
        /// <param name="type">用户类型</param>
        /// <returns></returns>
        public LoginState Login(string name, string pwd, out int type) {
            type = -1;
            //type = -1      没有
            //type = 0       员工
            //type = 1       老板
            //type = 2       顾客
            //根据用户名进行对象的查询
            ManagerInfo mi = miDal.GetByName(name);
            MemberInfo member = memberDal.GetByName(name);
            if (mi != null)
            {
                //用户名正确
                if (MD5Helper.EncryptString(mi.MPwd).Equals(MD5Helper.EncryptString(pwd))) {
                    //密码正确
                    //登录成功
                    type = mi.MType;
                    //保存用户session
                    GlobalVariable.userId = Convert.ToString(mi.Id) ;
                    GlobalVariable.userName = mi.MName;
                    GlobalVariable.pwd = mi.MPwd;
                    return LoginState.Ok;
                } else {
                    //密码错误
                    return LoginState.PwdError;
                }
            }
            else if (member != null)
            {
                //用户名正确
                if (MD5Helper.EncryptString(member.MPwd).Equals(MD5Helper.EncryptString(pwd)))
                {
                    //密码正确
                    //登录成功
                    type = 2;         //顾客
                    //保存用户session
                    GlobalVariable.userId = Convert.ToString(member.Id);
                    GlobalVariable.userName = member.MName;
                    GlobalVariable.pwd = member.MPwd;
                    GlobalVariable.phone = member.MPhone;
                    return LoginState.Ok;
                }
                else
                {
                    //密码错误
                    return LoginState.PwdError;
                }
            }
            return LoginState.NameError;
        }
    }
}
