
using System.ComponentModel.DataAnnotations.Schema;

namespace Ujo.Repository.Infrastructure
{
    public interface IObjectState
    {
        [NotMapped]
        ObjectState ObjectState { get; set; }
    }
}