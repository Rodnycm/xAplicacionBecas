﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntitiesLayer;
using System.Collections;
using System.Transactions;
using System.Data.SqlClient;
using System.Data;


namespace DAL
{

    public class CursoRepository : IRepository<Curso>
    {

        private static CursoRepository instance;
        private List<IEntity> _insertItems;
        private List<IEntity> _deleteItems;
        private List<IEntity> _updateItems;



        private CursoRepository()
        {
            _insertItems = new List<IEntity>();
            _deleteItems = new List<IEntity>();
            _updateItems = new List<IEntity>();
        }




        //<summary> Método que se encarga de instanciar un CursoRepository</summary>
        //<author> Valeria Ramírez Cordero </author> 
        //<param> No recibe valor  </param>
        //<returns> Retorna una instancia de la clase CursoRepository.</returns> 
        public static CursoRepository Instance
        {

            get
            {
                if (instance == null)
                {
                    instance = new CursoRepository() { };
                }
                return instance;
            }
        }


        //<summary> Método que se encarga de agregar un curso a la lista de cursos que se desean insertar</summary>
        //<author> Valeria Ramírez Cordero </author> 
        //<param name "entity">  variable de tipo Curso que contiene el objeto Curso que se desea insertar  </param>
        //<returns> No retorna valor.</returns> 
        public void Insert(Curso entity)
        {
            _insertItems.Add(entity);
        }


        //<summary> Método que se encarga de agregar un curso a la lista de cursos que se desean eliminar</summary>
        //<author>  Valeria Ramírez Cordero  </author> 
        //<param name "entity">  variable de tipo Curso que contiene el objeto Curso que se desea eliminar  </param>
        //<returns> No retorna valor.</returns> 
        public void Delete(Curso entity)
        {
            _deleteItems.Add(entity);
        }

        //<summary> Método que se encarga de agregar un curso a la lista de cursos que se desean modificar</summary>
        //<author> Valeria Ramírez Cordero </author> 
        //<param name "entity">  variable de tipo Curso que contiene el objeto Curso que se desea modificar  </param>
        //<returns> No retorna valor.</returns> 
        public void Update(Curso entity)
        {
            _updateItems.Add(entity);
        }


        //<summary> Método que se encarga de traer de la base de datos todos los cursos registrados </summary>
        //<author>Valeria Ramírez Cordero</author> 
        //<param> no recibe parametros </param>
        //<returns>Retorna una lista con todos los cursos registrados en el sistema.</returns> 
        public IEnumerable<Curso> GetAll()
        {

            List<Curso> pCurso = null;
            SqlCommand cmd = new SqlCommand();
            DataSet ds = DBAccess.ExecuteSPWithDS(ref cmd, "Sp_buscarCursos");

            if (ds.Tables[0].Rows.Count > 0)
            {
                pCurso = new List<Curso>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    pCurso.Add(new Curso
                    {

                        codigo = dr["Codigo"].ToString(),
                        nombre = dr["Nombre"].ToString(),
                        cuatrimestre = dr["Cuatrimestre"].ToString(),
                        creditos = Convert.ToInt32(dr["Creditos"]),
                        precio = Convert.ToDouble(dr["Precio"]),
                        Id = Convert.ToInt32(dr["IdCurso"])
                    });
                }
            }

            return pCurso;
        }


        //<summary> Método que se encarga de traer de la base de datos un curso en específico </summary>
        //<author>Valeria Ramírez Cordero</author> 
        //<param name "id"> parámetro de tipo int que contiene el Id del curso que se desea traer </param>
        //<returns>Retorna el curso deseado</returns> 
        public Curso GetById(int Id)
        {

            Curso objCurso = null;
            var sqlQuery = "SELECT Id, Nombre, Precio FROM Producto WHERE id = @idProducto";
            SqlCommand cmd = new SqlCommand(sqlQuery);
            cmd.Parameters.AddWithValue("@idProducto", Id);

            //var ds = DBAccess.ExecuteQuery(cmd);
            DataSet ds = DBAccess.ExecuteSPWithDS(ref cmd, "Sp_buscarUnCurso");

            if (ds.Tables[0].Rows.Count > 0)
            {

                var dr = ds.Tables[0].Rows[0];
                objCurso = new Curso
                {

                    nombre = dr["Nombre"].ToString(),
                    codigo = dr["Codigo"].ToString(),
                    cuatrimestre = dr["Cuatrimestre"].ToString(),
                    creditos = Convert.ToInt32(dr["Creditos"]),
                    precio = Convert.ToDouble(dr["Precio"]),
                    Id = Convert.ToInt32(dr["IdCurso"]),
                };
            }

            return objCurso;
        }



