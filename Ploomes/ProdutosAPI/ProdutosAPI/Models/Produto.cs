﻿using System.ComponentModel.DataAnnotations;

namespace ProdutosAPI.Models
{
    public class Produto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; } = string.Empty;

        [Required]
        public string Categoria { get; set; } = string.Empty;

        [Required]
        public decimal Preco { get; set; }

        public int Quantidade { get; set; }
    }
}
