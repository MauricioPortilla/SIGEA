CREATE TABLE Cuenta (
    id_cuenta int PRIMARY KEY NOT NULL IDENTITY(1, 1),
    usuario varchar(15) NOT NULL,
    contrasenia varchar(255) NOT NULL,
);

CREATE TABLE Organizador (
    id_organizador int PRIMARY KEY NOT NULL IDENTITY(1, 1),
    id_cuenta int NOT NULL,
    FOREIGN KEY (id_cuenta) REFERENCES Cuenta(id_cuenta),
    nombre varchar(50) NOT NULL,
    paterno varchar(50) NOT NULL,
    materno varchar(50) NULL,
    telefono varchar(10) NOT NULL,
    correo varchar(100) NOT NULL
);

CREATE TABLE Evento (
    id_evento int PRIMARY KEY NOT NULL IDENTITY(1, 1),
    id_organizador int NOT NULL,
    FOREIGN KEY (id_organizador) REFERENCES Organizador(id_organizador),
    nombre varchar(50) NOT NULL,
    fechaInicio date NOT NULL,
    fechaFin date NOT NULL,
    cuota float NOT NULL DEFAULT 0,
    sede varchar(50) NOT NULL
);

CREATE TABLE Gasto (
    id_gasto int PRIMARY KEY NOT NULL IDENTITY(1, 1),
    id_evento int NOT NULL,
    FOREIGN KEY (id_evento) REFERENCES Evento(id_evento),
    cantidad float NOT NULL,
    motivo varchar(100) NOT NULL,
    fecha date NOT NULL
);

CREATE TABLE Comite (
    id_comite int PRIMARY KEY NOT NULL IDENTITY(1, 1),
    nombre varchar(50) NOT NULL,
    responsabilidades varchar(100)
);

CREATE TABLE Tarea (
    id_tarea int PRIMARY KEY NOT NULL IDENTITY(1, 1),
    id_comite int NOT NULL,
    FOREIGN KEY (id_comite) REFERENCES Comite(id_comite),
    titulo varchar(30) NOT NULL,
    descripcion varchar(100) NOT NULL,
    asignadoA varchar(50) NOT NULL
);

CREATE TABLE ComiteEvento (
    id_comite int NOT NULL,
    FOREIGN KEY (id_comite) REFERENCES Comite(id_comite),
    id_evento int NOT NULL
    FOREIGN KEY (id_evento) REFERENCES Evento(id_evento),
    PRIMARY KEY (id_comite, id_evento)
);

CREATE TABLE ComiteLider (
    id_comite int NOT NULL,
    FOREIGN KEY (id_comite) REFERENCES Comite(id_comite),
    id_organizador int NOT NULL,
    FOREIGN KEY (id_organizador) REFERENCES Organizador(id_organizador),
    PRIMARY KEY (id_comite, id_organizador)
);

CREATE TABLE ComiteOrganizador (
    id_comite int NOT NULL,
    FOREIGN KEY (id_comite) REFERENCES Comite(id_comite),
    id_organizador int NOT NULL,
    FOREIGN KEY (id_organizador) REFERENCES Organizador(id_organizador),
    PRIMARY KEY (id_comite, id_organizador)
);

CREATE TABLE Actividad (
    id_actividad int PRIMARY KEY NOT NULL IDENTITY(1, 1),
    id_evento int NOT NULL,
    FOREIGN KEY (id_evento) REFERENCES Evento(id_evento),
    nombre varchar(25) NOT NULL,
    descripcion varchar(255) NOT NULL,
    tipo varchar(20) NOT NULL
);

CREATE TABLE TareaActividad (
    id_tarea int NOT NULL,
    FOREIGN KEY (id_tarea) REFERENCES Tarea(id_tarea),
    id_actividad int NOT NULL,
    FOREIGN KEY (id_actividad) REFERENCES Actividad(id_actividad),
    PRIMARY KEY (id_tarea, id_actividad)
);

CREATE TABLE Presentacion (
    id_presentacion int PRIMARY KEY NOT NULL IDENTITY(1, 1),
    id_actividad int NOT NULL,
    FOREIGN KEY (id_actividad) REFERENCES Actividad(id_actividad),
    fechaPresentacion date NOT NULL,
    horaInicio time NOT NULL,
    horaFin time NOT NULL
);

CREATE TABLE Magistral (
    id_magistral int PRIMARY KEY NOT NULL IDENTITY(1, 1),
    id_actividad int NOT NULL,
    FOREIGN KEY (id_actividad) REFERENCES Actividad(id_actividad),
    nombre varchar(50) NOT NULL,
    paterno varchar(50) NOT NULL,
    materno varchar(50) NULL,
    telefono varchar(10) NOT NULL,
    correo varchar(100) NOT NULL,
    lugarOrigen varchar(25) NOT NULL
);

CREATE TABLE GastoMagistral (
    id_gasto int UNIQUE NOT NULL,
    FOREIGN KEY (id_gasto) REFERENCES Gasto(id_gasto),
    id_magistral int NOT NULL,
    FOREIGN KEY (id_magistral) REFERENCES Magistral(id_magistral)
);

