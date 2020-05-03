using System.Collections.Generic;
using CaterDal;
using CaterModel;

namespace CaterBll {
    /// <summary>
    /// 会员信息业务层
    /// </summary>
    public partial class MemberInfoBll
    {
        private MemberInfoDal memInfoDal=new MemberInfoDal();
        public List<MemberInfo> GetList(Dictionary<string,string> dic)
        {
            return memInfoDal.GetList(dic);
        }

        public bool Add(MemberInfo mi)
        {
            return memInfoDal.Insert(mi) > 0;
        }

        public bool Edit(MemberInfo mi)
        {
            return memInfoDal.Update(mi) > 0;
        }

        public bool Remove(int id)
        {
            return memInfoDal.Delete(id) > 0;
        }
        public MemberInfo GetByName(string name)
        {
            return memInfoDal.GetByName(name);
        }
    }
}
