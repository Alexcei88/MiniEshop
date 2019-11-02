using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MiniEshop.Domain
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public Guid ParentId { get; set; }

        public virtual List<Good> Goods { get; set; }

        /// <summary>
        /// Этот тип фактически не используется, но добавлен на случай, если будут запаршивать данные по иерархии(отдельно запрашивать поддеревья).
        /// В данной задаче это не предусмотрено, дерево получем полностью
        /// </summary>
        public SqlHierarchyId  HierarchyId {get; set;}
    }
}
