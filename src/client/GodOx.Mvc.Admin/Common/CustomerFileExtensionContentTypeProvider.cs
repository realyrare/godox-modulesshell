using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;

namespace GodOx.Mvc.Admin.Common
{
    public class CustomerFileExtensionContentTypeProvider : FileExtensionContentTypeProvider
    {
        public CustomerFileExtensionContentTypeProvider() :
            base(new Dictionary<string, string>(
                StringComparer.OrdinalIgnoreCase)
            {
                {
                    ".zip",
                    "application/x-zip-compressed"
                },
                {
                    ".less",
                    "stylesheet/css"
                }
            }
            )
        {

        }
    }
}