CREATE TABLE Adscripcion (
    id_adscripcion int PRIMARY KEY NOT NULL IDENTITY(1, 1),
    nombreDependencia varchar(50) NOT NULL,
    direccion varchar(100) NOT NULL,
    telefono varchar(10) NOT NULL,
    puesto varchar(25) NOT NULL
);

CREATE TABLE Asistente (
    id_asistente int PRIMARY KEY NOT NULL IDENTITY(1, 1),
    id_adscripcion int NOT NULL,
    FOREIGN KEY (id_adscripcion) REFERENCES Adscripcion(id_adscripcion),
    nombre varchar(50) NOT NULL,
    paterno varchar(50) NOT NULL,
    materno varchar(50) NULL,
    correo varchar(100) NOT NULL
);

CREATE TABLE AsistenteActividad (
    id_asistente int NOT NULL,
    FOREIGN KEY (id_asistente) REFERENCES Asistente(id_asistente),
    id_actividad int NOT NULL,
    FOREIGN KEY (id_actividad) REFERENCES Actividad(id_actividad),
    PRIMARY KEY (id_asistente, id_actividad)
);

CREATE TABLE Pago (
    id_pago int PRIMARY KEY NOT NULL IDENTITY(1, 1),
    cantidad float NOT NULL,
    fecha date NOT NULL
);

CREATE TABLE PagoAsistente (
    id_pago int UNIQUE NOT NULL,
    FOREIGN KEY (id_pago) REFERENCES pago(id_pago),
    id_asistente int NOT NULL,
    FOREIGN KEY (id_asistente) REFERENCES Asistente(id_asistente)
);

CREATE TABLE Track (
    id_track int PRIMARY KEY NOT NULL IDENTITY(1, 1),
    id_evento int NOT NULL,
    FOREIGN KEY (id_evento) REFERENCES Evento(id_evento),
    nombre varchar(25) NOT NULL,
    descripcion varchar(100) NOT NULL
);

CREATE TABLE Articulo (
    id_articulo int PRIMARY KEY NOT NULL IDENTITY(1, 1),
    id_track int NOT NULL,
    FOREIGN KEY (id_track) REFERENCES Track(id_track),
    titulo varchar(100) NOT NULL,
    anio int NOT NULL,
    estado varchar(20) NOT NULL,
    resumen varchar(255) NOT NULL,
    keywords varchar(100) NOT NULL,
    archivo varchar(255) NOT NULL
);

CREATE TABLE PresentacionArticulo (
    id_presentacion int UNIQUE NOT NULL,
    FOREIGN KEY (id_presentacion) REFERENCES Presentacion(id_presentacion),
    id_articulo int NOT NULL,
    FOREIGN KEY (id_articulo) REFERENCES Articulo(id_articulo)
);

CREATE TABLE PagoArticulo (
    id_pago int NOT NULL,
    FOREIGN KEY (id_pago) REFERENCES Pago(id_pago),
    id_articulo int NOT NULL,
    FOREIGN KEY (id_articulo) REFERENCES Articulo(id_articulo),
    PRIMARY KEY (id_pago, id_articulo)
);

CREATE TABLE Autor (
    id_autor int PRIMARY KEY NOT NULL IDENTITY(1, 1),
    id_adscripcion int NOT NULL,
    FOREIGN KEY (id_adscripcion) REFERENCES Adscripcion(id_adscripcion),
    nombre varchar(50) NOT NULL,
    paterno varchar(50) NOT NULL,
    materno varchar(50) NULL,
    telefono varchar(10) NOT NULL,
    correo varchar(100) NOT NULL
);

CREATE TABLE AutorArticulo (
    id_autor int NOT NULL,
    FOREIGN KEY (id_autor) REFERENCES Autor(id_autor),
    id_articulo int NOT NULL,
    FOREIGN KEY (id_articulo) REFERENCES Articulo(id_articulo),
    fecha date NOT NULL,
    PRIMARY KEY (id_autor, id_articulo)
);

CREATE TABLE Revisor (
    id_revisor int PRIMARY KEY NOT NULL IDENTITY(1, 1),
    id_cuenta int NOT NULL,
    FOREIGN KEY (id_cuenta) REFERENCES Cuenta(id_cuenta),
    nombre varchar(50) NOT NULL,
    paterno varchar(50) NOT NULL,
    materno varchar(50) NULL,
    correo varchar(100) NOT NULL,
);

CREATE TABLE RevisorArticulo (
    id_revisorArticulo int PRIMARY KEY NOT NULL IDENTITY(1, 1),
    id_revisor int NOT NULL,
    FOREIGN KEY (id_revisor) REFERENCES Revisor(id_revisor),
    id_articulo int NOT NULL,
    FOREIGN KEY (id_articulo) REFERENCES Articulo(id_articulo)
);

CREATE TABLE EvaluacionArticulo (
    id_revisorArticulo int PRIMARY KEY NOT NULL,
    FOREIGN KEY (id_revisorArticulo) REFERENCES RevisorArticulo(id_revisorArticulo),
    calificacion int NOT NULL,
    observaciones text NOT NULL,
    fecha date NOT NULL,
    gradoExpertiz int NOT NULL,
    estado varchar(20) NOT NULL
);
