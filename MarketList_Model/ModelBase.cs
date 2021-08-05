using System.ComponentModel.DataAnnotations;

namespace MarketList_Model
{
    public class ModelBase
    {
        [Key]
        public int Id { get; set; }                
    }
}