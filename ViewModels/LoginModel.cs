using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.OpenApi.MicrosoftExtensions;

namespace AlunoWeb.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "O campo Email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O campo Email deve ser um endereço de email válido.")]
        public  string Email { get; set; }

        [Required(ErrorMessage = "O campo Senha é obrigatório.")]
        [StringLength(20, ErrorMessage = "O campo Senha deve ter no máximo 20 caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
}}