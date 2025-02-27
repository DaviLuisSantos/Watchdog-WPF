using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watchdog.Models
{
    public partial class Process:ObservableObject
    {
        [Key]
        public int Id { get; set; }
        [property:Column(TypeName ="varchar(80)")]
        [ObservableProperty]
        private string path,name;


    }
}
