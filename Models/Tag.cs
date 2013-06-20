using System.ComponentModel.DataAnnotations;

namespace Ninesky.Models
{
    /// <summary>
    /// 标签模型
    /// </summary>
    public class Tag
    {

        [Display(Name = "TagID")]
        public int TagID { get; set;}

        [Display(Name = "UserID")]
        public int UserID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Display(Name="标签名称",Description="2-20个字符")]
        [Required(ErrorMessage="×")]
        [StringLength(50,ErrorMessage="×")]
        public string Name { get; set; }

        [Display(Name = "MailID")]
        public int MailID { get; set; }

    }
}