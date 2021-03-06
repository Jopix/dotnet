﻿using Ninesky.Models;
using Ninesky.Repository;
using System.Web.Mvc;
using System;

namespace Ninesky.Controllers
{
    public class MailController : Controller
    {
        MailRepository mailRsy;
        public MailController()
        {
            mailRsy = new MailRepository();
        }
        

        /// <summary>
        /// 邮件内容
        /// </summary>
        /// <param name="id">CommonModelId</param>
        /// <param name="view">视图</param>
        public PartialViewResult MailDetail(int id, string view = "MailDetail")
        {
            return PartialView(view, mailRsy.Find(id));
        }


        /// <summary>
        /// 用户默认页
        /// </summary>
        [UserAuthorize]
        public ActionResult MailDefault()
        {
            return View();
        }


        /// 写邮件
        [UserAuthorize]
        public ActionResult MailAdd()
        {
            return View(new Mail());
        }


        [HttpPost]
        [UserAuthorize]
        [ValidateInput(false)]
        public ActionResult MailAdd(Mail mail)
        {
            mail.FromUserName = UserController.UserName;

            if (ModelState.IsValid)
            {
                mail.IsSend ^= true;
                if (mailRsy.Add(mail))
                {
                    Notice _n = new Notice { Title = "添加邮件成功", Details = "您已经成功添加[" , DwellTime = 5, NavigationName = "收件箱", NavigationUrl = Url.Action("MailInbox", "Mail") };
                    return RedirectToAction("UserNotice", "Prompt", _n);
                }
                else
                {
                    Error _e = new Error { Title = "添加邮件失败", Details = "在添加邮件时，未能保存到数据库", Cause = "系统错误", Solution = Server.UrlEncode("<li>返回<a href='" + Url.Action("MailAdd", "Mail") + "'>添加邮件</a>页面，输入正确的信息后重新操作</li><li>返回<a href='" + Url.Action("MailDefault", "Mail") + "'>邮件管理首页</a>。</li><li>联系网站管理员</li>") };
                    return RedirectToAction("ManageError", "Prompt", _e);
                }
            }
            return View(mail);
        }


        /// <summary>
        /// 删除邮件
        /// </summary>
        /// <param name="id">公共模型id</param>
        /// <returns></returns>
        [UserAuthorize]
        public ActionResult MailDelete(int id)
        {
            bool _deleted = mailRsy.Delete(id);
            if (Request.IsAjaxRequest())
            {
                return Json(_deleted);
            }
            else
            {
                if (_deleted)
                {
                    Notice _n = new Notice { Title = "删除邮件成功", Details = "您已经成功删除了该邮件！", DwellTime = 5, NavigationName = "我的邮件", NavigationUrl = Url.Action("MailInbox", "Mail") };
                    return RedirectToAction("UserNotice", "Prompt", _n);
                }
                else
                {
                    Error _e = new Error { Title = "删除邮件失败", Details = "在删除邮件时发生错误", Cause = "该邮件已经被删除", Solution = Server.UrlEncode("<li>返回<a href='" + Url.Action("UserOwn", "Mail") + "'>我的邮件</a>页面，输入正确的信息后重新操作</li><li>返回<a href='" + Url.Action("UserDefault", "Mail") + "'>邮件管理首页</a>。</li><li>联系网站管理员</li>") };
                    return RedirectToAction("ManageError", "Prompt", _e);
                }
            }
        }
        /// <summary>
        /// 修改邮件
        /// </summary>
        /// <param name="id">mailid</param>
        [UserAuthorize]
        public ActionResult MailEdit(int id)
        {

            return View(mailRsy.Find(id));
        }
        [HttpPost]
        [UserAuthorize]
        [ValidateInput(false)]
        public ActionResult MailEdit(Mail mail)
        {

            if (ModelState.IsValid)
            {
                var _mail = mailRsy.Find(mail.MailID);
                if (_mail == null)//邮件不存在
                {
                    Error _e = new Error { Title = "邮件不存在", Details = "查询不到MailId为【" + mail.MailID.ToString() + "】的邮件", Cause = "邮件已被删除或向服务器提交邮件时数据丢失", Solution = Server.UrlEncode("<li>返回<a href='" + Url.Action("UserOwn", "Mail") + "'>我的邮件</a>重新操作</li><li>返回<a href='" + Url.Action("UserDefault", "Mail") + "'>邮件管理首页</a>。</li><li>联系网站管理员</li>") };
                    return RedirectToAction("ManageError", "Prompt", _e);
                }
                if (mail.SendTime != null) _mail.SendTime = mail.SendTime;
                if (mail.FromUserName != null) _mail.FromUserName = mail.FromUserName;
                if (mail.ToUserName != null) _mail.ToUserName = mail.ToUserName;
                if (mail.Title != null) _mail.Title = mail.Title;
                _mail.Content = mail.Content;
                if (mailRsy.Update(_mail))
                {
                    Notice _n = new Notice { Title = "修改邮件成功", Details = "您已经成功修改了[" + mail.Title + "]邮件！", DwellTime = 5, NavigationName = "我的邮件", NavigationUrl = Url.Action("UserOwn", "Mail") };
                    return RedirectToAction("UserNotice", "Prompt", _n);
                }
                else
                {
                    Error _e = new Error { Title = "修改邮件失败", Details = "在修改邮件时，未能保存到数据库", Cause = "系统错误", Solution = Server.UrlEncode("<li>返回<a href='" + Url.Action("UserAdd", "Mail", new { id = mail.MailID }) + "'>修改邮件</a>页面，输入正确的信息后重新操作</li><li>返回<a href='" + Url.Action("UserDefault", "Mail") + "'>邮件管理首页</a>。</li><li>联系网站管理员</li>") };
                    return RedirectToAction("ManageError", "Prompt", _e);
                }
            }
            return View(mail);
        }

