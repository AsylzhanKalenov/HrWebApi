using HrWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrWebApi.Data
{
    public class PrivData
    {
        public UserPriv userPriv { get; set; }
        public Children children { get; set; }

        public PrivData(UserPriv userPriv, Children children) {
            this.userPriv = userPriv;
            this.children = children;
        }
    }
}
