using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InfoWebApp.Entity
{

    public class Tree
    {
        public Tree()
        {
            Id = 0;
            Name = String.Empty;
            List = new List<Tree>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Tree> List { get; set; }
    }
}