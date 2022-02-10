using System;
using System.Collections.Generic;
using imagessharedpassword.Data;

namespace imagessharedpassword.web.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
    public class ImageViewModel 
    {
        public int ImageId { get; set; }
        public string Password { get; set; }
    }
    public class ShowImageViewModel 
    {
        public Image Image { get; set; }
        public bool ShowImage { get; set; }
        public int Id { get; set; }
        public bool FalsePassword { get; set; }
        public List<int> Ids { get; set; }
    }
}
