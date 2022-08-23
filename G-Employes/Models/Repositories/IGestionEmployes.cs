using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionEmployes.Models
{
    public interface IGestionEmployes<IEntity>
    {
        IList<IEntity> List();
        public void Add(IEntity entity);
        public void Update(int id, IEntity entity);
        public void Delete(int id);
        public IEntity Find(int id);
    }
}
