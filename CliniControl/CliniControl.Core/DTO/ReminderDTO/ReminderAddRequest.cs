using CliniControl.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CliniControl.Core.Domain.IdentityEntities;
using Microsoft.AspNetCore.Mvc;
using CliniControl.Core.Enums;
using CliniControl.Core.CustomValidators;

namespace CliniControl.Core.DTO.ReminderDTO
{
    public class ReminderAddRequest
    {
        [Required(ErrorMessage = "Por favor a descrição da atividade é obrigatório")]
        public string ActivityDescription { get; set; } = null!;

        [Required(ErrorMessage = "Por favor selecione um dia válido")]
        [DataType(DataType.Date)]
        [DateValidatorAttibute(ErrorMessage = "Data deve ser maior que o dia de hoje")]
        public DateTime? ActityDate { get; set; }
        public bool Finished { get; set; } = false;

        [Required(ErrorMessage = "Por favor selecione um tipo de lembrete válido")]
        public ReminderTypeOptions? ReminderType { get; set; }

        public Reminder ToReminder()
        {
            return new Reminder()
            {
                ActivityDescription = ActivityDescription,
                Finished = Finished,
                ActityDate = ActityDate,
                ReminderType = ReminderType.ToString()
            };
        }
    }
}
