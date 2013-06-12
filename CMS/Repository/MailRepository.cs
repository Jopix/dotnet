using Ninesky.Models;
using System.Linq;
using System.Web.Mvc;
using Ninesky.Repository;

namespace Ninesky.Repository
{
    public class MailRepository:RepositoryBase<Mail>
    {

        /// <summary>
        /// 写邮件
        /// </summary>
        /// <param name="mail"></param>
        /// <returns></returns>

        public override bool Add(Mail mail)
        {
            dbContext.Mails.Add(mail);
            return dbContext.SaveChanges() > 0;
        }

        /// <summary>
        ///  修改邮件
        /// </summary>
        /// <param name="mail"></param>
        /// <returns></returns>

        public override bool Update(Mail mail)
        {
            dbContext.Mails.Attach(mail);
            dbContext.Entry<Mail>(mail).State = System.Data.EntityState.Modified;
            return dbContext.SaveChanges() > 0;
        }

        /// 删除邮件
        /// <param name="commonModelId">id</param>
        /// 
        public override bool Delete(int mailID)
        {
            dbContext.Mails.Remove(dbContext.Mails.SingleOrDefault(a => a.MailID == mailID));
            return dbContext.SaveChanges() > 0;
        }

        /// 查找邮件
        public override Mail Find(int Id)
        {
            return dbContext.Mails.AsNoTracking().SingleOrDefault(a => a.MailID == Id);
        }

        /// <summary>
        /// 获取分页邮件列表
        /// </summary>
        /// <param name="categoryId">栏目Id</param>
        /// <param name="cChildren">是否包含子栏目</param>
        /// <param name="userName">用户名</param>
        /// <param name="currentPage">当前页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <param name="order">排序方式</param>
        /// <returns>分页数据</returns>
        /// 
        public PagerData<Mail> List(int categoryId, bool cChildren, string userName, int currentPage, int pageSize, int order)
        {
            PagerConfig _pConfig = new PagerConfig { CurrentPage = currentPage, PageSize = pageSize };
            var _cModels = dbContext.Mails.Include("Category").AsQueryable();
            if (categoryId != 0)
            {
                if (cChildren)//包含子栏目
                {
                    CategoryRepository _cRsy = new CategoryRepository();
                    IQueryable<int> _children = _cRsy.Children(categoryId, 0).Select(c => c.CategoryId);
                    _cModels = _cModels.Where(m => _children.Contains(m.CategoryId));
                }
                else
                    _cModels = _cModels.Where(m => m.CategoryId == categoryId);//不包含子栏目
            }

            _pConfig.TotalRecord = _cModels.Count();//总记录数
            //排序
            switch (order)
            {
                case 1://id降序
                    _cModels = _cModels.OrderByDescending(m => m.MailID);
                    break;
                case 2://Id升序
                    _cModels = _cModels.OrderBy(m => m.MailID);
                    break;
                case 3://发布日期降序
                    _cModels = _cModels.OrderByDescending(m => m.SendTime);
                    break;
                case 4://发布日期升序
                    _cModels = _cModels.OrderBy(m => m.SendTime);
                    break;
                default://默认id降序
                    _cModels = _cModels.OrderByDescending(m => m.IsRead);
                    break;
            }
            //分页
            _cModels = _cModels.Skip((_pConfig.CurrentPage - 1) * _pConfig.PageSize).Take(_pConfig.PageSize);
            PagerData<Mail> _pData = new PagerData<Mail>(_cModels, _pConfig);
            return _pData;
        }


        /// <summary>
        /// 获取收件箱邮件列表
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="cChildren"></param>
        /// <param name="userName"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="order"></param>
        /// <returns></returns>

        public PagerData<Mail> List_Inbox(string userName, int currentPage, int pageSize)
        {
            PagerConfig _pConfig = new PagerConfig { CurrentPage = currentPage, PageSize = pageSize };
            var _cModels = dbContext.Mails.AsQueryable();
            _cModels = _cModels.Where(m => m.ToUser == userName);//不包含子栏目

            _pConfig.TotalRecord = _cModels.Count();//总记录数
            //排序
            _cModels = _cModels.OrderByDescending(m => m.SendTime);
            //分页
            _cModels = _cModels.Skip((_pConfig.CurrentPage - 1) * _pConfig.PageSize).Take(_pConfig.PageSize);
            PagerData<Mail> _pData = new PagerData<Mail>(_cModels, _pConfig);
            return _pData;
        }

        /// <summary>
        /// 发件箱
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public PagerData<Mail> List_Outbox(string userName, int currentPage, int pageSize)
        {
            PagerConfig _pConfig = new PagerConfig { CurrentPage = currentPage, PageSize = pageSize };
            var _cModels = dbContext.Mails.AsQueryable();
            _cModels = _cModels.Where(m => m.FromUser == userName);//不包含子栏目

            _pConfig.TotalRecord = _cModels.Count();//总记录数
            //排序
            _cModels = _cModels.OrderByDescending(m => m.SendTime);
            //分页
            _cModels = _cModels.Skip((_pConfig.CurrentPage - 1) * _pConfig.PageSize).Take(_pConfig.PageSize);
            PagerData<Mail> _pData = new PagerData<Mail>(_cModels, _pConfig);
            return _pData;
        }
    }
}