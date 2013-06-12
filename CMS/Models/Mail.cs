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
        
        /// 栏目Id
        /// </summary>
        [Display(Name = "栏目")]
        [Required(ErrorMessage = "×")]
        public int CategoryId { get; set; }


        [Display(Name="发件人")]
        [StringLength(80, ErrorMessage = "×")]
        public string FromUser { get; set; }


        [Display(Name = "收件人")]
        [StringLength(80, ErrorMessage = "×")]
        public string ToUser { get; set; }

        [NotMapped]
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

        /// <summary>
        /// 栏目
        /// </summary>
        public virtual Category Category { get; set; }

        public Mail()
        {
            SendTime = System.DateTime.Now;
        }

        [NotMapped]
        public static List<SelectListItem> ContentOrders
        {
            get
            {
                List<SelectListItem> _cOrders = new List<SelectListItem>(6);
                _cOrders.Add(new SelectListItem { Text = "默认排序", Value = "0" });
                _cOrders.Add(new SelectListItem { Text = "Id降序", Value = "1" });
                _cOrders.Add(new SelectListItem { Text = "Id升序", Value = "2" });
                _cOrders.Add(new SelectListItem { Text = "发送时间降序", Value = "3" });
                _cOrders.Add(new SelectListItem { Text = "发布时间升序", Value = "4" });
                _cOrders.Add(new SelectListItem { Text = "未阅读", Value = "5" });
                return _cOrders;
            }
        }
    }
}