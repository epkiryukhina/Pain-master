
//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace Pain
{

    using System;
    using System.Collections.Generic;

    public partial class TypeOfPrice
    {

        public TypeOfPrice()
        {

            this.TypeOfService = new HashSet<TypeOfService>();

        }


        public int Id { get; set; }

        public string Type { get; set; }

        public virtual ICollection<TypeOfService> TypeOfService { get; set; }

        public override string ToString()
        {
            return Type;
        }

    }

}
