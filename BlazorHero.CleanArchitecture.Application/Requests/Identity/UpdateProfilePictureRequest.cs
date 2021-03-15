using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Requests.Identity
{
    public class UpdateProfilePictureRequest
    {
        public string ProfilePictureDataUrl { get; set; }
    }
}
