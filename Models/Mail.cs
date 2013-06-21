using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using Ninesky.Models;

namespace Ninesky.Models
{
    /// 邮件模型
    public class Mail
    {
        [Key]
        public int MailID { get; set; }

        [Display(Name="发件人")]
        [StringLength(80, ErrorMessage = "×")]
        public string FromUserName { get; set; }

        public int FromUserID { get; set; }

        [Display(Name = "收件人")]
        [StringLength(80, ErrorMessage = "×")]
        public string ToUserName { get; set; }

        public int ToUserID { get; set; }

 
        [Display(Name="主题",Description="最多50个字符。")]
        [StringLength(50, ErrorMessage = "×")]
        public string Title { get; set; }

        [Display(Name="内容")]
        [Required(ErrorMessage = "×")]
        [DataType(DataType.Html)]
        public string Content { get; set; }


        [Display(Name = "发送时间")]
        [Required(ErrorMessage = "×")]
        [DataType(DataType.Html)]
        public System.DateTime SendTime { get; set; }

        public bool IsRead { get; set; }

        public bool IsSend { get; set; }

        public Mail()
        {
            SendTime = System.DateTime.Now;
        }
    }
}