        /// 收件箱
        /// <param name="id">id</param>
        /// <param name="page">页号</param>
        [UserAuthorize]
        public ActionResult MailInbox(int id = 0, int page = 1)
        {
            int _pageSize = 8;
            mailRsy = new MailRepository();
            PagerData<Mail> _aData;
            _aData = mailRsy.List_Inbox(UserController.UserName, page, _pageSize);
            string rn = "邮件";
            string ru = "篇";
            _aData.Config.RecordName =rn;
            _aData.Config.RecordUnit = ru;
            return View(_aData);
        }
        /// <summary>
        /// 发件箱
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [UserAuthorize]
        public ActionResult MailOutbox(int id = 0, int page = 1)
        {
            int _pageSize = 8;
            mailRsy = new MailRepository();
            PagerData<Mail> _aData;
            _aData = mailRsy.List_Outbox(UserController.UserName, page, _pageSize);
            string rn = "邮件";
            string ru = "篇";
            _aData.Config.RecordName = rn;
            _aData.Config.RecordUnit = ru;
            return View(_aData);
        }


        /// <summary>
        /// 发件箱
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [UserAuthorize]
        public ActionResult MailDraft(int id = 0, int page = 1)
        {
            int _pageSize = 8;
            mailRsy = new MailRepository();
            PagerData<Mail> _aData;
            _aData = mailRsy.List_Draft(UserController.UserName, page, _pageSize);
            string rn = "邮件";
            string ru = "篇";
            _aData.Config.RecordName = rn;
            _aData.Config.RecordUnit = ru;
            return View(_aData);
        }



        /// <summary>
        /// 垃圾箱
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [UserAuthorize]
        public ActionResult MailRecycle(int id = 0, int page = 1)
        {
            int _pageSize = 8;
            mailRsy = new MailRepository();
            PagerData<Mail> _aData;
            _aData = mailRsy.List_Draft(UserController.UserName, page, _pageSize);
            string rn = "邮件";
            string ru = "篇";
            _aData.Config.RecordName = rn;
            _aData.Config.RecordUnit = ru;
            return View(_aData);
        }

        /// <summary>
        /// 导航菜单
        /// </summary>
        [UserAuthorize]
        public PartialViewResult PartialUserNavMenus()
        {
            return PartialView();
        }

        ///// <summary>
        ///// 内容列表
        ///// </summary>
        ///// <param name="id">栏目Id</param>
        ///// <param name="cChildren">是否包含子栏目</param>
        ///// <param name="page">页码</param>
        ///// <param name="pageSize">每页显示的数目【0表示依栏目设置，如栏目不存在设为20】</param>
        ///// <param name="order">排序【0依栏目设置或默认】</param>
        ///// <param name="view">视图</param>
        //public PartialViewResult PartialList(int id, bool cChildren = false, int page = 1, int pageSize = 0, int order = 0, string view = "PartialList")
        //{
        //    if (!cChildren && ((pageSize == 0) || (order == 0)))
        //    {
        //        var _category = _categoryRsy.Find(id);
        //        if (_category != null)
        //        {
        //            if (pageSize == 0) pageSize = (int)_category.PageSize;
        //            if (order == 0) order = _category.Order;
        //        }
        //        else if (pageSize == 0) pageSize = 20;
        //    }
        //    var _cModelPd = mailRsy.List(id, cChildren, null, page, pageSize, order);
        //    return PartialView(view, _cModelPd);
        //}
    }
}
