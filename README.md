![Main Banner](https://tipi-pod.sfo3.cdn.digitaloceanspaces.com/github/iot-demo-banner.jpg)

# Acerca del proyecto
Demo sobre el funcionamiento del broker de Mqqt de AWS, el demo contempla envio y recepcion de información utilizando un Thing de AWS. 

## Tecnologías utilizadas

El demo se realizo sobre la versión 6.0 de .NET dentro de un API con PostgreSql como base de datos y Dapper como ORM.

![Makbi Stack](https://tipi-pod.sfo3.cdn.digitaloceanspaces.com/github/iot-demo-stack.jpg)

## ¿Como iniciar?
Para iniciar primero se debe de configurar dentro del IOT Core un dispositivo y generar su certificado, puedes leer la documentación del IOT Core [aqui](https://aws.amazon.com/es/iot-core/). Una vez creado esto puede dirigirte al código del proyecto API
dentro del archivo program.cs deberas configurar las siguientes variables con tu información:
- broker
- port
- clientId
- certPass

Luego deberas de colocar el certificado en la carpeta /certs

Por ultimo deberas de agregar en tu srchivo appsettings.json la información para tu conexión a la base de datos de esta maner
```
{
  "ConnectionString": "Server=YOUR_HOST; Port=YOUR_PORT; Database=YOUR_DATABASE; Uid=YOUR_USER; Pwd=YOUR_PASSWORD"
}
```

📒 Observaciones
Este proyecto fue construido como demo sobre el uo de IOT Core, por lo que cuenta con modelos preestablecidos de responses prestablecidas por dispositivos utilizandos en su momento, sin embargo la demostración permite extraer conocmiento sobre la conexión con IOT Core.