        //<summary> Método que se encarga de traer de la base de datos un curso específico </summary>
        //<author> Valeria Ramírez Cordero</author> 
        //<param name "parametro"> parámetro de tipo string que contiene el nombre o código del curso que se desea traer </param>
        //<returns>Retorna el curso deseado</returns> 
        public Curso GetByNombre(String parametro)
        {
            Curso objCurso = null;
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@parametro", parametro);

            DataSet ds = DBAccess.ExecuteSPWithDS(ref cmd, "Sp_buscarUnCurso");

            if (ds.Tables[0].Rows.Count > 0)
            {
                var dr = ds.Tables[0].Rows[0];
                objCurso = new Curso
                {
                    codigo = dr["Codigo"].ToString(),
                    nombre = dr["Nombre"].ToString(),
                    cuatrimestre = dr["Cuatrimestre"].ToString(),
                    creditos = Convert.ToInt32(dr["Creditos"]),
                    precio = Convert.ToDouble(dr["Precio"]),
                    Id = Convert.ToInt32(dr["IdCurso"])


                };
                //objCurso.Id = Convert.ToInt32(dr["Id"]);
            }

            return objCurso;
        }


        //<summary> Método que se encarga de guardar en la base de datos los cambios realizados </summary>
        //<author>Valeria Ramírez Cordero</author> 
        //<param> No recibe parámetros </param>
        //<returns>No retorna valor</returns> 
        public void Save()
        {

            using (TransactionScope scope = new TransactionScope())
            {

                try
                {

                    if (_insertItems.Count > 0)
                    {

                        foreach (Curso objCurso in _insertItems)
                        {
                            InsertCurso(objCurso);
                        }
                    }

                    if (_updateItems.Count > 0)
                    {
                        foreach (Curso p in _updateItems)
                        {
                            UpdateCurso(p);
                        }
                    }

                    if (_deleteItems.Count > 0)
                    {
                        foreach (Curso p in _deleteItems)
                        {
                            DeleteCurso(p);
                        }
                    }

                    scope.Complete();
                }
                catch (TransactionAbortedException ex)
                {

                }
                catch (ApplicationException ex)
                {

                }
                finally
                {
                    Clear();
                }

            }
        }


        //<summary> Método que se encarga limpiar todas las listas </summary>
        //<author> Valeria Ramírez Cordero </author> 
        //<param> No recibe parámetros </param>
        //<returns>No retorna valor </returns> 

        public void Clear()
        {
            _insertItems.Clear();
            _deleteItems.Clear();
            _updateItems.Clear();
        }

        //<summary> Método que se encarga de insertar en la base de datos un curso</summary>
        //<author>Valeria Ramírez Cordero</author> 
        //<param name "objCurso"> variable de tipo Curso que contiene el objeto curso que se desea insertar en la base de datos </param>
        //<returns> No retorna valor</returns> 
        private void InsertCurso(Curso objCurso)
        {

            try
            {

                SqlCommand cmd = new SqlCommand();

                cmd.Parameters.Add(new SqlParameter("@Codigo", objCurso.codigo));
                cmd.Parameters.Add(new SqlParameter("@Nombre", objCurso.nombre));
                cmd.Parameters.Add(new SqlParameter("@Cuatrimestre", objCurso.cuatrimestre));
                cmd.Parameters.Add(new SqlParameter("@Creditos", objCurso.creditos));
                cmd.Parameters.Add(new SqlParameter("@Precio", objCurso.precio));

                DataSet ds = DBAccess.ExecuteSPWithDS(ref cmd, "Sp_crearCurso ");

            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        //<summary> Método que se encarga de modificar en la base de datos un curso </summary>
        //<author> Valeria Ramírez Cordero </author> 
        //<param name "objCurso"> variable de tipo Curso que contiene el objeto curso que se desea modificar en la base de datos </param>
        //<returns> No retorna valor</returns> 
        private void UpdateCurso(Curso objCurso)
        {

            try
            {
                SqlCommand cmd = new SqlCommand();

                cmd.Parameters.Add(new SqlParameter("@Código", objCurso.codigo));
                cmd.Parameters.Add(new SqlParameter("@Nombre", objCurso.nombre));
                cmd.Parameters.Add(new SqlParameter("@Cuatrimestre", objCurso.cuatrimestre));
                cmd.Parameters.Add(new SqlParameter("@Creditos", objCurso.cuatrimestre));
                cmd.Parameters.Add(new SqlParameter("@Precio", objCurso.precio));
                cmd.Parameters.Add(new SqlParameter("@idCurso", objCurso.Id));

                DataSet ds = DBAccess.ExecuteSPWithDS(ref cmd, "Sp_modificarCursos");

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //<summary> Método que se encarga de eliminar un curso de la base de datos </summary>
        //<author> Valeria Ramírez Cordero </author> 
        //<param name "objCurso"> variable de tipo Curso que contiene el objeto curso que se desea eliminar de la base de datos </param>
        //<returns> No retorna valor</returns> 
        private void DeleteCurso(Curso objCurso)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Parameters.Add(new SqlParameter("@IdCurso", objCurso.Id));
                DataSet ds = DBAccess.ExecuteSPWithDS(ref cmd, "Sp_eliminarCurso");

            }
            catch (SqlException ex)
            {
                throw ex;
                //logear la excepcion a la bd con un Exception
                //throw new DataAccessException("Ha ocurrido un error al eliminar un usuario", ex);

            }
            catch (Exception ex)
            {
                throw ex;
                //logear la excepcion a la bd con un Exception
                //throw new DataAccessException("Ha ocurrido un error al eliminar un usuario", ex);
            }
        }
    }
}