using Ninesky.Models;
using System.Linq;
using System.Web.Mvc;
using Ninesky.Repository;
using System.Text.RegularExpressions;


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
            _cModels = _cModels.Where(m => m.ToUserName == userName);//不包含子栏目

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
            _cModels = _cModels.Where(m => m.FromUserName == userName && m.IsSend == true);

            _pConfig.TotalRecord = _cModels.Count();//总记录数
            //排序
            _cModels = _cModels.OrderByDescending(m => m.SendTime);
            //分页
            _cModels = _cModels.Skip((_pConfig.CurrentPage - 1) * _pConfig.PageSize).Take(_pConfig.PageSize);
            PagerData<Mail> _pData = new PagerData<Mail>(_cModels, _pConfig);

            foreach (Mail element in _pData)
            {
                element.Title = getShortTitle(element.Title);
                element.Content = getShortContext(element.Content);
            }
            return _pData;
        }


        /// <summary>
        /// 草稿箱
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public PagerData<Mail> List_Draft(string userName, int currentPage, int pageSize)
        {
            PagerConfig _pConfig = new PagerConfig { CurrentPage = currentPage, PageSize = pageSize };
            var _cModels = dbContext.Mails.AsQueryable();
            _cModels = _cModels.Where(m => m.FromUserName == userName && m.IsSend == false);

            _pConfig.TotalRecord = _cModels.Count();//总记录数
            //排序
            _cModels = _cModels.OrderByDescending(m => m.SendTime);
            //分页
            _cModels = _cModels.Skip((_pConfig.CurrentPage - 1) * _pConfig.PageSize).Take(_pConfig.PageSize);
            PagerData<Mail> _pData = new PagerData<Mail>(_cModels, _pConfig);

            foreach (Mail element in _pData)
            {
                element.Title = getShortTitle(element.Title);
                element.Content = getShortContext(element.Content);
            }
            return _pData;
        }



        public string getShortContext(string context)
        {
            context = striphtml(context);
  
            int len = 60 < context.Length ? 60 : context.Length;
           string ret = context.Substring(0, len);        

            for (int i = 0; i < len; i++)
            {
                if (ret[i] == '\n')
                    ret.Remove(i, 1);
            }
            return ret;
        }

        public string getShortTitle(string title)
        {
            if (title == null)
                title = "无主题";
            int len = 10 < title.Length ? 30 : title.Length;
            string ret = title.Substring(0, len);
            for (int i = 0; i < len; i++)
            {
                if (ret[i] == '\n')
                    ret.Remove(i, 1);
            }
            return ret;
        }

        public static string striphtml(string strhtml)
        {
            string stroutput = strhtml;
            Regex regex = new Regex(@"<[^>]+>|</[^>]+>");
            stroutput = regex.Replace(stroutput, "");
            return stroutput;
        }
    }
}