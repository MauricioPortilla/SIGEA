CREATE TABLE rol (
    id_rol int PRIMARY KEY NOT NULL IDENTITY(1, 1),
    nombre varchar(20) NOT NULL
);

CREATE TABLE cuenta (
    id_cuenta int PRIMARY KEY NOT NULL IDENTITY(1, 1),
    id_rol int NOT NULL,
    FOREIGN KEY (id_rol) REFERENCES rol(id_rol),
    usuario varchar(15) NOT NULL,
    contrasenia varchar(255) NOT NULL,
);

CREATE TABLE organizador (
    id_organizador int PRIMARY KEY NOT NULL IDENTITY(1, 1),
    id_cuenta int NOT NULL,
    FOREIGN KEY (id_cuenta) REFERENCES cuenta(id_cuenta),
    nombre varchar(50) NOT NULL,
    paterno varchar(50) NOT NULL,
    materno varchar(50) NULL,
    telefono varchar(10) NOT NULL,
    correo varchar(100) NOT NULL
);

CREATE TABLE evento (
    id_evento int PRIMARY KEY NOT NULL IDENTITY(1, 1),
    id_organizador int NOT NULL,
    FOREIGN KEY (id_organizador) REFERENCES organizador(id_organizador),
    nombre varchar(50) NOT NULL,
    tipo varchar(50) NOT NULL,
    fechaInicio date NOT NULL,
    fechaFin date NOT NULL,
    cuota float NOT NULL DEFAULT 0,
    sede varchar(50) NOT NULL
);

CREATE TABLE gasto (
    id_gasto int PRIMARY KEY NOT NULL IDENTITY(1, 1),
    id_evento int NOT NULL,
    FOREIGN KEY (id_evento) REFERENCES evento(id_evento),
    cantidad float NOT NULL,
    motivo varchar(100) NOT NULL,
    fecha date NOT NULL
);

CREATE TABLE comite (
    id_comite int PRIMARY KEY NOT NULL IDENTITY(1, 1),
    nombre varchar(50) NOT NULL,
    responsabilidades varchar(100)
);

CREATE TABLE tarea (
    id_tarea int PRIMARY KEY NOT NULL IDENTITY(1, 1),
    id_comite int NOT NULL,
    FOREIGN KEY (id_comite) REFERENCES comite(id_comite),
    titulo varchar(30) NOT NULL,
    descripcion varchar(100) NOT NULL,
    asignadoA varchar(50) NOT NULL
);

CREATE TABLE comite_evento (
    id_comite int NOT NULL,
    FOREIGN KEY (id_comite) REFERENCES comite(id_comite),
    id_evento int NOT NULL
    FOREIGN KEY (id_evento) REFERENCES evento(id_evento),
    PRIMARY KEY (id_comite, id_evento)
);

CREATE TABLE comite_lider (
    id_comite int NOT NULL,
    FOREIGN KEY (id_comite) REFERENCES comite(id_comite),
    id_organizador int NOT NULL,
    FOREIGN KEY (id_organizador) REFERENCES organizador(id_organizador),
    PRIMARY KEY (id_comite, id_organizador)
);

CREATE TABLE comite_organizador (
    id_comite int NOT NULL,
    FOREIGN KEY (id_comite) REFERENCES comite(id_comite),
    id_organizador int NOT NULL,
    FOREIGN KEY (id_organizador) REFERENCES organizador(id_organizador),
    PRIMARY KEY (id_comite, id_organizador)
);

CREATE TABLE actividad (
    id_actividad int PRIMARY KEY NOT NULL IDENTITY(1, 1),
    id_evento int NOT NULL,
    FOREIGN KEY (id_evento) REFERENCES evento(id_evento),
    nombre varchar(25) NOT NULL,
    descripcion varchar(255) NOT NULL,
    tipo varchar(20) NOT NULL
);

CREATE TABLE tarea_actividad (
    id_tarea int NOT NULL,
    FOREIGN KEY (id_tarea) REFERENCES tarea(id_tarea),
    id_actividad int NOT NULL,
    FOREIGN KEY (id_actividad) REFERENCES actividad(id_actividad),
    PRIMARY KEY (id_tarea, id_actividad)
);

CREATE TABLE presentacion (
    id_presentacion int PRIMARY KEY NOT NULL IDENTITY(1, 1),
    id_actividad int NOT NULL,
    FOREIGN KEY (id_actividad) REFERENCES actividad(id_actividad),
    fechaPresentacion date NOT NULL,
    horaInicio time NOT NULL,
    horaFin time NOT NULL
);

CREATE TABLE magistral (
    id_magistral int PRIMARY KEY NOT NULL IDENTITY(1, 1),
    id_actividad int NOT NULL,
    FOREIGN KEY (id_actividad) REFERENCES actividad(id_actividad),
    nombre varchar(50) NOT NULL,
    paterno varchar(50) NOT NULL,
    materno varchar(50) NULL,
    telefono varchar(10) NOT NULL,
    correo varchar(100) NOT NULL,
    lugarOrigen varchar(25) NOT NULL
);

