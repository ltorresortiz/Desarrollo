﻿using AeiWebServices.Dominio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AeiWebServices.Permanencia
{
    public class SqlServerTwitter: DAOTwitter
    {

        public int agregarUsuario(Twitter usuario)
        {
            ConexionSqlServer conexion = new ConexionSqlServer(); 
            int respuesta= conexion.insertar("INSERT INTO Twitter (ScreenName, idUsuario, OauthTokenSecret, OauthToken) VALUES ('"+usuario.ScreenName+"','"+usuario.IdUsuario+"','"+usuario.OauthTokenSecret+"','"+usuario.OauthToken+"');");
            conexion.cerrarConexion();
            return respuesta;
        }

        public Twitter buscarUsuario(string screenName)
        {
            ConexionSqlServer conexion = new ConexionSqlServer();
            SqlDataReader tabla = conexion.consultar("select * from Twitter where ScreenName= '"+screenName+"';");
            Twitter twitter = new Twitter();
            while (tabla!=null && tabla.Read())
            {
                twitter = new Twitter(tabla["IDUSUARIO"].ToString(), tabla["SCREENNAME"].ToString(), tabla["OauthToken"].ToString(), tabla["OauthTokenSecret"].ToString());
            }
            conexion.cerrarConexion();
            return twitter;
        }
    }
}