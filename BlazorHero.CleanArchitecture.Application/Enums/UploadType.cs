using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Enums
{
    public enum UploadType
    {
        [Description(@"Images\Products")]
        Product,
        [Description(@"Images\ProfilePictures")]
        ProfilePicture
    }
}
