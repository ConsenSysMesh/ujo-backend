using System.ComponentModel.DataAnnotations.Schema;

namespace Ujo.Repository.Infrastructure
{
    public abstract class Entity : IObjectState
    {
        [NotMapped]
        public ObjectState ObjectState { get; set; }
    }
}