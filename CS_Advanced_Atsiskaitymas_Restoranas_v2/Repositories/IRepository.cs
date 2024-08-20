namespace CS_Advanced_Atsiskaitymas_Restoranas_v2.Repositories
{
    internal interface IRepository<T>
    {
        void Create(T entity);
        void Delete(T entity);
        public List<T> GetAll();
        public T GetById(int id);
        public int GetLastId();
        void Update(T entity);
    }
}