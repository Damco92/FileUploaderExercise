using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FileUploader.Domain.Models
{
    public class File
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int FileId { get; set; }
        [Required]
        [MaxLength(100)]
        public string FileName { get; set; }
        [Required]
        [MaxLength(8000)]
        public byte[] FileData { get; set; }
        [Required]
        public DateTime Created { get; set; }
    }
}
