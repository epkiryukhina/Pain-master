
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
    
public partial class Job
{

    public Job()
    {

        this.Employee = new HashSet<Employee>();

    }


    public int Id { get; set; }

    public string JobName { get; set; }



    public virtual ICollection<Employee> Employee { get; set; }

}

}