CREATE TABLE gasto_magistral (
    id_gasto int UNIQUE NOT NULL,
    FOREIGN KEY (id_gasto) REFERENCES gasto(id_gasto),
    id_magistral int NOT NULL,
    FOREIGN KEY (id_magistral) REFERENCES magistral(id_magistral)
);

CREATE TABLE adscripcion (
    id_adscripcion int PRIMARY KEY NOT NULL IDENTITY(1, 1),
    nombreDependencia varchar(50) NOT NULL,
    direccion varchar(100) NOT NULL,
    telefono varchar(10) NOT NULL,
    puesto varchar(25) NOT NULL
);

CREATE TABLE asistente (
    id_asistente int PRIMARY KEY NOT NULL IDENTITY(1, 1),
    id_adscripcion int NOT NULL,
    FOREIGN KEY (id_adscripcion) REFERENCES adscripcion(id_adscripcion),
    nombre varchar(50) NOT NULL,
    paterno varchar(50) NOT NULL,
    materno varchar(50) NULL,
    correo varchar(100) NOT NULL
);

CREATE TABLE asistente_actividad (
    id_asistente int NOT NULL,
    FOREIGN KEY (id_asistente) REFERENCES asistente(id_asistente),
    id_actividad int NOT NULL,
    FOREIGN KEY (id_actividad) REFERENCES actividad(id_actividad),
    PRIMARY KEY (id_asistente, id_actividad)
);

CREATE TABLE pago (
    id_pago int PRIMARY KEY NOT NULL IDENTITY(1, 1),
    cantidad float NOT NULL,
    fecha date NOT NULL
);

CREATE TABLE pago_asistente (
    id_pago int UNIQUE NOT NULL,
    FOREIGN KEY (id_pago) REFERENCES pago(id_pago),
    id_asistente int NOT NULL,
    FOREIGN KEY (id_asistente) REFERENCES asistente(id_asistente)
);

CREATE TABLE track (
    id_track int PRIMARY KEY NOT NULL IDENTITY(1, 1),
    id_evento int NOT NULL,
    FOREIGN KEY (id_evento) REFERENCES evento(id_evento),
    nombre varchar(25) NOT NULL,
    descripcion varchar(100) NOT NULL
);

CREATE TABLE articulo (
    id_articulo int PRIMARY KEY NOT NULL IDENTITY(1, 1),
    id_track int NOT NULL,
    FOREIGN KEY (id_track) REFERENCES track(id_track),
    titulo varchar(100) NOT NULL,
    anio int NOT NULL,
    estado varchar(20) NOT NULL,
    resumen varchar(255) NOT NULL,
    keywords varchar(100) NOT NULL,
    archivo varchar(255) NOT NULL
);

CREATE TABLE presentacion_articulo (
    id_presentacion int UNIQUE NOT NULL,
    FOREIGN KEY (id_presentacion) REFERENCES presentacion(id_presentacion),
    id_articulo int NOT NULL,
    FOREIGN KEY (id_articulo) REFERENCES articulo(id_articulo)
);

CREATE TABLE pago_articulo (
    id_pago int NOT NULL,
    FOREIGN KEY (id_pago) REFERENCES pago(id_pago),
    id_articulo int NOT NULL,
    FOREIGN KEY (id_articulo) REFERENCES articulo(id_articulo),
    PRIMARY KEY (id_pago, id_articulo)
);

CREATE TABLE autor (
    id_autor int PRIMARY KEY NOT NULL IDENTITY(1, 1),
    nombre varchar(50) NOT NULL,
    paterno varchar(50) NOT NULL,
    materno varchar(50) NULL,
    telefono varchar(10) NOT NULL,
    correo varchar(100) NOT NULL
);

CREATE TABLE autor_articulo (
    id_autor int NOT NULL,
    FOREIGN KEY (id_autor) REFERENCES autor(id_autor),
    id_articulo int NOT NULL,
    FOREIGN KEY (id_articulo) REFERENCES articulo(id_articulo),
    fecha date NOT NULL,
    PRIMARY KEY (id_autor, id_articulo)
);

CREATE TABLE revisor (
    id_revisor int PRIMARY KEY NOT NULL IDENTITY(1, 1),
    id_cuenta int NOT NULL,
    FOREIGN KEY (id_cuenta) REFERENCES cuenta(id_cuenta),
    nombre varchar(50) NOT NULL,
    paterno varchar(50) NOT NULL,
    materno varchar(50) NULL,
    correo varchar(100) NOT NULL,
);

CREATE TABLE revisor_articulo (
    id_revisorArticulo int PRIMARY KEY NOT NULL IDENTITY(1, 1),
    id_revisor int NOT NULL,
    FOREIGN KEY (id_revisor) REFERENCES revisor(id_revisor),
    id_articulo int NOT NULL,
    FOREIGN KEY (id_articulo) REFERENCES articulo(id_articulo)
);

CREATE TABLE evaluacion_articulo (
    id_revisorArticulo int PRIMARY KEY NOT NULL,
    FOREIGN KEY (id_revisorArticulo) REFERENCES revisor_articulo(id_revisorArticulo),
    calificacion int NOT NULL,
    observaciones text NOT NULL,
    fecha date NOT NULL,
    gradoExpertiz int NOT NULL,
    estado varchar(20) NOT NULL
);
