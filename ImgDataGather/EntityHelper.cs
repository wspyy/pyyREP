using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Data.Entity.Infrastructure;
using System.Data;
using System.Data.Entity.Validation;
using ImgDataGather;

namespace MapUni.DataCenter.SQLServer.DAL
{
    public static class EntityHelper
    {
        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static bool Add<T>(T entity) where T : class
        {
            try
            {
                using (JinchengDB2Entities _emdc = new JinchengDB2Entities())
                {
                    DbEntityEntry entitEntry = _emdc.Entry<T>(entity);
                    entitEntry.State = EntityState.Added;
                    return (_emdc.SaveChanges() > 0);
                }
            }
            catch (DbEntityValidationException e)
            {
                string exception = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    exception += string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        exception += string.Format("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                exception += "";
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static bool Update<T>(T entity) where T : class
        {
            bool bResult = false;
            try
            {
                using (JinchengDB2Entities _emdc = new JinchengDB2Entities())
                {
                    DbEntityEntry<T> entry = _emdc.Entry<T>(entity);
                    entry.State = EntityState.Modified;
                    return (_emdc.SaveChanges() >= 0);
                }
            }
            catch (Exception ex)
            {
                //
            }
            return bResult;
        }

        /// <summary>
        /// 针对修改主键的实体操作(先删除后添加)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static bool Update<T>(T oldEntity, T newEntity) where T : class
        {
            bool bResult = false;
            try
            {
                using (JinchengDB2Entities _emdc = new JinchengDB2Entities())
                {
                    DbEntityEntry<T> entry = _emdc.Entry<T>(oldEntity);
                    entry.State = EntityState.Deleted;
                    if (_emdc.SaveChanges() > 0)
                    {
                        DbEntityEntry<T> entryNew = _emdc.Entry<T>(newEntity);
                        entryNew.State = EntityState.Added;
                        bResult = (_emdc.SaveChanges() >= 0);
                    }
                }
            }
            catch (Exception ex)
            {
                //
            }
            return bResult;
        }

        /// <summary>
        /// 添加实体列表
        /// </summary>
        /// <param name="listT"></param>
        /// <returns></returns>
        public static bool Add<T>(List<T> listT)
        {
            try
            {
                using (JinchengDB2Entities _emdc = new JinchengDB2Entities())
                {
                    DbSet Db_Set = _emdc.Set(typeof(T));
                    foreach (T temp in listT)
                    {
                        Db_Set.Add(temp);
                    }
                    return (_emdc.SaveChanges() > 0);
                }
            }
            catch (Exception ex)
            {
                //throw ex;
                return false;
            }
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static bool Remove<T>(T entity) where T : class
        {
            try
            {
                using (JinchengDB2Entities _emdc = new JinchengDB2Entities())
                {
                    DbEntityEntry<T> entry = _emdc.Entry<T>(entity);
                    entry.State = EntityState.Deleted;
                    return (_emdc.SaveChanges() > 0);
                }
            }
            catch (Exception ex)
            {
                //throw ex;
                return false;
            }
        }

        /// <summary>
        /// 根据Lamda表达式删除实体
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static bool Remove<T>(Func<T, bool> predicate)
        {
            try
            {
                using (JinchengDB2Entities _emdc = new JinchengDB2Entities())
                {
                    DbSet Db_Set = _emdc.Set(typeof(T));
                    List<T> entityList = ((IEnumerable<T>)Db_Set).Where(predicate).ToList();
                    foreach (T temp in entityList)
                    {
                        Db_Set.Remove(temp);
                    }
                    return (_emdc.SaveChanges() > 0);
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 删除实体列表
        /// </summary>
        /// <param name="lisT"></param>
        /// <returns></returns>
        public static bool Remove<T>(List<T> lisT)
        {
            try
            {
                using (JinchengDB2Entities _emdc = new JinchengDB2Entities())
                {
                    DbSet Db_Set = _emdc.Set(typeof(T));
                    foreach (T temp in Db_Set)
                    {
                        Db_Set.Remove(temp);
                    }
                    return (_emdc.SaveChanges() > 0);
                }
            }
            catch (Exception ex)
            {
                //throw ex;
                return false;
            }
        }

        /// <summary>
        /// 获取实体集实体数量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static int Count<T>()
        {
            try
            {
                using (JinchengDB2Entities _emdc = new JinchengDB2Entities())
                {
                    DbSet Db_Set = _emdc.Set(typeof(T));
                    return Db_Set.Local.Count;
                }
            }
            catch (Exception ex)
            {
                //throw ex;
                return 0;
            }
        }

        /// <summary>
        /// 获取指定条件下的实体集实体数量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static int Count<T>(Func<T, bool> predicate)
        {
            try
            {
                using (JinchengDB2Entities _emdc = new JinchengDB2Entities())
                {
                    return ((IEnumerable<T>)_emdc.Set(typeof(T))).Where(predicate).Count();
                }
            }
            catch (Exception ex)
            {
                //throw ex;
                return 0;
            }
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static T Get<T>(Func<T, bool> predicate)
        {
            try
            {
                using (JinchengDB2Entities _emdc = new JinchengDB2Entities())
                {
                    DbSet Db_Set = _emdc.Set(typeof(T));
                    return ((IEnumerable<T>)Db_Set).FirstOrDefault(predicate);
                }
            }
            catch (Exception ex)
            {
                //throw ex;
                return default(T);
            }
        }

        /// <summary>
        /// 获取实体列表
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static List<T> GetList<T>(Func<T, bool> predicate)
        {
            try
            {
                using (JinchengDB2Entities _emdc = new JinchengDB2Entities())
                {
                    DbSet Db_Set = _emdc.Set(typeof(T));
                    return ((IEnumerable<T>)Db_Set).Where(predicate).ToList();
                }
            }
            catch (Exception ex)
            {
                //throw ex;
                return new List<T>();
            }
        }

        /// <summary>
        /// 克隆泛型对象操作
        /// </summary>
        /// <param name="ObjectInstance"></param>
        /// <param name="ObjectInstanceNew"></param>
        /// <returns></returns>
        public static T Clone<T>(T ObjectInstance) where T : new()
        {
            if (ObjectInstance == null)
                return new T();
            T ObjectInstanceNew = new T();
            foreach (var prop in ObjectInstance.GetType().GetProperties())
            {
                prop.SetValue(ObjectInstanceNew, prop.GetValue(ObjectInstance, null), null);
            }
            return ObjectInstanceNew;

        }
    }
}
