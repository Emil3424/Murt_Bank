//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Murt_Bank
{
    using System;
    using System.Collections.Generic;
    
    public partial class History
    {
        public int IDOperation { get; set; }
        public string NameOperation { get; set; }
        public System.DateTime DateTime { get; set; }
        public double Amount { get; set; }
        public string NumberAccount { get; set; }
    
        public virtual BankAccount BankAccount { get; set; }
    }
}
