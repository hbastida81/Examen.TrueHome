# Examen.TrueHome
# para ejecutar en iisexpress tiene que cambiar la  conexion  en el archivo appsettings , por ejemplo

  "ConnectionStrings": {
    "Connection-Postgres": "Server=172.17.0.2:5432;Database=db_examen;User Id=postgres;Password=user_01;" // conexion a red interna DOCKER 
    //"Connection-Postgres": "Server=localhost:5432;Database=db_examen;User Id=postgres;Password=user_01;" // conexion desde aplicacion iis
  }
 
 Se utilizo la red interna del docker para conectar con la base de datos misma que se encuentra en Docker
