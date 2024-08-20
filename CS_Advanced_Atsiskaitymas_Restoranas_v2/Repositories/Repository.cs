using CS_Advanced_Atsiskaitymas_Restoranas_v2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CS_Advanced_Atsiskaitymas_Restoranas_v2.Repositories
{
    internal class Repository<T> : IRepository<T> where T : EntityBase//, new() - no new, we need to find Next Id for entity
    {
        protected readonly string _filePath;

        public Repository(string filePath)
        {
            _filePath = filePath;
        }
        public virtual void Create(T entity)
        {
            try
            {
                if (!File.Exists(_filePath)) File.Create(_filePath);

                using(var writer = new StreamWriter(_filePath, append:true)) 
                { 
                    writer.WriteLine(entity.ToString());
                }
            }
            catch(Exception ex) 
            {
                Console.WriteLine("Failed to write entity to file.");
                Console.WriteLine(ex.Message);
            }
        }
        public virtual List<T>? GetAll()
        {
            List<T> entities = new List<T>();
            ConstructorInfo constructor = typeof(T).GetConstructor(new Type[] { typeof(string) });
            if (constructor == null)
                return default;
                       
            try
            {
                using (var reader = new StreamReader(_filePath))
                {
                    string csvLine;
                    while(null != (csvLine = reader.ReadLine()))
                    {
                        var entity = (T)constructor.Invoke(new object[] { csvLine });
                        if(entity != null) 
                            entities.Add(entity);
                    }
                }
            }
            catch (Exception ex) 
            {
                //Console.WriteLine("Get all failed");
                Console.WriteLine(ex.Message);
            }
            return entities == null ? default : entities;
        }
        public virtual T? GetById(int id)
        {
            T? entity = GetAll().Find(x => x.Id == id);
            return entity;
        }

        public virtual int GetLastId()
        {
            List<T>? entityList = GetAll()?.OrderByDescending(x => x.Id).ToList();
            if (entityList.Count > 0)
                return entityList[0].Id;

            return 0;   //use base.Id = GetLastId() + 1 when creating new entity.
        }
        public virtual void Update(T entity)    //match by Id
        {
            if (entity == null) return;
            //csvLine ctor
            ConstructorInfo constructor = typeof(T).GetConstructor(new Type[] { typeof(string) });
            List<T> entities = new List<T>();
            try
            {
                string csvLine;
                using(var reader = new StreamReader(_filePath))
                {
                    while(null != (csvLine = reader.ReadLine()))
                    {
                        T existingEntity = (T)constructor.Invoke(new object[] { (string)csvLine });
                        if (!(entity.Id == existingEntity.Id))
                            entities.Add(existingEntity);
                        else
                            entities.Add(entity);
                    }
                }
                File.Create(_filePath).Close();
                using(var writer = new StreamWriter(_filePath, append:true)) 
                {
                    foreach(var item in entities)
                    {
                        writer.WriteLine(item.ToString());
                    }
                }
            }
            catch(Exception ex) 
            {
                Console.WriteLine("Failed to update entity.");
                Console.WriteLine(ex.Message);
            }
        }
        public virtual void Delete(T entity)
        {
            if (entity == null) return;
            //csvLine ctor
            ConstructorInfo constructor = typeof(T).GetConstructor(new Type[] { typeof(string) });
            List<T> entities = new List<T>();
            try
            {
                string csvLine;
                using (var reader = new StreamReader(_filePath))
                {
                    while (null != (csvLine = reader.ReadLine()))
                    {
                        T existingEntity = (T)constructor.Invoke(new object[] { (string)csvLine });
                        if (!(entity.Id == existingEntity.Id))
                            entities.Add(existingEntity);

                        else if(typeof(T) == typeof(Order)) //exception for Orders, we want to keep them and mark as Completed
                        {
                            string[] csvLineArr = csvLine.Split(";");
                            csvLineArr[3] = $"{true}";
                            var sb = new StringBuilder();
                            for(int i = 0; i <csvLineArr.Length; i++)
                            {
                                if(i == csvLineArr.Length - 1)
                                    sb.Append(csvLineArr[i]);
                                else
                                    sb.Append(csvLineArr[i] + ";");
                            }
                            string newCsvLine = sb.ToString();
                            entities.Add((T)constructor.Invoke(new object[] { (string)newCsvLine }));
                        }
                    }
                }
                File.Create(_filePath).Close();
                using (var writer = new StreamWriter(_filePath, append: true))
                {
                    foreach (var item in entities)
                    {
                        writer.WriteLine(item.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to delete entity.");
                Console.WriteLine(ex.Message);
            }
        }
    }
}
