using System;
using System.ComponentModel.DataAnnotations;

namespace Examen.Entity
{
	public class Activity
    {
        public int Activity_Id { get; set; }

        [Required(ErrorMessage = "Property_Id es requeido!")]
        public int Property_Id { get; set; }

        [Required(ErrorMessage = "Activity_Schedule es requerido!")]
        public DateTime Activity_Schedule { get; set; }
        public string Activity_Title { get; set; }
        public DateTime Activity_Created_at { get; set; }
        public DateTime Activity_Updated_at { get; set; }
        public string Activity_Status { get; set; }

        public virtual string Property_Title { get; set; }
        public virtual string Property_Address { get; set; }

    }
}
