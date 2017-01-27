using System.ComponentModel.DataAnnotations.Schema;
using Ujo.Repository.Infrastructure;

namespace Ujo.Model
{
    public abstract class Entity : IObjectState
    {
        [NotMapped]
        public ObjectState ObjectState { get; set; }
    }
}